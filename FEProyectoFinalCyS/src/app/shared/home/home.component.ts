import { Component, OnInit } from '@angular/core';
import {ReservaService} from "../../services/reserva.service";
import {CommonModule} from "@angular/common";
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from "@angular/forms";
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
    }, {validators: this.horaInicioMenorQueHoraFin});
  }

  ngOnInit(): void {
    this.obtenerReservas();
    this.obtenerSalas();
  }


  isSending = false;

  guardarReserva() {
    // Verificamos si hay al menos una reserva en la lista temporal
    if (this.reservasTemp.length > 0) {
      this.isSending = true; // Deshabilitar botón

      // Enviar todas las reservas de la lista temporal
      this._reservaService.postReservar(this.reservasTemp).subscribe(
        (response: any) => {
          this.toastr.success(response.message, 'Éxito!');
          this.obtenerReservas();
          this.reservasTemp = []; // Limpiar la lista temporal
          this.isSending = false; // Habilitar botón
        },
        (error: any) => {
          this.toastr.error('Opss.. Ha ocurrido un error al enviar las reservas', 'Error!');
          console.log('Error en el envío de reservas: ', error);
          this.reservasTemp = []; // Limpiar la lista temporal
          this.isSending = false; // Habilitar botón
        }
      );
    } else {
      // Si no hay reservas en la lista temporal, validamos el formulario normalmente
      if (this.reservaForm.valid) {
        const reserva: any = {
          UsuarioId: this.reservaForm.get('UsuarioId')?.value,
          HoraInicio: this.getAdjustedTime(this.reservaForm.get('HoraInicio')?.value),
          HoraFin: this.getAdjustedTime(this.reservaForm.get('HoraFin')?.value),
          Prioridad: this.reservaForm.get('Prioridad')?.value,
          capacidad: this.reservaForm.get('capacidad')?.value
        };

        if (this.id === undefined) {
          this.reservasTemp.push(reserva);
          this.toastr.success('Reserva añadida a la lista temporal', 'Éxito!');
          this.reservaForm.reset(); // Limpiar el formulario después de agregar
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
              this.reservasTemp = []; // Limpiar la lista temporal
            },
            (error: any) => {
              this.toastr.error('Opss.. Ha ocurrido un error al actualizar la reserva', 'Error!');
              console.log('Error al actualizar la reserva: ', error);
              this.isSending = false; // Habilitar botón
              this.reservasTemp = []; // Limpiar la lista temporal
            }
          );
        }
      } else {
        this.toastr.error('Por favor, complete todos los campos requeridos.', 'Error!');
      }
    }
  }

  getAdjustedTime(dateString: string): string {
    const date = new Date(dateString);
    // Restar 3 horas (3 * 60 * 60 * 1000 milisegundos)
    const adjustedDate = new Date(date.getTime() - (3 * 60 * 60 * 1000));
    return adjustedDate.toISOString(); // Asegúrate de enviarlo en formato ISO
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
  horaInicioMenorQueHoraFin(control: AbstractControl):  ValidationErrors | null {
    const horaInicio = control.get('HoraInicio')?.value;
    const horaFin = control.get('HoraFin')?.value;

    if (horaInicio && horaFin && new Date(horaInicio) >= new Date(horaFin)) {
      return { horaInicioMenor: true }; // Retorna un error si HoraInicio no es menor que HoraFin
    }
    return null; // Sin errores
  }

}
