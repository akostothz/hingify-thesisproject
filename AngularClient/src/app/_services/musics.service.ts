import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Behavior } from '../_models/behavior';
import { Music } from '../_models/music';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class MusicsService {
  baseUrl = environment.apiUrl;
  likedsongs: Music[] = [];
  gendersongs: Music[] = [];
  bycountrysongs: Music[] = [];
  agesongs: Music[] = [];
  personalizedsongs: Music[] = [];
  foundsongs: Music[] = [];

  constructor(private http: HttpClient) { }

  likeMusic(model: any) {
    return this.http.post(this.baseUrl + 'Music/LikeSong', model);
    
  }
  public ToHttpParams(request: any): HttpParams {
    let httpParams = new HttpParams();
    Object.keys(request).forEach(function (key) {
      httpParams = httpParams.append(key, request[key]);
    });
    return httpParams;
  }

  search(expr: string) {
    if (this.foundsongs.length > 0) return of(this.foundsongs);
    return this.http.get<Music[]>(this.baseUrl + 'Music/Search/' + expr).pipe(
      map(foundsongs => {
        this.foundsongs = this.foundsongs;
        return foundsongs;
      })
    );
  }

  getLikedSongs(id: number) {
    if (this.likedsongs.length > 0) return of(this.likedsongs);
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetLikedSongs/' + id).pipe(
      map(likedsongs => {
        this.likedsongs = this.likedsongs;
        return likedsongs;
      })
    );
  }

  getMusicsBySex(id: number) {
    if (this.gendersongs.length > 0) return of(this.gendersongs);
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetMusicsBySex/' + id).pipe(
      map(gendersongs => {
        this.gendersongs = this.gendersongs;
        return gendersongs;
      })
    );
  }

  getMusicsByCountry(id: number) {
    if (this.bycountrysongs.length > 0) return of(this.bycountrysongs);
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetMusicsByCountry/' + id).pipe(
      map(bycountrysongs => {
        this.bycountrysongs = this.bycountrysongs;
        return bycountrysongs;
      })
    );
  }

  getMusicsByAgeGroup(id: number) {
    if (this.agesongs.length > 0) return of(this.agesongs);
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetMusicsByAgeGroup/' + id).pipe(
      map(agesongs => {
        this.agesongs = this.agesongs;
        return agesongs;
      })
    );
  }

  getPersonalizedMix(id: number) {
    if (this.personalizedsongs.length > 0) return of(this.personalizedsongs);
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetPersonalizedMix/' + id).pipe(
      map(personalizedsongs => {
        this.personalizedsongs = this.personalizedsongs;
        return personalizedsongs;
      })
    );
  }
 
}
