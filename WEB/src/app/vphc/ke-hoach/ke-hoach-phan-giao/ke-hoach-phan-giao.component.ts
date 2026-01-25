import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { TemplateService } from 'src/app/cores/pages/template/service/template.service';
import { Constants, TrangThaiKHKTEnum } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { FileService } from 'src/app/cores/shared/services/file.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { UserType, AuthService } from 'src/app/modules/auth';
import { KeHoachService } from '../../service/ke-hoach.service';


export interface IDonViPhanGiao {
  idDonVi: string | null,
  ngayNhanPhanGiao: Date | null,
  soVu: number | null,
  soDoiTuong: number | null,
  tongTienXuPhat: number | null,
  ngayKetThuc: number | null,
}

@Component({
  selector: 'app-ke-hoach-phan-giao',
  templateUrl: './ke-hoach-phan-giao.component.html',
  styleUrl: './ke-hoach-phan-giao.component.scss'
})
export class KeHoachPhanGiaoComponent implements OnInit {
  //#region define variable
  id: any = '';

  model: any = {
    id: null,
    idDonVi: null,
    soQuyetDinhBanHanh: null,
    canCu: null,
    mucDich: null,
    yeuCau: null,
    noiDungKiemTra: null,
    tuNgayThucHienKeHoach: null,
    denNgayThucHienKeHoach: null,
    trangThaiKeHoachKiemTra: 1
  };

  public fields = { text: 'name', value: 'id' };
  public lstYear: any;
  public listDonVi: any;
  trangThaiKeHoach: number = 1;
  user$: Observable<UserType>;
  //#endregion

  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private keHoachService: KeHoachService,
    private router: Router,
    private fileService: FileService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private modalService: NgbModal,
    private comboboxService: ComboboxCoreService,
    private auth: AuthService,
    private changeDetectorRef: ChangeDetectorRef,
    private templateService: TemplateService,


  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  //#region -------------Xự kiện của trang
  ngOnInit(): void {
    this.fileProcess.fileModel = {};
    this.fileProcess.FileDataBase = null;
    this.user$ = this.auth.currentUserSubject.asObservable();
    this.id = this.routeA.snapshot.paramMap.get('id') ?? '';
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.getDataCombobox();

    if (this.id) {
      // this.getById();
    } else {
      this.user$.subscribe(async (user) => {
        if (user != null) {
          this.model.idDonVi = user.idDonVi;
        }
      });
    }
  }

  //#endregion

  //#region -------------Xử lý combobox-----------
  getDataCombobox() {
    const currentYear = new Date().getFullYear();
    this.lstYear = Array.from({ length: 5 }, (_, i) => {
      const year = (currentYear - i).toString();
      return {
        id: year,
        name: year
      };
    });

    this.comboboxService.getDonViByDonVi().subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.listDonVi = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion


  //#endregion ------------Xử lý lưu CSDL
  create(isContinue: any) {
    let body = {
      ...this.model,
      // dataFileChoDuyet: this.uploadedFiles,
    }
    this.keHoachService.create(body).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.messageService.showSuccess('Thêm mới kế hoạch thành công!');
          if (isContinue) {
            this.id = result.data;
            // this.getById();
          } else {
            this.router.navigate(['/ke-hoach']);
          }
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }



  //////////////////////////////////////////////////////////////
  //////////////////////////////////////////////////////////////


  lstDonViPhanGiao: IDonViPhanGiao[] = [
    {
      idDonVi: null,
      ngayNhanPhanGiao: null,
      soDoiTuong: null,
      soVu: null,
      ngayKetThuc: null,
      tongTienXuPhat: null
    }
  ];

  onAddRow() {
    this.lstDonViPhanGiao.push(<IDonViPhanGiao>{
      idDonVi: null,
      ngayNhanPhanGiao: null,
      soDoiTuong: null,
      soVu: null,
      ngayKetThuc: null,
      tongTienXuPhat: null
    })
  }

  save() { }

  close() {
    this.router.navigate(['/ke-hoach']);
  }
}
