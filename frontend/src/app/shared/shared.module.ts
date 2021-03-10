import { DragDropModule } from '@angular/cdk/drag-drop';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';

import { AddBoardTileComponent } from './components/add-board-tile/add-board-tile.component';
import { AddCardTileComponent } from './components/add-card-tile/add-card-tile.component';
import { AddColumnTileComponent } from './components/add-column-tile/add-column-tile.component';
import { BoardCanvasComponent } from './components/board-canvas/board-canvas.component';
import { BoardTileComponent } from './components/board-tile/board-tile.component';
import { ColumnCanvasComponent } from './components/column-canvas/column-canvas.component';
import { ColumnMenuActionsComponent } from './components/column-menu-actions/column-menu-actions.component';
import { EditableComponent } from './components/editable/editable.component';
import { AddBoardDialogComponent } from './dialogs/add-board-dialog/add-board-dialog.component';
import { EditModeDirective } from './directives/edit-mode.directive';
import { EditableOnEnterDirective } from './directives/editable-on-enter.directive';
import { ViewModeDirective } from './directives/view-mode.directive';

@NgModule({
    declarations: [
        EditModeDirective,
        ViewModeDirective,
        EditableOnEnterDirective,
        EditableComponent,
        BoardTileComponent,
        AddBoardTileComponent,
        AddBoardDialogComponent,
        BoardCanvasComponent,
        AddColumnTileComponent,
        ColumnCanvasComponent,
        AddCardTileComponent,
        ColumnMenuActionsComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatCardModule,
        MatDialogModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
        FlexLayoutModule,
        DragDropModule,
    ],
    exports: [
        EditModeDirective,
        ViewModeDirective,
        EditableOnEnterDirective,
        EditableComponent,
        BoardTileComponent,
        AddBoardTileComponent,
        AddBoardDialogComponent,
        BoardCanvasComponent,
        AddColumnTileComponent,
        ColumnCanvasComponent,
        AddCardTileComponent,
        ColumnMenuActionsComponent,
    ],
})
export class SharedModule {}
