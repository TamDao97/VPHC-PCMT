import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserModel } from '../../models/user.model';
import { environment } from '../../../../../environments/environment';
import { AuthModel } from '../../models/auth.model';

const API_URL = `${environment.apiUrl}api/`;

const httpOptionsJson = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8','Access-Control-Allow-Origin':'*' ,'X-Content-Type-Options':'nosniff'})
};

@Injectable({
  providedIn: 'root',
})

export class AuthHTTPService {
  constructor(private http: HttpClient) {}

  // public methods
  login(loginModel:any): Observable<any> {
    return this.http.post<any>(API_URL + 'auth/login', loginModel,httpOptionsJson);
  }

  // CREATE =>  POST: add a new user to the server
  createUser(user: UserModel): Observable<UserModel> {
    return this.http.post<UserModel>(API_URL, user);
  }

  // Your server should check email => If email exists send link to the user and return true | If email doesn't exist return false
  forgotPassword(email: string): Observable<boolean> {
    return this.http.post<boolean>(environment.apiUrl + 'auth/forgot-password', {
      email,
    });
  }

  getUserByToken(token: string): Observable<any> {
    const httpHeaders = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
     // Tạo header chứa token
     //const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`).set('Content-Type', 'application/json');
     const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    };
    return this.http.get<any>(API_URL + 'user/get-user-by-token', httpOptions);
  }

  changePassword(model: any): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    };
    return this.http.put<any>(API_URL + 'user/change-pass', model, httpOptions);
  }
}
