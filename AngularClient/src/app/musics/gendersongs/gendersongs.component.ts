import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Observable } from 'rxjs';
import { LikedSong } from 'src/app/_models/likedsong';
import { Music } from 'src/app/_models/music';
import { MusicsService } from 'src/app/_services/musics.service';

@Component({
  selector: 'app-gendersongs',
  templateUrl: './gendersongs.component.html',
  styleUrls: ['./gendersongs.component.css']
})
export class GendersongsComponent implements OnInit {
  gendersongs$: Observable<Music[]> | undefined;

  constructor(private musicService: MusicsService, private sanitizer: DomSanitizer) {
  }

  ngOnInit(): void {
    this.gendersongs$ = this.musicService.getMusicsBySex(JSON.parse(localStorage.getItem('user'))?.id);
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
    console.log(res);
    return res;
  }

  srcgenerator(trrackId: string) {
    let x = 'https://open.spotify.com/embed/track/' + trrackId + '?utm_source=generator';
    
    return this.sanitizer.bypassSecurityTrustResourceUrl(x);
  }
}
