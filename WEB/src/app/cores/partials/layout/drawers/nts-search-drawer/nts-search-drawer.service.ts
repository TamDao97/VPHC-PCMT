import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { MenuOptions } from 'src/app/cores/shared/models/menu-options.model';

@Injectable({
  providedIn: 'root',
})
export class NtsToolbarSearchService {
  public meuOptions: MenuOptions = {};
  private onDataChangedSource = new BehaviorSubject<any>(null); // Dữ liệu tìm kiếm
  onDataChanged = this.onDataChangedSource.asObservable(); // Observable để các component lắng nghe

  updateSearchModel(model: any) {
    this.onDataChangedSource.next(model); // Cập nhật dữ liệu
  }

  private onKeywordChangedSource = new BehaviorSubject<any>(null); // Dữ liệu tìm kiếm
  onKeywordChanged = this.onKeywordChangedSource.asObservable(); // Observable để các component lắng nghe
  updateKeyword(value: any) {
    this.onKeywordChangedSource.next(value); // Cập nhật dữ liệu
  }

  private onChangedConfigSource = new BehaviorSubject<any>(null); // Lưu trữ searchFields
  onChangedConfig = this.onChangedConfigSource.asObservable(); // Observable để cung cấp searchFields

  // Cập nhật cấu hình searchFields
  setConfig(fields: any) {
    this.meuOptions = fields;
    this.onChangedConfigSource.next(fields);
  }

  private onRefreshDataSource = new BehaviorSubject<any>(null); // Lưu trữ searchFields
  onRefreshData = this.onRefreshDataSource.asObservable();

  refreshSearchModel(model: any) {
    this.onRefreshDataSource.next(model); // Cập nhật dữ liệu
  }
}
