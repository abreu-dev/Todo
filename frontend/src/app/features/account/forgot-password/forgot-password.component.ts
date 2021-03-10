import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';

import { AuthenticationService } from '@core/services/authentication.service';
import { SnackbarService } from '@core/services/snackbar.service';

@Component({
    selector: 'app-forgot-password',
    templateUrl: './forgot-password.component.html',
    styleUrls: ['./forgot-password.component.scss'],
})
export class ForgotPasswordComponent implements OnInit {
    public form!: FormGroup;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private authenticationService: AuthenticationService,
        private snackbarService: SnackbarService,
        private titleService: Title
    ) {
        this.titleService.setTitle('Todo | Recuperar senha');
    }

    public ngOnInit(): void {
        this.form = this.formBuilder.group({
            username: ['', Validators.required],
        });
    }

    public onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.authenticationService.forgotPassword(this.form.controls.username.value).subscribe({
            next: () => {
                this.router
                    .navigate(['/account/login'])
                    .then(() => this.snackbarService.success('Por favor, cheque a caixa de entrada do seu email.'));
            },
            error: () => {},
        });
    }
}
