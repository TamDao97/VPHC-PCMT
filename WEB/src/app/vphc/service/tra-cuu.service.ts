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
export class TraCuuService {

  constructor(
    private http: HttpClient
  ) { }

  search(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'tra-cuu-nguoi-vp/search', model, httpOptions);
  }

  quickDashboard(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'tra-cuu-nguoi-vp/quick-dashboard', model, httpOptions);
  }

  exportExcel(model: any): Observable<any> {
      let apiPath = API_URL + 'tra-cuu-nguoi-vp/export-excel';
      var tr = this.http.post(apiPath, model, { responseType: "blob" });
      return tr;
    }
}
