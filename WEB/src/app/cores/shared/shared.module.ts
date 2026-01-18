import { NgModule } from '@angular/core';
import { KeeniconComponent } from './keenicon/keenicon.component';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NtsViewFileComponent } from './component/nts-view-file/nts-view-file.component';
import { ImageViewerComponent } from './component/image-viewer/image-viewer.component';
import { NtsImageComponent } from './component/nts-image/nts-image.component';
import { NtsTextMoreComponent } from './component/nts-text-more/nts-text-more.component';
import { NtsHorizontalStepperComponent } from './component/nts-horizontal-stepper/nts-horizontal-stepper.component';
import { NTSSearchBarComponent } from './component/nts-search-bar/nts-search-bar.component';
import { SwitchLanguageComponent } from './component/switch-language/switch-language.component';
import { ImportExcelComponent } from './component/import-excel/import-excel.component';
import { NtsStatusComponent } from './component/nts-status/nts-status.component';
import { NtsStatusBadgeComponent } from './component/nts-status-badge/nts-status-badge.component';
import { MessageconfirmcodeComponent } from './component/messageconfirmcode/messageconfirmcode.component';
import { MessageComponent } from './component/message/message.component';
import { MessageconfirmComponent } from './component/messageconfirm/messageconfirm.component';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { UipermissionDirective } from '../layout/directives/uipermission.directive';
import { NtsTinymceComponent } from './tinymce/nts-tinymce.component';
import { NtscurrencyPipe } from './pipe/ntscurrency.pipe';
import { NtsNumberPipe } from './pipe/ntsnumber.pipe';
import { TruncatePipe } from './pipe/truncate.pipe';
import { SafeHtmlPipe } from './pipe/safe-html.pipe';
import { OrderByPipe } from './pipe/orderby.pipe';
import { ComboboxCoreService } from './services/combobox-core.service';
import { NtsPaginationComponent } from './nts-pagination/nts-pagination.component';
import { TranslationModule } from 'src/app/modules/i18n';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    KeeniconComponent,
    UipermissionDirective,
    MessageconfirmComponent,
    MessageComponent,
    MessageconfirmcodeComponent,
    NtsStatusBadgeComponent,
    NtsStatusComponent,
    ImportExcelComponent,
    SwitchLanguageComponent,
    NTSSearchBarComponent,
    NtsHorizontalStepperComponent,
    NtsTextMoreComponent,
    NtsImageComponent,
    NtsViewFileComponent,
    ImageViewerComponent,
    NtsTinymceComponent,
    NtscurrencyPipe,
    NtsNumberPipe,
    TruncatePipe,
    SafeHtmlPipe,
    OrderByPipe,
    NtsPaginationComponent
  ],
  imports: [CommonModule, FormsModule, NgbModule, NgSelectModule,TranslationModule,
      TranslateModule,],
  exports: [
    KeeniconComponent,
    UipermissionDirective,
    MessageconfirmComponent,
    MessageComponent,
    MessageconfirmcodeComponent,
    NtsStatusBadgeComponent,
    NtsStatusComponent,
    ImportExcelComponent,
    SwitchLanguageComponent,
    NTSSearchBarComponent,
    NtsHorizontalStepperComponent,
    NtsTextMoreComponent,
    NtsImageComponent,
    NtsViewFileComponent,
    ImageViewerComponent,
    NtsTinymceComponent,
    NtscurrencyPipe,
    NtsNumberPipe,
    TruncatePipe,
    SafeHtmlPipe,
    OrderByPipe,
    NtsPaginationComponent
  ],
})
export class SharedModule {}
