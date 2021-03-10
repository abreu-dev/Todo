import { Component, ViewChild } from '@angular/core';
import { MatMenuTrigger } from '@angular/material/menu';

import { AuthenticationService } from '@core/services/authentication.service';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
    @ViewChild(MatMenuTrigger) accountMenuTrigger!: MatMenuTrigger;

    public username = '';
    public email = '';

    constructor(private authenticationService: AuthenticationService) {}

    public isLoggedIn(): boolean {
        const username = this.authenticationService.currentUser?.username;
        const email = this.authenticationService.currentUser?.email;

        if (username && email) {
            this.username = username;
            this.email = email;
        }

        return this.authenticationService.isLoggedIn();
    }

    public onLogout(): void {
        this.authenticationService.logout();
    }

    public closeAccountMenu(): void {
        this.accountMenuTrigger.closeMenu();
    }
}
