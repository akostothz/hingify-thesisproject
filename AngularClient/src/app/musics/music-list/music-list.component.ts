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
  /*
  countrymix: Music[] = [];
  agemix: Music[] = [];
  gendermix: Music[] = [];
  */
  constructor(private musicService: MusicsService) {

  }

  ngOnInit(): void {
    this.loadMusics();
  }
  
  loadMusics() {
      this.musicService.getLikedSongs(JSON.parse(localStorage.getItem('user'))?.id).subscribe({
        next: likedsongs => this.likedsongs = likedsongs
      })
  }
  
 
}
