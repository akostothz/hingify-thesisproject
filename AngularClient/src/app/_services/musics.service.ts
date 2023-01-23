import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Music } from '../_models/music';

@Injectable({
  providedIn: 'root'
})
export class MusicsService {
  baseUrl = environment.apiUrl;

  httpOptions = {
    headers: new HttpHeaders({
      Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
    })
  };

  constructor(private http: HttpClient) { }

  

  getLikedSongs(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetLikedSongs/' + id, this.httpOptions);
  }
/*
  getHttpOptions() {
    const userString = localStorage.getItem('user');
    if (!userString) return null;
    const user = JSON.parse(userString);
    return {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
      })
    }
  }
  
  getPersonalizedMix(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'GetPersonalizedMix/' + id, this.getHttpOptions());
  }

  getMusicsBySex(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'GetMusicsBySex/' + id, this.getHttpOptions());
  }

  getMusicsByCountry(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'GetMusicsByCountry/' + id, this.getHttpOptions());
  }

  getMusicsByAgeGroup(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'GetMusicsByAgeGroup/' + id, this.getHttpOptions());
  }
  */

 
}
