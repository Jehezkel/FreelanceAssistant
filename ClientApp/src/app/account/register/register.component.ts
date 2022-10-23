import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiClientService } from '../../_services/api-client.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  RegisterForm: FormGroup;
  ShowCompleteMsg: boolean = false;
  constructor(
    private apiClient: ApiClientService,
    fb: FormBuilder,
    private router: Router
  ) {
    this.RegisterForm = fb.group({
      userName: ['', Validators.required],
      email: ['', [Validators.email, Validators.required]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      rePassword: ['', Validators.required],
    });
  }

  ngOnInit(): void {}
  submitRegister() {
    this.apiClient
      .register(
        this.RegisterForm.controls.userName.value,
        this.RegisterForm.controls.email.value,
        this.RegisterForm.controls.password.value
      )
      .subscribe({
        next: (result) => {
          this.finalizeRegistration();
        },
        error: (error) => console.log('error occured ', error),
      });
  }
  finalizeRegistration() {
    this.ShowCompleteMsg = true;
    setTimeout(() => this.router.navigateByUrl('/login'), 5000);
  }
}
