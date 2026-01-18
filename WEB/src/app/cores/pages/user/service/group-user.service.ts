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
export class GroupUserService {

  constructor(
    private http: HttpClient
  ) { }

  searchGroupUser(model: any): Observable<any> {
    var tr = this.http.post<any>(API_URL + 'group-users/search', model, httpOptions);
    return tr;
  }

  getGroupUserInfo(id: string): Observable<any> {
    var tr = this.http.get<any>(API_URL + 'group-users/get-group-user?id=' + id);
    return tr;
  }

  createGroupUser(model: any): Observable<any> {
    var tr = this.http.post<any>(API_URL + 'group-users/create', model, httpOptions);
    return tr;
  }

  updateGroupUser(id: string, model: any): Observable<any> {
    var tr = this.http.put<any>(API_URL + 'group-users/' + id, model);
    return tr;
  }

  deleteGroupUser(id: string): Observable<any> {
    var tr = this.http.delete<any>(API_URL+ 'group-users/' + id, httpOptions);
    return tr;
  }
}
