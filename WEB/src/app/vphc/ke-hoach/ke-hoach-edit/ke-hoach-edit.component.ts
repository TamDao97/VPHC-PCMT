import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ContextMenuItemModel, GridComponent } from '@syncfusion/ej2-angular-grids';
import { Observable, firstValueFrom } from 'rxjs';
import { Constants } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { FileService } from 'src/app/cores/shared/services/file.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { AuthService, UserType } from 'src/app/modules/auth';
import { KeHoachService } from '../../service/ke-hoach.service';

@Component({
  selector: 'app-ke-hoach-edit',
  templateUrl: './ke-hoach-edit.component.html',
  styleUrl: './ke-hoach-edit.component.scss'
})
export class KeHoachEditComponent {

}