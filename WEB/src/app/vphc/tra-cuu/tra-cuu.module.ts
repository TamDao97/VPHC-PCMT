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
import { SharedModule } from 'src/app/cores/shared/shared.module';
import { TabModule, TreeViewModule } from '@syncfusion/ej2-angular-navigations';
import {
  DatePickerModule,
  DateTimePickerModule,
} from '@syncfusion/ej2-angular-calendars';
import { UploaderModule } from '@syncfusion/ej2-angular-inputs';
import { DropDownListModule } from '@syncfusion/ej2-angular-dropdowns';
import { SplitterModule } from '@syncfusion/ej2-angular-layouts';
import { ListViewModule } from '@syncfusion/ej2-angular-lists';
import { AmountToTextPipe } from 'src/app/cores/shared/pipe/amount-to-text';
import { NgxFileDragDropModule } from 'ngx-file-drag-drop';
import { MatChipsModule } from '@angular/material/chips';
import { NguoiViPhamSearchComponent } from './nguoi-vi-pham-search.component';

const routes: Routes = [
  {
    path: 'nguoi-vi-pham',
    component: NguoiViPhamSearchComponent,
    data: { animation: 'VuViecManage' },
  }
];

@NgModule({
  declarations: [
    NguoiViPhamSearchComponent
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
export class TraCuuModule {}
// // AOT compilation support
// export function httpTranslateLoader(http: HttpClient) {
//   return new MultiTranslateHttpLoader(http, [{ prefix: '/assets/i18n/cores/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/cores/message/', suffix: '.json' },
//   { prefix: '/assets/i18n/projects/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/projects/message/', suffix: '.json' },
//   { prefix: '/assets/i18n/modules/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/modules/message/', suffix: '.json' },]);
// }
