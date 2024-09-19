import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RegistroService {
  private myAppUrl = 'https://localhost:44345/';
  private myApiUrl = 'api/usuario/registro'

  constructor(private http: HttpClient) { }

  postRegistrarUsuario(usuario : any): Observable<any>{
    return this.http.post(this.myAppUrl + this.myApiUrl, usuario);
  }


}
