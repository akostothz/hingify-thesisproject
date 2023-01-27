import { outputAst } from '@angular/compiler';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) {}

  ngOnInit(): void {

  }

  register() {
    this.accountService.register(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('/musics');
    }, 
    error: error => { 
      this.toastr.error(error.error)
    }
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
