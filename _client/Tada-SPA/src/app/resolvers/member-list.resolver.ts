import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { User } from '../models/user';
import { AlertifyService } from '../services/alertify.service';
import { UserService } from '../services/user.service';

@Injectable()
export class MemberListResolver implements Resolve<User[]>{

    constructor(private userService: UserService, private alert: AlertifyService, private router: Router) { }

    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        return this.userService.getUsers()
            .pipe(
                catchError(error => {
                    this.alert.error('Problem retrieving data');
                    this.router.navigateByUrl('/');
                    return of(null);
                })
            );
    }

}
