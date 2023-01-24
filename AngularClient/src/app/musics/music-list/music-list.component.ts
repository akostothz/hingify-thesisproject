import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
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
  
  constructor(private musicService: MusicsService) {

  }

  ngOnInit(): void {
    this.likedsongs$ = this.musicService.getLikedSongs(JSON.parse(localStorage.getItem('user'))?.id);
    this.originsongs$ = this.musicService.getMusicsByCountry(JSON.parse(localStorage.getItem('user'))?.id);
    this.agesongs$ = this.musicService.getMusicsByAgeGroup(JSON.parse(localStorage.getItem('user'))?.id);
    this.gendersongs$ = this.musicService.getMusicsBySex(JSON.parse(localStorage.getItem('user'))?.id);
  }
 
}
