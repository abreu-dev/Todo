import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '@shared/guards/auth.guard';

import { BoardManagementComponent } from './board-management.component';

const routes: Routes = [
    {
        path: '',
        component: BoardManagementComponent,
        canActivate: [AuthGuard],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BoardManagementRoutingModule {}
