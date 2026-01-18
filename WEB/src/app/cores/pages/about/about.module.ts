import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { NgSelectModule } from '@ng-select/ng-select';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';
import { SetupAboutComponent } from './about-edit/setup-about.component';
import { AboutComponent } from './about-view/about.component';
import { TranslationModule } from 'src/app/modules/i18n';
import { TranslateModule } from '@ngx-translate/core';

const routes: Routes = [
  {
    path: '',
    component: AboutComponent,
  },
  {
    path: 'cau-hinh',
    component: SetupAboutComponent,
  },
];

@NgModule({
  declarations: [AboutComponent, SetupAboutComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    TranslationModule,
    TranslateModule,
    FormsModule,
    NgSelectModule,
    SharedModule,
    FormsModule,
  ],
})
export class AboutModule {}
