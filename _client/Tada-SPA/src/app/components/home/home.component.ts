import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  values: any;
  registerMode = false;
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getValues();
  }

  registerToggle() {
    this.registerMode = true;
  }

  cancelMode(mode: boolean) {
    this.registerMode = mode;
  }

  getValues() {
    this.http.get('http://localhost:5000/api/values')
      .subscribe((response) => {
        this.values = response;
      }, errors => {
          console.log(errors);
    });
  }
}
