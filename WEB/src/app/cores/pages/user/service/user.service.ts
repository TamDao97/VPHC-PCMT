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
export class UserService {

  constructor(
    private http: HttpClient
  ) { }

 
  searchUser(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'user/search', model, httpOptions);
  }

  search(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'user/search-user', model, httpOptions);
  }

  deleteUser(id: string): Observable<any> {
    return this.http.delete<any>(API_URL + 'user/delete/' + id, httpOptions);
  }

  createUser(model: any): Observable<any> {
    return this.http.post<any>(API_URL + 'user/create', model, httpOptions);
  }

  updateUser(id: string, model: any): Observable<any> {
    return this.http.put<any>(API_URL + 'user/update/' + id, model, httpOptions);
  }

  getUserById(id: string): Observable<any> {
    return this.http.get<any>(API_URL + 'user/get-user-by-id/' + id, httpOptions);
  }

  userAdminLockOrUnlock(id: string, islock: boolean): Observable<any> {
    return this.http.put<any>(API_URL + 'user/lock/' + id +'?isunlock=' + islock, httpOptions);
  }
  
  getPermission(groupuserid: string): Observable<any> {
    var tr = this.http.get<any>(API_URL + 'user/get-permission'+'?groupUserId=' +groupuserid, httpOptions);
    return tr;
  }
  updateUserInfo(id: string, model: any): Observable<any> {
    return this.http.put<any>(API_URL + 'user/update-info/' + id, model, httpOptions);
  }

  getUserInfo(id: string): Observable<any> {
    return this.http.get<any>(API_URL + 'user/get-user-by-info/' + id, httpOptions);
  }
}
