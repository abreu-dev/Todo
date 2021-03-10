import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Title } from '@angular/platform-browser';

import { AddBoardDialogComponent } from '@shared/dialogs/add-board-dialog/add-board-dialog.component';
import { Board } from '@shared/models/board';
import { BoardService } from '@shared/services/board.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
    public boards: Board[] = [];

    constructor(private boardService: BoardService, private dialog: MatDialog, private titleService: Title) {
        this.titleService.setTitle('Todo | Quadros');
    }

    public ngOnInit(): void {
        this.realoadBoards();
    }

    public realoadBoards(): void {
        this.boardService.getAllBoards().subscribe((boards) => {
            this.boards = boards;
        });
    }

    public openAddBoardDialog(): void {
        const dialogRef = this.dialog.open(AddBoardDialogComponent, {
            position: {
                top: '65px',
            },
        });

        dialogRef.afterClosed().subscribe((result: any) => {
            if (result) {
                this.boardService.addBoard(result.boardTitle).subscribe(() => this.realoadBoards());
            }
        });
    }
}
