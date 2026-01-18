import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClient } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TemplateManageComponent } from './template/template-manage/template-manage.component';
import { TemplateCreateComponent } from './template/template-create/template-create.component';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';

const routes: Routes = [
  {
    path: '',   component: TemplateManageComponent, data: { animation: 'TemplateManage' },
  }, 
];

@NgModule({
  declarations: [
    TemplateManageComponent,
    TemplateCreateComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    NgbModule,
    NgSelectModule,
    SharedModule,
    FormsModule,
    TranslateModule
  //   TranslateModule.forRoot({
  //     loader: {
  //         provide: TranslateLoader,
  //         useFactory: httpTranslateLoader,
  //         deps: [HttpClient]
  //     }
  // })    
  ]
})
export class TemplateModule { }
// export function httpTranslateLoader(http: HttpClient) {
//   return new MultiTranslateHttpLoader(http, [{ prefix: '/assets/i18n/cores/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/cores/message/', suffix: '.json' },
//   { prefix: '/assets/i18n/projects/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/projects/message/', suffix: '.json' },
//   { prefix: '/assets/i18n/modules/label/', suffix: '.json' },
//   { prefix: '/assets/i18n/modules/message/', suffix: '.json' },]);
// }