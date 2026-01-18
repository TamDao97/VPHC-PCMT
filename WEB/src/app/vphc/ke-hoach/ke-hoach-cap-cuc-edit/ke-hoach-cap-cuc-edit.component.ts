import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { Constants } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { FileService } from 'src/app/cores/shared/services/file.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { UserType, AuthService } from 'src/app/modules/auth';
import { KeHoachService } from '../../service/ke-hoach.service';

@Component({
  selector: 'app-ke-hoach-cap-cuc-edit',
  templateUrl: './ke-hoach-cap-cuc-edit.component.html',
  styleUrl: './ke-hoach-cap-cuc-edit.component.scss'
})
export class KeHoachCapCucEditComponent implements OnInit, AfterViewInit, OnDestroy {

  files: any[] = [];   // 👈 BẮT BUỘC PHẢI CÓ

  loadFiles() {
    this.files = [
      {
        fileName: 'hop-dong.pdf',
        url: '/uploads/hop-dong.pdf'
      }
    ];
  }

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
    // ngayBanHanhKeHoach: null,
    // namThucHienKeHoach: null,
    // tuNgayThucHienKeHoach: new Date(),
    // denNgayThucHienKeHoach: new Date(),
    // diaBanKiemTraTheoKeHoach: null,
    // thanhPhanLucLuongKiemTra: null,
    // phanCongNhiemVu: null,
    // dieuKienPhucVuKiemTra: null,
    // cheDoBaoCao: null,
  };

  public fields = { text: 'name', value: 'id' };
  public lstYear: any;
  public listDonVi: any;

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
    private auth: AuthService
  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  //#region -------------Xự kiện của trang
  ngOnInit(): void {
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
      this.getById();
    } else {
      this.user$.subscribe(async (user) => {
        if (user != null) {
          this.model.idDonVi = user.idDonVi;
        }
      });
    }
    this.loadFiles();
  }

  ngAfterViewInit() {
  }

  ngOnDestroy() { }
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

  //#region ------------Lấy thông tin cập nhật---------
  getById() {
    this.keHoachService.getById(this.id).subscribe({
      next: async (result) => {
        if (result.isStatus) {
          this.model = result.data;
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
  //#endregion

  //#endregion ------------Xử lý lưu CSDL
  // saveAndContinue() {
  //   this.save(true);
  // }

  save(isContinue: boolean = false) {
    if (this.id) {
      this.update();
    } else {
      this.create(isContinue);
    }
  }

  create(isContinue: any) {
    this.keHoachService.create(this.model).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.messageService.showSuccess('Thêm mới kế hoạch thành công!');
          if (isContinue) {
            this.id = result.data;
            this.getById();
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

  update() {
    this.keHoachService.update(this.id, this.model).subscribe({
      next: (result) => {
        if (result.isStatus) {
          this.messageService.showSuccess('Cập nhập vụ việc thành công!');
          this.router.navigate(['/ke-hoach']);
        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }

  close() {
    this.router.navigate(['/ke-hoach']);
  }
  //#endregion
}
