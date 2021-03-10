import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import { AuthenticationService } from '@core/services/authentication.service';

@Injectable({
    providedIn: 'root',
})
export class AuthGuard implements CanActivate {
    constructor(private router: Router, private authenticationService: AuthenticationService) {}

    public canActivate(): boolean {
        if (this.authenticationService.isLoggedIn()) {
            return true;
        }

        this.router.navigate(['/account/login']);
        return false;
    }
}
