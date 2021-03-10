import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RouterModule } from '@angular/router';

import { AuthGuard } from '@shared/guards/auth.guard';
import { BoardService } from '@shared/services/board.service';
import { SharedModule } from '@shared/shared.module';

import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';

@NgModule({
    declarations: [HomeComponent],
    imports: [CommonModule, HomeRoutingModule, RouterModule, SharedModule, FlexLayoutModule],
    providers: [AuthGuard, BoardService],
})
export class HomeModule {}
