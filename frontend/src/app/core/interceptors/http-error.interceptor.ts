import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { AuthenticationService } from '@core/services/authentication.service';
import { SnackbarService } from '@core/services/snackbar.service';

import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
    constructor(private snackbarService: SnackbarService, private authenticationService: AuthenticationService) {}

    public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((err) => {
                if (err.status === 401) {
                    this.authenticationService.logout();
                    return throwError(err);
                }

                let error = err.error.message || err.error.title || err.error.Error;

                if (err.status === 0) {
                    error = 'Não foi possível conectar-se ao servidor.';
                }

                if (err.status === 400) {
                    error = err.error.Error || err.error.errors[0];
                }

                this.snackbarService.error(error);
                return throwError(error);
            })
        );
    }
}
