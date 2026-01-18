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
export class UserHistoryService {

  constructor(
    private http: HttpClient
  ) { }

  searchUserHistory(model: any): Observable<any> {
    var tr = this.http.post<any>(API_URL + 'user-history/search', model, httpOptions);
    return tr;
  }

  exportExcel(model: any): Observable<any> {
    let apiPath = API_URL + 'user-history/export-excel';
    var tr = this.http.post(apiPath, model, { responseType: "blob" });
    return tr;
  }

  exportPdf(model: any): Observable<any> {
    let apiPath = API_URL + 'user-history/export-pdf';
    var tr = this.http.post(apiPath, model, { responseType: "blob" });
    return tr;
  }
}
