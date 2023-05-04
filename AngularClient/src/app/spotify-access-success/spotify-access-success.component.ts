import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-spotify-access-success',
  templateUrl: './spotify-access-success.component.html',
  styleUrls: ['./spotify-access-success.component.css']
})
export class SpotifyAccessSuccessComponent implements OnInit {
  
  ngOnInit(): void {
    this.getAccessToken();
  }

  constructor(private accountService: AccountService) { }

  getAccessToken() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);

    const authorizationCode = urlParams.get('code');

    console.log(authorizationCode);
    this.accountService.retrieveFromSpotify(authorizationCode);
  }
}