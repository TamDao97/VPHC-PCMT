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
import { DanhMucBienBanComponent } from '../danh-muc/danh-muc-choose.component';
import { BienBanModifyComponent } from '../bien-ban-modify/bien-ban-modify.component';
import { BienBanService } from 'src/app/vphc/service/bien-ban-service';
import { NtsViewFileComponent } from 'src/app/cores/shared/component/nts-view-file/nts-view-file.component';
// import { VuViecXacMinhService } from '../../service/vu-viec-bien-ban.service';

@Component({
  providers: [ContextMenuService],
  selector: 'app-bien-ban-list',
  templateUrl: './bien-ban-list.component.html',
  styleUrls: ['./bien-ban-list.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class BienBanListComponent implements OnInit, AfterViewInit, OnDestroy {
  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private bienBanService: BienBanService,
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
      text: 'Tải biên bản',
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
  public listBienBan: any[] = [];

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
      this.getBienBanVuViec();
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
  getBienBanVuViec() {
    this.bienBanService.getBienBanVuViec(this.id).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listBienBan = result.data;
          this.totalBienBan = this.listBienBan.length;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion

  //#region ----------------Người vi phạm----------------
  @ViewChild('grBienBan')
  public grBienBan: GridComponent;
  public totalBienBan: any = 0;

  public contextMenuItems_BienBan?: ContextMenuItemModel[];

  openModalDanhMuc(id: any = '', model: any = null): void {
    let activeModal = this.modalService.open(DanhMucBienBanComponent, {
      container: 'body',
      windowClass: 'danh-muc-bb-choose',
      backdrop: 'static',
    });
    activeModal.result.then((result) => {
      if (result.success) {
        this.openModalBienBan('', result.data.id);
      }
    });
  }

  openModalBienBan(id: any, idDanhMuc: any = ''): void {
    let activeModal = this.modalService.open(BienBanModifyComponent, {
      container: 'body',
      windowClass: 'bien-ban-modify',
      backdrop: 'static',
    });
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.idVuViec = this.id;
    activeModal.componentInstance.idDanhMuc = idDanhMuc;
    activeModal.result.then((result) => {
      if (result) {
        // Làm mới lưới
        this.getBienBanVuViec();
        if (!id) {
          this.changeTotal.emit(1);
        }
      }
    });
  }

  /**Xác nhận và xóa tạm người vi phạm */
  showConfirmDelete(id: string) {
    this.messageService
      .showConfirm('Bạn có chắc muốn xoá biên bản này không?')
      .then((data) => {
        this.bienBanService.delete(id).subscribe({
          next: (data) => {
            if (data.isStatus) {
              this.messageService.showSuccess('Xóa biên bản thành công!');
              this.getBienBanVuViec();
              this.changeTotal.emit(-1);
            }
          },
          error: (error) => {
            this.messageService.showError(error);
          },
        });
        // Làm mới lưới
        this.getBienBanVuViec();
      });
  }

  contextMenuClick_BienBan(args: any): void {
    const id = args.rowInfo.rowData.idBienBan;
    if (args.item.id === 'edit') {
      this.openModalBienBan(id);
    } else if (args.item.id === 'detail') {
      this.viewBienVan(id);
    } else if (args.item.id === 'download') {
      this.downloadBienVan(id);
    } else if (args.item.id === 'delete') {
      this.showConfirmDelete(id);
    }
  }

  viewBienVan(id: string) {
    this.bienBanService.exportPdf(id).subscribe({
      next: (data) => {
        let activeModal = this.modalService.open(NtsViewFileComponent, {
          container: 'body',
          windowClass: 'viewpdf-file-modal',
          backdrop: 'static',
        });
        activeModal.componentInstance.fileContentResult = data;
        activeModal.componentInstance.nameFile = 'BienBan_VPHC.pdf';
        activeModal.result.then((result: any) => {
          if (result) {
          }
        });
        // var blob = new Blob([data], { type: 'octet/stream' });
        // var url = window.URL.createObjectURL(blob);
        // // this.fileProcess.downloadFileLink(url, 'LichSuThaoTac.xlsx');
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

  downloadBienVan(id: string) {
    this.bienBanService.exportWord(id).subscribe({
      next: (data) => {
        var blob = new Blob([data], { type: 'octet/stream' });
        var url = window.URL.createObjectURL(blob);
        this.fileProcess.downloadFileLink(url, 'BienBan_VPHC.doc');
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
