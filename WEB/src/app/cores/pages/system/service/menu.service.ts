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
export class MenuService {

  constructor(
    private http: HttpClient
  ) { }

  searchMenu(model: any) {
    var tr = this.http.post<any>(API_URL + 'menu/search', model, httpOptions);
    return tr;
  }

  getMenu(): Observable<any> {
    var tr = this.http.get<any>(API_URL + 'menu/get', httpOptions);
    return tr;
  }
  getMenuById(id: string): Observable<any> {
    var tr = this.http.get<any>(API_URL + 'menu/get-by-id/' + id);
    return tr;
  }
  createMenu(model: any) {
    var tr = this.http.post<any>(API_URL + 'menu/create', model, httpOptions);
    return tr;
  }

  updateMenu(id: string, model: any) {
    var tr = this.http.put<any>(API_URL + 'menu/update/' + id, model, httpOptions);
    return tr;
  }
  disableMenu(id: string) {
    var tr = this.http.put<any>(API_URL + 'menu/disable/' + id, httpOptions);
    return tr;
  }
  deleteMenu(id: string): Observable<any> {
    return this.http.delete<any>(API_URL + 'menu/delete/' + id, httpOptions);
  }

  updateIndex(model: any) {
    var tr = this.http.post<any>(API_URL + 'menu/update-index', model, httpOptions);
    return tr;
  }

  getFuntionAuto() {
    var tr = this.http.get<any>(API_URL + 'menu/list-funtion-auto', httpOptions);
    return tr;
  }
}
