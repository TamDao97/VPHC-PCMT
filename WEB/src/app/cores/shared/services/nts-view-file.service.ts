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
export class NtsViewFileService {

  constructor(
    private http: HttpClient
  ) { }

  getFile(url: string): Observable<any> {
    let apiPath = environment.apiUrl  + 'view-file/get-file-view?path=' + url;
    var tr = this.http.get(apiPath, { responseType: "blob" });
    return tr;
  }
}
