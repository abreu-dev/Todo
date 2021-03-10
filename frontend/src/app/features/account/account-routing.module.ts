import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NoAuthGuard } from '@shared/guards/no-auth.guard';

import { AccountAppComponent } from './account.app.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ForgotUsernameComponent } from './forgot-username/forgot-username.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

const routes: Routes = [
    {
        path: '',
        component: AccountAppComponent,
        children: [
            {
                path: 'login',
                component: LoginComponent,
                canActivate: [NoAuthGuard],
            },
            {
                path: 'register',
                component: RegisterComponent,
                canActivate: [NoAuthGuard],
            },
            {
                path: 'forgot-password',
                component: ForgotPasswordComponent,
                canActivate: [NoAuthGuard],
            },
            {
                path: 'forgot-username',
                component: ForgotUsernameComponent,
                canActivate: [NoAuthGuard],
            },
            {
                path: 'reset-password',
                component: ResetPasswordComponent,
                canActivate: [NoAuthGuard],
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AccountRoutingModule {}
