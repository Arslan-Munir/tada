import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { User } from 'src/app/models/user';
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
  user: User;
  registrationForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private authService: AuthService, private alert: AlertifyService, private fb: FormBuilder, private router: Router) {
  }

  ngOnInit(): void {
    this.bsConfig = {
      containerClass: 'theme-red',
      adaptivePosition: true,
      isAnimated: true
    };

    this.buildRegistrationForm();
    // this.registrationForm = new FormGroup({
    //   username: new FormControl('', Validators.required),
    //   password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
    //   confirmPassword: new FormControl('', Validators.required)
    // }, this.passwordMatchValidator);
  }

  buildRegistrationForm() {
    this.registrationForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, {validators: this.passwordMatchValidator});
  }
  passwordMatchValidator(group: FormGroup) {
    return group.get('password').value === group.get('confirmPassword').value ? null : { mismatch: true };
  }

  register() {
    if (this.registrationForm.valid) {
      this.user = Object.assign({}, this.registrationForm.value);
      this.authService.register(this.user)
        .subscribe(() => {
          this.alert.success('Registration successful.');
        }, errors => {
            this.alert.error(errors);
        }, () => {
            this.authService.login(this.user)
              .subscribe(() => {
                this.router.navigateByUrl('/members');
              });
        });
    }
  }

  cancel() {
    this.cancelMode.emit(false);
    this.alert.message('Canceled!');
  }

  invalidKnownAs() {
    return this.registrationForm.get('knownAs').errors
      && this.registrationForm.get('knownAs').touched;
  }

  invalidDateOfBirth() {
    return this.registrationForm.get('dateOfBirth').errors
      && this.registrationForm.get('dateOfBirth').touched;
  }

  invalidCity() {
    return this.registrationForm.get('city').errors
      && this.registrationForm.get('city').touched;
  }

  invalidCountry() {
    return this.registrationForm.get('country').errors
      && this.registrationForm.get('country').touched;
  }

  invalidUserName() {
    return this.registrationForm.get('username').errors
      && this.registrationForm.get('username').touched;
  }

  invalidPassword() {
    return this.registrationForm.get('password').errors
      && this.registrationForm.get('password').touched;
  }

  passwordMissMatch() {
    return (
      this.registrationForm.hasError('mismatch'))
        || (this.registrationForm.get('confirmPassword').touched
            && this.registrationForm.get('confirmPassword').errors);
  }
}
