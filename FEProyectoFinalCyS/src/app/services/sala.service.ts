import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SalaService {
  private myAppUrl = 'https://localhost:44345/';
  private myApiUrl = 'api/salas/agregar'
  private myApiUrlEliminarSala = 'api/salas/eliminar/'
  private myApiUrlActualizar = 'api/salas/actualizar/'

  constructor(private http: HttpClient) {
  }

  postSala(sala: any): Observable<any>{
    return this.http.post(this.myAppUrl + this.myApiUrl, sala);
  }

  deleteSala(id: number): Observable<any>{
    return this.http.delete(this.myAppUrl + this.myApiUrlEliminarSala + id)
  }

  updateSala(id: number, sala: any): Observable<any>{
    return this.http.put(this.myAppUrl + this.myApiUrlActualizar + id, sala)
  }
}
