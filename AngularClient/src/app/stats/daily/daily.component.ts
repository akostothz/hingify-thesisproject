import { Component, OnInit } from '@angular/core';
import { Chart, ChartOptions, ChartConfiguration } from 'chart.js/auto';

@Component({
  selector: 'app-daily',
  templateUrl: './daily.component.html',
  styleUrls: ['./daily.component.css']
})
export class DailyComponent implements OnInit {
  day: string;

  constructor() {

  }

  ngOnInit(): void {
    this.getToday();
    this.drawChart();
  }

  getToday() {
    var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    var d = new Date(Date.now()); 
    this.day = days[d.getDay()];
  }

  drawChart(): void {
    const ctx = document.getElementById('myChart') as HTMLCanvasElement;
    const options: ChartOptions<'bar'> = {
      plugins: {
        legend: {
          display: false,
        },
      },
      scales: {
        x: {
          ticks: {
            color: 'white',
          },
          grid: {
            color: 'white',
          },
        },
        y: {
          ticks: {
            color: 'white',
          },
          grid: {
            color: 'white',
          },
        },
      },
      color: 'white',
    };

    const chartConfig: ChartConfiguration<'bar'> = {
      type: 'bar',
      data: {
        labels: ['11.13. (Monday)', '11.14. (Tuesday)', '11.15. (Wednesday)', '11.16. (Thursday)', '11.17. (Friday)', '11.18. (Saturday)', '11.19. (Sunday)'],
        datasets: [{
          label: 'Minutes spent on listening to music',
          data: [73, 94, 167, 15, 44, 87, 104],
          backgroundColor: '#F0E199'
        }],
      },
      options: options,
    };

    const myChart = new Chart(ctx.getContext('2d'), chartConfig);
  }
}
