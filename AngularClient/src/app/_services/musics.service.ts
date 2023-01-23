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

  getMusicsBySex(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetMusicsBySex/' + id, this.httpOptions);
  }

  getMusicsByCountry(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetMusicsByCountry/' + id, this.httpOptions);
  }

  getMusicsByAgeGroup(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetMusicsByAgeGroup/' + id, this.httpOptions);
  }
  

 
}
