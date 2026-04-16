import { Component, ViewChild } from '@angular/core';
import { PopoverModule, Popover } from 'primeng/popover';

@Component({
  selector: 'app-grup-menu-component',
  standalone: true,
  imports: [PopoverModule],
  templateUrl: './grup-menu-component.html',
  styleUrl: './grup-menu-component.css',
})
export class GrupMenuComponent {
  @ViewChild('op') op!: Popover;
}
