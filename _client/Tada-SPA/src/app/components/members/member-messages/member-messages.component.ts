import { Component, Input, OnInit } from '@angular/core';
import { tap } from 'rxjs/operators';
import { Message } from 'src/app/models/Message';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {

  // tslint:disable-next-line: no-input-rename
  @Input('receiver-id') receiverId: number;
  messages: Message[];

  newMessage: any = {};
  constructor(private authService: AuthService, private userService: UserService, private alert: AlertifyService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    const currentUserId = +this.authService.decodeToken().nameid;

    this.userService.getMessageThread(this.authService.decodeToken().nameid, this.receiverId)
      .pipe(
        tap(messages => {
          for (const message of messages) {
            if (!message.isRead && message.receiverId === currentUserId) {
              this.userService.markAsRead(message.id, currentUserId);
             }
          }
        })
      )
      .subscribe(messages => {
        this.messages = messages;
      }, error => {
          this.alert.error(error);
      });
  }

  sendMessage() {
    this.newMessage.receiverId = this.receiverId;
    this.userService.sendMessage(this.authService.decodeToken().nameid, this.newMessage)
      .subscribe((message: Message) => {
        this.messages.push(message);
        this.newMessage.content = '';
      }, error => {
          this.alert.error(error);
      });
  }
}
