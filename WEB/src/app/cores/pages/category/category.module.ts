import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
//import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { TreeGridModule } from '@syncfusion/ej2-angular-treegrid';
import { DragDropModule } from '@angular/cdk/drag-drop';
//import { CurrencyMaskModule } from 'ng2-currency-mask';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
//import { MultiTranslateHttpLoader } from 'ngx-translate-multi-http-loader';
import { RouterModule, Routes } from '@angular/router';
import { CategoryManageComponent } from './category/category-manage/category-manage.component';
import { CategoryCreateComponent } from './category/category-create/category-create.component';
import { SharedModule } from '../../shared/shared.module';
import { TranslationModule } from 'src/app/modules/i18n';

const routes: Routes = [
  {
    path: '',
    component: CategoryManageComponent,
  },
  {
    path: 'create',
    component: CategoryCreateComponent,
  },
];

@NgModule({
  declarations: [
    CategoryManageComponent,
    CategoryCreateComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    TranslationModule,
    TranslateModule,
    NgbModule,
    //PerfectScrollbarModule,
    NgSelectModule,
    SharedModule,
    FormsModule,
    TreeGridModule,
    DragDropModule,
    HttpClientModule
    //CurrencyMaskModule,
  //   TranslateModule.forRoot({
  //     loader: {
  //         provide: TranslateLoader,
  //         useFactory: httpTranslateLoader,
  //         deps: [HttpClient]
  //     }
  // })
    
  ]
})
export class CategoryModule { }
// export function httpTranslateLoader(http: HttpClient) {
//   return new MultiTranslateHttpLoader(http, [{ prefix: '/assets/i18n/cores/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/cores/message/', suffix: '.json' },
//   { prefix: '/assets/i18n/projects/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/projects/message/', suffix: '.json' },
//   { prefix: '/assets/i18n/modules/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/modules/message/', suffix: '.json' },]);
// }