import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const API_URL = `${environment.apiUrl}api/`;

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};

@Injectable({
  providedIn: 'root',
})
export class BienBanService {
  constructor(private http: HttpClient) {}
  getDanhMuc(): Observable<any> {
    return this.http.get<any>(API_URL + 'bien-ban/get-danh-muc', httpOptions);
  }
  create(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'bien-ban/create', model, httpOptions);
  }

  update(id: string, model: any): Observable<any> {
    return this.http.put<any>(
      API_URL + 'bien-ban/update/' + id,
      model,
      httpOptions
    );
  }

  getById(id: string): Observable<any> {
    return this.http.get<any>(
      API_URL + 'bien-ban/get-by-id/' + id,
      httpOptions
    );
  }

  delete(id: string): Observable<any> {
    return this.http.delete<any>(
      API_URL + 'bien-ban/delete/' + id,
      httpOptions
    );
  }

  getBienBanVuViec(id: string): Observable<any> {
    return this.http.get<any>(
      API_URL + 'bien-ban/get-bien-ban-vu-viec/' + id,
      httpOptions
    );
  }

  exportWord(id: any): Observable<any> {
    let apiPath = API_URL + 'bien-ban/xuat-bien-ban-doc/' + id;
    var tr = this.http.post(apiPath, {}, { responseType: 'blob' });
    return tr;
  }

  exportPdf(id: any): Observable<any> {
    let apiPath = API_URL + 'bien-ban/xuat-bien-ban-pdf/' + id;
    var tr = this.http.post(apiPath, {}, { responseType: 'blob' });
    return tr;
  }
}
