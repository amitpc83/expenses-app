import { Routes } from '@angular/router';
import { TransactionListComponent } from './components/transaction-list/transaction-list';
import { Login } from './components/login/login';
import { Signup } from './components/signup/signup';
import { TransactionFormComponent } from './components/transaction-form/transaction-form';
import { authGuard } from './guards/auth.guard';
import { guestGuard } from './guards/guest.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/transactions', pathMatch: 'full' },
  { path: 'transactions', component: TransactionListComponent, canActivate: [authGuard] },
  { path: 'login', component: Login, canActivate: [guestGuard] },
  { path: 'signup', component: Signup, canActivate: [guestGuard] },
  { path: 'create', component: TransactionFormComponent, canActivate: [authGuard] },
  { path: 'edit/:id', component: TransactionFormComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: '/transactions' },
];
