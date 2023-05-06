import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Music } from '../_models/music';
import { MusicsService } from '../_services/musics.service';

@Component({
  selector: 'app-artist',
  templateUrl: './artist.component.html',
  styleUrls: ['./artist.component.css']
})
export class ArtistComponent implements OnInit {
  route: ActivatedRoute;
  songs$: Observable<Music[]> | undefined;
  artist: string = '';

  constructor(private musicService: MusicsService, private sanitizer: DomSanitizer, route: ActivatedRoute) {
    this.route = route;
  }

  ngOnInit(): void {
    this.route.params.subscribe(param => {
    let searchedartist = param['artistName'];
    this.artist = param['artistName'];

    this.songs$ = this.musicService.findArtistMusics(searchedartist);
  })  
  }

  srcgenerator(trrackId: string) {
    let x = 'https://open.spotify.com/embed/track/' + trrackId + '?utm_source=generator';

    return this.sanitizer.bypassSecurityTrustResourceUrl(x);
  }

}
