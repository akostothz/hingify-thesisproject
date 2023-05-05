import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Observable } from 'rxjs';
import { Music } from 'src/app/_models/music';
import { MusicsService } from 'src/app/_services/musics.service';

@Component({
  selector: 'app-foryou',
  templateUrl: './foryou.component.html',
  styleUrls: ['./foryou.component.css']
})
export class ForyouComponent implements OnInit {
  lhsongs$: Observable<Music[]> | undefined;
  day: string;
  timeofday: string;

  constructor(private musicService: MusicsService, private sanitizer: DomSanitizer) {

  }

  ngOnInit(): void {
    this.loadMusics();    
    this.TimeSetter();
  }

  loadMusics() { 
    this.lhsongs$ = this.musicService.getLikedSongs(JSON.parse(localStorage.getItem('user'))?.id);
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

  srcgenerator(trrackId: string) {
    let x = 'https://open.spotify.com/embed/track/' + trrackId + '?utm_source=generator';
    console.log(x);
    return this.sanitizer.bypassSecurityTrustResourceUrl(x);
  }
}
