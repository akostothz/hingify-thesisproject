import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './about/about.component';
import { EditPhotoComponent } from './edit-photo/edit-photo.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { ForyouComponent } from './musics/foryou/foryou.component';
import { MusicDetailComponent } from './musics/music-detail/music-detail.component';
import { MusicListComponent } from './musics/music-list/music-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';

const routes: Routes = [
  {path: '', component: AboutComponent},
  {path: 'home', component: HomeComponent},
  {path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'musics', component: MusicListComponent},
      {path: 'musics/:id', component: MusicDetailComponent},
      {path: 'edit', component: EditProfileComponent, canDeactivate: [PreventUnsavedChangesGuard]},
      {path: 'editphoto', component: EditPhotoComponent, canDeactivate: [PreventUnsavedChangesGuard]},
      {path: 'foryou', component: ForyouComponent}
    ]
  }, 
  {path: 'errors', component: TestErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: 'about', component: AboutComponent},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'} //the invalid route
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
