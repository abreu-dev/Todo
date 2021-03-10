import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        redirectTo: '/home',
        pathMatch: 'full',
    },
    {
        path: 'home',
        loadChildren: () => import('./features/home/home.module').then((m) => m.HomeModule),
    },
    {
        path: 'account',
        loadChildren: () => import('./features/account/account.module').then((m) => m.AccountModule),
    },
    {
        path: 'board/:id',
        loadChildren: () => import('./features/board-management/board-management.module').then((m) => m.BoardManagementModule),
    },
    {
        path: 'about',
        loadChildren: () => import('./features/about/about.module').then((m) => m.AboutModule),
    },
    {
        path: '**',
        redirectTo: '/home',
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {}
