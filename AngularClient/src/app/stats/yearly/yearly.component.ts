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

  last7daysDays: string[] = [];
  last7daysMins: number[] = [];

  constructor(private musicService: MusicsService) {

  }

  async ngOnInit(): Promise<void> {
    await this.getStatistics();
    this.drawChart();
  }
  
  async getStatistics(): Promise<void> {
    
    const userId = JSON.parse(localStorage.getItem('user'))?.id;

    try {
      this.last7daysDays = await this.musicService.getLast7Days(userId).toPromise();
      this.last7daysMins = await this.musicService.getLast7DaysMins(userId).toPromise();
    } catch (error) {
      console.error('Error fetching data:', error);
    }     
      
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
        labels: this.last7daysDays,
        datasets: [{
          label: 'Minutes spent on listening to music',
          data: this.last7daysMins,
          backgroundColor: '#ebcf45'
        }],
      },
      options: options,
    };

    const myChart = new Chart(ctx.getContext('2d'), chartConfig);
  }
}
