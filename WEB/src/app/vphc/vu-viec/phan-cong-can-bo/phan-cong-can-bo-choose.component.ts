import {
  ChangeDetectorRef,
  Component,
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
import { TranslateService } from '@ngx-translate/core';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { FileService } from 'src/app/cores/shared/services/file.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { v4 as uuidv4 } from 'uuid';
import { CanBoService } from '../../service/can-bo-service';
import { GridComponent } from '@syncfusion/ej2-angular-grids';

@Component({
  selector: 'app-phan-cong-can-bo-choose',
  templateUrl: './phan-cong-can-bo-choose.component.html',
  styleUrls: ['./phan-cong-can-bo-choose.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class PhanCongCanBoComponent implements OnInit {
  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private canBoService: CanBoService,
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

  @ViewChild('grCanBo')
  public grCanBo: GridComponent;

  //Id đơn vị mở hồ sơ
  id: any = '';
  //Danh sách cán bộ đã chọn
  public listIdCanBoSelected: any[];

  //#region -----------Các sự kiện của modal--------------
  ngOnInit(): void {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.getDataCanBo();
  }
  
  save(isContinue: boolean = false) {
    this.activeModal.close({
      success: true,
      data: this.listCanBoChoose.filter((item: any) => {
        return item.checked;
      }),
    });
  }

  close(result: false): void {
    this.activeModal.close(result); // Đóng modal
  }
  //#endregion

  //#region ----------------Danh sách cán bộ----------------
  public fields = { text: 'name', value: 'id' };
  public listCanBoChoose: any[];
  getDataCanBo() {
    this.canBoService.getByIdDonVi(this.id).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listCanBoChoose = result.data;
          //Xử lý check các cán bộ đã chọn
          this.listCanBoChoose.map(item => {
            if (this.listIdCanBoSelected.includes(item.id)) {
              item.checked = true; 
            }
          });
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion

  //#endregion xử lý Check, Check all
  public isAllChecked: boolean = false;
  public onCheckAll(event: any): void {
    const isChecked = event.target.checked;
    this.isAllChecked = isChecked;

    // Cập nhật trạng thái cho tất cả các dòng
    this.listCanBoChoose.forEach((item) => {
      item.checked = isChecked;
    });
    this.grCanBo.refresh();
  }

  // Hàm xử lý khi checkbox trong từng dòng thay đổi
  public onCheckboxChange(event: any, updatedRecord: any): void {
    const isChecked = event.target.checked;

    // Cập nhật trạng thái dòng được chỉnh sửa
    const index = this.listCanBoChoose.findIndex(
      (item) => item.id === updatedRecord.id
    );
    if (index !== -1) {
      this.listCanBoChoose[index].checked = isChecked;
    }

    // Kiểm tra trạng thái "Check All"
    this.isAllChecked = this.listCanBoChoose.every((item) => item.checked);
  }
  //#endregion
}
