import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './about/about.component';
import { EditPhotoComponent } from './edit-photo/edit-photo.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { ForyouComponent } from './musics/foryou/foryou.component';
import { MusicDetailComponent } from './musics/music-detail/music-detail.component';
import { MusicListComponent } from './musics/music-list/music-list.component';
import { RegisterComponent } from './register/register.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { SpotifyAuthComponent } from './spotify-auth/spotify-auth.component';
import { SpotifyAccessSuccessComponent } from './spotify-access-success/spotify-access-success.component';
import { SearchComponent } from './search/search.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { HelperComponent } from './helper/helper.component';
import { DiscovermoreComponent } from './discovermore/discovermore.component';
import { ArtistComponent } from './artist/artist.component';
import { MoreComponent } from './musics/more/more.component';
import { LikedsongsComponent } from './musics/likedsongs/likedsongs.component';
import { AgegroupsongsComponent } from './musics/agegroupsongs/agegroupsongs.component';
import { GendersongsComponent } from './musics/gendersongs/gendersongs.component';
import { CountrysongsComponent } from './musics/countrysongs/countrysongs.component';
import { SpotifyRegisterComponent } from './spotify-register/spotify-register.component';

const routes: Routes = [
  {path: '', component: AboutComponent},
  {path: 'home', component: HomeComponent},
  {path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'musics', component: MusicListComponent},
      {path: 'musics/liked', component: LikedsongsComponent},
      {path: 'musics/agegroup', component: AgegroupsongsComponent},
      {path: 'musics/gender', component: GendersongsComponent},
      {path: 'musics/country', component: CountrysongsComponent},
      {path: 'musics/:trackId', component: MusicDetailComponent},
      {path: 'edit', component: EditProfileComponent, canDeactivate: [PreventUnsavedChangesGuard]},
      {path: 'editphoto', component: EditPhotoComponent, canDeactivate: [PreventUnsavedChangesGuard]},
      {path: 'foryou', component: ForyouComponent},
      {path: 'spotify-auth', component: SpotifyAuthComponent},
      {path: 'spotify-success', component: SpotifyAccessSuccessComponent},
      {path: 'search', component: SearchComponent},
      {path: 'statistics', component: StatisticsComponent},
      {path: 'help', component: HelperComponent},
      {path: 'discover/:trackId', component: DiscovermoreComponent},
      {path: 'artist/:artistName', component: ArtistComponent},
      {path: 'musics/more/:type', component: MoreComponent}
    ]
  }, 
  {path: 'errors', component: TestErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: 'spotify-register', component: SpotifyRegisterComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'login', component: LoginComponent},
  {path: 'about', component: AboutComponent},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'} //the invalid route
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
