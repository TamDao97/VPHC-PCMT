import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const API_URL = `${environment.apiUrl}api/`;

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};

@Injectable({
  providedIn: 'root',
})
export class ComboboxCoreService {
  constructor(private http: HttpClient) {}

  //lấy thông tin combobox theo tên table
  getDataCombobox(tableName: string): Observable<any> {
    return this.http.get<any>(
      API_URL + 'combobox/get-data/' + tableName,
      httpOptions
    );
  }

  //
  getListGroupuser(): Observable<any> {
    return this.http.get<any>(
      API_URL + 'combobox/get-list-group-user',
      httpOptions
    );
  }

  getListUser(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-list-user', httpOptions);
  }

  getListMenu(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-list-menu', httpOptions);
  }

  getSystemFunctionAuto(): Observable<any> {
    return this.http.get<any>(
      API_URL + 'combobox/get-system-function-auto',
      httpOptions
    );
  }

  getDonVi(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-don-vi', httpOptions);
  }

  getTinh(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-tinh', httpOptions);
  }

  getHuyen(id: any): Observable<any> {
    return this.http.get<any>(
      API_URL + 'combobox/get-huyen-by-tinh/' + id,
      httpOptions
    );
  }

  getXa(id: any): Observable<any> {
    return this.http.get<any>(
      API_URL + 'combobox/get-xa-by-huyen/' + id,
      httpOptions
    );
  }

  getGioiTinh(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-gioi-tinh', httpOptions);
  }

  getDanToc(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-dan-toc', httpOptions);
  }

  getQuocTich(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-quoc-tich', httpOptions);
  }

  getNgheNghiep(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-nghe-nghiep', httpOptions);
  }

  getTonGiao(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-ton-giao', httpOptions);
  }

  getNguonTin(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-nguon-tin', httpOptions);
  }

  getXuLyVPHC(phanloai:any): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-xu-ly-vphc/'+phanloai, httpOptions);
  }

  getPhanLoaiTin(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-phan-loai-tin', httpOptions);
  }

  getKetLuanVPHC(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-ket-luan-vphc', httpOptions);
  }

  getLinhVucBCTH(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-linh-vuc-bcth', httpOptions);
  }

  getLoaiTangVat(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-loai-tang-vat', httpOptions);
  }

  getDonViTinh(id:any): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-don-vi-tinh/'+id, httpOptions);
  }

  getLoaiPhuongTien(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-loai-phuong-tien', httpOptions);
  }
  getHinhThucPhatChinh(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-hinh-thuc-phat-chinh', httpOptions);
  }
  getHinhThucPhatBoSung(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-hinh-thuc-phat-bo-sung', httpOptions);
  }

  getXuLyTangVat(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-xu-ly-tang-vat', httpOptions);
  }

  getXuLyPhuongTien(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-xu-ly-phuong-tien', httpOptions);
  }

  getXuLyGiayTo(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-xu-ly-giay-to', httpOptions);
  }

  getDonViByDonVi(): Observable<any> {
    return this.http.get<any>(API_URL + 'combobox/get-don-vi-by-don-vi', httpOptions);
  }
}
