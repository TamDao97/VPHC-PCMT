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
  selector: 'app-nguoi-vi-pham-modify',
  templateUrl: './nguoi-vi-pham-modify.component.html',
  styleUrls: ['./nguoi-vi-pham-modify.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class NguoiViPhamModifyComponent implements OnInit {
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
    idNguoiVPHC: '',
    hoVaTen: '',
    ngaySinh: null,
    gioiTinh: '',
    idQuocTich: '',
    idDanToc: '',
    idTonGiao: '',
    idNgheNghiep: '',
    trinhDoVanHoa: '',
    soDienThoai: '',
    cmnd: '',
    ngayCap: null,
    noiCap: '',
    idTinh: '',
    idHuyen: '',
    idXa: '',
    diaChiDayDu: '',
    idTinhHienNay: '',
    idHuyenHienNay: '',
    idXaHienNay: '',
    diaChi: '',
    diaChiHienNayDayDu: '',
    ghiChu: '',
    tuoi: null,
    hoanCanhKhoKhan: false,
    chiTietHoanCanh: '',
  };
  listData: any[] = [];
  filedata: any = null;
  isEdit: boolean = false;

  ngOnInit(): void {
    if (!this.id) {
      this.model.idNguoiVPHC = uuidv4();
    } else {
      this.isEdit = true;
      if (this.model.idTinh) {
        this.getHuyen(this.model.idTinh, false);
      }
      if (this.model.idHuyen) {
        this.getXa(this.model.idHuyen, false);
      }
      if (this.model.idTinhHienNay) {
        this.getHuyen(this.model.idTinhHienNay, true);
      }
      if (this.model.idHuyenHienNay) {
        this.getXa(this.model.idHuyenHienNay, true);
      }
    }
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.getDataCombobox();
  }

  //#region -------------Xử lý combobox-----------
  public fields = { text: 'name', value: 'id' };
  public listTinh: any[];
  public listHuyen: any[];
  public listXa: any[];
  public listTinhHienNay: any[];
  public listHuyenHienNay: any[];
  public listXaHienNay: any[];
  public listGioiTinh: any[];
  public listQuocTich: any[];
  public listDanToc: any[];
  public listTonGiao: any[];
  public listNgheNghiep: any[];

  getDataCombobox() {
    this.comboboxService.getTinh().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listTinh = result.data;
          this.listTinhHienNay = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });

    this.comboboxService.getGioiTinh().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listGioiTinh = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });

    this.comboboxService.getQuocTich().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listQuocTich = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });

    this.comboboxService.getDanToc().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listDanToc = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });

    this.comboboxService.getTonGiao().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listTonGiao = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });

    this.comboboxService.getNgheNghiep().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listNgheNghiep = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }

  getHuyen(id: any, residence: boolean) {
    this.comboboxService.getHuyen(id).subscribe({
      next: (result) => {
        if (result.isStatus) {
          if (residence) {
            this.listHuyenHienNay = result.data;
          } else {
            this.listHuyen = result.data;
          }
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }

  getXa(id: any, residence: boolean) {
    this.comboboxService.getXa(id).subscribe({
      next: (result) => {
        if (result.isStatus) {
          if (residence) {
            this.listXaHienNay = result.data;
          } else {
            this.listXa = result.data;
          }
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }

  //#endregion

  //#region ------------Sự kiện select-------------------
  onSelect_Tinh(event: any, residence: boolean): void {
    if (residence) {
      this.model.tenTinhHienNay = event.itemData.name;
    } else {
      this.model.tenTinh = event.itemData.name;
    }

    this.getHuyen(event.value, residence);
  }

  onSelect_Huyen(event: any, residence: boolean): void {
    if (residence) {
      this.model.tenHuyenHienNay = event.itemData.name;
    } else {
      this.model.tenHuyen = event.itemData.name;
    }
    this.getXa(event.value, residence);
  }

  onSelect_Xa(event: any, residence: boolean): void {
    if (residence) {
      this.model.tenXaHienNay = event.itemData.name;
    } else {
      this.model.tenXa = event.itemData.name;
    }
  }

  onSelect(event: any, type: any): void {
    if (type == 'GioiTinh') {
      this.model.tenGioiTinh = event.itemData.name;
    } else if (type == 'QuocTich') {
      this.model.tenQuocTich = event.itemData.name;
    } else if (type == 'DanToc') {
      this.model.tenDanToc = event.itemData.name;
    } else if (type == 'TonGiao') {
      this.model.tenTonGiao = event.itemData.name;
    } else if (type == 'NgheNghiep') {
      this.model.tenNgheNghiep = event.itemData.name;
    }
  }
  //#endregion

  save(isContinue: boolean = false) {
    this.xuLyChuoiDiaChi();
    if (this.id) {
      this.activeModal.close({ success: true, data: this.model });
    } else {
      this.listData.push(this.model);
      this.model = {};
      if (this.fileProcess.FileDataBase != null) {
        this.filedata = null;
        this.fileProcess.fileModel.DataURL = null;
      }
      if (!isContinue) {
        this.activeModal.close({ success: true, data: this.listData });
      }
    }
  }

  xuLyChuoiDiaChi() {
    if (this.model.tenXa) {
      this.model.diaChiDayDu += this.model.tenXa;
    }

    if (this.model.tenHuyen) {
      this.model.diaChiDayDu += ' - ' + this.model.tenHuyen;
    }

    if (this.model.tenTinh) {
      this.model.diaChiDayDu += ' - ' + this.model.tenTinh;
    }

    if (this.model.diaChi) {
      this.model.diaChiHienNayDayDu += this.model.diaChi;
    }

    if (this.model.tenXaHienNay) {
      this.model.diaChiHienNayDayDu += ' - ' + this.model.tenXaHienNay;
    }

    if (this.model.tenHuyenHienNay) {
      this.model.diaChiHienNayDayDu += ' - ' + this.model.tenHuyenHienNay;
    }

    if (this.model.tenTinhHienNay) {
      this.model.diaChiHienNayDayDu += ' - ' + this.model.tenTinhHienNay;
    }
  }

  close(result: false): void {
    this.activeModal.close(result); // Đóng modal
  }

  onFileChange($event: any) {
    this.fileProcess.onAFileChange($event);
    this.changeDetectorRef.detectChanges();
  }

  deleteFile($event: any) {
    if (this.filedata != null) {
      // this.modelDelteFile.anh = this.filedata;
    }

    if (this.fileProcess.FileDataBase != null) {
      this.filedata = null;
      this.fileProcess.fileModel.DataURL = null;
    }
  }
}
