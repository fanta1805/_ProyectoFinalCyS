import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ReservaService {
  private myAppUrl = 'https://localhost:44345/';
  private myApiUrl = 'api/reserva/usuario/listaReservas'
  private myApiUrlSalas = 'api/salas/salasDisponibles'
  private myApiUrlReservar = 'api/reserva/usuario/reservar'




  constructor(private http: HttpClient) {}

  getListaReservas(): Observable<any>{
    return this.http.get(this.myAppUrl + this.myApiUrl);

  }

  getListaSalas(): Observable<any>{
    return this.http.get(this.myAppUrl + this.myApiUrlSalas);
  }

  postReservar(reservas : any[]): Observable<any>{
    return this.http.post(this.myAppUrl + this.myApiUrlReservar, reservas);
  }







}
