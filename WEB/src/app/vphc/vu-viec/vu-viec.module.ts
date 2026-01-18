import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  NgbCollapseModule,
  NgbDropdownModule,
  NgbModule,
  NgbNavModule,
  NgbTooltipModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';

import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpBackend, HttpClient } from '@angular/common/http';
import {
  ContextMenuService,
  TreeGridModule,
} from '@syncfusion/ej2-angular-treegrid';
import { RouterModule, Routes } from '@angular/router';
import { TranslationModule } from 'src/app/modules/i18n';
import {
  EditService,
  FilterService,
  GridModule,
  PageService,
  SortService,
} from '@syncfusion/ej2-angular-grids';
import { VuViecManageComponent } from './vu-viec-manage/vu-viec-manage.component';
import { VuViecCreateComponent } from './vu-viec-create/vu-viec-create.component';
import { SharedModule } from 'src/app/cores/shared/shared.module';
import { TestComponent } from './test/test.component';
import { TabModule, TreeViewModule } from '@syncfusion/ej2-angular-navigations';
import {
  DatePickerModule,
  DateTimePickerModule,
} from '@syncfusion/ej2-angular-calendars';
import { UploaderModule } from '@syncfusion/ej2-angular-inputs';
import { NguoiViPhamModifyComponent } from './nguoi-vi-pham/nguoi-vi-pham-modify.component';
import { DropDownListModule } from '@syncfusion/ej2-angular-dropdowns';
import { SplitterModule } from '@syncfusion/ej2-angular-layouts';
import { ToChucModifyComponent } from './to-chuc/to-chuc-modify.component';
import { ListViewModule } from '@syncfusion/ej2-angular-lists';
import { NguoiPhienDichModifyComponent } from './nguoi-phien-dich/nguoi-phien-dich-modify.component';
import { NguoiChungKienModifyComponent } from './nguoi-chung-kien/nguoi-chung-kien-modify.component';
import { TangVatModifyComponent } from './tang-vat/tang-vat-modify.component';
import { PhuongTienModifyComponent } from './phuong-tien/phuong-tien-modify.component';
import { GiayPhepChungChiModifyComponent } from './chung-chi-giay-phep/giay-phep-chung-chi-modify.component';
import { XacMinhModifyComponent } from './xac-minh/xac-minh-modify.component';
import { PhanCongCanBoComponent } from './phan-cong-can-bo/phan-cong-can-bo-choose.component';
import { DanhMucBienBanComponent } from './bien-ban/danh-muc/danh-muc-choose.component';
import { BienBanModifyComponent } from './bien-ban/bien-ban-modify/bien-ban-modify.component';
import { BienBanListComponent } from './bien-ban/ban-ban-list/bien-ban-list.component';
import { QuyetDinhListComponent } from './quyet-dinh/quyet-dinh-list/quyet-dinh-list.component';
import { QuyetDinhModifyComponent } from './quyet-dinh/quyet-dinh-modify/quyet-dinh-modify.component';
import { DanhMucQuyetDinhComponent } from './quyet-dinh/danh-muc/danh-muc-choose.component';
import { AmountToTextPipe } from 'src/app/cores/shared/pipe/amount-to-text';
import { CurrencyFormatterDirective } from 'src/app/cores/shared/pipe/currency-formatter-directive';
import { XuLyModifyComponent } from './xu-ly/xu-ly-modify.component';
import { TaiLieuListComponent } from './tai-lieu/tai-lieu-list/tai-lieu-list.component';
import { TaiLieuUploadComponent } from './tai-lieu/tai-lieu-upload/tai-lieu-upload.component';
import { NgxFileDragDropModule } from 'ngx-file-drag-drop';
import { MatChipsModule } from '@angular/material/chips';
import { NguoiViPhamSearchComponent } from '../tra-cuu/nguoi-vi-pham-search.component';

const routes: Routes = [
  {
    path: 'vu-viec',
    children: [
      {
        path: '',
        component: VuViecManageComponent,
        data: { animation: 'VuViecManage' },
      },
      {
        path: 'chinh-sua/:id',
        component: VuViecCreateComponent,
        data: { animation: 'VuViecUpdate' },
      },
      { path: 'test', component: TestComponent, data: { animation: 'Test' } },
    ],
  },
  {
    path: 'them-moi',
    component: VuViecCreateComponent,
    data: { animation: 'VuViecCreate' },
  }
];

@NgModule({
  declarations: [
    VuViecManageComponent,
    VuViecCreateComponent,
    NguoiViPhamModifyComponent,
    ToChucModifyComponent,
    NguoiPhienDichModifyComponent,
    NguoiChungKienModifyComponent,
    TangVatModifyComponent,
    PhuongTienModifyComponent,
    GiayPhepChungChiModifyComponent,
    XacMinhModifyComponent,
    PhanCongCanBoComponent,
    BienBanListComponent,
    BienBanModifyComponent,
    DanhMucBienBanComponent,
    QuyetDinhListComponent,
    QuyetDinhModifyComponent,
    DanhMucQuyetDinhComponent,
    AmountToTextPipe,
    CurrencyFormatterDirective,
    XuLyModifyComponent,
    TaiLieuListComponent,
    TaiLieuUploadComponent
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
    TreeViewModule,
    // ListViewModule,
    // ButtonModule,
    TabModule,
    DateTimePickerModule,
    DatePickerModule,
    UploaderModule,
    DropDownListModule,
    SplitterModule,
    ListViewModule,
    ReactiveFormsModule,
    MatChipsModule,
    NgxFileDragDropModule
  ],
  providers: [SortService, PageService, FilterService, ContextMenuService,EditService,AmountToTextPipe ],
})
export class VuViecModule {}
// // AOT compilation support
// export function httpTranslateLoader(http: HttpClient) {
//   return new MultiTranslateHttpLoader(http, [{ prefix: '/assets/i18n/cores/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/cores/message/', suffix: '.json' },
//   { prefix: '/assets/i18n/projects/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/projects/message/', suffix: '.json' },
//   { prefix: '/assets/i18n/modules/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/modules/message/', suffix: '.json' },]);
// }
