import { Component } from '@angular/core';
import { Chart } from 'angular-highcharts';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('200ms ease-in', style({ opacity: 1 })),
      ]),
      transition(':leave', [
        animate('200ms ease-out', style({ opacity: 0 })),
      ])
    ])
  ],
})
export class HomeComponent {
  
  appointmentNumsChart = new Chart({ 
    chart: {
      type: 'areaspline',
      renderTo: 'appointments-line',
      style: {
        fontFamily: 'Tajawal'
      }
    },
    title: {
      text: 'أعداد الحجوزات',
      align: 'right',
      margin:0,
      style:{
        fontFamily:"Tajawal",
        fontSize: '20px',
        fontWeight: 'bold',
        color:'Black'
      }
    },
    subtitle: {
      text: 'مقارنة بين الشهر الحالي والماضي',
      align: 'right',
      style:{
        fontFamily:"Tajawal",
        fontSize: '16px',
        fontWeight: '500',
        color:'#7C8FAC',
      }
    },
    xAxis: {
      categories: ['10/04', '13/04', '16/04', '19/04', '22/04', '25/04', '28/04'],
      reversed: true
    },
    yAxis: {
      title: { text: '' }
    },
    tooltip: {
      shared: true
    },
    plotOptions: {
      areaspline: {
        fillOpacity: 0.3
      }
    },
    series: [
      {
        name: 'الشهر الحالي',
        data: [45, 80, 65, 90, 60, 70, 100],
        type: 'areaspline',
        color: '#3D5DA7'
      },
      {
        name: 'الشهر الماضي',
        data: [30, 40, 35, 55, 45, 50, 75],
        type: 'areaspline',
        color: '#ED3B93'
      }
    ],
    legend: {
      events: {
        itemClick: function (event) {
          return false;
        },
      },
    },
    credits: {
      enabled: false
    }
  });

  completedAppsChart = new Chart({
    chart: {
        type: 'pie',
        renderTo: 'appointments-donut',
        backgroundColor: 'transparent',
    },
    title: {
        text: 'المواعيد المنجزة',
        align: 'center',
        verticalAlign: 'top',
        style: {
            fontSize: '23px',
            fontWeight: 'bold',
            fontFamily: 'Tajawal, sans-serif',
            color: 'Black',
        }
    },
    subtitle: {
        text: '<span style="font-size: 40px; font-weight: bold; color: #5A6A85; font-family: Roboto, sans-serif;">77%</span>',
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
                y: 77,
                color: '#3D5DA7'
            },
            {
                name: 'لم يتم إنجازها',
                y: 23,
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
  });

  doctorsChart = new Chart({
    chart: {
      type: 'bar',
      renderTo: 'doctors-stacked-bar',
      backgroundColor: 'transparent'
    },
    title: {
      text: 'أفضل الأطباء',
      align: 'right',
      style: {
        fontSize: '20px',
        fontWeight: 'bold',
        fontFamily: 'Tajawal, sans-serif',
        color: 'Black',
      }
    },
    subtitle:{
      text: 'عدد الحالات اللتي أنجزها الطبيب',
      align: 'right',
      style:{
        fontSize: '16px',
        fontWeight: 'bold',
        fontFamily: 'Tajawal, sans-serif',
        color: '#7C8FAC',
      }
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
    
    series: [
      {
        name: 'د. أحمد',
        type: 'bar',
        data: [5, 7, 6, 5],
        color: '#D94A91',
        
      },
      {
        name: 'د. ندى',
        type: 'bar',
        data: [3, 5, 4, 6],
        color: '#435AA0'
      },
      {
        name: 'د. سالم',
        type: 'bar',
        data: [7, 3, 5, 4],
        color: '#70A1FF'
      }
    ]
  });

  clinicChart = new Chart({
    chart: {
      type: 'column'
    },
    title: {
      text: 'توزيع ضغط الحجوزات',
      align: 'right',
      style:{
        fontFamily:"Tajawal",
        fontSize: '20px',
        fontWeight: 'bold',
        color:'Black'
      }
    },
    subtitle:{
      text: 'في هذا القسم يتم حساب ضغط الحجوزات على مدار الشهر الحالى على خدماتك الطبية ',
      align: 'right',
      style:{
        fontSize: '16px',
        fontWeight: '500',
        fontFamily: 'Tajawal, sans-serif',
        color:'#7C8FAC',
      }
    },
    xAxis: {
      categories: [
        'العظام',
        'الباطنة',
        'الجلدية',
        'الأسنان',
        'القلب',
        'النساء',
        'الأطفال',
        'تحاليل دم',
        'تحاليل بول',
        'أشعة سينية',
        'أشعة رنين',
        'عيادة تغذية'
      ],
      labels:{
        style: {
          fontFamily: 'Tajawal, sans-serif',
          color:"#A1A7C4",
          fontSize:'12px',
          fontWeight:'500'
        }
      }
    },
    yAxis: {
      min: 0,
      labels:{
        style: {
          fontFamily: 'Tajawal, sans-serif',
          color:"#A1A7C4",
          fontSize:'12px',
          fontWeight:'500'
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
      data: [320, 250, 180, 440, 350, 270, 190, 220, 180, 300, 280, 240],
      color: '#435AA0',
      
    }],
    legend: {
      enabled:false
    },
    credits: {
      enabled: false
    }
  });
}
