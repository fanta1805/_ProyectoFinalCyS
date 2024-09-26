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
  accion = "Agregar";
  id: number | undefined;

  reservaForm: FormGroup

  constructor(private fb: FormBuilder,private _reservaService: ReservaService, private toastr: ToastrService) {
    this.reservaForm = this.fb.group({
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

  guardarReserva() {
    if (this.reservaForm.valid) {
      const reserva: any = {
        UsuarioId: this.reservaForm.get('UsuarioId')?.value,
        HoraInicio: new Date(this.reservaForm.get('HoraInicio')?.value).toISOString(),
        HoraFin: new Date(this.reservaForm.get('HoraFin')?.value).toISOString(),
        Prioridad: this.reservaForm.get('Prioridad')?.value,
        capacidad: this.reservaForm.get('capacidad')?.value
      };

      if (this.id === undefined) {
        // Si el id no está definido, creamos una nueva reserva
        this.reservasTemp.push(reserva);
        this.isSending = true; // Deshabilitar botón
        this._reservaService.postReservar(this.reservasTemp).subscribe(
          (response: any) => {
            this.toastr.success(response.message, 'Éxito!');
            this.obtenerReservas();
            this.reservasTemp = []; // Limpiar lista temporal
            this.isSending = false; // Habilitar botón
          },
          (error: any) => {
            this.toastr.error('Opss.. Ha ocurrido un error al enviar las reservas', 'Error!');
            console.log('Error en el envío de reservas: ', error);
            this.isSending = false; // Habilitar botón
          }
        );
      } else {
        // Si el id está definido, actualizamos la reserva
        this.isSending = true; // Deshabilitar botón
        this._reservaService.updateReserva(this.id, reserva).subscribe(
          (response: any) => {
            this.toastr.success('Reserva actualizada con éxito', 'Reserva actualizada');
            this.obtenerReservas();
            this.isSending = false; // Habilitar botón
            this.id = undefined; // Reiniciar el id para futuras acciones
            this.accion = "Agregar"; // Cambiar la acción de vuelta a 'Agregar'
            this.reservaForm.reset(); // Reiniciar el formulario
          },
          (error: any) => {
            this.toastr.error('Opss.. Ha ocurrido un error al actualizar la reserva', 'Error!');
            console.log('Error al actualizar la reserva: ', error);
            this.isSending = false; // Habilitar botón
          }
        );
      }
    } else {
      this.toastr.error('Por favor, complete todos los campos requeridos.', 'Error!');
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

  eliminarReserva(id: number){
    this._reservaService.deleteReserva(id).subscribe(data =>{
      this.toastr.success('Reserva removida con exito!');
      this.obtenerReservas();

    }, error => {
      console.log(error);
    })

  }

  editarReserva(reserva: any){
    this.accion = "Editar";
    this.id = reserva.idReserva;

    console.log("editar llamado " + this.id);
    this.reservaForm.patchValue({
      UsuarioId: reserva.usuarioId,
      HoraInicio: reserva.horaInicio,
      HoraFin: reserva.horaFin,
      Prioridad: reserva.prioridad,
      capacidad: reserva.capacidad

    })
  }
}
