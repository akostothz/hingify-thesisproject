import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  user: User | undefined;
  picUrl: string;

  constructor(private accountService: AccountService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    })
  }

  ngOnInit(): void {
    this.accountService.getPicture(JSON.parse(localStorage.getItem('user'))?.id).subscribe(url => {
      this.picUrl = url[0].valueOf();
  })
  }

  submitChanges() {
    this.accountService.update(this.editForm?.value).subscribe({
      next: _ => {
          this.toastr.success('Profile successfully updated');
          this.editForm?.reset(this.user);
      }
    });
  }

}
