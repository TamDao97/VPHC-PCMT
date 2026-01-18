import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateStruct, NgbModal } from '@ng-bootstrap/ng-bootstrap';
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
import { environment } from 'src/environments/environment';
import { TabComponent } from '@syncfusion/ej2-angular-navigations';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { NguoiViPhamModifyComponent } from '../nguoi-vi-pham/nguoi-vi-pham-modify.component';
import { MatDialog } from '@angular/material/dialog';
import { ToChucModifyComponent } from '../to-chuc/to-chuc-modify.component';
import {
  ContextMenuItemModel,
  GridComponent,
} from '@syncfusion/ej2-angular-grids';
import { NguoiPhienDichModifyComponent } from '../nguoi-phien-dich/nguoi-phien-dich-modify.component';
import { NguoiChungKienModifyComponent } from '../nguoi-chung-kien/nguoi-chung-kien-modify.component';
import { TangVatModifyComponent } from '../tang-vat/tang-vat-modify.component';
import { PhuongTienModifyComponent } from '../phuong-tien/phuong-tien-modify.component';
import { GiayPhepChungChiModifyComponent } from '../chung-chi-giay-phep/giay-phep-chung-chi-modify.component';
import { PhanCongCanBoComponent } from '../phan-cong-can-bo/phan-cong-can-bo-choose.component';
import { XuLyService } from '../../service/xu-ly.service';
import { QuyetDinhService } from '../../service/quyet-dinh-service';
import { QuyetDinhModifyComponent } from '../quyet-dinh/quyet-dinh-modify/quyet-dinh-modify.component';
import { DanhMucQuyetDinhComponent } from '../quyet-dinh/danh-muc/danh-muc-choose.component';
import { NtsViewFileComponent } from 'src/app/cores/shared/component/nts-view-file/nts-view-file.component';
import { firstValueFrom } from 'rxjs';

