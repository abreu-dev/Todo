import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthenticationService } from '@core/services/authentication.service';
import { SnackbarService } from '@core/services/snackbar.service';

@Component({
    selector: 'app-reset-password',
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {
    public form!: FormGroup;
    public hidePassword = true;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private authenticationService: AuthenticationService,
        private snackbarService: SnackbarService,
        private titleService: Title
    ) {
        this.titleService.setTitle('Todo | Alterar senha');
    }

    public ngOnInit(): void {
        this.route.queryParams.subscribe((params: any) => {
            this.form = this.formBuilder.group({
                username: [params.username],
                newPassword: ['', Validators.required],
                token: [params.token],
            });
        });
    }

    public onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.authenticationService
            .resetPassword(this.form.controls.username.value, this.form.controls.newPassword.value, this.form.controls.token.value)
            .subscribe({
                next: () => {
                    this.router.navigate(['/account/login']).then(() => this.snackbarService.success('Senha alterada com sucesso!'));
                },
                error: () => {},
            });
    }
}
