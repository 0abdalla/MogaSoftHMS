import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MenuSidebarItem } from '../../../Models/Generics/MenuSidebarItem';
import { MenueService, MenuType } from '../../../Services/Generics/menue.service';
import { Home } from '../../../Models/HMS/home';
import { DashboardService } from '../../../Services/HMS/dashboard.service';
import { Chart } from 'angular-highcharts';

@Component({
  selector: 'app-hms-home',
  templateUrl: './hms-home.component.html',
  styleUrl: './hms-home.component.css'
})
export class HmsHomeComponent implements OnInit {
  selectedTabName: string;
  menuItem: MenuSidebarItem;
  today = new Date();
  isToggle = false;
  home: Home = new Home();
  topDoctors: any[] = [];
  medicalServices: any[] = [];
  selectedMonth: string;
  selectedYear: string;
  appointmentNumsChart: Chart
  completedAppsChart: Chart
  doctorsChart: Chart
  clinicChart: Chart
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

  constructor(private dashboardService: DashboardService, private route: ActivatedRoute, private menuService: MenueService) {
    const currentMonth = new Date().getMonth();
    this.selectedMonth = this.months[currentMonth].value;
    this.selectedYear = new Date().getFullYear().toString();
    this.getDataForHome(this.selectedMonth);
    this.getTopDoctors(this.selectedMonth);
    this.getMedicalServices();
  }

  ngOnInit(): void {

    this.route.params.subscribe(params => {
      this.menuItem = null;
      if (params['tabName']) {
        this.selectedTabName = params['tabName'];
        this.menuItem = this.menuService.getMenuById(MenuType.ZAInstitution, this.selectedTabName);
      }
    });
  }

  onToggleContent() {
    this.isToggle = !this.isToggle;
    const htmlElement = document.querySelector('html');
    if (this.isToggle) {
      htmlElement.style.cssText = `overflow: hidden`;
    } else {
      htmlElement.style.cssText = `overflow: auto`;
    }
  }

  onOverlayClicked() {
    this.isToggle = false;
    const htmlElement = document.querySelector('html');
    htmlElement.style.cssText = `overflow: auto`;
  }

  getDataForHome(month: string) {
    this.dashboardService.getHome(month).subscribe((res: Home) => {
      this.home = res.results;
      this.updateChart();
    })
  }
  getTopDoctors(month: string) {
    this.dashboardService.getTopDoctors(month).subscribe((res: any) => {
      this.topDoctors = res;
      this.updateChart();
    })
  }
  getMedicalServices() {
    this.dashboardService.getMedicalServices().subscribe((res: any) => {
      this.medicalServices = res;
      this.updateChart();
    })
  }
  onMonthChange(event: any) {
    this.selectedMonth = event.target.value;
    this.getDataForHome(this.selectedMonth);
    this.getTopDoctors(this.selectedMonth);
  }

