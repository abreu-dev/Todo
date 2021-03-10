import { Component, Input } from '@angular/core';

import { Board } from '@shared/models/board';

@Component({
    selector: 'app-board-tile',
    templateUrl: './board-tile.component.html',
    styleUrls: ['./board-tile.component.scss'],
})
export class BoardTileComponent {
    @Input() public board!: Board;
}
