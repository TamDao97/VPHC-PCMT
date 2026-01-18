import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const API_URL = `${environment.apiUrl}api/`;

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json'})
};

@Injectable({
  providedIn: 'root'
})
export class FunctionConfigService {

  constructor(
    private http: HttpClient
  ) { }

  searchFunction(model: any){
    var tr = this.http.post<any>(API_URL + 'system-function/search-config',model,httpOptions);
    return tr;
  }

  getTableName(): Observable<any> {
    var tr = this.http.get<any>(API_URL + 'system-function/table' , httpOptions);
    return tr;
  }
  getColumnTable(tableName: string): Observable<any> {
    var tr = this.http.get<any>(API_URL + 'system-function/column-table/' + tableName);
    return tr;
  }
  getComboxColumnTable(tableName: string): Observable<any> {
    var tr = this.http.get<any>(API_URL + 'system-function/combox-column-table/' + tableName);
    return tr;
  }

  getUpdateFunctionConfigById(id: string){
    var tr = this.http.get<any>(API_URL + 'system-function/get-update-config/' +id ,httpOptions);
    return tr;  
  }

  createFunctionConfig(model: any){
    var tr = this.http.post<any>(API_URL + 'system-function/create-config',model,httpOptions);
    return tr;
  }

  updateFunctionConfig(id: string,model: any){
    var tr = this.http.put<any>(API_URL + 'system-function/update-config/' +id, model ,httpOptions);
    return tr;  
  }
  deleteFunctionConfig(id: string): Observable<any> {
    return this.http.delete<any>(API_URL + 'system-function/delete-config/' + id, httpOptions);
  }
}
