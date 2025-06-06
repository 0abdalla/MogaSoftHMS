import { Component, Input, OnInit } from '@angular/core';
import { DashboardService } from '../../Services/HMS/dashboard.service';
import { Home } from '../../Models/HMS/home';

@Component({
  selector: 'app-overview-card',
  templateUrl: './overview-card.component.html',
  styleUrl: './overview-card.component.css'
})
export class OverviewCardComponent implements OnInit {
  home: Home = new Home();
  selectedMonth: string;
  statisticsCardList: any[] = [
    {
      title: 'عدد المتبرعين',
      number: 56,
      statusIcon: 'fa-arrow-circle-up fas',
      statusBgClass: 'bg-success-gradient',
    },
    {
      title: 'إجمالي الايرادات',
      number: 150000,
      statusIcon: 'fa-arrow-circle-up fas',
      statusBgClass: 'bg-primary-gradient',
    },
    {
      title: 'إجمالي الصادرات',
      number: 25000,
      statusIcon: 'fa-arrow-circle-up fas',
      statusBgClass: 'bg-secondary-gradient',
    }
  ];
  months = [
    { value: 'January', name: 'يناير' },
    { value: 'February', name: 'فبراير' },
    { value: 'March', name: 'مارس' },
    { value: 'April', name: 'أبريل' },
    { value: 'May', name: 'مايو' },
    { value: 'June', name: 'يونيو' },
    { value: 'July', name: 'يوليو' },
    { value: 'August', name: 'أغسطس' },
    { value: 'September', name: 'سبتمبر' },
    { value: 'October', name: 'أكتوبر' },
    { value: 'November', name: 'نوفمبر' },
    { value: 'December', name: 'ديسمبر' }
  ];

  constructor(private dashboardService: DashboardService) {
    const currentMonth = new Date().getMonth();
    this.selectedMonth = this.months[currentMonth].value;
    this.getDataForHome(this.selectedMonth);
  }

  getDataForHome(month: string) {
    this.dashboardService.getHome(month).subscribe((res: Home) => {
      this.home = res.results;
    })
  }

  ngOnInit(): void {

  }

}
