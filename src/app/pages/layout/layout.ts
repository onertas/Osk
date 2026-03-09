import { Component } from '@angular/core';
import { Navbar } from '../navbar/navbar';
import { MainSidebar } from '../main-sidebar/main-sidebar';
import { ControlSidebar } from '../control-sidebar/control-sidebar';
import { Footer } from '../footer/footer';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-layout',
  imports: [Navbar, MainSidebar, ControlSidebar, Footer,RouterOutlet],
  standalone: true,
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout {

}
