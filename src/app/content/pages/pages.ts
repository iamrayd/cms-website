import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

interface PageItem {
  title: string;
  slug: string;
  description: string;
  status: 'draft' | 'published';
}

@Component({
  selector: 'app-pages',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pages.html',
  styleUrls: ['./pages.scss']
})
export class PagesComponent implements OnInit {
  pages: PageItem[] = [];
  isLoading = true;

  ngOnInit(): void {
    // TEMPORARY DATA (sample lang sa karon)
    this.pages = [
      {
        title: 'Home Page',
        slug: 'home',
        description: 'Main landing page of the website.',
        status: 'published'
      },
      {
        title: 'About Us',
        slug: 'about',
        description: 'Company background and mission.',
        status: 'published'
      },
      {
        title: 'Contact',
        slug: 'contact',
        description: 'Contact form and details.',
        status: 'draft'
      }
    ];

    this.isLoading = false;
  }
}
