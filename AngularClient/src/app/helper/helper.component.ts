import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { MusicsService } from '../_services/musics.service';

@Component({
  selector: 'app-helper',
  templateUrl: './helper.component.html',
  styleUrls: ['./helper.component.css']
})
export class HelperComponent implements OnInit {
  token: string = '';
  cid: string = '';

  constructor(private musicService: MusicsService, private accountService: AccountService) { }

  ngOnInit(): void {
    this.getAccessToken();
  }

  getAccessToken() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);

    const authorizationCode = urlParams.get('code');
    this.token = authorizationCode;

    this.accountService.updateTokenForHelp(this.token);
  }

  currentlyPlaying() {
    this.musicService.currentlyPlaying();
  }

  addSong() {
    let inputElement = document.getElementById('cid') as HTMLInputElement;
    this.cid = inputElement.value.toString();
    this.musicService.addASong(this.cid);
  }
}
