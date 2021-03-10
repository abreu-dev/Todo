/* eslint-disable max-len */
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

import { BoardService } from '@shared/services/board.service';
import { SharedModule } from '@shared/shared.module';

import { BoardManagementRoutingModule } from './board-management-routing.module';
import { BoardManagementComponent } from './board-management.component';

@NgModule({
    declarations: [BoardManagementComponent],
    imports: [CommonModule, RouterModule, BoardManagementRoutingModule, SharedModule, FormsModule, ReactiveFormsModule, MatButtonModule],
    providers: [BoardService],
})
export class BoardManagementModule {}
