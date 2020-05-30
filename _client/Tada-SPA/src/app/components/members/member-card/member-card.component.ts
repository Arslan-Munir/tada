import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  // tslint:disable-next-line: no-input-rename
  @Input('user') user: User;

  constructor(private authService: AuthService, private userService: UserService, private alert: AlertifyService) { }

  ngOnInit(): void {
  }

  like() {
    const likerId = this.authService.decodeToken().nameid;
    this.userService.sendLike(likerId, this.user.id)
      .subscribe(data => {
        this.alert.success('Liked ' + this.user.knownAs + '!');
      }, error => {
        this.alert.error(error);
      });
  }
}
