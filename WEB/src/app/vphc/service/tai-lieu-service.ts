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
export class TaiLieuService {
  constructor(private http: HttpClient) {}
  getDanhMuc(): Observable<any> {
    return this.http.get<any>(API_URL + 'tai-lieu/get-danh-muc', httpOptions);
  }

  getDanhMucTotal(id: string): Observable<any> {
    return this.http.get<any>(
      API_URL + 'tai-lieu/get-danh-muc-total/' + id,
      httpOptions
    );
  }

  create(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'tai-lieu/create', model, httpOptions);
  }

  update(id: string, model: any): Observable<any> {
    return this.http.put<any>(
      API_URL + 'tai-lieu/update/' + id,
      model,
      httpOptions
    );
  }

  getById(id: string): Observable<any> {
    return this.http.get<any>(
      API_URL + 'tai-lieu/get-by-id/' + id,
      httpOptions
    );
  }

  delete(id: string): Observable<any> {
    return this.http.delete<any>(
      API_URL + 'tai-lieu/delete/' + id,
      httpOptions
    );
  }

  getTaiLieuVuViec(id: string, iddanhmuc: string): Observable<any> {
    return this.http.get<any>(
      API_URL + 'tai-lieu/get-tai-lieu-vu-viec/' + id + '/' + iddanhmuc,
      httpOptions
    );
  }

  getFileTaiLieu(id: any): Observable<any> {
    let apiPath = API_URL + 'tai-lieu/get-file-tai-lieu/' + id;
    var tr = this.http.post(apiPath, {}, { responseType: 'blob' });
    return tr;
  }
}
