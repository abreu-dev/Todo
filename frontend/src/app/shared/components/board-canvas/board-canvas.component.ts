import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { Board } from '@shared/models/board';

@Component({
    selector: 'app-board-canvas',
    templateUrl: './board-canvas.component.html',
    styleUrls: ['./board-canvas.component.scss'],
})
export class BoardCanvasComponent implements OnInit {
    @Input() public board!: Board;
    @Output() addColumnToBoard = new EventEmitter();
    @Output() addCardToColumn = new EventEmitter();
    @Output() updateCardPriorityInColumn = new EventEmitter();
    @Output() moveCardBetweenColumns = new EventEmitter();
    @Output() updateColumnTitle = new EventEmitter();
    @Output() updateColumnPositionInBoard = new EventEmitter();
    public addColumnTileDisplayTitle = 'Adicionar uma lista';

    ngOnInit(): void {
        this.addColumnTileDisplayTitle = this.board.columns.length === 0 ? 'Adicionar uma lista' : 'Adicionar outra lista';
    }

    public onAddColumnSubmited(columnTitle: string) {
        this.addColumnToBoard.emit(columnTitle);
    }

    public onUpdateColumnTitle(data: any) {
        this.updateColumnTitle.emit(data);
    }

    public onAddCardSubmited(data: any) {
        this.addCardToColumn.emit(data);
    }

    public onUpdateCardPriorityInColumn(data: any) {
        this.updateCardPriorityInColumn.emit(data);
    }

    public onMoveCardBetweenColumns(data: any) {
        this.moveCardBetweenColumns.emit(data);
    }

    public onUpdateColumnPositionInBoard(data: any) {
        const columnThatHasChanged = this.board.columns.find((x) => x.id === data.columnId);
        if (columnThatHasChanged !== undefined) {
            let sumPosition = 0;

            if (data.toLeft) {
                sumPosition = -1;
            }

            if (data.toRight) {
                sumPosition = 1;
            }

            const columnThatWasInThisPosition = this.board.columns.find(
                (x) => x.positionInBoard === columnThatHasChanged.positionInBoard + sumPosition
            );

            if (columnThatWasInThisPosition !== undefined) {
                columnThatWasInThisPosition.positionInBoard = columnThatWasInThisPosition.positionInBoard - sumPosition;
            }

            columnThatHasChanged.positionInBoard = columnThatHasChanged.positionInBoard + sumPosition;
        }

        this.board.columns.sort((a, b) => a.positionInBoard - b.positionInBoard);
        this.updateColumnPositionInBoard.emit({ columnId: data.columnId, newColumnPositionInBoard: columnThatHasChanged?.positionInBoard });
    }
}
