import { Routes } from '@angular/router';
import {LoginComponent} from "./shared/login/login.component";
import {RegistroComponent} from "./shared/registro/registro.component";
import {HomeComponent} from "./shared/home/home.component";
import {HomeUserComponent} from "./shared/home-user/home-user.component";

export const routes: Routes = [
  { path: '', redirectTo: '/registro', pathMatch: 'full' },  // Redirige a registro por defecto
  {path: 'login', component: LoginComponent},
  {path: 'registro', component: RegistroComponent},
  { path: 'home', component: HomeComponent },
  { path: 'home-user', component: HomeUserComponent },
];
