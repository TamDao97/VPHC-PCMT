import {
  AfterViewInit,
  Component,
  OnDestroy,
  OnInit,
  ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  NgbActiveModal,
  NgbDateStruct,
  NgbModal,
} from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import {
  ContextMenuService,
  TreeGridComponent,
} from '@syncfusion/ej2-angular-treegrid';
import { FormControl, Validators } from '@angular/forms';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { FileService } from 'src/app/cores/shared/services/file.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { VuViecService } from 'src/app/vphc/service/vu-viec.service';
import { AmountToTextPipe } from 'src/app/cores/shared/pipe/amount-to-text';
import { FileValidators } from 'ngx-file-drag-drop';
import { TaiLieuService } from 'src/app/vphc/service/tai-lieu-service';
// import { VuViecXacMinhService } from '../../service/vu-viec-quyet-dinh.service';

@Component({
  providers: [ContextMenuService],
  selector: 'app-tai-lieu-upload',
  templateUrl: './tai-lieu-upload.component.html',
  styleUrls: ['./tai-lieu-upload.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class TaiLieuUploadComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  constructor(
    public fileProcess: FileProcess,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private taiLieuService: TaiLieuService,
    private fileService: FileService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private activeModal: NgbActiveModal,
  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  //Id tài liệu
  public id: any;
  public idVuViec: any;
  model: any = {
    idVuViec: '',
    idCategory: '',
    listFileUpload:[],
  };

  //#region -------------Xự kiện của trang
  ngOnInit(): void {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.getDataCombobox();

    this.model.idVuViec = this.idVuViec;

    if (this.id) {
      this.getById();
    }
  }

  //Khởi tạo xong component
  ngAfterViewInit() {}

  //Kết thúc component
  ngOnDestroy() {}

  close(result: boolean = false): void {
    this.activeModal.close(result); // Đóng modal
  }
  //#endregion

  //#region -------------Xử lý combobox-----------
  public fields = { text: 'name', value: 'id' };
  public listDanhMuc: any;

  getDataCombobox() {
    this.taiLieuService.getDanhMuc().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listDanhMuc = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion

  //#region ------------Lấy thông tin cập nhật---------
  getById() {
    this.taiLieuService.getById(this.id).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.model = result.data;
          this.model.doiTuongViPham = this.model.doiTuongViPham.toString();
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion

  //#endregion ------------Xử lý lưu CSDL
  public save() {
    this.fileService
        .uploadFiles(this.filesUpload, 'VuViec')
        .subscribe({
          next: (result: any) => {
            this.model.listFileUpload = result.data;
             this.create();
          },
          error: (error: any) => {
            this.messageService.showError(error);
          },
        });
  }

  create() {
    this.taiLieuService.create(this.model).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.messageService.showSuccess('Thêm mới tài liệu thành công!');
          this.close(true);
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion

  //#region ------------Xử lý upload file---------------
  filesUpload: File[] = [];
  fileControl = new FormControl(
    [],
    [FileValidators.required, FileValidators.maxFileCount(2)]
  );

  onValueChange(files: File[]) {
    this.filesUpload = files;
    console.log('File changed!');
  }
  //#endregion
}
