import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

export interface Widget {
  id: number;
  title: string;
  number: number;
  icon: string;
}

@Component({
  selector: 'app-widget',
  standalone: true,
  imports:[CommonModule],
  templateUrl: './widgets.html',
  styleUrls: ['./widgets.scss']
})
export class WidgetComponent {
  @Input() data!: Widget; // <-- needed for [data] binding
}
