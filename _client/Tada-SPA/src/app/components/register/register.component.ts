import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  // tslint:disable-next-line: no-output-rename
  @Output('cancel-mode') cancelMode = new EventEmitter();
  model: any = {};

  constructor(private authService: AuthService, private alert: AlertifyService) {
  }

  ngOnInit(): void {
  }

  register() {
    this.authService.register(this.model)
      .subscribe(() => {
        this.alert.success('Registration successful.');
      }, errors => {
          this.alert.error(errors);
    });
  }

  cancel() {
    this.cancelMode.emit(false);
    this.alert.message('Canceled!');
  }
}
