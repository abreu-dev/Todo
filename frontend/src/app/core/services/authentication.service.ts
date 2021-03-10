import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { environment } from '@environments/environment';

import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root',
})
export class AuthenticationService {
    constructor(private router: Router, private http: HttpClient) {}

    public isLoggedIn(): boolean {
        if (localStorage.getItem('todo.user')) {
            return true;
        }
        return false;
    }

    public get currentUser(): any {
        const currentUser = localStorage.getItem('todo.user');

        if (currentUser) {
            return JSON.parse(currentUser);
        }

        return null;
    }

    public login(username: string, password: string) {
        return this.http
            .post<any>(`${environment.apiUrl}/accounts/login`, {
                username,
                password,
            })
            .pipe(
                map((response) => {
                    localStorage.setItem('todo.user', JSON.stringify(response));
                    return response;
                })
            );
    }

    public register(email: string, username: string, password: string) {
        return this.http
            .post<any>(`${environment.apiUrl}/accounts/register`, {
                email,
                username,
                password,
            })
            .pipe(
                map((response) => {
                    localStorage.setItem('todo.user', JSON.stringify(response));
                    return response;
                })
            );
    }

    public forgotPassword(username: string) {
        return this.http.post<any>(`${environment.apiUrl}/accounts/forgot-password`, {
            username,
        });
    }

    public resetPassword(username: string, newPassword: string, token: string) {
        return this.http.post<any>(`${environment.apiUrl}/accounts/reset-password`, {
            username,
            newPassword,
            token,
        });
    }
    public forgotUsername(email: string) {
        return this.http.post<any>(`${environment.apiUrl}/accounts/forgot-username`, {
            email,
        });
    }

    public logout() {
        localStorage.removeItem('todo.user');
        localStorage.clear();
        this.router.navigate(['/account/login']);
    }
}
