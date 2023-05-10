import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { DetailedMusic } from 'src/app/_models/detailedmusic';
import { LikedSong } from 'src/app/_models/likedsong';
import { Music } from 'src/app/_models/music';
import { MusicsService } from 'src/app/_services/musics.service';

@Component({
  selector: 'app-music-detail',
  templateUrl: './music-detail.component.html',
  styleUrls: ['./music-detail.component.css']
})
export class MusicDetailComponent implements OnInit {
  route: ActivatedRoute;
  song$: Observable<DetailedMusic[]> | undefined;

  constructor(private musicService: MusicsService, private sanitizer: DomSanitizer, route: ActivatedRoute) {
    this.route = route;
  }
  
  ngOnInit(): void {
      this.route.params.subscribe(param => {
      let searchedTrackId = param['trackId'];

      this.song$ = this.musicService.findMusic(searchedTrackId);
    })  
  }

  srcgenerator(trrackId: string) {
    let x = 'https://open.spotify.com/embed/track/' + trrackId + '?utm_source=generator';
    
    return this.sanitizer.bypassSecurityTrustResourceUrl(x);
  }

  likeSong(id: number) {
    var lsong: LikedSong = {
      userId: JSON.parse(localStorage.getItem('user'))?.id,
      musicId: id
    }
    this.musicService.likeMusic(lsong);
  }

  msToMins(ms: number) {
    let conv: number = 1.6667E-5;
    return <number>conv * ms;
  }
}