  updateChart() {
    const currentMonth = this.home.currentMonthAppointments;
    const previousMonth = this.home.previousMonthAppointments;
    const categories = Object.keys(currentMonth)
    const currentMonthData = categories.map((date) => currentMonth[date]);
    const previousMonthData = Object.keys(previousMonth)
      .map((date) => previousMonth[date]);
    // 
    const completedAppointmentsRate = this.home.completedAppointmentsRate;
    // 
    const topDoctorsData = this.topDoctors[0]?.topDoctors || [];
    const seriesData = topDoctorsData.map(doctor => {
      return {
        name: doctor.doctorName,
        type: 'bar',
        data: [
          doctor.weeklyActivityCounts['Week 1'] || 0,
          doctor.weeklyActivityCounts['Week 2'] || 0,
          doctor.weeklyActivityCounts['Week 3'] || 0,
          doctor.weeklyActivityCounts['Week 4'] || 0
        ],
        color: this.getRandomColor()
      };
    });
    const doctorColors = ['#D94A91', '#435AA0', '#70A1FF'];
    // 
    const medicalServicesData = this.medicalServices || [];
    const servicesArray = Object.keys(medicalServicesData).map(key => ({
      name: key,
      value: medicalServicesData[key]
    }));
    servicesArray.sort((a, b) => b.value - a.value);
    const serviceCategories = servicesArray.map(service => service.name);
    const serviceData = servicesArray.map(service => service.value);


    this.appointmentNumsChart = new Chart({
      chart: {
        type: 'areaspline',
        renderTo: 'appointments-line',
        style: {
          fontFamily: 'Tajawal',
        },
      },
      accessibility: {
        enabled: false
      },
      title: {
        text: '',

      },
      subtitle: {
        text: ' ',

      },
      xAxis: {
        categories: categories.reverse(),
        reversed: true,
      },
      yAxis: {
        title: { text: '' },
      },
      tooltip: {
        shared: true,
      },
      plotOptions: {
        areaspline: {
          fillOpacity: 0.3,
        },
      },
      series: [
        {
          name: 'الشهر الحالي',
          data: currentMonthData.reverse(),
          type: 'areaspline',
          color: '#3D5DA7',
        },
        {
          name: 'الشهر الماضي',
          data: previousMonthData.reverse(),
          type: 'areaspline',
          color: '#ED3B93',
        },
      ],
      legend: {
        events: {
          itemClick: function (event) {
            return false;
          },
        },
      },
      credits: {
        enabled: false,
      },
    });

    this.completedAppsChart = new Chart({
      accessibility: {
        enabled: false
      },
      chart: {
        type: 'pie',
        renderTo: 'appointments-donut',
        backgroundColor: 'transparent',
      },
      title: {
        text: '',

      },
      subtitle: {
        text: '<span style="font-size: 40px; font-weight: bold; color: #5A6A85; font-family: Roboto, sans-serif;">' + completedAppointmentsRate + '%</span>',
        floating: true,
        useHTML: true,
        align: 'center',
        verticalAlign: 'middle',
        y: 25
      },
      tooltip: {
        pointFormat: '<b>{point.percentage:.1f}%</b>'
      },
      plotOptions: {
        pie: {
          innerSize: '80%',
          dataLabels: {
            enabled: false
          },
          borderWidth: 0
        }
      },
      series: [{
        name: 'نسبة الانجاز',
        type: 'pie',
        data: [
          {
            name: 'تم إنجازها',
            y: completedAppointmentsRate,
            color: '#3D5DA7'
          },
          {
            name: 'لم يتم إنجازها',
            y: 100 - completedAppointmentsRate,
            color: '#ED3B93'
          }
        ]
      }],
      credits: { enabled: false },
      legend: {
        events: {
          itemClick: function (event) {
            return false;
          },
        },
      }
    })

    this.doctorsChart = new Chart({
      accessibility: {
        enabled: false
      },
      chart: {
        type: 'bar',
        renderTo: 'doctors-stacked-bar',
        backgroundColor: 'transparent'
      },
      title: {
        text: '',
      },
      subtitle: {
        text: '',
      },
      xAxis: {
        categories: ['الأسبوع الأول', 'الأسبوع الثاني', 'الأسبوع الثالث', 'الأسبوع الرابع'],
        title: { text: null },
        labels: {
          style: {
            fontFamily: 'Tajawal, sans-serif'
          }
        }
      },
      yAxis: {
        min: 0,
        title: {
          text: '',
          align: 'high'
        },
        labels: {
          overflow: 'justify',
          style: {
            fontFamily: 'Tajawal, sans-serif'
          }
        }
      },
      tooltip: {
        shared: true,
        valueSuffix: 'مرضى'
      },
      plotOptions: {
        series: {
          stacking: 'normal'
        },
        bar: {
          pointWidth: 35,
          dataLabels: {
            enabled: true,
            style: {
              fontFamily: 'Tajawal, sans-serif',
              color: '#fff',
              fontSize: '16px'
            }
          }
        }
      },
      legend: {
        layout: 'horizontal',
        align: 'center',
        verticalAlign: 'bottom',
        itemStyle: {
          fontFamily: 'Tajawal, sans-serif',
          fontSize: '16px',
          color: '#7C8FAC'
        },
        events: {
          itemClick: function (event) {
            return false;
          },
        },
      },
      credits: {
        enabled: false
      },
      series: topDoctorsData.map((doctor, index) => ({
        name: doctor.doctorName,
        type: 'bar',
        data: [
          doctor.weeklyActivityCounts['Week 1'] || 0,
          doctor.weeklyActivityCounts['Week 2'] || 0,
          doctor.weeklyActivityCounts['Week 3'] || 0,
          doctor.weeklyActivityCounts['Week 4'] || 0
        ],
        color: doctorColors[index] || this.getRandomColor()
      }))
    });

    this.clinicChart = new Chart({
      chart: {
        type: 'column'
      },
      accessibility: {
        enabled: false
      },
      title: {
        text: 'توزيع ضغط الحجوزات',
        align: 'right',
        style: {
          fontFamily: "Tajawal",
          fontSize: '20px',
          fontWeight: 'bold',
          color: 'Black'
        }
      },
      subtitle: {
        text: 'في هذا القسم يتم حساب ضغط الحجوزات على مدار الشهر الحالى على خدماتك الطبية',
        align: 'right',
        style: {
          fontSize: '16px',
          fontWeight: '500',
          fontFamily: 'Tajawal, sans-serif',
          color: '#7C8FAC',
        }
      },
      xAxis: {
        categories: serviceCategories, // Use dynamic categories from API
        labels: {
          style: {
            fontFamily: 'Tajawal, sans-serif',
            color: "#A1A7C4",
            fontSize: '12px',
            fontWeight: '500'
          }
        }
      },
      yAxis: {
        min: 0,
        labels: {
          style: {
            fontFamily: 'Tajawal, sans-serif',
            color: "#A1A7C4",
            fontSize: '12px',
            fontWeight: '500'
          }
        },
        title: {
          text: ''
        }
      },
      tooltip: {
        pointFormat: 'عدد الحجوزات: <b>{point.y}</b>'
      },
      plotOptions: {
        column: {
          borderRadius: 8,
          pointPadding: 0.2,
          borderWidth: 0
        }
      },
      series: [{
        name: 'الحجوزات',
        type: 'column',
        data: serviceData, // Use dynamic data from API
        color: '#435AA0',
      }],
      legend: {
        enabled: false
      },
      credits: {
        enabled: false
      }
    });
  }

  getRandomColor() {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }
}
