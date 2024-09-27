import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {RegistroService} from "../../services/registro.service";
import {HttpClientModule} from "@angular/common/http";
import {ToastrModule, ToastrService} from "ngx-toastr";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-registro',
  standalone: true,
  templateUrl: './registro.component.html',
  styleUrls: ['./registro.component.css'],
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterLink] // Importa módulos necesarios
})
export class RegistroComponent {
  registroForm: FormGroup

  constructor(private fb: FormBuilder, private _registro: RegistroService, private toastr: ToastrService) {
    this.registroForm = this.fb.group({
      Name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      contrasena: ['', [Validators.required, Validators.minLength(8)]],
      Rol: ['', [Validators.required, Validators.pattern('^(ADMIN|USUARIO)$')]],
      piso: ['', Validators.required]
    })
  }

  registrarUsuario(){
    const registro: any = {
      Name: this.registroForm.get('Name')?.value,
      email: this.registroForm.get('email')?.value,
      contrasena: this.registroForm.get('contrasena')?.value,
      Rol: this.registroForm.get('Rol')?.value,
      piso: this.registroForm.get('piso')?.value
    }

    this._registro.postRegistrarUsuario(registro).subscribe(data =>{

      this.toastr.success('Usuario Registrado con exito!', 'Exito!')
      this.registroForm.reset();


      }, error => {
        this.toastr.error('Opss.. Ha ocurrido un error al registrar al usuario', 'Error!')
        console.log(error);


      }
    );

    this.registroForm.reset();

  }
}