@Component({
  providers: [ContextMenuService],
  selector: 'app-xu-ly-modify',
  templateUrl: './xu-ly-modify.component.html',
  styleUrls: ['./xu-ly-modify.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class XuLyModifyComponent implements OnInit, AfterViewInit, OnDestroy {
  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private xuLyService: XuLyService,
    private router: Router,
    private fileService: FileService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private modalService: NgbModal,
    private comboboxService: ComboboxCoreService,
    private QuyetDinhService: QuyetDinhService
  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  public contextMenuItems?: ContextMenuItemModel[] = [
    {
      text: 'Chỉnh sửa',
      target: '.e-content',
      id: 'edit',
      iconCss: 'fa fa-edit',
    },
    {
      text: 'Xem chi tiết',
      target: '.e-content',
      id: 'detail',
      iconCss: 'fas fa-file-alt',
    },
    {
      text: 'Xóa',
      target: '.e-content',
      id: 'delete',
      iconCss: 'fas fa-trash-alt',
    },
  ];

  @Input() id: string;
  @Output() changeTotal = new EventEmitter<number>();
  model: any = {
    phanLoai: 1,
    tongTienPhat: 0,
    idXuLy: '',
    donViKhacXuLy: '',
    soBienBanDVKhac: '',
    ngayBanGiaoDVKhac: null,
    donViTiepNhanHS: '',
    soBienBanHS: '',
    ngayBanGiaoHS: null,
    taiLieuChuyenGiao: '',
    noiDungNhacNho: '',
    lyDoKhongQDXP: '',
    tongQDXuPhat: 0,
    tongQDThiHanh: 0,
    tongQDKhieuKien: 0,
    tongQDChuyen: 0,
    tongQDDangXuLy: 0,
    tongQDMienGiam: 0,
    tongQDCuongChe: 0,
    listTangVat: [],
    listPhuongTien: [],
    listGiayPhepChungChi: [],
  };

  //#region -------------Xự kiện của trang
  async ngOnInit(): Promise<void> {
    this.id = this.routeA.snapshot.paramMap.get('id') ?? '';

    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.getDataCombobox();

    if (this.id) {
      const result = await firstValueFrom(this.xuLyService.getById(this.id));
      if (result.isStatus) {
        this.model = result.data;
        this.totalTangVat = this.model.listTangVat.length;
        this.totalPhuongTien = this.model.listPhuongTien.length;
        this.totalGiayPhepChungChi = this.model.listGiayPhepChungChi.length;
      }
    }

    this.getQuyetDinhVuViec();
  }

  ngAfterViewInit() {}

  ngOnDestroy() {}
  //#endregion

  //#region -------------Xử lý combobox-----------
  public fields = { text: 'name', value: 'id' };
  public listXuLy: any;
  public listXuLyTangVat: any;
  public listXuLyPhuongTien: any;
  public listXuLyGiayPhep: any;

  getDataCombobox() {
    this.comboboxService.getXuLyVPHC(this.model.phanLoai).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listXuLy = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });

    this.comboboxService.getXuLyTangVat().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listXuLyTangVat = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });

    this.comboboxService.getXuLyPhuongTien().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listXuLyPhuongTien = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });

    this.comboboxService.getXuLyGiayTo().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listXuLyGiayPhep = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }

  onSelect(event: any, type: any): void {}

  async updateTotal() {
    const result = await firstValueFrom(this.xuLyService.getById(this.id));
    if (result.isStatus) {
      this.model.tongTienPhat = result.data.tongTienPhat;
      this.model.tongQDXuPhat = result.data.tongQDXuPhat;
      this.model.tongQDThiHanh = result.data.tongQDThiHanh;
      this.model.tongQDKhieuKien = result.data.tongQDKhieuKien;
      this.model.tongQDChuyen = result.data.tongQDChuyen;
      this.model.tongQDDangXuLy = result.data.tongQDDangXuLy;
      this.model.tongQDMienGiam = result.data.tongQDMienGiam;
      this.model.tongQDCuongChe = result.data.tongQDCuongChe;
    }
  }
  //#endregion

  //#endregion ------------Xử lý lưu CSDL
  public save(isContinue: boolean = false) {
    if (this.id) {
      this.update();
    }
  }

  private update() {
    this.xuLyService.update(this.id, this.model).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.messageService.showSuccess('Lưu xử lý vụ việc thành công!');
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion

  //#region ----------------Tang vật----------------
  @ViewChild('grTangVat')
  public grTangVat: GridComponent;
  public totalTangVat: any = 0;

  dataTangVat_Change(event: any, updatedRecord: any, columnName: any) {
    // Cập nhật giá trị mới vào dữ liệu gốc
    const index = this.model.listTangVat.findIndex(
      (item: any) => item.idTangVatVPHC === updatedRecord.idTangVatVPHC
    );
    if (index !== -1) {
      if (columnName === 'XuLy')
        this.model.listTangVat[index].xuLy = event.value;
      else if (columnName === 'GhiChu')
        this.model.listTangVat[index].ghiChu = event.target.value;
    }
  }
  //#endregion

  //#region ----------------Phương tiện----------------
  @ViewChild('grPhuongTien')
  public grPhuongTien: GridComponent;
  public totalPhuongTien: any = 0;

  dataPhuongTien_Change(event: any, updatedRecord: any, columnName: any) {
    // Cập nhật giá trị mới vào dữ liệu gốc
    const index = this.model.listPhuongTien.findIndex(
      (item: any) => item.idPhuongTienVPHC === updatedRecord.idPhuongTienVPHC
    );
    if (index !== -1) {
      if (columnName === 'XuLy')
        this.model.listPhuongTien[index].xuLy = event.value;
      else if (columnName === 'GhiChu')
        this.model.listPhuongTien[index].ghiChu = event.target.value;
    }
  }
  //#endregion

  //#region ----------------Giấy phép, chứng chỉ hành nghề----------------
  @ViewChild('grGiayPhepChungChi')
  public grGiayPhepChungChi: GridComponent;
  public totalGiayPhepChungChi: any = 0;

  dataGiayPhep_Change(event: any, updatedRecord: any, columnName: any) {
    // Cập nhật giá trị mới vào dữ liệu gốc
    const index = this.model.listGiayPhepChungChi.findIndex(
      (item: any) =>
        item.idChungChiGiayPhep === updatedRecord.idChungChiGiayPhep
    );
    if (index !== -1) {
      if (columnName === 'XuLy')
        this.model.listGiayPhepChungChi[index].xuLy = event.value;
      else if (columnName === 'GhiChu')
        this.model.listGiayPhepChungChi[index].ghiChu = event.target.value;
    }
  }
  //#endregion

  //#region ---------Quyết định------------
  //#region ------------Lấy thông tin cập nhật---------
  getQuyetDinhVuViec() {
    this.QuyetDinhService.getQuyetDinhVuViec(this.id).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listQuyetDinh = result.data.dataResults;
          this.totalQuyetDinh = this.listQuyetDinh.length;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion

  public listQuyetDinh: any[] = [];
  @ViewChild('grQuyetDinh')
  public grQuyetDinh: GridComponent;
  public totalQuyetDinh: any = 0;

  public contextMenuItems_QuyetDinh?: ContextMenuItemModel[];

  openModalDanhMuc(id: any = '', model: any = null): void {
    let activeModal = this.modalService.open(DanhMucQuyetDinhComponent, {
      container: 'body',
      windowClass: 'danh-muc-qd-choose',
      backdrop: 'static',
    });
    activeModal.result.then((result) => {
      if (result.success) {
        this.openModalQuyetDinh('', result.data.id);
      }
    });
  }

  openModalQuyetDinh(id: any, idDanhMuc: any = ''): void {
    let activeModal = this.modalService.open(QuyetDinhModifyComponent, {
      container: 'body',
      windowClass: 'quyet-dinh-modify',
      backdrop: 'static',
    });
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.idVuViec = this.id;
    activeModal.componentInstance.idDanhMuc = idDanhMuc;
    activeModal.result.then((result) => {
      if (result) {
        // Làm mới lưới
        this.getQuyetDinhVuViec();
        this.updateTotal();
        if (!id) {
          this.changeTotal.emit(1);
        }
      }
    });
  }

  /**Xác nhận và xóa tạm người vi phạm */
  showConfirmDelete(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá quyết định này không?')
      .then((data) => {
        this.QuyetDinhService.delete(id).subscribe({
          next: (data) => {
            if (data.isStatus) {
              this.messageService.showSuccess('Xóa quyết định thành công!');
              this.getQuyetDinhVuViec();
              this.changeTotal.emit(-1);
              this.updateTotal();
            }
          },
          error: (error) => {
            this.messageService.showError(error);
          },
        });
        // Làm mới lưới
        this.getQuyetDinhVuViec();
      });
  }

  contextMenuClick_QuyetDinh(args: any): void {
    const id = args.rowInfo.rowData.idQuyetDinh;
    if (args.item.id === 'edit') {
      this.openModalQuyetDinh(id);
    } else if (args.item.id === 'detail') {
      this.viewQuyetDinh(id);
    } else if (args.item.id === 'download') {
      this.downloadQuyetDinh(id);
    } else if (args.item.id === 'delete') {
      this.showConfirmDelete(id);
    }
  }

  viewQuyetDinh(id: string) {
    this.QuyetDinhService.exportPdf(id).subscribe({
      next: (data) => {
        let activeModal = this.modalService.open(NtsViewFileComponent, {
          container: 'body',
          windowClass: 'viewpdf-file-modal',
          backdrop: 'static',
        });
        activeModal.componentInstance.fileContentResult = data;
        activeModal.componentInstance.nameFile = 'QuyetDinh_VPHC.pdf';
        activeModal.result.then((result: any) => {
          if (result) {
          }
        });
      },
      error: (error) => {
        const blb = new Blob([error.error], { type: 'text/plain' });
        const reader = new FileReader();

        reader.onload = () => {
          if (reader && reader.result)
            this.messageService.showMessage(
              reader.result.toString().replace('"', '').replace('"', '')
            );
        };
        // Start reading the blob as text.
        reader.readAsText(blb);
      },
    });
  }

  downloadQuyetDinh(id: string) {
    this.QuyetDinhService.exportWord(id).subscribe({
      next: (data) => {
        var blob = new Blob([data], { type: 'octet/stream' });
        var url = window.URL.createObjectURL(blob);
        this.fileProcess.downloadFileLink(url, 'QuyetDinh_VPHC.doc');
      },
      error: (error) => {
        const blb = new Blob([error.error], { type: 'text/plain' });
        const reader = new FileReader();

        reader.onload = () => {
          if (reader && reader.result)
            this.messageService.showMessage(
              reader.result.toString().replace('"', '').replace('"', '')
            );
        };
        // Start reading the blob as text.
        reader.readAsText(blb);
      },
    });
  }
  //#endregion
}
