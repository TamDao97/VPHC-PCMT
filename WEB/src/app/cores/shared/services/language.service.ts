import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class LanguageService {
    public static LOCAL_STORAGE_KEY = 'lg';
    public static DEFAULT_LANGUAGE = 'vi';
    private _onLanguageChanged: BehaviorSubject<any>;

    constructor() {
        
        this._onLanguageChanged = new BehaviorSubject(this.getLanguage());
    }

    get onLanguageChanged(): Observable<any> {
        this._onLanguageChanged.next(null);
        return this._onLanguageChanged.asObservable();
    }

    public setLanguage(languageCode: string) {
        localStorage.setItem(
            LanguageService.LOCAL_STORAGE_KEY,
            languageCode || LanguageService.DEFAULT_LANGUAGE
        );
        this._onLanguageChanged.next(languageCode);
    }

    public getLanguage() {
        return localStorage.getItem(LanguageService.LOCAL_STORAGE_KEY) || LanguageService.DEFAULT_LANGUAGE;
    }
}