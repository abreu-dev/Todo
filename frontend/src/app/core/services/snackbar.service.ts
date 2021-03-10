import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
    providedIn: 'root',
})
export class SnackbarService {
    private duration = 5000;
    constructor(private snackbar: MatSnackBar) {}

    public success(message: string): void {
        this.snackbar.open(message, 'Close', { duration: this.duration, panelClass: ['snackbar-success'] });
    }

    public error(message: string): void {
        this.snackbar.open(message, 'Close', { duration: this.duration, panelClass: ['snackbar-error'] });
    }
}
