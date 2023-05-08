import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  picUrl: string;

    constructor(public accountService: AccountService, private router: Router, 
      private toastr: ToastrService) {}

    ngOnInit(): void {
      this.accountService.getPicture(JSON.parse(localStorage.getItem('user'))?.id).subscribe(url => {
        this.picUrl = url[0].valueOf();
    })
  }

    logout() {
      this.accountService.logout();
      this.router.navigateByUrl('/');
    }
}
