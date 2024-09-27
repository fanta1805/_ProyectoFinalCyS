import { Component } from '@angular/core';
import {DatePipe, NgForOf, NgIf, UpperCasePipe} from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {SalaService} from "../../services/sala.service";
import {ToastrService} from "ngx-toastr";
import {ReservaService} from "../../services/reserva.service";

@Component({
  selector: 'app-home-user',
  standalone: true,
  imports: [
    NgForOf,
    DatePipe,
    NgIf,
    ReactiveFormsModule,
    UpperCasePipe
  ],
  templateUrl: './home-user.component.html',
  styleUrl: './home-user.component.css'
})
export class HomeUserComponent {
  salaForm: FormGroup;
  listSalas: any[]=[];
  accion = "Agregar";
  id: number | undefined;

  constructor(private fb: FormBuilder, private _salaService: SalaService, private toastr: ToastrService, private _reservaService: ReservaService) {
    this.salaForm = this.fb.group({
      nombreSala: ['', Validators.required],
      Ubicacion: ['', Validators.required],  // "Ubicacion" con U mayúscula
      capacidad: ['', Validators.required]
    })

  }

  ngOnInit(): void{
    this.obtenerSalas();
  }

  isSending = false;
  obtenerSalas(){
    this._reservaService.getListaSalas().subscribe(data=>{
        this.listSalas = data;
      },error=>{
        console.log(error);
      }
    )
  }

  agregarSala(){
    if(this.salaForm.valid){
      const sala: any = {
        nombreSala: this.salaForm.get('nombreSala')?.value,
        ubicacion: this.salaForm.get('Ubicacion')?.value,  // Usa "Ubicacion" consistente
        capacidad: this.salaForm.get('capacidad')?.value,
      };

      if (this.id === undefined){
        this.isSending = true; // Deshabilitar botón
        this._salaService.postSala(sala).subscribe(data =>{
          this.toastr.success('Sala creada con exito!', 'Exito!' );
          this.salaForm.reset();
          this.obtenerSalas();
          this.isSending = false; // Habilitar botón

        }, error =>{
          this.toastr.error('No se pudo crear la sala', 'Error!');
          this.isSending = false; // Habilitar botón

        })
      }else{
        this.isSending = true; // Deshabilitar botón

        this._salaService.updateSala(this.id, sala).subscribe(
          (response: any) => {
            this.toastr.success('Sala actualizada con éxito', 'Sala actualizada');
            this.obtenerSalas();
            this.isSending = false; // Habilitar botón
            this.id = undefined; // Reiniciar el id para futuras acciones
            this.accion = "Agregar"; // Cambiar la acción de vuelta a 'Agregar'
            this.salaForm.reset(); // Reiniciar el formulario
          },
          (error: any) => {
            this.toastr.error('Opss.. Ha ocurrido un error al actualizar la sala', 'Error!');
            console.log('Error al actualizar la sala: ', error);
            this.isSending = false; // Habilitar botón

          }
        );
      }

    }else{
      this.toastr.error('Por favor, complete todos los campos requeridos.', 'Error!');
    }

  }

  editarSala(sala: any){
    this.accion = "Editar";
    this.id = sala.idSala;

    this.salaForm.patchValue({
      nombreSala: sala.nombreSala,
      Ubicacion: sala.ubicacion,  // Asegúrate que coincida el nombre
      capacidad: sala.capacidad
    });

  }

  eliminarSala(id: number){
    console.log('eliminar');
    console.log(id);
    this._salaService.deleteSala(id).subscribe(data =>{
      this.toastr.success('Reserva removida con exito!');
      this.toastr.warning('Se eliminaron todas las reservas de la sala: ' + id);
      this.obtenerSalas();
    }, error => {
      console.log(error);
    })
  }
}
