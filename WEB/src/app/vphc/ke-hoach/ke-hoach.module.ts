import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatChipsModule } from '@angular/material/chips';
import { RouterModule, Routes } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { DateTimePickerModule, DatePickerModule } from '@syncfusion/ej2-angular-calendars';
import { DropDownListModule } from '@syncfusion/ej2-angular-dropdowns';
import { GridModule } from '@syncfusion/ej2-angular-grids';
import { UploaderModule } from '@syncfusion/ej2-angular-inputs';
import { SplitterModule } from '@syncfusion/ej2-angular-layouts';
import { ListViewModule } from '@syncfusion/ej2-angular-lists';
import { TreeViewModule, TabModule } from '@syncfusion/ej2-angular-navigations';
import { TreeGridModule, SortService, PageService, FilterService, ContextMenuService, EditService } from '@syncfusion/ej2-angular-treegrid';
import { NgxFileDragDropModule } from 'ngx-file-drag-drop';
import { AmountToTextPipe } from 'src/app/cores/shared/pipe/amount-to-text';
import { SharedModule } from 'src/app/cores/shared/shared.module';
import { TranslationModule } from 'src/app/modules/i18n';
import { KeHoachListComponent } from './ke-hoach-list/ke-hoach-list.component';
import { KeHoachEditComponent } from './ke-hoach-edit/ke-hoach-edit.component';
import { KeHoachCapCucEditComponent } from './ke-hoach-cap-cuc-edit/ke-hoach-cap-cuc-edit.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: KeHoachListComponent,
        data: { animation: 'KeHoachManage' },
        // component: KeHoachEditComponent,
        // data: { animation: 'KeHoachCreate' },
      },
      {
        path: 'chinh-sua/:id',
        component: KeHoachEditComponent,
        data: { animation: 'KeHoachUpdate' },
      },
    ],
  },
  {
    path: 'them-moi',
    component: KeHoachEditComponent,
    data: { animation: 'KeHoachCreate' },
  }
];

@NgModule({
  declarations: [
    KeHoachEditComponent,
    KeHoachCapCucEditComponent,
    KeHoachListComponent
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
  providers: [SortService, PageService, FilterService, ContextMenuService, EditService, AmountToTextPipe],
})
export class KeHoachModule { }


