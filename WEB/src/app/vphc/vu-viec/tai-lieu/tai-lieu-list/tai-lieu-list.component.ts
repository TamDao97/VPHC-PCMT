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
  output,
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
import { TaiLieuUploadComponent } from '../tai-lieu-upload/tai-lieu-upload.component';
import { NtsViewFileComponent } from 'src/app/cores/shared/component/nts-view-file/nts-view-file.component';
import { TaiLieuService } from 'src/app/vphc/service/tai-lieu-service';
import { ListView, SelectEventArgs } from '@syncfusion/ej2-angular-lists';
// import { VuViecXacMinhService } from '../../service/vu-viec-quyet-dinh.service';

@Component({
  providers: [ContextMenuService],
  selector: 'app-tai-lieu-list',
  templateUrl: './tai-lieu-list.component.html',
  styleUrls: ['./tai-lieu-list.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class TaiLieuListComponent implements OnInit, AfterViewInit, OnDestroy {
  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private taiLieuService: TaiLieuService,
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
      text: 'Xem chi tiết',
      target: '.e-content',
      id: 'detail',
      iconCss: 'fas fa-file-alt',
    },
    {
      text: 'Tải tài liệu',
      target: '.e-content',
      id: 'download',
      iconCss: 'fas fa-file-download',
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
  public listTaiLieu: any[] = [];

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
      this.getTaiLieuVuViec();
    }

    this.getDanhMucTaiLieu();
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
  getTaiLieuVuViec() {
    this.taiLieuService.getTaiLieuVuViec(this.id, this.idDanhMuc).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listTaiLieu = result.data;
          this.totalTaiLieu = this.listTaiLieu.length;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion

  //#region ----------------Quyết định----------------
  @ViewChild('grTaiLieu')
  public grTaiLieu: GridComponent;
  public totalTaiLieu: any = 0;

  public contextMenuItems_TaiLieu?: ContextMenuItemModel[];

  openModalTaiLieu(id: any = ''): void {
    let activeModal = this.modalService.open(TaiLieuUploadComponent, {
      container: 'body',
      windowClass: 'tai-lieu-upload',
      backdrop: 'static',
    });
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.idVuViec = this.id;
    activeModal.result.then((result) => {
      if (result) {
        // Làm mới lưới
        this.getTaiLieuVuViec();
        this.getDanhMucTaiLieu();
        if (!id) {
          this.changeTotal.emit(1);
        }
      }
    });
  }

  /**Xác nhận và xóa tạm người vi phạm */
  showConfirmDelete(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá tài liệu này không?')
      .then((data) => {
        this.taiLieuService.delete(id).subscribe({
          next: (data) => {
            if (data.isStatus) {
              this.messageService.showSuccess('Xóa tài liệu thành công!');
              this.getTaiLieuVuViec();
              this.getDanhMucTaiLieu();
              this.changeTotal.emit(-1);
            }
          },
          error: (error) => {
            this.messageService.showError(error);
          },
        });
        // Làm mới lưới
        this.getTaiLieuVuViec();
        this.getDanhMucTaiLieu();
      });
  }

  contextMenuClick_TaiLieu(args: any): void {
    const id = args.rowInfo.rowData.idTaiLieu;
    const ten = args.rowInfo.rowData.ten;
    if (args.item.id === 'detail') {
      this.viewTaiLieu(id, ten);
    } else if (args.item.id === 'download') {
      this.downloadTaiLieu(id, ten);
    } else if (args.item.id === 'delete') {
      this.showConfirmDelete(id);
    }
  }

  viewTaiLieu(id: string, ten: string) {
    this.taiLieuService.getFileTaiLieu(id).subscribe({
      next: (data) => {
        let activeModal = this.modalService.open(NtsViewFileComponent, {
          container: 'body',
          windowClass: 'viewpdf-file-modal',
          backdrop: 'static',
        });
        activeModal.componentInstance.fileContentResult = data;
        activeModal.componentInstance.nameFile = ten;
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

  downloadTaiLieu(id: string, ten: string) {
    this.taiLieuService.getFileTaiLieu(id).subscribe({
      next: (data) => {
        var blob = new Blob([data], { type: 'octet/stream' });
        var url = window.URL.createObjectURL(blob);
        this.fileProcess.downloadFileLink(url, ten);
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

  //#region ------------Danh mục----------
  @ViewChild('lvDanhMuc') public lvDanhMuc: ListView;
  listDanhMuc: any;
  getDanhMucTaiLieu() {
    this.taiLieuService.getDanhMucTotal(!this.id ? 'all' : this.id).subscribe({
      next: (data) => {
        this.listDanhMuc = data.data;
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }

  // Xử lý sự kiện click
  idDanhMuc: any = '';
  onItemClick(event: any): void {
    this.idDanhMuc =event.data.id;
    this.getTaiLieuVuViec();
  }
  //#endregion
}
