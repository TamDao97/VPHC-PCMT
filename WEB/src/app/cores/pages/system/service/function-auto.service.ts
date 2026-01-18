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
export class FunctionAutoService {

  constructor(
    private http: HttpClient
  ) { }

  getConfigDesign(slug: string): Observable<any> {
    var tr = this.http.get<any>(API_URL + 'system-function/get-config-design/' + slug);
    return tr;
  }

  getTreeData(slug: string): Observable<any> {
    var tr = this.http.get<any>(API_URL + 'system-function/get-tree-data/' + slug);
    return tr;
  }

  search(slug: string, model: any): Observable<any> {
    var tr = this.http.post<any>(API_URL + 'system-function/search/' + slug, model, httpOptions);
    return tr;
  }

  getUpdateById(slug: string,id: string): Observable<any> {
    var tr = this.http.get<any>(API_URL + 'system-function/get-update/'+slug+'/' + id);
    return tr;
  }

  create(slug: string,model: any): Observable<any> {
    var tr = this.http.post<any>(API_URL + 'system-function/create/' + slug, model, httpOptions);
    return tr;
  }

  update(slug: string,id: string, model: any): Observable<any> {
    var tr = this.http.put<any>(API_URL + 'system-function/update/' + slug + '/' + id, model);
    return tr;
  }

  delete(slug: string,id: string): Observable<any> {
    var tr = this.http.delete<any>(API_URL + 'system-function/delete/' + slug + '/' + id, httpOptions);
    return tr;
  }

  getDetailById(slug: string,id: string): Observable<any> {
    var tr = this.http.get<any>(API_URL + 'system-function/get-detail/'+slug+'/' + id);
    return tr;
  }
}
