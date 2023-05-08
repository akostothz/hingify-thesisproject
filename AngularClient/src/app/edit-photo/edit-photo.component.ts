import { Component, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-edit-photo',
  templateUrl: './edit-photo.component.html',
  styleUrls: ['./edit-photo.component.css']
})
export class EditPhotoComponent implements OnInit {
  user: User | undefined;
  uploader: FileUploader | undefined;
  spotyUploader: FileUploader | undefined;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  picUrl: string;

  constructor(private accountService: AccountService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    })
  }
  
  ngOnInit(): void {
    this.initializeUploader();
    this.accountService.getPicture(JSON.parse(localStorage.getItem('user'))?.id).subscribe(url => {
      this.picUrl = url[0].valueOf();
  })
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader( {
      url: this.baseUrl + 'account/addphoto',
      authToken: 'Bearer ' + this.user?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false
    }
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.user.photoUrl = photo.photoUrl;
      }
    }
    
    }

    retrieveSpotifyPic() {
      /*
      const queryString = window.location.search;
      const urlParams = new URLSearchParams(queryString);

      const authorizationCode = urlParams.get('code');
      console.log(authorizationCode);
      this.accountService.retrieveFromSpotify(authorizationCode);*/
      
      
    }

    upload() {
      this.accountService.uploadPhoto(this.uploader);
      this.accountService.getPicture(JSON.parse(localStorage.getItem('user'))?.id).subscribe(url => {
        this.picUrl = url[0].valueOf();
    })
    }
  }


