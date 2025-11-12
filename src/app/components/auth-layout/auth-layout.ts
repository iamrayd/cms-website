import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from '../header/header';

@Component({
  selector: 'app-auth-layout',
  standalone: true,
  imports: [RouterOutlet, Header],
  template: `
    <app-header />
    <main class="auth-main">
      <router-outlet />
    </main>
  `,
  styles: [`
    .auth-main {
      min-height: calc(100vh - 90px); 
    }
  `]
})
export class AuthLayoutComponent {}