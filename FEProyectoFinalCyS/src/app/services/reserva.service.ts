import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ReservaService {
  private myAppUrl = 'https://localhost:44345/';
  private myApiUrl = 'api/reserva/usuario/modificar'

  constructor(private http: HttpClient) {}




}
