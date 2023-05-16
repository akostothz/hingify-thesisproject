import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Observable, Subscription } from 'rxjs';
import { Music } from '../_models/music';
import { MusicsService } from '../_services/musics.service';

@Component({
  selector: 'app-playbacktest',
  templateUrl: './playbacktest.component.html',
  styleUrls: ['./playbacktest.component.css']
})
export class PlaybacktestComponent implements OnInit {
    lhsongs$: Observable<Music[]> | undefined;
    tracks: Music[] = []
    trackIds: Array<String> = [];

    constructor(private musicService: MusicsService, private sanitizer: DomSanitizer) { }

    ngOnInit(): void {
      this.lhsongs$ = this.musicService.getLikedSongs(JSON.parse(localStorage.getItem('user'))?.id);
      
    }

  getLikedSongs() {
    this.trackIds = this.musicService.getTrackIds();

  }
  srcgenerator(trrackId: string) {
    let x = 'https://open.spotify.com/embed/track/' + trrackId + '?utm_source=generator';
    
    return this.sanitizer.bypassSecurityTrustResourceUrl(x);
  }
}
