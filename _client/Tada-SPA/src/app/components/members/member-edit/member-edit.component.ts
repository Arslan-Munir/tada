import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/models/user';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  @ViewChild('editForm', { static: true }) editForm: NgForm;
  user: User;
  photoUrl: string;

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private userService: UserService, private authService: AuthService,
              private alert: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.user = data.user;
    });

    this.authService.photoUrl.subscribe(url => this.photoUrl = url);
  }

  updateUser() {
    this.userService.update(this.authService.decodeToken().nameid, this.user)
      .subscribe(next => {
        this.alert.success('Profile updated');
        this.editForm.reset(this.user);
      }, error => {
          this.alert.error(error);
      });
  }

  updateMainPhoto(photoUrl: string) {
    this.user.photoUrl = photoUrl;
  }
}
