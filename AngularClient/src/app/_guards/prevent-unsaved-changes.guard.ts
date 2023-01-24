import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { EditProfileComponent } from '../edit-profile/edit-profile.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<EditProfileComponent> {
  canDeactivate(component: EditProfileComponent): boolean {
    if (component.editForm?.dirty) {
      return confirm('Are you sure? Any unsaved changes will be lost.')
    }
    return true;
  }
  
}
