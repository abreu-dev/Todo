import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

import { Card } from '@shared/models/card';
import { Column } from '@shared/models/column';

@Component({
    selector: 'app-column-canvas',
    templateUrl: './column-canvas.component.html',
    styleUrls: ['./column-canvas.component.scss'],
})
export class ColumnCanvasComponent implements OnInit {
    @Input() public column!: Column;
    @Input() public isLastColumn!: boolean;
    public columnTitleFormControl!: FormControl;
    @Output() addCardToBoard = new EventEmitter();
    @Output() updateCardPriorityInColumn = new EventEmitter();
    @Output() moveCardBetweenColumns = new EventEmitter();
    @Output() updateColumnTitle = new EventEmitter();
    @Output() updateColumnPositionInBoard = new EventEmitter();
    public addCardTileDisplayTitle = 'Adicionar cartão';

    ngOnInit(): void {
        this.columnTitleFormControl = new FormControl(this.column.title, Validators.required);
    }

    public onUpdateColumnTitle(): void {
        if (this.columnTitleFormControl.valid && this.columnTitleFormControl.value !== this.column.title) {
            this.updateColumnTitle.emit({ columnId: this.column.id, newColumnTitle: this.columnTitleFormControl.value });
            this.column.title = this.columnTitleFormControl.value;
        }
    }

    public onAddCardSubmited(cardTitle: string) {
        this.addCardToBoard.emit({ cardTitle, columnId: this.column.id });
    }

    public onUpdateColumnPositionInBoard(data: any) {
        this.updateColumnPositionInBoard.emit({
            columnId: this.column.id,
            toRight: data.toRight,
            toLeft: data.toLeft,
        });
    }

    public moveCard(event: CdkDragDrop<Card[]>, columnId: string): void {
        if (event.previousContainer === event.container) {
            if (event.previousIndex === event.currentIndex) {
                return;
            }

            moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
            this.updateCardPriorityInColumn.emit({
                columnId: this.column.id,
                cardId: event.container.data[event.currentIndex].id,
                newCardPriorityInColumn: event.currentIndex + 1,
            });
        } else {
            transferArrayItem(event.previousContainer.data, event.container.data, event.previousIndex, event.currentIndex);
            this.moveCardBetweenColumns.emit({
                fromColumnId: event.container.data[event.currentIndex].columnId,
                toColumnId: columnId,
                cardId: event.container.data[event.currentIndex].id,
                newCardPriorityInColumn: event.currentIndex + 1,
            });
            event.container.data[event.currentIndex].columnId = columnId;
        }
    }
}
