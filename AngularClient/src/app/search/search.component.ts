import { Component, OnInit } from '@angular/core';
import { MusicsService } from '../_services/musics.service';
import { Observable } from 'rxjs';
import { Music } from '../_models/music';
import { DomSanitizer } from '@angular/platform-browser';
import { LikedSong } from '../_models/likedsong';


@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  foundsongs$: Observable<Music[]> | undefined;
  searchInput: string  = '';

  ngOnInit(): void {
    
  }

  constructor(private musicService: MusicsService, private sanitizer: DomSanitizer) { }

  play(trackId: string) {
    
  }
  search() {
    let inputElement = document.getElementById('search-input') as HTMLInputElement;
    this.searchInput = inputElement.value;
    
    this.foundsongs$ = this.musicService.search(this.searchInput);
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

    return this.sanitizer.bypassSecurityTrustResourceUrl(x);
  }

}
