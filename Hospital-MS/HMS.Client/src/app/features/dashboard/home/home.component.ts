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
        fontFamily: 'Cairo, sans-serif'
      }
    },
    title: {
      text: 'اعداد المواعيد',
      align: 'right',
      margin:0
    },
    subtitle: {
      text: 'مقارنة بين العيادات الداخلية والخارجية',
      align: 'right',
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
      events: {
        render: function () {
          const chart: any = this
          const percentage = '77%';

          if (chart.customLabel) {
            chart.customLabel.destroy();
          }

          const centerX = chart.plotLeft + chart.plotWidth / 2;
          const centerY = chart.plotTop + chart.plotHeight / 2;
          chart.customLabel = chart.renderer.text(
            percentage,
            centerX,
            centerY
          )
          .css({
            color: '#3D5DA7',
            fontSize: '40px',
            fontWeight: 'bold',
            textAnchor: 'middle'
          })
          .attr({
            align: 'center'
          })
          .add();

          const bbox = chart.customLabel.getBBox();
          chart.customLabel.attr({
            y: centerY - bbox.height / 2
          });
        }
      }
    },
    title: {
      text: 'المواعيد المنجزة',
      align: 'center',
      verticalAlign: 'top',
      style: {
        fontSize: '16px',
        fontWeight: 'bold',
        fontFamily: 'Cairo, sans-serif'
      }
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
      layout: 'horizontal',
      align: 'center',
      verticalAlign: 'bottom',
      symbolRadius: 0,
      itemStyle: {
        fontFamily: 'Cairo, sans-serif',
        fontSize: '13px'
      },
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
        fontFamily: 'Cairo, sans-serif'
      }
    },
    subtitle:{
      text: 'عدد الحالات اللتي أنجزها الطبيب',
      align: 'right',
    },
    xAxis: {
      categories: ['يناير', 'فبراير', 'مارس', 'أبريل'],
      title: { text: null },
      labels: {
        style: {
          fontFamily: 'Cairo, sans-serif'
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
          fontFamily: 'Cairo, sans-serif'
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
        dataLabels: {
          enabled: true,
          style: {
            fontFamily: 'Cairo, sans-serif'
          }
        }
      }
    },
    legend: {
      layout: 'horizontal',
      align: 'center',
      verticalAlign: 'bottom',
      itemStyle: {
        fontFamily: 'Cairo, sans-serif',
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
        color: '#7D8EFF'
      },
      {
        name: 'د. ندى',
        type: 'bar',
        data: [3, 5, 4, 6],
        color: '#EF7DA0'
      },
      {
        name: 'د. سالم',
        type: 'bar',
        data: [7, 3, 5, 4],
        color: '#60D7B1'
      }
    ]
  });

  appointmentsChart = new Chart({
    chart: {
      type: 'pie',
    },
    title: {
      text: 'نسبة إنجاز المواعيد'
    },
    series: [
      {
        type: 'pie',
        name: 'عدد المواعيد',
        data: [
          ['مكتملة', 55],
          ['فائتة', 15],
          ['ملغاة', 10],
          ['أُعيد جدولتها', 20]
        ],
        colors: ['green', 'lightgrey', 'red', 'orange']
      }
    ],
    accessibility: {
      point: {
        valueSuffix: '%'
      }
    },
    credits: {
      enabled: false
    },
    legend: {
      events: {
        itemClick: function (event) {
          return false;
        },
      },
    }
  });

  growthChart = new Chart({
    chart: {
      type: 'area'
    },
    title: {
      text: 'نمو المواعيد شهريًا'
    },
    xAxis: {
      categories: ['يناير', 'فبراير', 'مارس', 'أبريل', 'مايو', 'يونيو'],
      tickmarkPlacement: 'on',
    },
    yAxis: {
      title: {
        text: 'العدد / الإيرادات'
      }
    },
    tooltip: {
      shared: true,
      valueSuffix: ''
    },
    series: [
      {
        name: 'عدد المواعيد',
        type: 'area',
        data: [120, 135, 150, 170, 200, 230],
        color: 'var(--main-color)'
      },
      {
        name: 'الإيرادات',
        type: 'area',
        data: [10000, 12000, 14000, 17000, 21000, 26000],
        color: 'var(--second-color)'
      }
    ],
    credits: {
      enabled: false
    },
    legend: {
      events: {
        itemClick: function (event) {
          return false;
        },
      },
    },
  });

  departmentChart = new Chart({
    chart: {
      type: 'bar'
    },
    title: {
      text: 'توزيع ضغط العمل على الأقسام',
    },
    xAxis: {
      categories: ['الأسنان', 'الجلدية', 'القلب', 'العظام', 'النساء والولادة'],
      title: {
        text: 'الأقسام',
        
      }
    },
    yAxis: {
      min: 0,
      title: {
        text: 'عدد الحالات',
        
      }
    },
    series: [
      {
        name: 'عدد الحالات',
        type: 'bar',
        color: 'var(--main-color)',
        data: [120, 80, 50, 65, 90]
      }
    ],
    credits: {
      enabled: false
    },
    legend: {
      events: {
        itemClick: function (event) {
          return false;
        },
      },
    }
  });
  
  doctorChart = new Chart({
    chart: {
      type: 'column'
    },
    title: {
      text: 'أفضل الأطباء أداءً'
    },
    xAxis: {
      categories: ['د. أحمد', 'د. سارة', 'د. كريم', 'د. فاطمة', 'د. ياسين'],
      crosshair: true
    },
    yAxis: {
      min: 0,
      title: {
        text: 'عدد المواعيد / التقييم'
      }
    },
    tooltip: {
      shared: true
    },
    series: [
      {
        name: 'عدد المواعيد',
        type: 'column',
        data: [80, 95, 70, 60, 100],
        color: 'var(--main-color)'
      },
      {
        name: 'متوسط التقييم',
        type: 'column',
        data: [4.2, 4.8, 4.0, 3.9, 4.5],
        color: 'var(--second-color)'
      }
    ],
    credits: {
      enabled: false
    },
    legend: {
      events: {
        itemClick: function (event) {
          return false;
        },
      },
    }
  });

  clinicChart = new Chart({
    chart: {
      type: 'column'
    },
    title: {
      text: 'توزيع ضغط الحجوزات',
      align: 'right',
      style: {
        fontSize: '20px',
        fontWeight: 'bold',
        fontFamily: 'Cairo, sans-serif'
      }
    },
    xAxis: {
      categories: ['العظام', 'الباطنة', 'الجلدية', 'الأسنان', 'القلب', 'النساء', 'الأطفال'],
    },
    yAxis: {
      min: 0,
      title: {
        text: 'عدد الحجوزات'
      }
    },
    tooltip: {
      pointFormat: 'عدد الحجوزات: <b>{point.y}</b>'
    },
    series: [{
      name: 'الحجوزات',
      type: 'column',
      data: [320, 250, 180, 440, 350, 270, 190],
      color: '#3f51b5'
    }],
    legend: {
      enabled:false
    },
    credits: {
      enabled: false
    }
  });
}
