import { Component, OnInit } from '@angular/core';
import { Chart, ChartOptions, ChartConfiguration } from 'chart.js/auto';
import { Observable } from 'rxjs';
import { MusicsService } from 'src/app/_services/musics.service';

@Component({
  selector: 'app-yearly',
  templateUrl: './yearly.component.html',
  styleUrls: ['./yearly.component.css']
})
export class YearlyComponent implements OnInit {

  last7daysDays$: Observable<String[]> | undefined;
  last7daysMins$: Observable<number[]> | undefined;

  constructor(private musicService: MusicsService) {

  }

  ngOnInit(): void {
    this.getStatistics();
    this.drawChart();
  }
  
  getStatistics() {
    this.last7daysDays$ = this.musicService.getLast7Days(JSON.parse(localStorage.getItem('user'))?.id);
    this.last7daysMins$ = this.musicService.getLast7DaysMins(JSON.parse(localStorage.getItem('user'))?.id);
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
