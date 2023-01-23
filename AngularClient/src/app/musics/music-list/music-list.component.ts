import { Component, OnInit } from '@angular/core';
import { Music } from 'src/app/_models/music';
import { MusicsService } from 'src/app/_services/musics.service';

@Component({
  selector: 'app-music-list',
  templateUrl: './music-list.component.html',
  styleUrls: ['./music-list.component.css']
})
export class MusicListComponent implements OnInit {
  likedsongs: Music[] = [];
  originsongs: Music[] = [];
  agesongs: Music[] = [];
  gendersongs: Music[] = [];
  
  constructor(private musicService: MusicsService) {

  }

  ngOnInit(): void {
    this.loadMusics();
  }
  
  loadMusics() {
      this.musicService.getLikedSongs(JSON.parse(localStorage.getItem('user'))?.id).subscribe({
        next: likedsongs => this.likedsongs = likedsongs
      }),
      this.musicService.getMusicsByCountry(JSON.parse(localStorage.getItem('user'))?.id).subscribe({
        next: originsongs => this.originsongs = originsongs
      }),
      this.musicService.getMusicsByAgeGroup(JSON.parse(localStorage.getItem('user'))?.id).subscribe({
        next: agesongs => this.agesongs = agesongs
      }),
      this.musicService.getMusicsBySex(JSON.parse(localStorage.getItem('user'))?.id).subscribe({
        next: gendersongs => this.gendersongs = gendersongs
      })
  }
  
 
}
