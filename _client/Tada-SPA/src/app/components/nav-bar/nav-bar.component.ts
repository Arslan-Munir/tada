import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  model: any = {};
  
  constructor(public authService: AuthService, private alert: AlertifyService) { }

  ngOnInit(): void {
  }

  login() {
    this.authService.login(this.model)
      .subscribe(next => {
        this.alert.success('Login successful');
      }, error => {
          this.alert.error(error);
      });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    this.alert.message('Logout successful');
  }
}
