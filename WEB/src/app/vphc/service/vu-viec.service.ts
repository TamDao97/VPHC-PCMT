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
export class VuViecService {

  constructor(
    private http: HttpClient
  ) { }

  search(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'vu-viec/search', model, httpOptions);
  }

  quickDashboard(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'vu-viec/quick-dashboard', model, httpOptions);
  }

  create(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'vu-viec/create', model, httpOptions);
  }

  update(id: string, model: any): Observable<any> {
    return this.http.put<any>(API_URL + 'vu-viec/update/' + id, model, httpOptions);
  }

  getById(id: string): Observable<any> {
    return this.http.get<any>(API_URL + 'vu-viec/get-by-id/' + id, httpOptions);
  }

  deleteSoft(id: string): Observable<any> {
    return this.http.delete<any>(API_URL + 'vu-viec/soft-delete/' + id, httpOptions);
  }

  deleteHard(id: string): Observable<any> {
    return this.http.delete<any>(API_URL + 'vu-viec/hard-delete/' + id, httpOptions);
  }

  finish(id: string): Observable<any> {
    return this.http.put<any>(API_URL + 'vu-viec/finish/' + id, httpOptions);
  }

  exportExcel(model: any): Observable<any> {
      let apiPath = API_URL + 'vu-viec/export-excel';
      var tr = this.http.post(apiPath, model, { responseType: "blob" });
      return tr;
    }

    getVuViecTrongBienBan(id: string): Observable<any> {
      return this.http.get<any>(API_URL + 'vu-viec/get-vu-viec-trong-bien-ban/' + id, httpOptions);
    }

    getVuViecTrongQuyetDinh(id: string): Observable<any> {
      return this.http.get<any>(API_URL + 'vu-viec/get-vu-viec-trong-quyet-dinh/' + id, httpOptions);
    }

    checkViewNotify(id: string): Observable<any> {
      return this.http.get<any>(API_URL + 'vu-viec/check-view-notify/' + id, httpOptions);
    }
}
