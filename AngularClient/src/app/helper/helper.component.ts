import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { MusicsService } from '../_services/musics.service';
import { DomSanitizer } from '@angular/platform-browser';
import { Observable } from 'rxjs';
import { Music } from '../_models/music';
import { LikedSong } from '../_models/likedsong';

@Component({
  selector: 'app-helper',
  templateUrl: './helper.component.html',
  styleUrls: ['./helper.component.css']
})
export class HelperComponent implements OnInit {
  token: string = '';
  cid: string = '';
  addedSong$: Observable<Music[]> | undefined;

  constructor(private musicService: MusicsService, private sanitizer: DomSanitizer, private accountService: AccountService) { }

  ngOnInit(): void {
    this.getAccessToken();
  }

  write() {
    
  }

  getAccessToken() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);

    const authorizationCode = urlParams.get('code');
    this.token = authorizationCode;

    this.accountService.updateTokenForHelp(this.token);
  }

  currentlyPlaying() {
    this.addedSong$ = this.musicService.currentlyPlaying();
  }

  addSong() {
    let inputElement = document.getElementById('cid') as HTMLInputElement;
    this.cid = inputElement.value.toString();
    this.addedSong$ = this.musicService.addASong(this.cid);
  }
  
}
