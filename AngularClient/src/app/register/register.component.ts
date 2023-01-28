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
  countries: string[] = [];
  genders: string[] = [];
  selectedGender = '';
  selectedCountry = '';

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.fillCountries();
    this.fillGenders();
  }

  genderChanged(value: string) {
    this.selectedGender = value;
  }

  countryChanged(value: string) {
    this.selectedCountry = value;
  }

  register() {
    this.model.gender = this.selectedGender;
    this.model.country = this.selectedCountry;

    this.accountService.register(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('/musics');
    }, 
    error: error => { 
      this.toastr.error(error.error)
    }
    })
  }

  fillGenders() {
    this.genders.push("Male");
    this.genders.push("Female");
    this.genders.push("Other");
  }

  fillCountries() {
    this.countries.push("Afghanistan");
    this.countries.push("Albania");
    this.countries.push("Algeria");
    this.countries.push("Andorra");
    this.countries.push("Angola");
    this.countries.push("Antigua & Deps");
    this.countries.push("Argentina");
    this.countries.push("Armenia");
    this.countries.push("Australia");
    this.countries.push("Austria");
    this.countries.push("Azerbaijan");
    this.countries.push("Bahamas");
    this.countries.push("Bahrain");
    this.countries.push("Bangladesh");
    this.countries.push("Barbados");
    this.countries.push("Belarus");
    this.countries.push("Belgium");
    this.countries.push("Belize");
    this.countries.push("Benin");
    this.countries.push("Bhutan");
    this.countries.push("Bolivia");
    this.countries.push("Bosnia Herzegovina");
    this.countries.push("Botswana");
    this.countries.push("Brazil");
    this.countries.push("Brunei");
    this.countries.push("Bulgaria");
    this.countries.push("Burkina");
    this.countries.push("Burundi");
    this.countries.push("Cambodia");
    this.countries.push("Cameroon");
    this.countries.push("Canada");
    this.countries.push("Cape Verde");
    this.countries.push("Central African Rep");
    this.countries.push("Chad");
    this.countries.push("Chile");
    this.countries.push("China");
    this.countries.push("Colombia");
    this.countries.push("Comoros");
    this.countries.push("Congo");
    this.countries.push("Congo {Democratic Rep}");
    this.countries.push("Costa Rica");
    this.countries.push("Croatia");
    this.countries.push("Cuba");
    this.countries.push("Cyprus");
    this.countries.push("Czech Republic");
    this.countries.push("Denmark");
    this.countries.push("Djibouti");
    this.countries.push("Dominica");
    this.countries.push("Dominican Republic");
    this.countries.push("East Timor");
    this.countries.push("Ecuador");
    this.countries.push("Egypt");
    this.countries.push("El Salvador");
    this.countries.push("Equatorial Guinea");
    this.countries.push("Eritrea");
    this.countries.push("Estonia");
    this.countries.push("Ethiopia");
    this.countries.push("Fiji");
    this.countries.push("Finland");
    this.countries.push("France");
    this.countries.push("Gabon");
    this.countries.push("Gambia");
    this.countries.push("Georgia");
    this.countries.push("Germany");
    this.countries.push("Ghana");
    this.countries.push("Greece");
    this.countries.push("Grenada");
    this.countries.push("Guatemala");
    this.countries.push("Guinea");
    this.countries.push("Guinea-Bissau");
    this.countries.push("Guyana");
    this.countries.push("Haiti");
    this.countries.push("Honduras");
    this.countries.push("Hungary");
    this.countries.push("Iceland");
    this.countries.push("India");
    this.countries.push("Indonesia");
    this.countries.push("Iran");
    this.countries.push("Iraq");
    this.countries.push("Ireland {Republic}");
    this.countries.push("Israel");
    this.countries.push("Italy");
    this.countries.push("Ivory Coast");
    this.countries.push("Jamaica");
    this.countries.push("Japan");
    this.countries.push("Jordan");
    this.countries.push("Kazakhstan");
    this.countries.push("Kenya");
    this.countries.push("Kiribati");
    this.countries.push("Korea North");
    this.countries.push("Korea South");
    this.countries.push("Kosovo");
    this.countries.push("Kuwait");
    this.countries.push("Kyrgyzstan");
    this.countries.push("Laos");
    this.countries.push("Latvia");
    this.countries.push("Lebanon");
    this.countries.push("Lesotho");
    this.countries.push("Liberia");
    this.countries.push("Libya");
    this.countries.push("Liechtenstein");
    this.countries.push("Lithuania");
    this.countries.push("Luxembourg");
    this.countries.push("Macedonia");
    this.countries.push("Madagascar");
    this.countries.push("Malawi");
    this.countries.push("Malaysia");
    this.countries.push("Maldives");
    this.countries.push("Mali");
    this.countries.push("Malta");
    this.countries.push("Marshall Islands");
    this.countries.push("Mauritania");
    this.countries.push("Mauritius");
    this.countries.push("Mexico");
    this.countries.push("Micronesia");
    this.countries.push("Moldova");
    this.countries.push("Monaco");
    this.countries.push("Mongolia");
    this.countries.push("Montenegro");
    this.countries.push("Morocco");
    this.countries.push("Mozambique");
    this.countries.push("Myanmar, {Burma}");
    this.countries.push("Namibia");
    this.countries.push("Nauru");
    this.countries.push("Nepal");
    this.countries.push("Netherlands");
    this.countries.push("New Zealand");
    this.countries.push("Nicaragua");
    this.countries.push("Niger");
    this.countries.push("Nigeria");
    this.countries.push("Norway");
    this.countries.push("Oman");
    this.countries.push("Pakistan");
    this.countries.push("Palau");
    this.countries.push("Panama");
    this.countries.push("Papua New Guinea");
    this.countries.push("Paraguay");
    this.countries.push("Peru");
    this.countries.push("Philippines");
    this.countries.push("Poland");
    this.countries.push("Portugal");
    this.countries.push("Qatar");
    this.countries.push("Romania");
    this.countries.push("Russian Federation");
    this.countries.push("Rwanda");
    this.countries.push("St Kitts & Nevis");
    this.countries.push("St Lucia");
    this.countries.push("Saint Vincent & the Grenadines");
    this.countries.push("Samoa");
    this.countries.push("San Marino");
    this.countries.push("Sao Tome & Principe");
    this.countries.push("Saudi Arabia");
    this.countries.push("Senegal");
    this.countries.push("Serbia");
    this.countries.push("Seychelles");
    this.countries.push("Sierra Leone");
    this.countries.push("Singapore");
    this.countries.push("Slovakia");
    this.countries.push("Slovenia");
    this.countries.push("Solomon Islands");
    this.countries.push("Somalia");
    this.countries.push("South Africa");
    this.countries.push("South Sudan");
    this.countries.push("Spain");
    this.countries.push("Sri Lanka");
    this.countries.push("Sudan");
    this.countries.push("Suriname");
    this.countries.push("Swaziland");
    this.countries.push("Sweden");
    this.countries.push("Switzerland");
    this.countries.push("Syria");
    this.countries.push("Taiwan");
    this.countries.push("Tajikistan");
    this.countries.push("Tanzania");
    this.countries.push("Thailand");
    this.countries.push("Togo");
    this.countries.push("Tonga");
    this.countries.push("Trinidad & Tobago");
    this.countries.push("Tunisia");
    this.countries.push("Turkey");
    this.countries.push("Turkmenistan");
    this.countries.push("Tuvalu");
    this.countries.push("Uganda");
    this.countries.push("Ukraine");
    this.countries.push("United Arab Emirates");
    this.countries.push("United Kingdom");
    this.countries.push("United States");
    this.countries.push("Uruguay");
    this.countries.push("Uzbekistan");
    this.countries.push("Vanuatu");
    this.countries.push("Vatican City");
    this.countries.push("Venezuela");
    this.countries.push("Vietnam");
    this.countries.push("Yemen");
    this.countries.push("Zambia");
    this.countries.push("Zimbabwe");
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
