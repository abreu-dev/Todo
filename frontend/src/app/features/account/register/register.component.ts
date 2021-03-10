import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { AuthenticationService } from '@core/services/authentication.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
    public form!: FormGroup;
    public hidePassword = true;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private authenticationService: AuthenticationService,
        private titleService: Title
    ) {
        this.titleService.setTitle('Todo | Criar Conta');
    }

    public ngOnInit(): void {
        this.form = this.formBuilder.group({
            email: ['', Validators.required],
            username: ['', Validators.required],
            password: ['', Validators.required],
        });
    }

    public onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.authenticationService
            .register(this.form.controls.email.value, this.form.controls.username.value, this.form.controls.password.value)
            .subscribe({
                next: () => {
                    this.router.navigate(['/home']);
                },
                error: () => {},
            });
    }
}
