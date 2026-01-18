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
import { environment } from 'src/environments/environment';
import { TabComponent } from '@syncfusion/ej2-angular-navigations';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { NguoiViPhamModifyComponent } from '../../nguoi-vi-pham/nguoi-vi-pham-modify.component';
import { MatDialog } from '@angular/material/dialog';
import { ToChucModifyComponent } from '../../to-chuc/to-chuc-modify.component';
import {
  ContextMenuItemModel,
  GridComponent,
} from '@syncfusion/ej2-angular-grids';
import { NguoiPhienDichModifyComponent } from '../../nguoi-phien-dich/nguoi-phien-dich-modify.component';
import { NguoiChungKienModifyComponent } from '../../nguoi-chung-kien/nguoi-chung-kien-modify.component';
import { TangVatModifyComponent } from '../../tang-vat/tang-vat-modify.component';
import { PhuongTienModifyComponent } from '../../phuong-tien/phuong-tien-modify.component';
import { GiayPhepChungChiModifyComponent } from '../../chung-chi-giay-phep/giay-phep-chung-chi-modify.component';
import { PhanCongCanBoComponent } from '../../phan-cong-can-bo/phan-cong-can-bo-choose.component';
import { QuyetDinhService } from 'src/app/vphc/service/quyet-dinh-service';
import { VuViecService } from 'src/app/vphc/service/vu-viec.service';
import { AmountToTextPipe } from 'src/app/cores/shared/pipe/amount-to-text';
import { firstValueFrom } from 'rxjs';
// import { VuViecXacMinhService } from '../../service/vu-viec-quyet-dinh.service';

