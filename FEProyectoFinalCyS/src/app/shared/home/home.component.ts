import { Component, OnInit } from '@angular/core';
import {ReservaService} from "../../services/reserva.service";
import {CommonModule} from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import {RouterLink} from "@angular/router";
import {error} from "@angular/compiler-cli/src/transformers/util";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  listReservas: any[] = [];
  listSalas: any[] = [];
  reservasTemp: any[] = [];

  reservaForm: FormGroup

  constructor(private fb: FormBuilder,private _reservaService: ReservaService, private toastr: ToastrService) {
    this.reservaForm = this.fb.group({
      SalaId: ['', Validators.required],
      UsuarioId: ['', Validators.required],
      HoraInicio: ['', Validators.required],
      HoraFin: ['', Validators.required],
      Prioridad: ['', Validators.required],
      capacidad: ['', Validators.required]
    })
  }

  ngOnInit(): void {
    this.obtenerReservas();
    this.obtenerSalas();
  }


  isSending = false;

  realizarReserva() {
    if (this.reservasTemp.length > 0) {
      this.isSending = true; // Deshabilita el botón
      this._reservaService.postReservar(this.reservasTemp).subscribe(
        (response: any) => {
          this.toastr.success(response.message, 'Éxito!');
          this.obtenerReservas();
          this.reservasTemp = []; // Limpia la lista temporal después de enviar
          this.isSending = false; // Habilita el botón
        },
        (error: any) => {
          this.reservasTemp = []
          this.toastr.error('Opss.. Ha ocurrido un error al enviar las reservas', 'Error!');
          console.log('Error en el envío de reservas: ', error);
          this.isSending = false; // Habilita el botón
        }
      );
    } else {
      this.toastr.warning('No hay reservas para enviar', 'Atención!');
    }
  }




  obtenerReservas() {
    this._reservaService.getListaReservas().subscribe(data =>{
        console.log(data);
        this.listReservas = data;
      }, error => {
        console.log(error);
      }
    );
  }

  obtenerSalas(){
    this._reservaService.getListaSalas().subscribe(data=>{
        this.listSalas = data;
      },error=>{
        console.log(error);
      }
    )
  }


  anadirReserva() {
    console.log("Añadir reserva llamado")
    if (this.reservaForm.valid) {
      const reserva: any = {
        SalaId: this.reservaForm.get('SalaId')?.value,
        UsuarioId: this.reservaForm.get('UsuarioId')?.value,
        HoraInicio: new Date(this.reservaForm.get('HoraInicio')?.value).toISOString(),
        HoraFin: new Date(this.reservaForm.get('HoraFin')?.value).toISOString(),
        Prioridad: this.reservaForm.get('Prioridad')?.value,
        capacidad: this.reservaForm.get('capacidad')?.value
      };

      this.reservasTemp.push(reserva);
      this.toastr.success('Reserva añadida a la lista temporal', 'Éxito!');
      this.reservaForm.reset();
    } else {
      this.toastr.error('Por favor, complete todos los campos requeridos.', 'Error!');
    }
  }
}
