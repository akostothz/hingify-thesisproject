import { JsonPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { AccessToken } from '../_models/accesstoken';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  login(model: any){
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }

  register(model: any){
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }
  update(user: User) {
    return this.http.put(this.baseUrl + 'account/updateuser', user);
  }

  setCurrentUser(user: User){
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  uploadPhoto(file: any) {
    return this.http.post(this.baseUrl + 'account/updateuser', file);
  }

  retrieveFromSpotify(bearer: string) {
    let accesstoken = <AccessToken>({
      userid: JSON.parse(localStorage.getItem('user'))?.id,
      token: bearer
    });
    return this.http.put(this.baseUrl + 'account/spotifypic', accesstoken).subscribe(
      response => {
        map((response: User) => {
          const user = response;
          localStorage.removeItem('user');
          localStorage.setItem('user', JSON.stringify(user));
        })
        this.toastr.success('Profile successfully updated. Might need to hard refresh or log in again to see the changes.');
      },
    );
  }

  getAccessToken(token: string) {
    let accesstoken = <AccessToken>({
      userid: JSON.parse(localStorage.getItem('user'))?.id,
      token: token
    });
    return this.http.post(this.baseUrl + 'account/getaccesstoken', accesstoken);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