@Component({
  providers: [ContextMenuService],
  selector: 'app-quyet-dinh-modify',
  templateUrl: './quyet-dinh-modify.component.html',
  styleUrls: ['./quyet-dinh-modify.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class QuyetDinhModifyComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private QuyetDinhService: QuyetDinhService,
    private router: Router,
    private fileService: FileService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private modalService: NgbModal,
    private comboboxService: ComboboxCoreService,
    private activeModal: NgbActiveModal,
    private vuViecService: VuViecService,
    private amountToTextPipe: AmountToTextPipe
  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  //Id quyết định
  public id: any;
  public idVuViec: any;
  public idDanhMuc: any;
  model: any = {
    idDanhMucQuyetDinh: '',
    idVuViec: '',
    so: '',
    ngayRaQD: new Date(),
    canCu: '',
    doiTuongViPham: 0,
    idNguoiViPham: '',
    tenNguoiVP: '',
    tenGioiTinhNguoiVP: '',
    ngaySinhNguoiVP: '',
    quocTichNguoiVP: '',
    ngheNghiepNguoiVP: '',
    noiONguoiVP: '',
    soTheCCNguoiVP: '',
    ngayCapTheCCNguoiVP: '',
    noiCapTheCC: '',
    idToChucViPham: '',
    tenToChucVP: '',
    diaTrucSoToChucVP: '',
    maSoToChucVP: '',
    giayPhepToChucVP: '',
    ngayCapGPToChucVP: '',
    noiCapGPToChucVP: '',
    tenNguoiDDToChucVP: '',
    gioiTinhNguoiDDToChucVP: '',
    chucDanhNguoiDDToChucVP: '',
    hanhViViPham: '',
    quyDinhTai: '',
    tinhTietTangNang: '',
    tinhTietGiamNhe: '',
    phatChinh: '',
    cuThePC: '',
    mucPhat: 0,
    mucPhatText: '(Không đồng)',
    phatBoSung: '',
    cuThePBS: '',
    soNgayThucHienPBS: null,
    khacPhucHauQua: '',
    cuTheKPHQ: '',
    soNgayThucHienKPHQ: null,
    noiDungLienQuanKPHQ: '',
    chiPhiKPHQ: null,
    chiPhiKPHQText: '',
    coQuanThucHienKPHC: '',
    ngayQDCoHieuLuc: new Date(),
    hanNopPhat: null,
    diaDiemNopPhat: '',
    donViThuTienPhat: '',
    donViThucHien: '',
    donViPhoiHop: '',
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
    this.model.idDanhMucQuyetDinh = this.idDanhMuc;

    this.getVuViecChoQuyetDinh();

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
  public listPhatChinh: any;
  public listPhatBoSung: any;

  getDataCombobox() {
    this.comboboxService.getHinhThucPhatChinh().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listPhatChinh = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });

    this.comboboxService.getHinhThucPhatBoSung().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listPhatBoSung = result.data;
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
    this.QuyetDinhService.getById(this.id).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.model = result.data;
          this.model.doiTuongViPham = this.model.doiTuongViPham;
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
    if (!this.model.idNguoiViPham && !this.model.idToChucViPham) {
      this.messageService.showMessage(
        'Chưa chọn cá nhân hoặc tổ chức vi phạm để ra quyết định xử phạt.'
      );
      return;
    }

    this.model.mucPhatText = this.amountToTextPipe.transform(
      this.model.mucPhat
    );
    this.model.chiPhiKPHQText = this.amountToTextPipe.transform(
      this.model.chiPhiKPHQ
    );
    if (this.id) {
      this.update();
    } else {
      this.create();
    }
  }

  create() {
    this.QuyetDinhService.create(this.model).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.messageService.showSuccess('Thêm mới quyết định thành công!');
          this.close(true);
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }

  update() {
    this.QuyetDinhService.update(this.id, this.model).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.messageService.showSuccess('Cập nhập quyết định thành công!');
          this.close(true);
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion

  //#region -------------Xử lý thông tin vụ việc (người vi phạm, phiên dịch, làm chứng...) hiên thị lên quyết định----------------
  public listNguoiVP: any[] = [];
  public listToChucVP: any[] = [];
  public vuViecModel: any;
  public tongNguoiVP: any = 0;
  public tongToChucVP: any = 0;

  async getVuViecChoQuyetDinh() {
    const result = await firstValueFrom(
      this.vuViecService.getVuViecTrongQuyetDinh(this.idVuViec)
    );
    if (result.isStatus) {
      this.vuViecModel = result.data;
      this.listNguoiVP = this.vuViecModel.listNguoiVP;
      this.listToChucVP = this.vuViecModel.listToChucVP;
      this.tongNguoiVP = this.listNguoiVP.length;
      this.tongToChucVP = this.vuViecModel.listToChucVP.length;
      if (!this.id) {
        if (this.tongNguoiVP == 1) {
          this.model.idNguoiViPham = this.listNguoiVP[0].idNguoiVPHC;
          this.model.tenNguoiVP = this.listNguoiVP[0].hoVaTen;
          this.model.tenGioiTinhNguoiVP = this.listNguoiVP[0].tenGioiTinh;
          this.model.ngaySinhNguoiVP = this.listNguoiVP[0].ngaySinh;
          this.model.quocTichNguoiVP = this.listNguoiVP[0].tenQuocTich;
          this.model.ngheNghiepNguoiVP = this.listNguoiVP[0].tenNgheNghiep;
          this.model.noiONguoiVP = this.listNguoiVP[0].diaChiHienNayDayDu;
          this.model.soTheCCNguoiVP = this.listNguoiVP[0].cmnd;
          this.model.ngayCapTheCCNguoiVP = this.listNguoiVP[0].ngayCap;
          this.model.noiCapTheCC = this.listNguoiVP[0].noiCap;
          this.model.hanhViViPham = this.listNguoiVP[0].hanhViViPham;
          this.model.quyDinhTai = this.listNguoiVP[0].quyDinhTai;
          this.model.doiTuongViPham = 0;
        } else if (this.tongToChucVP == 1) {
          this.model.idToChucViPham = this.listToChucVP[0].idToChucVP;
          this.model.tenToChucVP = this.listToChucVP[0].ten;
          this.model.diaTrucSoToChucVP = this.listToChucVP[0].diaChiTruSo;
          this.model.maSoToChucVP = this.listToChucVP[0].maSoDoanhNghiep;
          this.model.giayPhepToChucVP = this.listToChucVP[0].soDKKD;
          this.model.ngayCapGPToChucVP = this.listToChucVP[0].ngayCapDKKD;
          this.model.noiCapGPToChucVP = this.listToChucVP[0].noiCapDKKD;
          this.model.tenNguoiDDToChucVP = this.listToChucVP[0].hoTenPhapNhan;
          this.model.gioiTinhNguoiDDToChucVP = this.listToChucVP[0].tenGioiTinh;
          this.model.chucDanhNguoiDDToChucVP = this.listToChucVP[0].chucVu;
          this.model.hanhViViPham = this.listToChucVP[0].hanhViViPham;
          this.model.quyDinhTai = this.listToChucVP[0].quyDinhTai;
          this.model.doiTuongViPham = 1;
        } else if (this.tongNguoiVP > 0) {
          this.model.doiTuongViPham = 0;
        } else if (this.tongToChucVP > 0) {
          this.model.doiTuongViPham = 1;
        }
      }
    }
    // this.vuViecService.getVuViecTrongQuyetDinh(this.idVuViec).subscribe({
    //   next: (result) => {
    //     if (result.isStatus) {
    //       this.vuViecModel = result.data;
    //       this.listNguoiVP = this.vuViecModel.listNguoiVP;
    //       this.listToChucVP = this.vuViecModel.listToChucVP;
    //       this.tongNguoiVP = this.listNguoiVP.length;
    //       this.tongToChucVP = this.vuViecModel.listToChucVP.length;
    //       if (!this.id) {
    //         if (this.tongNguoiVP == 1) {
    //           this.model.idNguoiViPham = this.listNguoiVP[0].idNguoiVPHC;
    //           this.model.tenNguoiVP = this.listNguoiVP[0].hoVaTen;
    //           this.model.tenGioiTinhNguoiVP = this.listNguoiVP[0].tenGioiTinh;
    //           this.model.ngaySinhNguoiVP = this.listNguoiVP[0].ngaySinh;
    //           this.model.quocTichNguoiVP = this.listNguoiVP[0].tenQuocTich;
    //           this.model.ngheNghiepNguoiVP = this.listNguoiVP[0].tenNgheNghiep;
    //           this.model.noiONguoiVP = this.listNguoiVP[0].diaChiHienNayDayDu;
    //           this.model.soTheCCNguoiVP = this.listNguoiVP[0].cmnd;
    //           this.model.ngayCapTheCCNguoiVP = this.listNguoiVP[0].ngayCap;
    //           this.model.noiCapTheCC = this.listNguoiVP[0].noiCap;
    //           this.model.doiTuongViPham = '0';
    //         } else if (this.tongToChucVP == 1) {
    //           this.model.idToChucViPham = this.listToChucVP[0].idToChucVP;
    //           this.model.tenToChucVP = this.listToChucVP[0].ten;
    //           this.model.diaTrucSoToChucVP = this.listToChucVP[0].diaChiTruSo;
    //           this.model.maSoToChucVP = this.listToChucVP[0].maSoDoanhNghiep;
    //           this.model.giayPhepToChucVP = this.listToChucVP[0].soDKKD;
    //           this.model.ngayCapGPToChucVP = this.listToChucVP[0].ngayCapDKKD;
    //           this.model.noiCapGPToChucVP = this.listToChucVP[0].noiCapDKKD;
    //           this.model.tenNguoiDDToChucVP =
    //             this.listToChucVP[0].hoTenPhapNhan;
    //           this.model.gioiTinhNguoiDDToChucVP =
    //             this.listToChucVP[0].tenGioiTinh;
    //           this.model.chucDanhNguoiDDToChucVP = this.listToChucVP[0].chucVu;
    //           this.model.doiTuongViPham = '1';
    //         }
    //       }
    //     }
    //   },
    //   error: (error) => {
    //     this.messageService.showError(error);
    //   },
    // });
  }

  onSelect(event: any, type: any): void {
    if (type == 'NguoiVP') {
      this.model.tenNguoiVP = event.itemData.hoVaTen;
      this.model.tenGioiTinhNguoiVP = event.itemData.tenGioiTinh;
      this.model.ngaySinhNguoiVP = event.itemData.ngaySinh;
      this.model.quocTichNguoiVP = event.itemData.tenQuocTich;
      this.model.ngheNghiepNguoiVP = event.itemData.tenNgheNghiep;
      this.model.noiONguoiVP = event.itemData.diaChiHienNayDayDu;
      this.model.soTheCCNguoiVP = event.itemData.cmnd;
      this.model.ngayTheCCNguoiVP = event.itemData.ngayCap;
      this.model.noiCapTheCC = event.itemData.noiCap;
      this.model.hanhViViPham = event.itemData.hanhViViPham;
      this.model.quyDinhTai = event.itemData.quyDinhTai;
    } else if (type == 'ToChucVP') {
      this.model.tenToChucVP = event.itemData.ten;
      this.model.diaTrucSoToChucVP = event.itemData.diaChiTruSo;
      this.model.maSoToChucVP = event.itemData.maSoDoanhNghiep;
      this.model.giayPhepToChucVP = event.itemData.soDKKD;
      this.model.ngayCapGPToChucVP = event.itemData.ngayCapDKKD;
      this.model.noiCapGPToChucVP = event.itemData.noiCapDKKD;
      this.model.tenNguoiDDToChucVP = event.itemData.hoTenPhapNhan;
      this.model.gioiTinhNguoiDDToChucVP = event.itemData.tenGioiTinh;
      this.model.chucDanhNguoiDDToChucVP = event.itemData.chucVu;
      this.model.hanhViViPham = event.itemData.hanhViViPham;
      this.model.quyDinhTai = event.itemData.quyDinhTai;
    } else if (type == 'PhatChinh') {
    } else if (type == 'PhatBoSung') {
    }
  }
  //#endregion
}
