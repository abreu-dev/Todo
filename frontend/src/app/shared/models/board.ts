import { Column } from './column';

export interface Board {
    id: string;
    title: string;
    columns: Column[];
}
