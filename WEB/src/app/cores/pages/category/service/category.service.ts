import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};

@Injectable({
  providedIn: 'root',
})

export class CategoryService {
  constructor(private http: HttpClient) {}

  searchCategory(): Observable<any> {
    return this.http.post<any>(
      environment.apiUrl + 'api/category/search',
      httpOptions
    );
  }

  createCategory(model: any): Observable<any> {
    return this.http.post<any>(
      environment.apiUrl + 'api/category',
      model,
      httpOptions
    );
  }

  updateCategory(id: string, model: any): Observable<any> {
    return this.http.put<any>(
      environment.apiUrl + 'api/category/' + id,
      model,
      httpOptions
    );
  }

  deleteCategory(id: string): Observable<any> {
    return this.http.delete<any>(
      environment.apiUrl + 'api/category/' + id,
      httpOptions
    );
  }

  getCategoryInfo(id: string): Observable<any> {
    return this.http.get<any>(
      environment.apiUrl + 'api/category/' + id,
      httpOptions
    );
  }

  getListOder(id: string): Observable<any> {
    return this.http.get<any>(
      environment.apiUrl + 'api/category/get-list-oder?id=' + id,
      httpOptions
    );
  }

  searchCategoryTable(model: any): Observable<any> {
    return this.http.post<any>(
      environment.apiUrl + 'api/category/table/search',
      model,
      httpOptions
    );
  }

  createCategoryTable(model: any): Observable<any> {
    return this.http.post<any>(
      environment.apiUrl + 'api/category/table',
      model,
      httpOptions
    );
  }

  updateCategoryTable(id: string, model: any): Observable<any> {
    return this.http.put<any>(
      environment.apiUrl + 'api/category/table/' + id,
      model,
      httpOptions
    );
  }

  deleteCategoryTable(id: string, tableName: string): Observable<any> {
    return this.http.delete<any>(
      environment.apiUrl + 'api/category/table/' + id + '?tableName=' + tableName,
      httpOptions
    );
  }

  getCategoryTableInfo(id: string, tableName: string): Observable<any> {
    return this.http.get<any>(
      environment.apiUrl + 'api/category/table/' + id + '?tableName=' + tableName,
      httpOptions
    );
  }

  getListOderTable(id: string, tableName: string): Observable<any> {
    return this.http.get<any>(
      environment.apiUrl +
        'api/category/table/get-list-oder?id=' +
        id +
        '&tableName=' +
        tableName,
      httpOptions
    );
  }

  getExport(): Observable<any> {
    return this.http.get(environment.apiUrl + 'api/category/export', {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      responseType: 'blob',
    });
  }
}