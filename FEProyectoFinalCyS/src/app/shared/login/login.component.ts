import { Component } from '@angular/core';
import {Router} from "@angular/router";
import { CommonModule } from '@angular/common';
import {HttpClientModule} from "@angular/common/http";
import {RouterLink} from "@angular/router";
import {LoginService} from "../../services/login.service";
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {ToastrService} from "ngx-toastr";


@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [CommonModule, HttpClientModule, RouterLink, FormsModule, ReactiveFormsModule] // Importa módulos necesarios
})
export class LoginComponent {
  inicioSesionForm: FormGroup

  constructor(private fb: FormBuilder, private _login: LoginService , private toastr: ToastrService, private router: Router) {
    this.inicioSesionForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      contrasena: ['', [Validators.required, Validators.minLength(8)]],
      Rol: ['', [Validators.required, Validators.pattern('^(ADMIN|USUARIO)$')]]
    })
  }

  iniciarSesion() {
    const login: any = {
      email: this.inicioSesionForm.get('email')?.value,
      contrasena: this.inicioSesionForm.get('contrasena')?.value,
      Rol: this.inicioSesionForm.get('Rol')?.value
    }

    this._login.postLoginUsuario(login).subscribe(data =>{
      if(data){
        this.toastr.success('Usuario inicio sesion', 'Exito!')
        if(login.Rol === "ADMIN"){
          this.router.navigate(['/home-user'])
          this.inicioSesionForm.reset();
        }if(login.Rol === "USER"){
          this.router.navigate(['/home'])
        }



      }else{
        this.toastr.error('Credenciales incorrectas', 'Error');
      }
      }, error => {
        this.toastr.error('Opss.. Ha ocurrido un error al iniciar sesion', 'Error!')
        console.log(error);

      }
    );
  }

}






