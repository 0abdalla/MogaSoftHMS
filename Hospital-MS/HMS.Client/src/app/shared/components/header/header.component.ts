import { Component } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  currentDate: string = '';
  userProfileImage: string = 'assets/vendors/imgs/avatar.png';
  searchQuery: string = '';

  constructor() { }

  ngOnInit(): void {
    this.setCurrentDate();
  }

  setCurrentDate(): void {
    const today = new Date();
    const day = today.getDate();
    const month = today.toLocaleString('ar-EG', { month: 'short' });
    const weekday = today.toLocaleString('ar-EG', { weekday: 'short' });
    this.currentDate = `اليوم، ${weekday} ${day} ${month}`;
  }

  onSearch(): void {
    console.log('Search query:', this.searchQuery);
  }
}
