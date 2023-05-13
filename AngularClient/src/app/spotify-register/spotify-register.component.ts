import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { Observable } from 'rxjs';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-spotify-register',
  templateUrl: './spotify-register.component.html',
  styleUrls: ['./spotify-register.component.css']
})
export class SpotifyRegisterComponent implements OnInit {
  registerForm: FormGroup = new FormGroup({});
  displayedUser: User;
  validationErrors: string[] | undefined;
  genders: string[] = [];
  selectedGender = 'Male';

  ngOnInit(): void {
    this.getAccessToken();
    this.fillGenders();
    this.initializeForm();
  }

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService, private fb: FormBuilder) { }

  initializeForm() {
    this.registerForm = this.fb.group({
      username: [],
      country: [],
      email: [],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]],
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      yearOfBirth: ['', Validators.required],
      gender: []
    });
    



    this.registerForm.controls['password'].valueChanges.subscribe( {
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    });
    this.registerForm.controls['gender'].setValue('Male');
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching: true}
    }
  }

  genderChanged(value: string) {
    this.registerForm.controls['gender'].setValue(value);
  }

  getAccessToken() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);

    const authorizationCode = urlParams.get('code');

    
    this.accountService.retrieveFromSpotifyWithString(authorizationCode).subscribe(x => this.displayedUser = x);

    console.log(this.displayedUser);
  }

  fillGenders() {
    this.genders.push("Male");
    this.genders.push("Female");
    this.genders.push("Other");
  }

  register() {
    this.registerForm.controls['username'].setValue(this.displayedUser.username);
    this.registerForm.controls['country'].setValue(this.displayedUser.country);
    this.registerForm.controls['email'].setValue(this.displayedUser.email);
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => {
        this.router.navigateByUrl('/foryou');
    }, 
    error: error => { 
      this.validationErrors = error
    }
    })
  }
}
