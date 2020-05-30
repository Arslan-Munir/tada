import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Message } from 'src/app/models/Message';
import { PaginatedResult, Pagination } from 'src/app/models/pagination';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages: Message[];
  pagination: Pagination;
  messagesType = 'unread';

  constructor(private authService: AuthService, private userService: UserService,
              private alert: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.messages = data.messages.result;
      this.pagination = data.messages.pagination;
    });

  }
  loadMessages(){
    this.userService.getMessages(this.authService.decodeToken().nameid, this.pagination.currentPage,
      this.pagination.itemsPerPage, this.messagesType)
      .subscribe((res: PaginatedResult<Message[]>) => {
        this.messages = res.result;
        this.pagination = res.pagination;
      }, error => {
          this.alert.error(error);
      });
  }

  deleteMessage(id: number) {
    this.alert.confirm('Are you sure you want to delete?', () => {
      this.userService.deleteMessage(id, this.authService.decodeToken().nameid)
        .subscribe(() => {
          this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
          this.alert.success('Message has been deleted');
        }, error => {
            this.alert.error('Failed to delete message.');
        });
    });
  }
  pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }
}
