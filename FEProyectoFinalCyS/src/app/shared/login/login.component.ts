import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpClientModule} from "@angular/common/http";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [CommonModule, HttpClientModule, RouterLink] // Importa módulos necesarios
})
export class LoginComponent {
  // Aquí puedes agregar la lógica del componente de login
}
