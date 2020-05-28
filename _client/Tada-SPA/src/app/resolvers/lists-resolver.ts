import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { User } from '../models/user';
import { AlertifyService } from '../services/alertify.service';
import { UserService } from '../services/user.service';

@Injectable()
export class ListsResolver implements Resolve<User[]>{

    currentNumber = 1;
    itemsPerPage = 5;
    likesParams = 'likers';

    constructor(private userService: UserService, private alert: AlertifyService, private router: Router) { }

    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        return this.userService.getUsers(this.currentNumber, this.itemsPerPage, null, this.likesParams)
            .pipe(
                catchError(error => {
                    this.alert.error('Problem retrieving data.');
                    this.router.navigateByUrl('/');
                    return of(null);
                })
            );
    }

}
