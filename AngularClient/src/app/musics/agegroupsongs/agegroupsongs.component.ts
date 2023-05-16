import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Observable } from 'rxjs';
import { Behavior } from 'src/app/_models/behavior';
import { LikedSong } from 'src/app/_models/likedsong';
import { Music } from 'src/app/_models/music';
import { MusicsService } from 'src/app/_services/musics.service';

@Component({
  selector: 'app-agegroupsongs',
  templateUrl: './agegroupsongs.component.html',
  styleUrls: ['./agegroupsongs.component.css']
})
export class AgegroupsongsComponent implements OnInit {
  agesongs$: Observable<Music[]> | undefined;
  addedBehaviors$: Observable<Behavior[]> | undefined;

  constructor(private musicService: MusicsService, private sanitizer: DomSanitizer) {
  }

  ngOnInit(): void {
    this.agesongs$ = this.musicService.getMusicsByAgeGroup(JSON.parse(localStorage.getItem('user'))?.id);
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
  
    srcgenerator(trrackId: string) {
      let x = 'https://open.spotify.com/embed/track/' + trrackId + '?utm_source=generator';
      
      return this.sanitizer.bypassSecurityTrustResourceUrl(x);
    }

    
    addBehaviorWithButton(trackId: String) {
    this.addedBehaviors$ = this.musicService.addNewBehaviorWithButton(trackId);
  }
}
