import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { TooltipModule} from 'ngx-bootstrap/tooltip';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MusicListComponent } from './musics/music-list/music-list.component';
import { MusicDetailComponent } from './musics/music-detail/music-detail.component';
import { ForyouComponent } from './musics/foryou/foryou.component';
import { AboutComponent } from './about/about.component';
import { SharedModule } from './_modules/shared.module';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { EditPhotoComponent } from './edit-photo/edit-photo.component';
import { LoginComponent } from './login/login.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { SpotifyAuthComponent } from './spotify-auth/spotify-auth.component';
import { SpotifyAccessSuccessComponent } from './spotify-access-success/spotify-access-success.component';
import { SearchComponent } from './search/search.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { HelperComponent } from './helper/helper.component';
import { DiscovermoreComponent } from './discovermore/discovermore.component';
import { ArtistComponent } from './artist/artist.component';
import { MoreComponent } from './musics/more/more.component';
import { LikedsongsComponent } from './musics/likedsongs/likedsongs.component';
import { GendersongsComponent } from './musics/gendersongs/gendersongs.component';
import { CountrysongsComponent } from './musics/countrysongs/countrysongs.component';
import { AgegroupsongsComponent } from './musics/agegroupsongs/agegroupsongs.component';
import { SpotifyRegisterComponent } from './spotify-register/spotify-register.component';
import { PlaybacktestComponent } from './playbacktest/playbacktest.component';
import { DailyComponent } from './stats/daily/daily.component';
import { WeeklyComponent } from './stats/weekly/weekly.component';
import { MonthlyComponent } from './stats/monthly/monthly.component';
import { YearlyComponent } from './stats/yearly/yearly.component';
import { MainComponent } from './stats/main/main.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MusicListComponent,
    MusicDetailComponent,
    ForyouComponent,
    AboutComponent,
    TestErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    EditProfileComponent,
    EditPhotoComponent,
    LoginComponent,
    TextInputComponent,
    SpotifyAuthComponent,
    SpotifyAccessSuccessComponent,
    SearchComponent,
    StatisticsComponent,
    HelperComponent,
    DiscovermoreComponent,
    ArtistComponent,
    MoreComponent,
    LikedsongsComponent,
    GendersongsComponent,
    CountrysongsComponent,
    AgegroupsongsComponent,
    SpotifyRegisterComponent,
    PlaybacktestComponent,
    DailyComponent,
    WeeklyComponent,
    MonthlyComponent,
    YearlyComponent,
    MainComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    TooltipModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
