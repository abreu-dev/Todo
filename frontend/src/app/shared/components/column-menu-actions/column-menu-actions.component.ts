import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { MatMenuTrigger } from '@angular/material/menu';

@Component({
    selector: 'app-column-menu-actions',
    templateUrl: './column-menu-actions.component.html',
    styleUrls: ['./column-menu-actions.component.scss'],
})
export class ColumnMenuActionsComponent {
    @ViewChild(MatMenuTrigger) columnMenuActionsMenuTrigger!: MatMenuTrigger;
    @Input() public canMoveToLeft!: boolean;
    @Input() public canMoveToRight!: boolean;
    @Output() updateColumnPositionInBoard = new EventEmitter();

    public closeColumnMenuActions(): void {
        this.columnMenuActionsMenuTrigger.closeMenu();
    }

    public onUpdateColumnPositionInBoard(toRight: boolean, toLeft: boolean) {
        this.updateColumnPositionInBoard.emit({ toRight, toLeft });
        this.closeColumnMenuActions();
    }

    public moveColumnToLeft(): void {
        this.onUpdateColumnPositionInBoard(false, true);
    }

    public moveColumnToRight(): void {
        this.onUpdateColumnPositionInBoard(true, false);
    }
}
