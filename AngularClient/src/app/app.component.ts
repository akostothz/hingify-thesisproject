import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Music Recommendation App';
  musics: any;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getMusics();
  }

  getMusics() {
    this.http.get('http://localhost:5034/api/home').subscribe(response => {
      this.musics = response;
    }, error => {
      console.log(error);
    })
  }
}
