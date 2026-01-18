import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

const httpOptionsJson = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(
    private http: HttpClient
  ) { }

  // Upload 1 file
  uploadFile(file: any, folderName: string): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('folderName', folderName);
    formData.append('file', file);
    var tr = this.http.post<any>(environment.apiUrl + 'uploads/upload-file', formData);
    return tr
  }

  // Upload nhiều file
  uploadFiles(files: any, model:any): Observable<any> {
    let formData: FormData = new FormData();
    formData.append('folderName', model);
    files.forEach((file:any) => {
      formData.append('files', file);
    });
    var tr = this.http.post<any>(environment.apiUrl + 'uploads/upload-files', formData);
    return tr
  }

  downloadFileZip(listfile:any): Observable<any> {
    var tr = this.http.post(environment.apiUrl + 'uploads/download-files', listfile, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      responseType: "blob"
    },);
    return tr
  }
}
