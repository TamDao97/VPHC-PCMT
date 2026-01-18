import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const API_URL = `${environment.apiUrl}api/`;

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class XuLyService {

  constructor(
    private http: HttpClient
  ) { }
  update(id: string, model: any): Observable<any> {
    return this.http.put<any>(API_URL + 'xu-ly/update/' + id, model, httpOptions);
  }

  getById(id: string): Observable<any> {
    return this.http.get<any>(API_URL + 'xu-ly/get-by-id/' + id, httpOptions);
  }
}
