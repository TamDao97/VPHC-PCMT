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
export class CanBoService {
  constructor(private http: HttpClient) {}
  getByIdDonVi(id: string): Observable<any> {
    return this.http.get<any>(
      API_URL + 'can-bo/get-by-id-don-vi/' + id,
      httpOptions
    );
  }
}
