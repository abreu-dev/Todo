import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatMenuTrigger } from '@angular/material/menu';

@Component({
    selector: 'app-add-column-tile',
    templateUrl: './add-column-tile.component.html',
    styleUrls: ['./add-column-tile.component.scss'],
})
export class AddColumnTileComponent {
    @ViewChild(MatMenuTrigger) addColumnTileMenuTrigger!: MatMenuTrigger;
    @Input() public displayTitle!: string;
    @Output() submitData = new EventEmitter();
    public columnTitleFormControl = new FormControl('', Validators.required);

    public closeAddColumnTileMenu() {
        this.addColumnTileMenuTrigger.closeMenu();
    }

    public onSubmit() {
        if (this.columnTitleFormControl.valid) {
            this.submitData.emit(this.columnTitleFormControl.value);
            this.columnTitleFormControl.setValue('');
            this.closeAddColumnTileMenu();
        }
    }
}
