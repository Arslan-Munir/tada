import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../models/Message';
import { AlertifyService } from '../services/alertify.service';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';

@Injectable()
export class MessagesResolver implements Resolve<Message[]>{

    currentNumber = 1;
    itemsPerPage = 5;
    messagesType = 'unread';

    constructor(private authService: AuthService, private userService: UserService,
                private alert: AlertifyService, private router: Router) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
        return this.userService.getMessages(this.authService.decodeToken().nameid, this.currentNumber,
            this.itemsPerPage, this.messagesType)
            .pipe(
                catchError(error => {
                    this.alert.error('Problem in retrieving messages.');
                    this.router.navigateByUrl('/');
                    return of(null);
                })
            );
    }

}
