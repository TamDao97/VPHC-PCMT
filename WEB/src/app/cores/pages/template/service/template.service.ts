import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';




const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json'})
};


@Injectable({
  providedIn: 'root'
})
export class TemplateService {

  constructor(
    private http: HttpClient
  ) { }

  SearchFileTemplate(model: any){
    var tr = this.http.post<any>(environment.apiUrl + 'filetemplate/search',model,httpOptions);
    return tr;
  }
  
  getFileTemplateById(id: string): Observable<any> {
    var tr = this.http.get<any>(environment.apiUrl + 'filetemplate/get/' + id);
    return tr;
  }
  createFileTemplate(model: any){
    var tr = this.http.post<any>(environment.apiUrl + 'filetemplate/create',model,httpOptions);
    return tr;
  }

  updateFileTemplate(id: string,model: any){
    var tr = this.http.put<any>(environment.apiUrl + 'filetemplate/update/' +id, model ,httpOptions);
    return tr;
  
  }
}
