import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Behavior } from '../_models/behavior';
import { Music } from '../_models/music';
import { User } from '../_models/user';
import { DetailedMusic } from '../_models/detailedmusic';
import { Stat } from '../_models/stat';

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
  discoveredsongs: Music[] = [];
  song: DetailedMusic[] = [];
  dailyStats: Stat[] = [];
  weeklyStats: Stat[] = [];
  monthlyStats: Stat[] = [];
  yearlyStats: Stat[] = [];
  artistSongs: Music[] = [];
  

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

  findArtistMusics(expr: string) {
    if (this.artistSongs.length > 0) return of(this.artistSongs);
    return this.http.get<Music[]>(this.baseUrl + 'Music/FindMoreByArtist/' + expr).pipe(
      map(artistSongs => {
        this.artistSongs = this.artistSongs;
        return artistSongs;
      })
    );
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

  getDailyStat(id: number) {
    return this.http.get<Stat[]>(this.baseUrl + 'Music/GetDailyStatistics/' + id).pipe(
      map(dailyStats => {
        this.dailyStats = this.dailyStats;
        return dailyStats;
      })
    );
  }

  getWeeklyStat(id: number) {
    return this.http.get<Stat[]>(this.baseUrl + 'Music/GetWeeklyStatistics/' + id).pipe(
      map(weeklyStats => {
        this.weeklyStats = this.weeklyStats;
        return weeklyStats;
      })
    );
  }

  getMonthlyStat(id: number) {
    return this.http.get<Stat[]>(this.baseUrl + 'Music/GetMonthlyStatistics/' + id).pipe(
      map(monthlyStats => {
        this.monthlyStats = this.monthlyStats;
        return monthlyStats;
      })
    );
  }

  getYearlyStat(id: number) {
    return this.http.get<Stat[]>(this.baseUrl + 'Music/GetYearlyStatistics/' + id).pipe(
      map(yearlyStats => {
        this.yearlyStats = this.yearlyStats;
        return yearlyStats;
      })
    );
  }

  discover(trackId: string) {
    if (this.discoveredsongs.length > 0) return of(this.discoveredsongs);
    return this.http.get<Music[]>(this.baseUrl + 'Music/FindMore/' + trackId).pipe(
      map(discoveredsongs => {
        this.discoveredsongs = this.discoveredsongs;
        return discoveredsongs;
      })
    );
  }

  findMusic(trackId: string) {
    return this.http.get<DetailedMusic[]>(this.baseUrl + 'Music/FindMusic/' + trackId).pipe(
      map(song => {
        this.song = this.song;
        return song;
      })
    );
  }

  getLikedSongs(id: number) {
    if (this.likedsongs.length > 0) return of(this.likedsongs);
    return this.http.get<Music[]>(this.baseUrl + 'Music/GetActualLikedSongs/' + id).pipe(
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
