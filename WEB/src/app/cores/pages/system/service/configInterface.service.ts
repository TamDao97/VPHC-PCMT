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
export class ConfigInterfaceService {

  constructor(
    private http: HttpClient
  ) { }
  
  createOrUpdate(model: any) {
    var tr = this.http.post<any>(API_URL + 'config-interface/create-or-update', model, httpOptions);
    return tr;
  }

  getConfig() {
    var tr = this.http.get<any>(API_URL + 'config-interface/get-detail', httpOptions);
    return tr;
  }
}
