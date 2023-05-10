import { Component, OnInit } from '@angular/core';
import { MusicsService } from '../_services/musics.service';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Music } from '../_models/music';
import { LikedSong } from '../_models/likedsong';

@Component({
  selector: 'app-discovermore',
  templateUrl: './discovermore.component.html',
  styleUrls: ['./discovermore.component.css']
})
export class DiscovermoreComponent implements OnInit {
  route: ActivatedRoute;
  disvoceredsongs$: Observable<Music[]> | undefined;

  constructor(private musicService: MusicsService, private sanitizer: DomSanitizer, route: ActivatedRoute) {
    this.route = route;
  }
  
  ngOnInit(): void {
      this.route.params.subscribe(param => {
      let searchedTrackId = param['trackId'];

      this.disvoceredsongs$ = this.musicService.discover(searchedTrackId);
    })  
  }

  likeSong(id: number) {
    var lsong: LikedSong = {
      userId: JSON.parse(localStorage.getItem('user'))?.id,
      musicId: id
    }
    this.musicService.likeMusic(lsong);
  }

  srcgenerator(trrackId: string) {
    let x = 'https://open.spotify.com/embed/track/' + trrackId + '?utm_source=generator';
    console.log(x);
    return this.sanitizer.bypassSecurityTrustResourceUrl(x);
  }

}
