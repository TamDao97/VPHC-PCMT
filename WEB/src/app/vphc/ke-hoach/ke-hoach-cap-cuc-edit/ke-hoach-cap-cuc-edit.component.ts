import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { Constants, TrangThaiKHKTEnum } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { FileService } from 'src/app/cores/shared/services/file.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { UserType, AuthService } from 'src/app/modules/auth';
import { KeHoachService } from '../../service/ke-hoach.service';
import { TemplateService } from 'src/app/cores/pages/template/service/template.service';

@Component({
  selector: 'app-ke-hoach-cap-cuc-edit',
  templateUrl: './ke-hoach-cap-cuc-edit.component.html',
  styleUrl: './ke-hoach-cap-cuc-edit.component.scss'
})
export class KeHoachCapCucEditComponent implements OnInit, AfterViewInit, OnDestroy {

  files: any[] = [];   // 👈 BẮT BUỘC PHẢI CÓ
  filedata: any = null;

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
          this.trangThaiKeHoach = result.data.trangThaiKeHoachKiemTra ?? 1;
          this.uploadedFiles = result.data.dataFileChoDuyet ?? [];
          this.uploadedFilesDaDuyet = result.data.dataFileDaDuyet ?? [];
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
    let body = {
      ...this.model,
      dataFileChoDuyet: this.uploadedFiles,
    }
    this.keHoachService.create(body).subscribe({
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
    let body = {
      ...this.model,
      dataFileChoDuyet: this.uploadedFiles,
      dataFileDaDuyet: this.uploadedFilesDaDuyet,
    }
    this.keHoachService.update(this.id, body).subscribe({
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
  selectedFiles: File[] = [];
  selectedFilesDaDuyet: File[] = [];
  uploadedFiles: any[] = [];
  uploadedFilesDaDuyet: any[] = [];

  folderName = 'Template/Download';

  // chọn nhiều file
  handleFileInput(event: any) {
    this.selectedFiles = Array.from(event.target.files);
    event.target.value = '';
  }

  // upload nhiều file
  uploadFiles() {
    if (!this.selectedFiles.length) {
      this.messageService.showMessage('Vui lòng chọn file');
      return;
    }

    this.fileService
      .uploadFiles(this.selectedFiles, this.folderName)
      .subscribe(
        (res) => {
          // BACKEND TRẢ THẲNG MẢNG
          this.uploadedFiles = res.data;
          console.log("file", this.uploadedFiles);

        },
        () => {
          this.messageService.showMessage('Upload thất bại');
        }
      );
  }
  // chọn nhiều file
  handleFileInputDaDuyet(event: any) {
    this.selectedFilesDaDuyet = Array.from(event.target.files);
    event.target.value = '';
  }

  // upload nhiều file
  uploadFilesDaDuyet() {
    if (!this.selectedFilesDaDuyet.length) {
      this.messageService.showMessage('Vui lòng chọn file');
      return;
    }

    this.fileService
      .uploadFiles(this.selectedFilesDaDuyet, this.folderName)
      .subscribe(
        (res) => {
          // BACKEND TRẢ THẲNG MẢNG
          this.uploadedFilesDaDuyet = res.data;
          console.log("file", this.uploadedFilesDaDuyet);

        },
        () => {
          this.messageService.showMessage('Upload thất bại');
        }
      );
  }
  // download từng file
  download(file: any) {
    const model = {
      PathFile: file.fileUrl
    };

    this.fileService.downloadFile({ PathFile: file.fileUrl, NameFile: file.fileName }).subscribe(
      data => {
        const blob = new Blob([data], {
          type: this.getContentType(file.extension)
        });

        const url = window.URL.createObjectURL(blob);
        this.fileProcess.downloadFileLink(url, file.fileName);
        window.URL.revokeObjectURL(url);
      },
      error => this.handleDownloadError(error)
    );
  }

  // helper content-type
  getContentType(ext: string): string {
    switch (ext) {
      case '.pdf':
        return 'application/pdf';
      case '.docx':
        return 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
      case '.doc':
        return 'application/msword';
      default:
        return 'application/octet-stream';
    }
  }

  handleDownloadError(error: any) {
    const blb = new Blob([error.error], { type: 'text/plain' });
    const reader = new FileReader();

    reader.onload = () => {
      this.messageService.showMessage(
        (reader.result?.toString() ?? '').replace(/"/g, '')
      );
    };

    reader.readAsText(blb);
  }

  removeFile(file: any, index: number) {
    this.uploadedFiles.splice(index, 1);
  }

  removeFileDaDuyet(file: any, index: number) {
    this.uploadedFilesDaDuyet.splice(index, 1);
  }

  getTrangThaiText(status: number): string {
    switch (status) {
      case 1: return 'Soạn thảo';
      case 2: return 'Đã duyệt';
      case 3: return 'Yêu cầu chỉnh sửa';
      default: return 'Không xác định';
    }
  }

  getTrangThaiClass(status: number): string {
    switch (status) {
      case 1:
        return 'status-draft';
      case 2:
        return 'status-approved';
      case 3:
        return 'status-reject';
      default:
        return '';
    }
  }

  /**Trình lãnh đạo cục kế hoach soạn thảo */
  updateApproved() {
    this.messageService
      .showConfirm('Bạn có chắc muốn duyệt kế hoạch này')
      .then((data) => {
        const payload = {
          id: this.id,
          trangThaiKeHoachKiemTra: TrangThaiKHKTEnum
            .BanHanh
        }
        this.keHoachService.updateStatus(payload).subscribe({
          next: (data) => {
            if (data.isStatus) {
              this.messageService.showSuccess('Kế hoạch đã được duỵệt!');
              this.router.navigate(['/ke-hoach']);
            }
          },
          error: (error) => {
            this.messageService.showError(error);
          },
        });
      });
  }

  //#endregion
}
