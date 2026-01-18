import { Injectable } from '@angular/core';
@Injectable({
    providedIn: 'root'
})
export class PermissionService {

    constructor() {

    }

    checkPermission(permission: any[]): boolean {
        var isAuthorize = false;
        var user = localStorage.getItem('CurrentUser');
        var listPermission: any[] = [];
        if (user) {
            let userString = localStorage.getItem('CurrentUser');
            var listPermission: any[] = [];
            if (userString)
                listPermission = JSON.parse(userString).permissions;
            if (listPermission != null && listPermission.length > 0 && permission) {
                permission.forEach(function (item: any) {
                    if (!isAuthorize && listPermission.indexOf(item) != -1) {
                        isAuthorize = true;
                    }
                });
            }
        }

        if (!permission || permission.length == 0) {
            isAuthorize = true;
        }
        

        return isAuthorize
    }
}
