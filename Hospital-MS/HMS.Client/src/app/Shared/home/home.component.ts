import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  Lang = 'en';
  UserModel: any;
  customerApplications: any[] = [];
  systemURL: string; //environment.systemUrl;

  breakpoints: any = {
    '0': {
      slidesPerView: 1
    },
    '575': {
      slidesPerView: 2
    },
    '767': {
      slidesPerView: 3
    },
    '1024': {
      slidesPerView: 4
    },
    '1200': {
      slidesPerView: 5
    },
    '1376': {
      slidesPerView: 6
    }
  };
  constructor() {

  }
  ngOnInit(): void {
    // this.UserModel = this.authService.getCurrentUser();
    this.createClock();
  }

  // ngOnInit(): void {
  //   this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
  //   this.Lang = localStorage.getItem('lang') ?? 'en';
  //   this.getCustomerApplications();

  // }

  GoToProductModule(App: any) {
    // this.router.navigateByUrl(App.redirectUri);
  }
  intervalClock;
  time = new Date();
  clock: string;
  createClock() {
    this.intervalClock = setInterval(() => {
      this.time = new Date();
      this.clock = this.time.getHours() + ':' + (this.time.getMinutes() < 10 ? '0' : '') + this.time.getMinutes()
    }, 1000);
  }

  // getCustomerApplications() {
  //   this._MainService.getCustomerApplications().subscribe((data: CustomerApplicationModel[]) => {
  //     this.customerApplications = data;
  //   }, (error) => {
  //   }, () => {

  //   })
  // }
}
