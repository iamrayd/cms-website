import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-content-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule
  ],
  templateUrl: './content.html',
  styleUrls: ['./content.scss']
})
export class ContentListComponent {
  activeTab: 'pages' | 'posts' | 'banners' = 'pages';
  searchTerm = '';

  contents = [
    { title: 'Page 1', description: 'This is a sample page description.', status: 'Published', tags: ['<h1>', '<p>'] },
    { title: 'Post 1', description: 'This is a sample post description.', status: 'Draft', tags: ['<p>'] },
    { title: 'Banner 1', description: 'Banner description', status: 'Archived', tags: ['<a>'] },
    { title: 'Page 2', description: 'Another page description.', status: 'Published', tags: ['<h1>'] },
  ];

  setTab(tab: 'pages' | 'posts' | 'banners') {
    this.activeTab = tab;
  }

  goToForm() {
    console.log('Go to form for', this.activeTab);
  }

  editItem(item: any) {
    console.log('Edit item', item);
  }

  get displayedContents() {
    return this.contents.filter(item => {
      if (this.activeTab === 'pages') return item.title.toLowerCase().includes('page');
      if (this.activeTab === 'posts') return item.title.toLowerCase().includes('post');
      if (this.activeTab === 'banners') return item.title.toLowerCase().includes('banner');
      return true;
    });
  }
}
