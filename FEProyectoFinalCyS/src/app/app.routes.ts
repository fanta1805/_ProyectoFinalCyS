import { Routes } from '@angular/router';
import {LoginComponent} from "./shared/login/login.component";
import {RegistroComponent} from "./shared/registro/registro.component";

export const routes: Routes = [
  { path: '', redirectTo: '/registro', pathMatch: 'full' },  // Redirige a registro por defecto
  {path: 'login', component: LoginComponent},
  {path: 'registro', component: RegistroComponent}
];
