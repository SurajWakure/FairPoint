import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Sidebar } from '../sidebar/sidebar.component';
import { Header } from '../header/header.component';

@Component({
  selector: 'app-layout',
  imports: [
    RouterOutlet,
    Sidebar,
    Header
  ],
  templateUrl: './app-layout.html',
  styleUrl: './app-layout.scss'
})
export class AppLayout {

}