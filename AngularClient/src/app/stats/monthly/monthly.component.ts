import { Component, OnInit } from '@angular/core';
import { Chart, ChartOptions, ChartConfiguration } from 'chart.js/auto';

@Component({
  selector: 'app-monthly',
  templateUrl: './monthly.component.html',
  styleUrls: ['./monthly.component.css']
})
export class MonthlyComponent implements OnInit {
  constructor() {

  }

  ngOnInit(): void {
    this.drawChart();
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
          backgroundColor: '#ebcf45'
        }],
      },
      options: options,
    };

    const myChart = new Chart(ctx.getContext('2d'), chartConfig);
  }
}
