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

export class DownloadService {

  constructor(
    private http: HttpClient
  ) { }

  downloadFile(model:any): Observable<any> {
    let apiPath = environment.apiUrl + 'file/download-file';
    var tr = this.http.post(apiPath, model, {
      responseType: "blob"
    });
    return tr
  }

  downloadFiles(model:any): Observable<any> {
    let apiPath = environment.apiUrl + 'uploads/download-files';
    var tr = this.http.post(apiPath, model, {
      responseType: "blob"
    });
    return tr
  }

}
