import { HttpClient, HttpHeaders } from '@angular/common/http';
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
export class PermissionService {

  constructor(
    private http: HttpClient
  ) { }

  
  deletePermision(id: string): Observable<any> {
    return this.http.delete<any>(API_URL + 'permission/user-delete/' + id, httpOptions);
  }

  getUser(model: any) {
    var tr = this.http.post<any>(API_URL + 'permission/user-info',model, httpOptions);
    return tr;
  }
}
