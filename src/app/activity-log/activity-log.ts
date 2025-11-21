import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

type ActivityStatus = 'Success' | 'Complete' | 'Failed' | string;

type ContentFilter = 'all' | 'page' | 'post' | 'banner' | 'popup' | 'system';

interface ActivityLogEntry {
  timestamp: string;      // ISO string or already formatted
  user: string;
  action: string;
  contentType: ContentFilter;
  contentAffected: string;
  contentId: string;
  status: ActivityStatus;
}

@Component({
  selector: 'app-activity-log',
  standalone: true,

  // ⭐ FIXED: usa ra ka "imports" ug gi-apil ang FormsModule
  imports: [CommonModule, FormsModule],

  templateUrl: './activity-log.html',
  styleUrls: ['./activity-log.scss'],
})
export class ActivityLogComponent {
  // current filter (dropdown)
  contentFilter: ContentFilter = 'all';

  // options for dropdown
  filterOptions: { value: ContentFilter; label: string }[] = [
    { value: 'all', label: 'All contents' },
    { value: 'page', label: 'Pages' },
    { value: 'post', label: 'Posts' },
    { value: 'banner', label: 'Banners' },
    { value: 'popup', label: 'Popups' },
    { value: 'system', label: 'System' },
  ];

  // mock data – later pwede nato ilisan ug API call
  activityLog: ActivityLogEntry[] = [
    {
      timestamp: '2025-10-29T08:32:00',
      user: 'Jane Doe',
      action: 'Updated',
      contentType: 'page',
      contentAffected: 'Page - “About Us”',
      contentId: 'page_001',
      status: 'Success',
    },
    {
      timestamp: '2025-10-29T09:05:00',
      user: 'Lawrence',
      action: 'Changed Banner Image',
      contentType: 'banner',
      contentAffected: 'Banner - “Hero_Banner.jpg”',
      contentId: 'banner_003',
      status: 'Success',
    },
    {
      timestamp: '2025-10-29T08:32:00',
      user: 'JaMes',
      action: 'Archived',
      contentType: 'post',
      contentAffected: 'Post - “Car Expo 2023”',
      contentId: 'post_014',
      status: 'Success',
    },
    {
      timestamp: '2025-10-29T08:32:00',
      user: 'Caral',
      action: 'Published',
      contentType: 'post',
      contentAffected: 'Post - “Tips for Car Maintenance”',
      contentId: 'post_015',
      status: 'Success',
    },
    {
      timestamp: '2025-10-29T08:32:00',
      user: 'Alexia',
      action: 'Added Popup',
      contentType: 'popup',
      contentAffected: 'Popup - “Subscribe to Newsletter”',
      contentId: 'popup_006',
      status: 'Success',
    },
    {
      timestamp: '2025-10-29T08:32:00',
      user: 'Chloe',
      action: 'Auto Backup',
      contentType: 'system',
      contentAffected: 'Database Snapshot',
      contentId: 'sys_20251029',
      status: 'Complete',
    },
    {
      timestamp: '2025-10-29T08:32:00',
      user: 'Immanuel',
      action: 'Restored',
      contentType: 'post',
      contentAffected: 'Archived Post - “Promo 2024”',
      contentId: 'post_009',
      status: 'Success',
    },
  ];

  // computed list para sa table
  get filteredLog(): ActivityLogEntry[] {
    if (this.contentFilter === 'all') {
      return this.activityLog;
    }
    return this.activityLog.filter(
      (entry) => entry.contentType === this.contentFilter
    );
  }

  // formatting helper para timestamp column
  formatTimestamp(iso: string): string {
    const d = new Date(iso);
    if (isNaN(d.getTime())) return iso;

    return d.toLocaleString('en-US', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      hour12: true,
    });
  }

  // optional: status CSS class (for color coding later)
  getStatusClass(status: ActivityStatus): string {
    const s = status.toLowerCase();
    if (s === 'success') return 'status-pill success';
    if (s === 'complete') return 'status-pill complete';
    if (s === 'failed' || s === 'error') return 'status-pill failed';
    return 'status-pill';
  }
}
