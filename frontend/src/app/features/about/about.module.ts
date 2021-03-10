import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';

import { AboutRoutingModule } from './about-routing.module';
import { AboutComponent } from './about.component';
import { AboutAccountManagementComponent } from './tabs/about-account-management/about-account-management.component';
import { AboutBackendComponent } from './tabs/about-backend/about-backend.component';
import { AboutBoardManagementComponent } from './tabs/about-board-management/about-board-management.component';
import { AboutFrontendComponent } from './tabs/about-frontend/about-frontend.component';
import { AboutVersioningComponent } from './tabs/about-versioning/about-versioning.component';

@NgModule({
    declarations: [
        AboutComponent,
        AboutBoardManagementComponent,
        AboutAccountManagementComponent,
        AboutBackendComponent,
        AboutFrontendComponent,
        AboutVersioningComponent,
    ],
    imports: [CommonModule, AboutRoutingModule, MatIconModule, MatTabsModule],
})
export class AboutModule {}
