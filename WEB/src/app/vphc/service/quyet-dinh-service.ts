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
export class QuyetDinhService {
  constructor(private http: HttpClient) {}
  getDanhMuc(): Observable<any> {
    return this.http.get<any>(API_URL + 'quyet-dinh/get-danh-muc', httpOptions);
  }
  create(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'quyet-dinh/create', model, httpOptions);
  }

  update(id: string, model: any): Observable<any> {
    return this.http.put<any>(
      API_URL + 'quyet-dinh/update/' + id,
      model,
      httpOptions
    );
  }

  getById(id: string): Observable<any> {
    return this.http.get<any>(
      API_URL + 'quyet-dinh/get-by-id/' + id,
      httpOptions
    );
  }

  delete(id: string): Observable<any> {
    return this.http.delete<any>(
      API_URL + 'quyet-dinh/delete/' + id,
      httpOptions
    );
  }

  getQuyetDinhVuViec(id: string): Observable<any> {
    return this.http.get<any>(
      API_URL + 'quyet-dinh/get-quyet-dinh-vu-viec/' + id,
      httpOptions
    );
  }

  exportWord(id: any): Observable<any> {
    let apiPath = API_URL + 'quyet-dinh/xuat-quyet-dinh-doc/' + id;
    var tr = this.http.post(apiPath, {}, { responseType: 'blob' });
    return tr;
  }

  exportPdf(id: any): Observable<any> {
    let apiPath = API_URL + 'quyet-dinh/xuat-quyet-dinh-pdf/' + id;
    var tr = this.http.post(apiPath, {}, { responseType: 'blob' });
    return tr;
  }
}
