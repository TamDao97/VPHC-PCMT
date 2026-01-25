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
export class KeHoachService {
  constructor(
    private http: HttpClient
  ) { }

  search(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'ke-hoach-kiem-tra/search', model, httpOptions);
  }

  create(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'ke-hoach-kiem-tra/create', model, httpOptions);
  }

  update(id: string, model: any): Observable<any> {
    return this.http.put<any>(API_URL + 'ke-hoach-kiem-tra/update/' + id, model, httpOptions);
  }

  getById(id: string): Observable<any> {
    return this.http.get<any>(API_URL + 'ke-hoach-kiem-tra/get-by-id/' + id, httpOptions);
  }

  deleteSoft(id: string): Observable<any> {
    return this.http.delete<any>(API_URL + 'ke-hoach-kiem-tra/soft-delete/' + id, httpOptions);
  }

  deleteHard(id: string): Observable<any> {
    return this.http.delete<any>(API_URL + 'ke-hoach-kiem-tra/hard-delete/' + id, httpOptions);
  }

  updateStatus(payload: any): Observable<any> {
    return this.http.post<any>(API_URL + 'ke-hoach-kiem-tra/update-status', payload, httpOptions);
  }
  assigneeTask(payload: any): Observable<any> {
    return this.http.post<any>(API_URL + 'ke-hoach-kiem-tra/assignee-task', payload, httpOptions);
  }
  GetDetailAssigneeTaskByIdKeHoachAsync(id: string): Observable<any> {
    return this.http.get<any>(API_URL + 'ke-hoach-kiem-tra/get-detail-assignee-task-by-idKeHoach/' + id, httpOptions);
  }
}
