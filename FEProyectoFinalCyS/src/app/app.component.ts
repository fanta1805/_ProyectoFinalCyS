import {Component, Injectable} from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './shared/login/login.component';
import { RegistroComponent } from './shared/registro/registro.component';
import {HttpClientModule} from "@angular/common/http";
import {ToastrModule, ToastrService} from "ngx-toastr";
import {RouterLink, RouterOutlet} from "@angular/router";


@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  imports: [
    CommonModule,
    LoginComponent,
    RegistroComponent,
    HttpClientModule,
    RouterLink,
    RouterOutlet
  ],
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'FEProyectoFinalCyS';
}
