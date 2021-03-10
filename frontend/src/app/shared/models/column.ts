import { Card } from './card';

export interface Column {
    id: string;
    title: string;
    positionInBoard: number;
    cards: Card[];
    boardId: string;
}
