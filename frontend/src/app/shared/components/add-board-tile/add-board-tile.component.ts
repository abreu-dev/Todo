import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-add-board-tile',
    templateUrl: './add-board-tile.component.html',
    styleUrls: ['./add-board-tile.component.scss'],
})
export class AddBoardTileComponent {
    @Input() public title!: string;
}
