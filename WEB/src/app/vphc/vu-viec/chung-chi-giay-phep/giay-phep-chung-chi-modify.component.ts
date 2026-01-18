import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  NgbActiveModal,
  NgbDateStruct,
  NgbModal,
} from '@ng-bootstrap/ng-bootstrap';
import { VuViecService } from '../../service/vu-viec.service';
import { TranslateService } from '@ngx-translate/core';
import { TreeGridComponent } from '@syncfusion/ej2-angular-treegrid';
import { FormControl, Validators } from '@angular/forms';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { FileService } from 'src/app/cores/shared/services/file.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { environment } from 'src/environments/environment';
import { TabComponent } from '@syncfusion/ej2-angular-navigations';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-giay-phep-chung-chi-modify',
  templateUrl: './giay-phep-chung-chi-modify.component.html',
  styleUrls: ['./giay-phep-chung-chi-modify.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class GiayPhepChungChiModifyComponent implements OnInit {
  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private vuViecService: VuViecService,
    private router: Router,
    private fileService: FileService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private activeModal: NgbActiveModal,
    private changeDetectorRef: ChangeDetectorRef,
    private comboboxService: ComboboxCoreService
  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  id: any = '';
  model: any = {
    ten:'',
    bienSo:'',
    soLuong:1,
    chungLoai:'',
    tinhTrangDacDiem:''
  };
  listData: any[] = [];
  isEdit: boolean = false;

  ngOnInit(): void {
    if (!this.id) {
      this.model.idChungChiGiayPhep = uuidv4();
    }else{
      this.isEdit = true;
    }
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });
  }

  save(isContinue: boolean = false) {
    if (this.id) {
      this.activeModal.close({ success: true, data: this.model });
    } else {
      this.listData.push(this.model);
      this.model = {};
      if (!isContinue) {
        this.activeModal.close({ success: true, data: this.listData });
      }
    }
  }

  close(result: false): void {
    this.activeModal.close(result); // Đóng modal
  }
}
