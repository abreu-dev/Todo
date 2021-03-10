import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'app-add-board-dialog',
    templateUrl: './add-board-dialog.component.html',
    styleUrls: ['./add-board-dialog.component.scss'],
})
export class AddBoardDialogComponent implements OnInit {
    public form!: FormGroup;

    constructor(private dialogRef: MatDialogRef<AddBoardDialogComponent>, private formBuilder: FormBuilder) {}

    public ngOnInit(): void {
        this.form = this.formBuilder.group({
            boardTitle: ['', Validators.required],
        });
    }

    public onSubmit(): void {
        if (this.form.dirty && this.form.valid) {
            this.dialogRef.close({ boardTitle: this.form.value.boardTitle });
            return;
        }

        this.form.markAllAsTouched();
    }
}
