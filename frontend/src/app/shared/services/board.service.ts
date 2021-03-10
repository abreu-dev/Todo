import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '@environments/environment';

import { Board } from '@shared/models/board';

import { Observable } from 'rxjs';

@Injectable()
export class BoardService {
    constructor(private http: HttpClient) {}

    public getAllBoards(): Observable<Board[]> {
        return this.http.get<Board[]>(`${environment.apiUrl}/boards`);
    }

    public getBoardById(boardId: string): Observable<Board> {
        return this.http.get<Board>(`${environment.apiUrl}/boards/${boardId}`);
    }

    public addBoard(boardTitle: string): Observable<any> {
        return this.http.post(`${environment.apiUrl}/boards`, { boardTitle });
    }

    public updateBoardTitle(boardId: string, newBoardTitle: string): Observable<any> {
        return this.http.put(`${environment.apiUrl}/boards/title`, { boardId, newBoardTitle });
    }

    public addColumnToBoard(boardId: string, columnTitle: string): Observable<any> {
        return this.http.post(`${environment.apiUrl}/boards/columns`, { boardId, columnTitle });
    }

    public updateColumnTitle(boardId: string, columnId: string, newColumnTitle: string): Observable<any> {
        return this.http.put(`${environment.apiUrl}/boards/columns/title`, { boardId, columnId, newColumnTitle });
    }

    public updateColumnPositionInBoard(boardId: string, columnId: string, newColumnPositionInBoard: number) {
        return this.http.put(`${environment.apiUrl}/boards/columns/position-in-board`, { boardId, columnId, newColumnPositionInBoard });
    }

    public addCardToColumn(boardId: string, columnId: string, cardTitle: string): Observable<any> {
        return this.http.post(`${environment.apiUrl}/boards/cards`, { boardId, columnId, cardTitle });
    }

    public updateCardPriorityInColumn(boardId: string, columnId: string, cardId: string, newCardPriorityInColumn: number): Observable<any> {
        return this.http.put(`${environment.apiUrl}/boards/cards/priority`, { boardId, columnId, cardId, newCardPriorityInColumn });
    }

    public moveCardBetweenColumns(
        boardId: string,
        fromColumnId: string,
        toColumnId: string,
        cardId: string,
        cardPriorityInColumn: number
    ): Observable<any> {
        return this.http.put(`${environment.apiUrl}/boards/cards/move-to-column`, {
            boardId,
            fromColumnId,
            toColumnId,
            cardId,
            cardPriorityInColumn,
        });
    }
}
