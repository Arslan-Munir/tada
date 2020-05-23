import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';

@Component({
  selector: 'member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  // tslint:disable-next-line: no-input-rename
  @Input('user') user: User;

  constructor() { }

  ngOnInit(): void {
  }

}
