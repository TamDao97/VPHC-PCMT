import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InlineSVGModule } from 'ng-inline-svg-2';
import { RouterModule, Routes } from '@angular/router';
import {
  NgbDropdownModule,
  NgbProgressbarModule,
  NgbTooltipModule,
} from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule } from '@ngx-translate/core';
import { TranslationModule } from '../../modules/i18n';
import { LayoutComponent } from './layout.component';
import { ExtrasModule } from '../partials/layout/extras/extras.module';
import { Routing } from '../../pages/routing';
import { HeaderComponent } from './components/header/header.component';
import { ContentComponent } from './components/content/content.component';
import { FooterComponent } from './components/footer/footer.component';
import { ScriptsInitComponent } from './components/scripts-init/scripts-init.component';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { PageTitleComponent } from './components/header/page-title/page-title.component';
import {
  DrawersModule,
  DropdownMenusModule,
  ModalsModule,
} from '../partials';
import { ThemeModeModule } from '../partials/layout/theme-mode-switcher/theme-mode.module';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { SidebarLogoComponent } from './components/sidebar/sidebar-logo/sidebar-logo.component';
import { SidebarMenuComponent } from './components/sidebar/sidebar-menu/sidebar-menu.component';
import { SidebarFooterComponent } from './components/sidebar/sidebar-footer/sidebar-footer.component';
import { NavbarComponent } from './components/header/navbar/navbar.component';
import { AccountingComponent } from './components/toolbar/accounting/accounting.component';
import { ClassicComponent } from './components/toolbar/classic/classic.component';
import { ExtendedComponent } from './components/toolbar/extended/extended.component';
import { ReportsComponent } from './components/toolbar/reports/reports.component';
import { SaasComponent } from './components/toolbar/saas/saas.component';
import {SharedModule} from "../shared/shared.module";
import { PageRouting } from '../pages/page.routing';
import { PageVPHCRouting } from 'src/app/vphc/vphc.routing';
import { DropDownListModule } from '@syncfusion/ej2-angular-dropdowns';
import { SidebarDepartmentComponent } from './components/sidebar/sidebar-department/sidebar-department.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: Routing.concat(PageRouting).concat(PageVPHCRouting),
  },
];

@NgModule({
  declarations: [
    LayoutComponent,
    HeaderComponent,
    ContentComponent,
    FooterComponent,
    ScriptsInitComponent,
    ToolbarComponent,
    PageTitleComponent,
    SidebarComponent,
    SidebarLogoComponent,
    SidebarDepartmentComponent,
    SidebarMenuComponent,
    SidebarFooterComponent,
    NavbarComponent,
    AccountingComponent,
    ClassicComponent,
    ExtendedComponent,
    ReportsComponent,
    SaasComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    TranslationModule,
    InlineSVGModule,
    NgbDropdownModule,
    NgbProgressbarModule,
    ExtrasModule,
    ModalsModule,
    DrawersModule,
    DropdownMenusModule,
    NgbTooltipModule,
    TranslateModule,
    ThemeModeModule,
    SharedModule,
    DropDownListModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [RouterModule],
})
export class LayoutModule {}
