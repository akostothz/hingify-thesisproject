import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, pipe } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Behavior } from '../_models/behavior';
import { Music } from '../_models/music';
import { User } from '../_models/user';
import { DetailedMusic } from '../_models/detailedmusic';
import { Stat } from '../_models/stat';
import { ToastrService } from 'ngx-toastr';
import { AccessToken } from '../_models/accesstoken';

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
  last7daysDays: string[] = [];
  last7daysMins: number[] = [];
  artistSongs: Music[] = [];
  isItLiked: Boolean = false;
  addedMusics: Music[] = [];
  day: string;
  timeofday: string;
  addedBehaviors: Behavior[] = [];

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  currentlyPlaying() {
    let id = JSON.parse(localStorage.getItem('user'))?.id;
    return this.http.get<Music[]>(this.baseUrl + 'Music/AddSongWithListening/' + id).pipe(
      map(addedMusics => {
        this.addedMusics = addedMusics;
        if(this.addedMusics.length > 0) {
          this.toastr.success('Song added to the database.');
        }
        else {
          this.toastr.error('Something went wrong. Either it is already in the database, or you are not listening to the song.');
        }
        return addedMusics;
      }));
  }
  addNewBehavior() {
    let userid = JSON.parse(localStorage.getItem('user'))?.id;
    this.TimeSetter();

    return this.http.get<Behavior[]>(this.baseUrl + 'Music/AddBehaviorWithListening/' + userid).pipe(
      map(addedBehaviors => {
        this.addedBehaviors = addedBehaviors;
        if(this.addedBehaviors.length > 0) {
          this.toastr.success('Behavior added. (' + this.day + ', ' + this.timeofday + ')');
        }
        else {
          this.toastr.error('Something went wrong. Refresh the page, and try again.');
        }
        return addedBehaviors;
      })
    );
  }

  addNewBehaviorWithButton(trackId: String) {
    let userid = JSON.parse(localStorage.getItem('user'))?.id;
    let ids: String = userid.toString() + '.' + trackId;
    this.TimeSetter();

    return this.http.get<Behavior[]>(this.baseUrl + 'Music/AddBehaviorWithButton/' + ids).pipe(
      map(addedBehaviors => {
        this.addedBehaviors = addedBehaviors;
        if(this.addedBehaviors.length > 0) {
          this.toastr.success('Behavior added. (' + this.day + ', ' + this.timeofday + ')');
        }
        else {
          this.toastr.error('Something went wrong. Refresh the page, and try again.');
        }
        return addedBehaviors;
      })
    );
  
  }
  TimeSetter() {
    var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    var d = new Date(Date.now()); 
    this.day = days[d.getDay()];

    var time = d.getHours();
    
    if (time < 5)
        this.timeofday = 'Dawn';
    else if (time >= 5 && time < 9)
        this.timeofday = 'Morning';
    else if (time >= 9 && time < 12)
        this.timeofday = 'Forenoon';
    else if (time >= 12 && time < 17)
        this.timeofday = 'Afternoon';
    else if (time >= 17 && time < 21)
        this.timeofday = 'Evening';
    else
        this.timeofday = 'Night';
  }

  addASong(cidUncut: String) {
    let cid: String = (JSON.parse(localStorage.getItem('user'))?.id).toString() + ';'
    let u: String[] = [];
    u.push(cidUncut);
    let cidPlus = u.map(e => e.replace('?utm_source=generator" width="100%" height="352" frameBorder="0" allowfullscreen="" allow="autoplay; clipboard-write; encrypted-media; fullscreen; picture-in-picture" loading="lazy"></iframe>', ''))
    cid += cidPlus[0].substring(77);
    
    console.log(cid);
    return this.http.get<Music[]>(this.baseUrl + 'Music/AddSongWithCid/' + cid).pipe(
      map(addedMusics => {
        this.addedMusics = addedMusics;
        if(this.addedMusics.length > 0) {
          this.toastr.success('Song added to the database.');
        }
        else {
          this.toastr.error('Something went wrong. Either it is already in the database, or the code is wrong.');
        }
        return addedMusics;
      })
    );
  }

  getTrackIds() {
    console.log(this.likedsongs);
    const mIds: Array<String> = [];
    this.likedsongs.forEach(x => {
      mIds.push(x.trackId);
   });
   return mIds;
  }

  createPlaylist(token: string) {
    
    const mIds: Array<String> = [];
    console.log('Token: ' + token)
    mIds.push((JSON.parse(localStorage.getItem('user'))?.id).toString());
    mIds.push(token);

    this.personalizedsongs.forEach(x => {
       mIds.push('spotify:track:' + x.trackId);
    });


    return this.http.post(this.baseUrl + 'Music/CreatePlaylist', mIds).subscribe(
      response => {
        map((response: any) => {
          console.log(response);
          
        })
        this.toastr.success('Playlist created on Spotify.')
      })

  }

  likeMusic(model: any) {
    return this.http.post(this.baseUrl + 'Music/LikeSong', model).subscribe(
      response => {
        map((response: any) => {
          console.log(response);
          
        })
        this.toastr.success('Song added to Liked Songs.');
      },
    );;
    
  }

  dislikeMusic(model: any) {
    return this.http.post(this.baseUrl + 'Music/DisikeSong', model).subscribe(
      response => {
        map((response: any) => {
          console.log(response);
          
        })
        this.toastr.warning('Song removed from Liked Songs.');
      },
    );;
    
  }

  isLiked(model: Music) {
      var contains = false;
      this.likedsongs.forEach(x => {
        if(x.trackId === model.trackId) {
          contains = true;
        }    
      });
     
      return contains;
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
        this.artistSongs = artistSongs;
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
        this.dailyStats = dailyStats;
        console.log(this.dailyStats)
        return dailyStats;
      })
    );
  }

  getWeeklyStat(id: number) {
    return this.http.get<Stat[]>(this.baseUrl + 'Music/GetWeeklyStatistics/' + id).pipe(
      map(weeklyStats => {
        this.weeklyStats = weeklyStats;
        console.log(this.weeklyStats)
        return weeklyStats;
      })
    );
  }

  getMonthlyStat(id: number) {
    return this.http.get<Stat[]>(this.baseUrl + 'Music/GetMonthlyStatistics/' + id).pipe(
      map(monthlyStats => {
        this.monthlyStats = monthlyStats;
        console.log(this.monthlyStats)
        return monthlyStats;
      })
    );
  }

  getYearlyStat(id: number) {
    return this.http.get<Stat[]>(this.baseUrl + 'Music/GetYearlyStatistics/' + id).pipe(
      map(yearlyStats => {
        this.yearlyStats = yearlyStats;
        console.log(this.yearlyStats)
        return yearlyStats;
      })
    );
  }

  getLast7DaysMins(id: number) {
    return this.http.get<number[]>(this.baseUrl + 'Music/GetLast7DaysMins/' + id).pipe(
      map(last7daysMins => {
        this.last7daysMins = last7daysMins;
        console.log('musicService')
        console.log(this.last7daysMins)
        return last7daysMins;
      })
    );
  }

  getLast7Days(id: number) {
    return this.http.get<string[]>(this.baseUrl + 'Music/GetLast7Days/' + id).pipe(
      map(last7daysDays => {
        this.last7daysDays = last7daysDays;
        console.log('musicService')
        console.log(this.last7daysDays)
        return last7daysDays;
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
        this.likedsongs = likedsongs;
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
        this.personalizedsongs = personalizedsongs;
        return personalizedsongs;
      })
    );
  }
 
}
