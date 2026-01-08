import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PageHeader } from 'multi-service-automotive-ecosystem-components';

@Component({
  selector: 'app-home',
  imports: [CommonModule, RouterLink, PageHeader],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {
}
