import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AlertifyService } from '../services/alertify.service';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private alert: AlertifyService, private router: Router) { }
  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      return true;
    }

    this.alert.error('Please login to continue.');
    this.router.navigateByUrl('/');
    return false;
  }

}
