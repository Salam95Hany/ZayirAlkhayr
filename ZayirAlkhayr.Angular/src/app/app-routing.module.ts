import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { NotAuthorizedComponent } from './Auth/not-authorized/not-authorized.component';
import { CreateOrderComponent } from './Admin/Shared/create-order/create-order.component';
import { AuthGuard } from './Auth/auth.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'not-authorized', component: NotAuthorizedComponent, canActivate: [AuthGuard] },
  { path: 'create-order', component: CreateOrderComponent },
  { path: 'admin', loadChildren: () => import('../app/Admin/admin.module').then(m => m.AdminModule) },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
