import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserManageComponent } from './user/user-manage/user-manage.component';
import { UserCreateComponent } from './user/user-create/user-create.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { GroupUserManageComponent } from './group-user/group-user-manage/group-user-manage.component';
import { GroupUserCreateComponent } from './group-user/group-user-create/group-user-create.component';
import { UserViewComponent } from './user/user-view/user-view.component';
import { UserInfoComponent } from './user/user-info/user-info.component';
import { UserHistoryComponent } from './user-history/user-history.component';
import { ForgotPasswordComponent } from './user/forgot-password/forgot-password.component';

import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { TreeGridModule } from '@syncfusion/ej2-angular-treegrid';
import { PermissionManageComponent } from './permission/permission-manage/permission-manage.component';
import { PermissionUpdateComponent } from './permission/permission-update/permission-update.component';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { TranslationModule } from 'src/app/modules/i18n';
import { FilterService, GridModule, PageService, SortService } from '@syncfusion/ej2-angular-grids';
import { SwitchModule } from '@syncfusion/ej2-angular-buttons';
import { DropDownListModule } from '@syncfusion/ej2-angular-dropdowns';

const routes: Routes = [
  {
    path: 'tai-khoan',
    children: [
      { path: '', component: UserManageComponent, data: { animation: 'UserManage' } },
      { path: 'them-moi', component: UserCreateComponent, data: { animation: 'UserCreate' } },
      { path: 'chinh-sua/:id', component: UserCreateComponent, data: { animation: 'UserCreate' } },
      { path: 'xem-tai-khoan/:id', component: UserViewComponent, data: { animation: 'UserView' } },
    ]
  },
  {
    path: 'nhom-nguoi-dung',
    component: GroupUserManageComponent, data: { animation: 'GroupUserManage' }
  },
  {
    path: 'quan-ly-phan-quyen',
    component: PermissionManageComponent, data: { animation: 'PermissionManage' }
  },
  {
    path: 'tra-cuu-lich-su',
    component: UserHistoryComponent, data: { animation: 'UserHistory' }
  },
 
];

@NgModule({
  declarations: [
    UserManageComponent,
    UserCreateComponent,
    GroupUserManageComponent,
    GroupUserCreateComponent,
    UserViewComponent,
    UserInfoComponent,
    UserHistoryComponent,
    ForgotPasswordComponent,
    PermissionManageComponent,
    PermissionUpdateComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    TranslationModule,
    TranslateModule,
    NgbModule,
    // PerfectScrollbarModule,
    NgSelectModule,
    SharedModule,
    FormsModule,
    TreeGridModule,
    GridModule,
    // ListViewModule,
    // ButtonModule,
    ReactiveFormsModule,
    SwitchModule,
    DropDownListModule
  ],
  providers: [SortService,PageService,FilterService]
})
export class UserModule { }
// // AOT compilation support
// export function httpTranslateLoader(http: HttpClient) {
//   return new MultiTranslateHttpLoader(http, [{ prefix: '/assets/i18n/cores/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/cores/message/', suffix: '.json' },
//   { prefix: '/assets/i18n/projects/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/projects/message/', suffix: '.json' },
//   { prefix: '/assets/i18n/modules/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/modules/message/', suffix: '.json' },]);
// }
