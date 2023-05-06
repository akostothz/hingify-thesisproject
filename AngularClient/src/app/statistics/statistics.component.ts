import { Component, OnInit } from '@angular/core';
import { MusicsService } from '../_services/musics.service';
import { Observable } from 'rxjs';
import { Stat } from '../_models/stat';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements OnInit {
  day: string;
  timeofday: string;
  dailyStats$: Observable<Stat[]> | undefined;
  weeklyStats$: Observable<Stat[]> | undefined;
  monthlyStats$: Observable<Stat[]> | undefined;
  yearlyStats$: Observable<Stat[]> | undefined;

  constructor(private musicService: MusicsService) {

  }

  ngOnInit(): void {   
    this.TimeSetter();
    this.getStatistics();
  }

  getStatistics() {
      this.dailyStats$ = this.musicService.getDailyStat(JSON.parse(localStorage.getItem('user'))?.id);
      this.weeklyStats$ = this.musicService.getWeeklyStat(JSON.parse(localStorage.getItem('user'))?.id);
      this.monthlyStats$ = this.musicService.getMonthlyStat(JSON.parse(localStorage.getItem('user'))?.id);
      this.yearlyStats$ = this.musicService.getYearlyStat(JSON.parse(localStorage.getItem('user'))?.id);
  }

  TimeSetter() {
    var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    var d = new Date(Date.now()); 
    this.day = days[d.getDay()];

    var time = d.getHours();
    
    if (time < 5)
        this.timeofday = 'Dawn';
    else if (time >= 5 && time < 9)
        this.timeofday = 'Morning';
    else if (time >= 9 && time < 12)
        this.timeofday = 'Forenoon';
    else if (time >= 12 && time < 17)
        this.timeofday = 'Afternoon';
    else if (time >= 17 && time < 21)
        this.timeofday = 'Evening';
    else
        this.timeofday = 'Night';
  }
}
