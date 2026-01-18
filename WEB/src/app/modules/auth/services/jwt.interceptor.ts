import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root',
})
export class JwtInterceptor implements HttpInterceptor {
    isLoadingSubject: BehaviorSubject<boolean>;
    private authLocalStorageToken = `${environment.appVersion}-${environment.USERDATA_KEY}`;
    
    constructor() {
        this.isLoadingSubject = new BehaviorSubject<boolean>(false);
     }

    private _inProgressCount = 0;

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // if (this.jwtHelper.isTokenExpired(token)) {
        //    this.tryRefreshingTokens(token);
        // }
        var noLoad = request.headers.get('no-load');
        if (noLoad != 'false') {
            if (this._inProgressCount <= 0) {
                this.isLoadingSubject.next(true);
            }
            this._inProgressCount++;
        }

        const auth = this.getAuthFromLocalStorage();

        if (auth && auth.token) {
            request = request.clone({
                setHeaders: {
                    Authorization: 'Bearer ' + auth.token
                }
            });
        }

        // if (!request.headers.has('Content-Type')) {
        //     request = request.clone({ headers: request.headers.set('Content-Type', 'application/json') });
        // }

        return next.handle(request).pipe(finalize(() => {
            if (noLoad != 'false') {
                this._inProgressCount--;
                if (this._inProgressCount === 0) {
                    this.isLoadingSubject.next(false);
                }
            }
        }));
    }

    public getAuthFromLocalStorage(): any | undefined {
        try {
          const lsValue = localStorage.getItem(this.authLocalStorageToken);
          if (!lsValue) {
            return undefined;
          }
    
          const authData = JSON.parse(lsValue);
          return authData;
        } catch (error) {
          console.error(error);
          return undefined;
        }
      }

    // public async tryRefreshingTokens(token: string): Promise<boolean> {
    //     // Try refreshing tokens using refresh token
    //     const refreshToken: string | null = localStorage.getItem("refreshToken");
    //     if (!token || !refreshToken) {
    //         return false;
    //     }

    //     const credentials = JSON.stringify({ accessToken: token, refreshToken: refreshToken });
    //     let isRefreshSuccess: boolean = false;

    //     this.authenticationService.refreshtoken(credentials).subscribe(
    //        (data: any) => {
    //             if (data.statusCode == this.constant.StatusCode.Success) {
    //                 localStorage.setItem("jwt", data.data.token);
    //                 localStorage.setItem("refreshToken", data.data.refreshToken);
    //                 isRefreshSuccess = true;
    //             }
    //             else {
    //                 isRefreshSuccess = false;
    //             }
    //         }, (error:any) => {
    //             isRefreshSuccess = false;
    //         }
    //     );

    //     return isRefreshSuccess;
    // }
}