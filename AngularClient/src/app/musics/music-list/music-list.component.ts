import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, ReplaySubject } from 'rxjs';
import { Behavior } from 'src/app/_models/behavior';
import { Music } from 'src/app/_models/music';
import { MusicsService } from 'src/app/_services/musics.service';

@Component({
  selector: 'app-music-list',
  templateUrl: './music-list.component.html',
  styleUrls: ['./music-list.component.css']
})
export class MusicListComponent implements OnInit {
  likedsongs$: Observable<Music[]> | undefined;
  originsongs$: Observable<Music[]> | undefined;
  agesongs$: Observable<Music[]> | undefined;
  gendersongs$: Observable<Music[]> | undefined;
  behavior: Behavior;
  
  constructor(private musicService: MusicsService, private sanitizer: DomSanitizer) {
  }

  ngOnInit(): void {
    this.likedsongs$ = this.musicService.getLikedSongs(JSON.parse(localStorage.getItem('user'))?.id);
    this.originsongs$ = this.musicService.getMusicsByCountry(JSON.parse(localStorage.getItem('user'))?.id);
    this.agesongs$ = this.musicService.getMusicsByAgeGroup(JSON.parse(localStorage.getItem('user'))?.id);
    this.gendersongs$ = this.musicService.getMusicsBySex(JSON.parse(localStorage.getItem('user'))?.id);
  }

  likeSong(id: number) {
    var behav: Behavior = {
      userid: JSON.parse(localStorage.getItem('user'))?.id,
      musicid: id,
      nameofday: '',
      timeofday: ''
    }
    this.behavior = behav;
    this.musicService.likeMusic(this.behavior).subscribe();
  }

  srcgenerator(trrackId: string) {
    let x = 'https://open.spotify.com/embed/track/' + trrackId + '?utm_source=generator';
    
    return this.sanitizer.bypassSecurityTrustResourceUrl(x);
  }
 
}
