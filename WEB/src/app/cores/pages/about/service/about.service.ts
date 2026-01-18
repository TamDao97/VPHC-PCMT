import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AboutService {

  constructor(
    private http: HttpClient
  ) { }

  getAbout(): Observable<any> {
    var tr = this.http.get<any>(environment.apiUrl + 'about', httpOptions);
    return tr;
  }

  create(model: any): Observable<any> {
    var tr = this.http.post<any>(environment.apiUrl + 'about', model, httpOptions);
    return tr;
  }
}
