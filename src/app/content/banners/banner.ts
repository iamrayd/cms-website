import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

// Angular Material
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-banners',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  templateUrl: './banner.html',
  styleUrls: ['./banner.scss'],
})
export class BannersComponent implements OnInit {
  banners: any[] = [];
  isLoading = true;
  error: string | null = null;

  showCreateModal = false;
  isSaving = false;
  isDeleting = false;

  editingBannerId: string | null = null;

  showPreviewModal = false;
  previewBanner: any | null = null;

  newBanner: any = {
    title: '',
    imageUrl: '',
    status: 'draft',
    link: '',
    publishAt: '',
    expireAt: '',
    content: ''
  };

  // ⭐ NEW: date + time controls for UI
  publishDate: Date | null = null;
  publishTime: string = '09:00 AM';

  expireDate: Date | null = null;
  expireTime: string = '05:00 PM';

  timeOptions: string[] = [];

  private apiUrl = 'https://localhost:7090/api/Banners';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.generateTimes();
    this.loadBanners();
  }

  // generate list of times (every 30 minutes, 12h AM/PM)
  generateTimes() {
    const options: string[] = [];

    // 1 AM to 11 AM
    for (let h = 1; h <= 11; h++) {
      const hh = h.toString().padStart(2, '0');
      options.push(`${hh}:00 AM`);
    }

    // 12 NOON
    options.push('12:00 PM');

    // 1 PM to 11 PM
    for (let h = 1; h <= 11; h++) {
      const hh = h.toString().padStart(2, '0');
      options.push(`${hh}:00 PM`);
    }

    // 12 MIDNIGHT
    options.push('12:00 AM');

    this.timeOptions = options;
  }

  loadBanners() {
    this.http.get<any[]>(this.apiUrl).subscribe({
      next: (data) => {
        const now = new Date();

        // auto-hide expired banners
        this.banners = data.filter(b => {
          if (!b.expireAt) return true;
          return new Date(b.expireAt) > now;
        });

        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'Failed to load banners.';
        this.isLoading = false;
      }
    });
  }

  openCreateModal() {
    this.editingBannerId = null;
    this.newBanner = {
      title: '',
      imageUrl: '',
      status: 'draft',
      link: '',
      publishAt: '',
      expireAt: '',
      content: ''
    };

    this.publishDate = null;
    this.publishTime = '09:00 AM';
    this.expireDate = null;
    this.expireTime = '05:00 PM';

    this.showCreateModal = true;
  }

  openEditModal(banner: any) {
    this.editingBannerId = banner.id;
    this.newBanner = {
      title: banner.title,
      imageUrl: banner.imageUrl,
      status: banner.status,
      link: banner.link,
      publishAt: banner.publishAt,
      expireAt: banner.expireAt,
      content: banner.content || ''
    };

    if (banner.publishAt) {
      const d = new Date(banner.publishAt);
      this.publishDate = d;
      this.publishTime = this.formatTime12h(d);
    } else {
      this.publishDate = null;
      this.publishTime = '09:00 AM';
    }

    if (banner.expireAt) {
      const d = new Date(banner.expireAt);
      this.expireDate = d;
      this.expireTime = this.formatTime12h(d);
    } else {
      this.expireDate = null;
      this.expireTime = '05:00 PM';
    }

    this.showCreateModal = true;
  }

  closeCreateModal() {
    if (this.isSaving || this.isDeleting) return;
    this.showCreateModal = false;
    this.editingBannerId = null;
  }

  saveBanner() {
    const publishIso = this.combineDateTime(this.publishDate, this.publishTime);
    const expireIso = this.combineDateTime(this.expireDate, this.expireTime);

    if (!this.newBanner.title || !this.newBanner.imageUrl || !publishIso || !expireIso) {
      alert('Please fill in title, image, publish date/time and expire date/time.');
      return;
    }

    this.newBanner.publishAt = publishIso;
    this.newBanner.expireAt = expireIso;

    this.isSaving = true;

    // UPDATE
    if (this.editingBannerId) {
      this.http.put(`${this.apiUrl}/${this.editingBannerId}`, this.newBanner)
        .subscribe({
          next: () => {
            this.isSaving = false;
            this.showCreateModal = false;
            this.editingBannerId = null;
            this.loadBanners();
          },
          error: (err) => {
            console.error(err);
            this.isSaving = false;
            alert('Failed to update banner.');
          }
        });
      return;
    }

    // CREATE
    this.http.post<any>(this.apiUrl, this.newBanner).subscribe({
      next: (created) => {
        this.banners.unshift(created);
        this.isSaving = false;
        this.showCreateModal = false;
      },
      error: (err) => {
        console.error(err);
        this.isSaving = false;
        alert('Failed to save banner.');
      }
    });
  }

  deleteBanner() {
    if (!this.editingBannerId) return;

    const confirmed = confirm('Delete this banner? This cannot be undone.');
    if (!confirmed) return;

    this.isDeleting = true;

    this.http.delete(`${this.apiUrl}/${this.editingBannerId}`).subscribe({
      next: () => {
        this.isDeleting = false;
        this.showCreateModal = false;
        this.editingBannerId = null;
        this.loadBanners();
      },
      error: (err) => {
        console.error(err);
        this.isDeleting = false;
        alert('Failed to delete banner.');
      }
    });
  }

  openPreviewModal(banner: any) {
    this.previewBanner = banner;
    this.showPreviewModal = true;
  }

  closePreviewModal() {
    this.showPreviewModal = false;
    this.previewBanner = null;
  }

  stripHtml(html: string): string {
    const div = document.createElement('div');
    div.innerHTML = html;
    return div.textContent || div.innerText || '';
  }

  getStatusClass(status: string | undefined): string {
    const s = (status || '').toLowerCase();
    return s === 'published' ? 'published' : 'draft';
  }

  // --- helpers for date/time ---

  private formatTime12h(date: Date): string {
    let h = date.getHours();
    const m = date.getMinutes();
    const ampm = h >= 12 ? 'PM' : 'AM';
    h = h % 12;
    if (h === 0) h = 12;
    const hh = h.toString().padStart(2, '0');
    const mm = m.toString().padStart(2, '0');
    return `${hh}:${mm} ${ampm}`;
  }

  private combineDateTime(date: Date | null, time: string): string | null {
    if (!date || !time) return null;

    const [timePart, ampm] = time.split(' ');
    const [hh, mm] = timePart.split(':').map(v => parseInt(v, 10));

    let hours = hh;
    if (ampm === 'PM' && hours < 12) hours += 12;
    if (ampm === 'AM' && hours === 12) hours = 0;

    const result = new Date(date);
    result.setHours(hours, mm, 0, 0);

    return result.toISOString();
  }
}
