import { Component, EventEmitter, OnInit, Output } from '@angular/core';
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

  constructor(private authService: AuthService) {
  }

  ngOnInit(): void {
  }

  register() {
    this.authService.register(this.model)
      .subscribe(() => {
        console.log('Registration successful.');
      }, errors => {
          console.log(errors);
    });
  }

  cancel() {
    this.cancelMode.emit(false);
    console.log('canceled');
  }
}
