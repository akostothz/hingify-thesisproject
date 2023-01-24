import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Music } from '../_models/music';

@Injectable({
  providedIn: 'root'
})
export class MusicsService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  

  getLikedSongs(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetLikedSongs/' + id);
  }

  getMusicsBySex(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetMusicsBySex/' + id);
  }

  getMusicsByCountry(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetMusicsByCountry/' + id);
  }

  getMusicsByAgeGroup(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetMusicsByAgeGroup/' + id);
  }

  getPersonalizedMix(id: number) {
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetPersonalizedMix/' + id);
  }
 
}
