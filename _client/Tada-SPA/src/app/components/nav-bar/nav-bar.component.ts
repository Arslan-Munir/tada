import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  model: any = {};
  photoUrl: string;

  constructor(public authService: AuthService, private alert: AlertifyService, private route: Router) { }

  ngOnInit(): void {
    this.authService.photoUrl.subscribe(url => this.photoUrl = url);
  }

  login() {
    this.authService.login(this.model)
      .subscribe(next => {
        this.alert.success('Login successful');
      }, error => {
          this.alert.error(error);
      }, () => {
          this.route.navigateByUrl('/members');
      });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentUser = null;

    this.alert.message('Logout successful');
    this.route.navigateByUrl('/');
  }
}
