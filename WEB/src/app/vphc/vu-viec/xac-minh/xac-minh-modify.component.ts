import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  Input,
  OnDestroy,
  OnInit,
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
import { VuViecXacMinhService } from '../../service/vu-viec-xac-minh.service';
import { sortList, updateIndex } from 'src/app/cores/shared/common/list-helpers';

@Component({
  providers: [ContextMenuService],
  selector: 'app-xac-minh-modify',
  templateUrl: './xac-minh-modify.component.html',
  styleUrls: ['./xac-minh-modify.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class XacMinhModifyComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private vuViecXacMinhService: VuViecXacMinhService,
    private router: Router,
    private fileService: FileService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private modalService: NgbModal,
    private comboboxService: ComboboxCoreService
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
  model: any = {
    nguoiUyQuyen: '',
    soQDUyQuyen: '',
    ngayUyQuyen: null,
    listCanBoPhanCong: [],
    thietHai: '',
    mucDoThietHai: '',
    giamNhe: '',
    tangNang: '',
    yKienNguoiViPham: '',
    yKienNguoiLamChung: '',
    yKienNguoiBiThietHai: '',
    xacMinhKhac: '',
    listNguoiVP: [],
    listToChucVP: [],
    listPhienDich: [],
    listChungKien: [],
    listTangVat: [],
    listPhuongTien: [],
    listGiayPhepChungChi: [],
  };

  //#region -------------Xự kiện của trang
  ngOnInit(): void {
    this.id = this.routeA.snapshot.paramMap.get('id') ?? '';

    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.getDataCombobox();

    if (this.id) {
      this.getById();
    }
  }

  ngAfterViewInit() {}

  ngOnDestroy() {}
  //#endregion

  //#region -------------Xử lý combobox-----------
  public fields = { text: 'name', value: 'id' };
  public listKetLuan: any;

  getDataCombobox() {
    this.comboboxService.getKetLuanVPHC().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listKetLuan = result.data;
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
    this.vuViecXacMinhService.getById(this.id).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.model = result.data;
          this.totalNguoiVP = this.model.listNguoiVP.length;
          this.totalToChucVP = this.model.listToChucVP.length;
          this.totalTangVat = this.model.listTangVat.length;
          this.totalPhuongTien = this.model.listPhuongTien.length;
          this.totalPhienDich = this.model.listPhienDich.length;
          this.totalChungKien = this.model.listChungKien.length;
          this.totalGiayPhepChungChi = this.model.listGiayPhepChungChi.length;
          this.totalCanBoPhanCong = this.model.listCanBoPhanCong.length;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion

  public validateData() {
    if (
      this.model.listNguoiVP.length > 0 &&
      this.model.listNguoiVP.filter(
        (f: any) => !f.ketLuanKiemTra || !f.hanhViViPham
      ).length > 0
    ) {
      this.messageService.showMessage(
        'Chưa nhập kết luận và hành vi phạm vi phạm của người vi phạm.'
      );
      return false;
    }
    if (
      this.model.listToChucVP.length > 0 &&
      this.model.listToChucVP.filter(
        (f: any) => !f.ketLuanKiemTra || !f.hanhViViPham
      ).length > 0
    ) {
      this.messageService.showMessage(
        'Chưa nhập kết luận và hành vi phạm vi phạm của tổ chức vi phạm.'
      );
      return false;
    }
    return true;
  }

  //#endregion ------------Xử lý lưu CSDL
  public save(isContinue: boolean = false) {
    if (this.id) {
      this.update();
    }
  }

  private update() {
    this.vuViecXacMinhService.update(this.id, this.model).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.messageService.showSuccess(
            'Cập nhập xác minh vụ việc thành công!'
          );
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion

  //#region ----------------Người vi phạm----------------
  @ViewChild('grNguoiVP')
  public grNguoiVP: GridComponent;
  public totalNguoiVP: any = 0;

  public contextMenuItems_NguoiVP?: ContextMenuItemModel[];

  openModalNguoiViPham(id: any = '', model: any = null): void {
    let activeModal = this.modalService.open(NguoiViPhamModifyComponent, {
      container: 'body',
      windowClass: 'nguoi-vi-pham-create',
      backdrop: 'static',
    });
    if (id) {
      activeModal.componentInstance.id = id;
      activeModal.componentInstance.model = model;
    }
    activeModal.result.then((result) => {
      if (result.success) {
        if (Array.isArray(result.data)) {
          result.data.forEach((item: any) => {
            item.ketLuanKiemTra = '';
            item.hanhViViPham = '';
            this.model.listNguoiVP.push(item); // Lặp qua array
          });
        } else {
          this.model.listNguoiVP.forEach((item: any) => {
            if (item.idNguoiVPHC === id) {
              item = result.data;
              return;
            }
          });
        }

        sortList(this.model.listNguoiVP, 'hoVaTen', 'asc');
        updateIndex(this.model.listNguoiVP);
        this.totalNguoiVP = this.model.listNguoiVP.length;
        // Làm mới lưới
        this.grNguoiVP.refresh();
      }
    });
  }

  /**Xác nhận và xóa tạm người vi phạm */
  showConfirmDeleteSoft(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá người vi phạm này không?')
      .then((data) => {
        this.model.listNguoiVP = this.model.listNguoiVP.filter((item: any) => {
          return item.idNguoiVPHC != id; // Thay đổi điều kiện theo nhu cầu
        });
        updateIndex(this.model.listNguoiVP);
        this.totalNguoiVP = this.model.listNguoiVP.length;
        // Làm mới lưới
        this.grNguoiVP.refresh();
      });
  }

  contextMenuClick_NguoiVP(args: any): void {
    const id = args.rowInfo.rowData.idNguoiVPHC;
    if (args.item.id === 'edit') {
      this.openModalNguoiViPham(id, args.rowInfo.rowData);
    } else if (args.item.id === 'detail') {
      this.openModalNguoiViPham(id, args.rowInfo.rowData);
    } else if (args.item.id === 'delete') {
      this.showConfirmDeleteSoft(id);
    }
  }

  dataNguoiVP_Change(event: any, updatedRecord: any, columnName: any) {
    // Cập nhật giá trị mới vào dữ liệu gốc
    const index = this.model.listNguoiVP.findIndex(
      (item: any) => item.idNguoiVPHC === updatedRecord.idNguoiVPHC
    );
    if (index !== -1) {
      if (columnName === 'ketLuanKiemTra')
        this.model.listNguoiVP[index].ketLuanKiemTra = event.value;
      else if (columnName === 'hanhViViPham')
        this.model.listNguoiVP[index].hanhViViPham = event.target.value;
    }
  }
  //#endregion

  //#region ----------------Tổ chức vi phạm----------------
  @ViewChild('grToChucVP')
  public grToChucVP: GridComponent;
  public totalToChucVP: any = 0;

  openModalToChucVP(id: any = '', model: any = null): void {
    let activeModal = this.modalService.open(ToChucModifyComponent, {
      container: 'body',
      windowClass: 'to-chuc-vp-create',
      backdrop: 'static',
    });
    if (id) {
      activeModal.componentInstance.id = id;
      activeModal.componentInstance.model = model;
    }
    activeModal.result.then((result) => {
      if (result.success) {
        if (Array.isArray(result.data)) {
          result.data.forEach((item: any) => {
            item.ketLuanKiemTra = '';
            item.hanhViViPham = '';
            this.model.listToChucVP.push(item); // Lặp qua array
          });
        } else {
          this.model.listToChucVP.forEach((item: any) => {
            if (item.idToChucVP === id) {
              item = result.data;
              return;
            }
          });
        }
        sortList(this.model.listToChucVP, 'ten', 'asc');
        updateIndex(this.model.listToChucVP);
        this.totalToChucVP = this.model.listToChucVP.length;
        // Làm mới lưới
        this.grToChucVP.refresh();
      }
    });
  }

  /**Xác nhận và xóa tạm người vi phạm */
  showConfirmDelete_ToChucVP(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá tổ chức vi phạm này không?')
      .then((data) => {
        this.model.listToChucVP = this.model.listToChucVP.filter(
          (item: any) => {
            return item.idToChucVP != id; // Thay đổi điều kiện theo nhu cầu
          }
        );
        updateIndex(this.model.listToChucVP);
        this.totalToChucVP = this.model.listToChucVP.length;
        // Làm mới lưới
        this.grToChucVP.refresh();
      });
  }

  contextMenuClick_ToChucVP(args: any): void {
    const id = args.rowInfo.rowData.idToChucVP;
    if (args.item.id === 'edit') {
      this.openModalToChucVP(id, args.rowInfo.rowData);
    } else if (args.item.id === 'detail') {
      this.openModalToChucVP(id, args.rowInfo.rowData);
    } else if (args.item.id === 'delete') {
      this.showConfirmDelete_ToChucVP(id);
    }
  }

  dataToChucVP_Change(event: any, updatedRecord: any, columnName: any) {
    // Cập nhật giá trị mới vào dữ liệu gốc
    const index = this.model.listToChucVP.findIndex(
      (item: any) => item.idToChucVP === updatedRecord.idToChucVP
    );
    if (index !== -1) {
      if (columnName === 'ketLuanKiemTra')
        this.model.listToChucVP[index].ketLuanKiemTra = event.value;
      else if (columnName === 'hanhViViPham')
        this.model.listToChucVP[index].hanhViViPham = event.target.value;
    }
  }
  //#endregion

  //#region ----------------Người phiên dịch----------------
  @ViewChild('grPhienDich')
  public grPhienDich: GridComponent;
  public totalPhienDich: any = 0;

  public contextMenuItems_PhienDich?: ContextMenuItemModel[];

  openModalPhienDich(id: any = '', model: any = null): void {
    let activeModal = this.modalService.open(NguoiPhienDichModifyComponent, {
      container: 'body',
      windowClass: 'nguoi-phien-dich-create',
      backdrop: 'static',
    });
    if (id) {
      activeModal.componentInstance.id = id;
      activeModal.componentInstance.model = model;
    }
    activeModal.result.then((result) => {
      if (result.success) {
        if (Array.isArray(result.data)) {
          result.data.forEach((item: any) => {
            this.model.listPhienDich.push(item); // Lặp qua array
          });
        } else {
          this.model.listPhienDich.forEach((item: any) => {
            if (item.idPhienDichVienVPHC === id) {
              item = result.data;
              return;
            }
          });
        }
        sortList(this.model.listPhienDich, 'hoVaTen', 'asc');
        updateIndex(this.model.listPhienDich);
        this.totalPhienDich = this.model.listPhienDich.length;
        // Làm mới lưới
        this.grPhienDich.refresh();
      }
    });
  }

  /**Xác nhận và xóa */
  showConfirmDelete_PhienDich(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá người phiên dich này không?')
      .then((data) => {
        this.model.listPhienDich = this.model.listPhienDich.filter(
          (item: any) => {
            return item.idPhienDichVienVPHC != id; // Thay đổi điều kiện theo nhu cầu
          }
        );
        updateIndex(this.model.listPhienDich);
        this.totalPhienDich = this.model.listPhienDich.length;
        // Làm mới lưới
        this.grPhienDich.refresh();
      });
  }

  contextMenuClick_PhienDich(args: any): void {
    const id = args.rowInfo.rowData.idPhienDichVienVPHC;
    if (args.item.id === 'edit') {
      this.openModalPhienDich(id, args.rowInfo.rowData);
    } else if (args.item.id === 'detail') {
      this.openModalPhienDich(id, args.rowInfo.rowData);
    } else if (args.item.id === 'delete') {
      this.showConfirmDelete_PhienDich(id);
    }
  }
  //#endregion

  //#region ----------------Người chứng kiến----------------
  @ViewChild('grChungKien')
  public grChungKien: GridComponent;
  public totalChungKien: any = 0;

  public contextMenuItems_ChungKien?: ContextMenuItemModel[];

  openModalChungKien(id: any = '', model: any = null): void {
    let activeModal = this.modalService.open(NguoiChungKienModifyComponent, {
      container: 'body',
      windowClass: 'nguoi-chung-kien-create',
      backdrop: 'static',
    });
    if (id) {
      activeModal.componentInstance.id = id;
      activeModal.componentInstance.model = model;
    }
    activeModal.result.then((result) => {
      if (result.success) {
        if (Array.isArray(result.data)) {
          result.data.forEach((item: any) => {
            this.model.listChungKien.push(item); // Lặp qua array
          });
        } else {
          this.model.listChungKien.forEach((item: any) => {
            if (item.idNguoiChungKien === id) {
              item = result.data;
              return;
            }
          });
        }
        sortList(this.model.listChungKien, 'hoVaTen', 'asc');
        updateIndex(this.model.listChungKien);
        this.totalChungKien = this.model.listChungKien.length;
        // Làm mới lưới
        this.grChungKien.refresh();
      }
    });
  }

  /**Xác nhận và xóa */
  showConfirmDelete_ChungKien(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá người chứng kiến này không?')
      .then((data) => {
        this.model.listChungKien = this.model.listChungKien.filter(
          (item: any) => {
            return item.idNguoiChungKien != id; // Thay đổi điều kiện theo nhu cầu
          }
        );
        updateIndex(this.model.listChungKien);
        this.totalChungKien = this.model.listChungKien.length;
        // Làm mới lưới
        this.grChungKien.refresh();
      });
  }

  contextMenuClick_ChungKien(args: any): void {
    const id = args.rowInfo.rowData.idNguoiChungKien;
    if (args.item.id === 'edit') {
      this.openModalChungKien(id, args.rowInfo.rowData);
    } else if (args.item.id === 'detail') {
      this.openModalChungKien(id, args.rowInfo.rowData);
    } else if (args.item.id === 'delete') {
      this.showConfirmDelete_ChungKien(id);
    }
  }
  //#endregion

  //#region ----------------Tang vật----------------
  @ViewChild('grTangVat')
  public grTangVat: GridComponent;
  public totalTangVat: any = 0;

  public contextMenuItems_TangVat?: ContextMenuItemModel[];

  openModalTangVat(id: any = '', model: any = null): void {
    let activeModal = this.modalService.open(TangVatModifyComponent, {
      container: 'body',
      windowClass: 'tang-vat-create',
      backdrop: 'static',
    });
    if (id) {
      activeModal.componentInstance.id = id;
      activeModal.componentInstance.model = model;
    }
    activeModal.result.then((result) => {
      if (result.success) {
        if (Array.isArray(result.data)) {
          result.data.forEach((item: any) => {
            this.model.listTangVat.push(item); // Lặp qua array
          });
        } else {
          this.model.listTangVat.forEach((item: any) => {
            if (item.idTangVatVPHC === id) {
              item = result.data;
              return;
            }
          });
        }
        sortList(this.model.listTangVat, 'tenLoaiTangVat', 'asc');
        updateIndex(this.model.listTangVat);
        this.totalTangVat = this.model.listTangVat.length;
        // Làm mới lưới
        this.grTangVat.refresh();
      }
    });
  }

  /**Xác nhận và xóa */
  showConfirmDelete_TangVat(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá tang vật này không?')
      .then((data) => {
        this.model.listTangVat = this.model.listTangVat.filter((item: any) => {
          return item.idTangVatVPHC != id; // Thay đổi điều kiện theo nhu cầu
        });
        updateIndex(this.model.listTangVat);
        this.totalTangVat = this.model.listTangVat.length;
        // Làm mới lưới
        this.grTangVat.refresh();
      });
  }

  contextMenuClick_TangVat(args: any): void {
    const id = args.rowInfo.rowData.idTangVatVPHC;
    if (args.item.id === 'edit') {
      this.openModalTangVat(id, args.rowInfo.rowData);
    } else if (args.item.id === 'detail') {
      this.openModalTangVat(id, args.rowInfo.rowData);
    } else if (args.item.id === 'delete') {
      this.showConfirmDelete_TangVat(id);
    }
  }
  //#endregion

  //#region ----------------Phương tiện----------------
  @ViewChild('grPhuongTien')
  public grPhuongTien: GridComponent;
  public totalPhuongTien: any = 0;

  public contextMenuItems_PhuongTien?: ContextMenuItemModel[];

  openModalPhuongTien(id: any = '', model: any = null): void {
    let activeModal = this.modalService.open(PhuongTienModifyComponent, {
      container: 'body',
      windowClass: 'phuong-tien-create',
      backdrop: 'static',
    });
    if (id) {
      activeModal.componentInstance.id = id;
      activeModal.componentInstance.model = model;
    }
    activeModal.result.then((result) => {
      if (result.success) {
        if (Array.isArray(result.data)) {
          result.data.forEach((item: any) => {
            this.model.listPhuongTien.push(item); // Lặp qua array
          });
        } else {
          this.model.listPhuongTien.forEach((item: any) => {
            if (item.idPhuongTienVPHC === id) {
              item = result.data;
              return;
            }
          });
        }
        sortList(this.model.listPhuongTien, 'tenLoaiPhuongTien', 'asc');
        updateIndex(this.model.listPhuongTien);
        this.totalPhuongTien = this.model.listPhuongTien.length;
        // Làm mới lưới
        this.grPhuongTien.refresh();
      }
    });
  }

  /**Xác nhận và xóa */
  showConfirmDelete_PhuongTien(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá phương tiện này không?')
      .then((data) => {
        this.model.listPhuongTien = this.model.listPhuongTien.filter(
          (item: any) => {
            return item.idPhuongTienVPHC != id; // Thay đổi điều kiện theo nhu cầu
          }
        );
        updateIndex(this.model.listPhuongTien);

        this.totalPhuongTien = this.model.listPhuongTien.length;
        // Làm mới lưới
        this.grPhuongTien.refresh();
      });
  }

  contextMenuClick_PhuongTien(args: any): void {
    const id = args.rowInfo.rowData.idPhuongTienVPHC;
    if (args.item.id === 'edit') {
      this.openModalPhuongTien(id, args.rowInfo.rowData);
    } else if (args.item.id === 'detail') {
      this.openModalPhuongTien(id, args.rowInfo.rowData);
    } else if (args.item.id === 'delete') {
      this.showConfirmDelete_PhuongTien(id);
    }
  }
  //#endregion

  //#region ----------------Giấy phép, chứng chỉ hành nghề----------------
  @ViewChild('grGiayPhepChungChi')
  public grGiayPhepChungChi: GridComponent;
  public totalGiayPhepChungChi: any = 0;

  public contextMenuItems_GiayPhepChungChi?: ContextMenuItemModel[];

  openModalGiayPhepChungChi(id: any = '', model: any = null): void {
    let activeModal = this.modalService.open(GiayPhepChungChiModifyComponent, {
      container: 'body',
      windowClass: 'giay-phep-chung-chi-create',
      backdrop: 'static',
    });
    if (id) {
      activeModal.componentInstance.id = id;
      activeModal.componentInstance.model = model;
    }
    activeModal.result.then((result) => {
      if (result.success) {
        if (Array.isArray(result.data)) {
          result.data.forEach((item: any) => {
            this.model.listGiayPhepChungChi.push(item); // Lặp qua array
          });
        } else {
          this.model.listGiayPhepChungChi.forEach((item: any) => {
            if (item.idGiayPhepChungChi === id) {
              item = result.data;
              return;
            }
          });
        }
        sortList(this.model.listGiayPhepChungChi, 'ten', 'asc');
        updateIndex(this.model.listGiayPhepChungChi);
        this.totalGiayPhepChungChi = this.model.listGiayPhepChungChi.length;
        // Làm mới lưới
        this.grGiayPhepChungChi.refresh();
      }
    });
  }

  /**Xác nhận và xóa */
  showConfirmDelete_GiayPhepChungChi(id: string) {
    this.messageService
      .showConfirm(
        'Bạn có chắc muốn xoá giấy phép, chứng chỉ hành nghề này không?'
      )
      .then((data) => {
        this.model.listGiayPhepChungChi =
          this.model.listGiayPhepChungChi.filter((item: any) => {
            return item.idChungChiGiayPhep != id; // Thay đổi điều kiện theo nhu cầu
          });
          updateIndex(this.model.listGiayPhepChungChi);
        this.totalGiayPhepChungChi = this.model.listGiayPhepChungChi.length;
        // Làm mới lưới
        this.grGiayPhepChungChi.refresh();
      });
  }

  contextMenuClick_GiayPhepChungChi(args: any): void {
    const id = args.rowInfo.rowData.idChungChiGiayPhep;
    if (args.item.id === 'edit') {
      this.openModalGiayPhepChungChi(id, args.rowInfo.rowData);
    } else if (args.item.id === 'detail') {
      this.openModalGiayPhepChungChi(id, args.rowInfo.rowData);
    } else if (args.item.id === 'delete') {
      this.showConfirmDelete_GiayPhepChungChi(id);
    }
  }
  //#endregion

  //#region ------------Xử lý phân công cán bộ-------------------
  @ViewChild('grCanBoPhanCong')
  public grCanBoPhanCong: GridComponent;

  public totalCanBoPhanCong: any = 0;

  openModalPhanCongCanBo() {
    let activeModal = this.modalService.open(PhanCongCanBoComponent, {
      container: 'body',
      windowClass: 'phan-cong-can-bo-choose',
      backdrop: 'static',
    });

    activeModal.componentInstance.id = this.model.idDonVi;
    activeModal.componentInstance.listIdCanBoSelected =
      this.model.listCanBoPhanCong.map((item: any) => item.id);
    activeModal.result.then((result) => {
      if (result.success) {
        this.model.listCanBoPhanCong = result.data;

        updateIndex(this.model.listCanBoPhanCong);

        this.totalCanBoPhanCong = this.model.listCanBoPhanCong.length;
        // Làm mới lưới
        this.grCanBoPhanCong.refresh();
      }
    });
  }
  //#endregion
}
