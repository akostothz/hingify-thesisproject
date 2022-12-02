import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './about/about.component';
import { HomeComponent } from './home/home.component';
import { LoggedHomeComponent } from './logged-home/logged-home.component';
import { ForyouComponent } from './musics/foryou/foryou.component';
import { MusicDetailComponent } from './musics/music-detail/music-detail.component';
import { MusicListComponent } from './musics/music-list/music-list.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'musics', component: MusicListComponent, canActivate: [AuthGuard]},
  {path: 'loggedHome', component: LoggedHomeComponent, canActivate: [AuthGuard]},
  {path: 'musics/:id', component: MusicDetailComponent, canActivate: [AuthGuard]},
  {path: 'foryou', component: ForyouComponent, canActivate: [AuthGuard]},
  {path: 'about', component: AboutComponent},
  {path: '**', component: HomeComponent, pathMatch: 'full'} //the invalid route
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
