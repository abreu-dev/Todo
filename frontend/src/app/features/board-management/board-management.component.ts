import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

import { Board } from '@shared/models/board';
import { BoardService } from '@shared/services/board.service';

@Component({
    selector: 'app-board-management',
    templateUrl: './board-management.component.html',
    styleUrls: ['./board-management.component.scss'],
})
export class BoardManagementComponent implements OnInit {
    public board!: Board;
    public id!: string;

    public boardTitleFormControl!: FormControl;

    constructor(private route: ActivatedRoute, private boardService: BoardService, private titleService: Title) {}

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');

        if (id !== null) {
            this.id = id;
            this.reloadBoard();
        }
    }

    public setTitle(): void {
        this.titleService.setTitle(`Todo | ${this.board.title}`);
    }

    public reloadBoard(): void {
        this.boardService.getBoardById(this.id).subscribe((board) => {
            if (board !== null) {
                this.board = board;
                this.boardTitleFormControl = new FormControl(this.board.title, Validators.required);
                this.setTitle();
            }
        });
    }

    public updateBoardTitle(): void {
        if (this.boardTitleFormControl != null && this.boardTitleFormControl.valid) {
            if (this.board.title !== this.boardTitleFormControl.value) {
                this.board.title = this.boardTitleFormControl.value;
                this.boardService.updateBoardTitle(this.board.id, this.boardTitleFormControl.value).subscribe();
                this.setTitle();
            }
        }
    }

    public addColumnToBoard(columnTitle: string): void {
        this.boardService.addColumnToBoard(this.board.id, columnTitle).subscribe(() => this.reloadBoard());
    }

    public updateColumnTitle(data: any): void {
        this.boardService.updateColumnTitle(this.board.id, data.columnId, data.newColumnTitle).subscribe();
    }

    public addCardToColumn(data: any): void {
        this.boardService.addCardToColumn(this.board.id, data.columnId, data.cardTitle).subscribe(() => this.reloadBoard());
    }

    public updateCardPriorityInColumn(data: any): void {
        this.boardService.updateCardPriorityInColumn(this.board.id, data.columnId, data.cardId, data.newCardPriorityInColumn).subscribe();
    }

    public moveCardBetweenColumns(data: any): void {
        this.boardService
            .moveCardBetweenColumns(this.board.id, data.fromColumnId, data.toColumnId, data.cardId, data.newCardPriorityInColumn)
            .subscribe();
    }

    public updateColumnPositionInBoard(data: any): void {
        this.boardService.updateColumnPositionInBoard(this.board.id, data.columnId, data.newColumnPositionInBoard).subscribe();
    }
}
