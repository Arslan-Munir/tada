import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from './models/user';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  constructor(private authService: AuthService, private router: Router){}
  ngOnInit(): void {
    const token = localStorage.getItem('token');
    const user: User = JSON.parse(localStorage.getItem('user'));

    if (token) {
      this.authService.decodedToken = this.authService.decodeToken(token);
    }
    if (user) {
      this.authService.currentUser = user;
      this.authService.changeMemberPhoto(user.photoUrl);
    }
  }
}
