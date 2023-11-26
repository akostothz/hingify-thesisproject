import { Component, OnInit } from '@angular/core';
import { Chart, ChartOptions, ChartConfiguration } from 'chart.js/auto';
import { Observable } from 'rxjs';
import { Stat } from 'src/app/_models/stat';
import { MusicsService } from 'src/app/_services/musics.service';

@Component({
  selector: 'app-daily',
  templateUrl: './daily.component.html',
  styleUrls: ['./daily.component.css']
})
export class DailyComponent implements OnInit {
  day: string;
  last7daysDays: string[] = [];
  last7daysMins: number[] = [];
  dailyStats$: Observable<Stat[]> | undefined;

  constructor(private musicService: MusicsService) {

  }

  async ngOnInit(): Promise<void> {
    this.getToday();
    await this.getStatistics();
    this.drawChart();
  }

  getToday() {
    var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    var d = new Date(Date.now()); 
    this.day = days[d.getDay()];
  }

  async getStatistics(): Promise<void> {
    
    const userId = JSON.parse(localStorage.getItem('user'))?.id;

    this.dailyStats$ = this.musicService.getDailyStat(userId);
    
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
