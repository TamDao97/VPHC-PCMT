import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ContextMenuItemModel, GridComponent } from '@syncfusion/ej2-angular-grids';
import { Observable, firstValueFrom } from 'rxjs';
import { Constants } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { FileService } from 'src/app/cores/shared/services/file.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { AuthService, UserType } from 'src/app/modules/auth';
import { KeHoachService } from '../../service/ke-hoach.service';

@Component({
  selector: 'app-ke-hoach-edit',
  templateUrl: './ke-hoach-edit.component.html',
  styleUrl: './ke-hoach-edit.component.scss'
})
export class KeHoachEditComponent implements OnInit {
  id: any = '';
  cap: any;
  idKeHoachCuc: any = null;
  isShowTabCuc: boolean = true;
  isEditCuc: boolean = true;
  isShowTabPhong: boolean = false;
  idKeHoachPhong: any = null;
  isEditPhong: boolean = true;
  user$: Observable<UserType>;

  isAddCuc: boolean = false;

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


  ) {
    this.translate.use(this.lgService.getLanguage());
  }
  ngOnInit(): void {
    this.fileProcess.fileModel = {};
    this.fileProcess.FileDataBase = null;
    this.user$ = this.auth.currentUserSubject.asObservable();
    this.id = this.routeA.snapshot.paramMap.get('id') ?? '';
    this.cap = this.routeA.snapshot.queryParamMap.get('cap');
    console.log(this.cap);

    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });


    if (this.id) {
      this.getById();
    } else {
      this.isAddCuc = true;
      this.user$.subscribe(async (user) => {
        if (user != null) {
        }
      });
    }

  }
  //#region ------------Lấy thông tin cập nhật---------
  getById() {
    this.keHoachService.getById(this.id).subscribe({
      next: async (result) => {
        if (result.isStatus) {
          let data = result.data;
          if (data.capKeHoach == 1) {
            //cấp cục
            this.idKeHoachCuc = data.id;
            this.isShowTabCuc = true;
          }
          if (data.capKeHoach == 2 && this.cap !== '3') {
            this.idKeHoachCuc = data.idKeHoachCha;
            this.idKeHoachPhong = data.id;
            this.isShowTabCuc = true;
            this.isShowTabPhong = true;
            this.isEditCuc = false;
          }
          if (data.capKeHoach == 2 && this.cap === '3') {
            this.isShowTabCuc = false;
            this.isShowTabPhong = true;
            this.idKeHoachPhong = data.id;
            this.isEditPhong = false;
            //cấp đồn
            console.log(data.capKeHoach, this.cap);

          }


        }
      },
      error: (error) => {
        this.messageService.showError(error);
      },
    });
  }
}