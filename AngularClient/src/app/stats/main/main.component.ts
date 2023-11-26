import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  day: string;

  constructor() {

  }

  ngOnInit(): void {
    this.getToday();
  }

  getToday() {
    var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    var d = new Date(Date.now()); 
    this.day = days[d.getDay()];
  }
}
