import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Observable } from 'rxjs';
import { LikedSong } from 'src/app/_models/likedsong';
import { Music } from 'src/app/_models/music';
import { AccountService } from 'src/app/_services/account.service';
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
  token: string = '';

  constructor(private musicService: MusicsService, private sanitizer: DomSanitizer, private accountService: AccountService) {

  }

  ngOnInit(): void {
    this.getAccessToken();
    this.loadMusics();  
    this.TimeSetter();
  }

  getAccessToken() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);

    const authorizationCode = urlParams.get('code');
    this.token = authorizationCode;

    this.accountService.updateToken(this.token);
    console.log(authorizationCode);
  }

  createPlaylist() {
    this.musicService.createPlaylist(this.token);
  }

  loadMusics() { 
    this.lhsongs$ = this.musicService.getPersonalizedMix(JSON.parse(localStorage.getItem('user'))?.id);
  }

  addBehavior() {
    this.musicService.addNewBehavior();
  }

  addBehaviorWithButton(trackId: String) {
    this.musicService.addNewBehaviorWithButton(trackId);
  }

  likeSong(id: number) {
    var lsong: LikedSong = {
      userId: JSON.parse(localStorage.getItem('user'))?.id,
      musicId: id
    }
    this.musicService.likeMusic(lsong);
  }

  dislikeSong(id: number) {
    var lsong: LikedSong = {
      userId: JSON.parse(localStorage.getItem('user'))?.id,
      musicId: id
    }
    this.musicService.dislikeMusic(lsong);
  }

  isLiked(music: Music) {
    var res = this.musicService.isLiked(music);

    return res;
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
    
    return this.sanitizer.bypassSecurityTrustResourceUrl(x);
  }
}
