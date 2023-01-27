import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  model: any = {}

    constructor(public accountService: AccountService, private router: Router, 
      private toastr: ToastrService) {}

    ngOnInit(): void {
      
    }

    login() {
      this.accountService.login(this.model).subscribe({
        next: _ => this.router.navigateByUrl('/foryou')
      })
    }

}
