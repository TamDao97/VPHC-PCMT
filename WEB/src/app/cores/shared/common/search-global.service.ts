import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { MenuOptions } from '../models/menu-options.model';
import { Menu } from '../models/menu.model';

@Injectable({
    providedIn: 'root'
})
export class SearchGlobalService {
    private _onDataChanged: BehaviorSubject<any>;
    private _onRefreshDataChanged: BehaviorSubject<any>;
    private _onExportExcel: BehaviorSubject<any>;
    private _onExportPdf: BehaviorSubject<any>;
    private _onMenuClick: BehaviorSubject<any>;
    private _onConfigChange: BehaviorSubject<any>;
    private _onDate: BehaviorSubject<any>;

    constructor() {
        this._onDataChanged = new BehaviorSubject(null);
        this._onRefreshDataChanged = new BehaviorSubject(null);
        this._onExportExcel = new BehaviorSubject(false);
        this._onExportPdf = new BehaviorSubject(false);
        this._onMenuClick = new BehaviorSubject(null);
        this._onConfigChange = new BehaviorSubject(null);
        this._onDate = new BehaviorSubject(null);
    }

    get onDataChanged(): Observable<any> {
        this._onDataChanged.next(null);
        return this._onDataChanged.asObservable();
    }

    get onRefreshData(): Observable<any> {
        this._onRefreshDataChanged.next(null);
        return this._onRefreshDataChanged.asObservable();
    }

    get onExportExcel(): Observable<any> {
        this._onExportExcel.next(false);
        return this._onExportExcel.asObservable();
    }

    get onExportPdf(): Observable<any> {
        this._onExportPdf.next(false);
        return this._onExportPdf.asObservable();
    }

    get onSearchDate(): Observable<any> {
        this._onDate.next(false);
        return this._onDate.asObservable();
    }

    get onMenuClick(): Observable<any> {
        this._onMenuClick.next(null);
        return this._onMenuClick.asObservable();
    }

    get onConfigChange(): Observable<any> {
        this._onConfigChange.next(null);
        return this._onConfigChange.asObservable();
    }

    search(data: any): void {
        this._onDataChanged.next(data);
    }

    refresh():void{
        this._onRefreshDataChanged.next(true);
    }

    /**
     * Xuất excel
     */
    exportExcel(menu?: Menu): void {
        this._onExportExcel.next(true);
    }

    /**
     * Xuất Pdf
     */
    exportPdf(menu?: Menu): void {
        this._onExportPdf.next(true);
    }

    /**
     * Tìm kiếm theo năm
     */
    searchDate(year?: number): void {
        this._onDate.next(year);
    }

    /**
     * click menu
     */
    menuClick(menu?: Menu): void {
        this._onMenuClick.next(menu);
    }

    /**
     * cấu hình toolbar
     */
    setConfig(menu: MenuOptions |null): void {
        this._onConfigChange.next(menu);
    }
}