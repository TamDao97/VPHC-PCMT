import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService, UserType } from 'src/app/modules/auth';

@Directive({
  selector: '[appUipermission]'
})
export class UipermissionDirective {

  user: UserType;
  
  constructor(private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef, private auth: AuthService) {
      this.auth.currentUserSubject.asObservable().subscribe((value) => {
        this.user = value;
      });
  }

  @Input() set appUipermission(permission: any) {

    var isAuthorize = false;
    if (this.user) {
      var listPermission: any[] = this.user.permission;
      if (listPermission != null && listPermission.length > 0 && permission) {
        permission.forEach(function (item: any) {
          if (!isAuthorize && listPermission.indexOf(item) != -1) {
            isAuthorize = true;
          }
        });
      }
    }

    if (!permission || permission.length == 0) {
      isAuthorize = false;
    }

    if (isAuthorize) {
      this.viewContainer.createEmbeddedView(this.templateRef);

    } else {
      this.viewContainer.clear();
    }
  }

}
