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
export class TrangChuService {

  constructor(
    private http: HttpClient
  ) { }

  search(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'trang-chu/tinh-hinh-chung', model, httpOptions);
  }
}
