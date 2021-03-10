import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatMenuTrigger } from '@angular/material/menu';

@Component({
    selector: 'app-add-card-tile',
    templateUrl: './add-card-tile.component.html',
    styleUrls: ['./add-card-tile.component.scss'],
})
export class AddCardTileComponent {
    @ViewChild(MatMenuTrigger) addCardTileMenuTrigger!: MatMenuTrigger;
    @Input() public displayTitle!: string;
    @Output() submitData = new EventEmitter();
    public cardTitleFormControl = new FormControl('', Validators.required);

    public closeAddCardTileMenu() {
        this.addCardTileMenuTrigger.closeMenu();
    }

    public onSubmit() {
        if (this.cardTitleFormControl.valid) {
            this.submitData.emit(this.cardTitleFormControl.value);
            this.cardTitleFormControl.setValue('');
            this.closeAddCardTileMenu();
        }
    }
}
