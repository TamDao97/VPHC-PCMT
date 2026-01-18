import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { FormsModule } from '@angular/forms';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { HttpClient } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { MenuManageComponent } from './menu-management/menu-manage.component';
import { MenuCreateComponent } from './menu-create/menu-create.component';
import { FilterService, PageService, SortService, TreeGridAllModule, TreeGridModule } from '@syncfusion/ej2-angular-treegrid';
import { FunctionManageComponent } from './function-config/function-manage/function-manage.component';
import { FunctionAutoCreateComponent } from './function-config/function-auto/function-auto-create/function-auto-create.component';
import { FunctionAutoManageComponent } from './function-config/function-auto/function-auto-manage/function-auto-manage.component';
import { PermissionCreateComponent } from './permission-create/permission-create.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MultiSelectModule } from '@syncfusion/ej2-angular-dropdowns';
import { FunctionAutoEditComponent } from './function-config/function-auto/function-auto-edit/function-auto-edit.component';
import { FunctionAutoDetailComponent } from './function-config/function-auto/function-auto-detail/function-auto-detail.component';
import { ConfigInterfaceCreateComponent } from './config-interface/config-interface-create/config-interface-create.component';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { ChoosePermissionAutoComponent } from './choose-permission-auto/choose-permission-auto.component';
import { TreeViewModule, ContextMenuModule } from '@syncfusion/ej2-angular-navigations';
import { GridModule } from '@syncfusion/ej2-angular-grids';
import { TranslationModule } from 'src/app/modules/i18n';

const routes: Routes = [
  {
    path: 'quan-ly-menu',   component: MenuManageComponent, data: { animation: 'MenuManage' },
  }, 
  {
    path: 'cau-hinh-he-thong',   component: ConfigInterfaceCreateComponent, data: { animation: 'InterfaceManage' },
  }, 
  {
    path: 'function-config',   component: FunctionManageComponent, data: { animation: 'FunctionConfig' },
  },
  {
    path: 'function-config', component: FunctionManageComponent, data: { animation: 'FunctionConfig' },
  },
  {
    path: 'function',
    children: [
      { path: 'manage/:slug', component: FunctionAutoManageComponent, data: { animation: 'FunctionManage' } },
      {path: 'create/:slug', component: FunctionAutoCreateComponent, data: { animation: 'FunctionCreate' }},
      {path: 'update/:slug/:id', component: FunctionAutoEditComponent, data: { animation: 'FunctionUpdate' }},
      {path: 'detail/:slug/:id', component: FunctionAutoDetailComponent, data: { animation: 'FunctionDetail' }}
    ]
  }
];

@NgModule({
  declarations: [
    MenuManageComponent,
    MenuCreateComponent,
    FunctionManageComponent,
    FunctionAutoManageComponent,
    FunctionAutoCreateComponent,
    FunctionAutoEditComponent,
    FunctionAutoDetailComponent,
    ChoosePermissionAutoComponent,
    PermissionCreateComponent,
    ConfigInterfaceCreateComponent
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
    DragDropModule,
    TreeGridModule,
    DragDropModule,
    GridModule, 
    MultiSelectModule,
    TreeViewModule,
    ContextMenuModule,
    // ImageCropperModule,
    // TranslateModule.forRoot({
    //   loader: {
    //       provide: TranslateLoader,
    //       useFactory: httpTranslateLoader,
    //       deps: [HttpClient]
    //   }
  // })   
  ],
  providers: [PageService,
    SortService,
    FilterService,
    // GroupService
  ] 
})
export class SystemModule { }
// export function httpTranslateLoader(http: HttpClient) {
//   return new MultiTranslateHttpLoader(http, [{ prefix: '/assets/i18n/cores/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/cores/message/', suffix: '.json' },
//   { prefix: '/assets/i18n/projects/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/projects/message/', suffix: '.json' },
//   { prefix: '/assets/i18n/modules/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/modules/message/', suffix: '.json' },]);
// }