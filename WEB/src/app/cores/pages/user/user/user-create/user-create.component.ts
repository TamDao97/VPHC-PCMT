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
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../../service/user.service';
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

@Component({
  selector: 'app-user-create',
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class UserCreateComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('grid')
  public grid!: TreeGridComponent;
  username: FormControl;
  password: FormControl;
  confirmationPassword: FormControl;
  isLoading: boolean;
  constructor(
    public fileProcess: FileProcess,
    private routeA: ActivatedRoute,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private userService: UserService,
    private router: Router,
    private fileService: FileService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private comboboxService: ComboboxCoreService,
    private changeDetectorRef: ChangeDetectorRef
  ) {
    this.translate.use(this.lgService.getLanguage());
    this.username = new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(20),
      Validators.pattern('[a-zA-Z0-9_]*'),
    ]);
    this.password = new FormControl('', [
      Validators.required,
      Validators.minLength(6),
      Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$'),
    ]);
    this.confirmationPassword = new FormControl('', [
      Validators.required,
      Validators.minLength(6),
      Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$'),
    ]);
  }

  height = 0;
  @ViewChild('treegrid')
  public treegrid: TreeGridComponent;

  @ViewChild('scrollPracticeMaterial') scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader')
  scrollPracticeMaterialHeader: ElementRef;

  @ViewChild('scrollPermession', { static: false })
  scrollPermession: ElementRef;
  @ViewChild('scrollPermessionHeader', { static: false })
  scrollPermessionHeader: ElementRef;
  id: string = '';
  type: string;
  filedata: any = null;
  minDateNotificationV: NgbDateStruct;
  isAction: boolean = false;
  listFunction: any[] = [];
  listPermission: any[] = [];
  listUserGroup: any[] = [];
  groupFunctions: any[] = [];

  listLoaiTK: any[] = [];
  listTinh: any[] = [];
  listHuyen: any[] = [];
  listXa: any[] = [];

  isSelectAll = false;
  listFunctionIndex = 0;
  model: any = {
    userName: '',
    fullName: '',
    email: '',
    phoneNumber: '',
    avatar: '',
    password: '',
    lockoutEnabled: false,
    description: '',
    isChecked: false,
    userGroupId: null,
    type: null,
    permissions: [],
    confirmationPassword: '',
  };

  modelDelteFile: any = {
    anh: '',
  };

  isIndeterminate = false;
  checkAll: boolean = false;
  groupSelect: any = {};
  groupSelectIndex: number = 0;

  ngOnInit(): void {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.fileProcess.fileModel = {};
    this.fileProcess.FileDataBase = null;
    this.id = this.routeA.snapshot.paramMap.get('id') ?? '';
    this.height = window.innerHeight - 620;
    this.getListGroupuser();
    if (this.id) {
      // this.appSetting.PageTitle = 'Cập nhật tài khoản';
      this.getUserById();
    } else {
      // this.appSetting.PageTitle = 'Thêm mới tài khoản';
    }
  }

  ngAfterViewInit() {
    this.scrollPracticeMaterial.nativeElement.addEventListener(
      'ps-scroll-x',
      (event: any) => {
        this.scrollPracticeMaterialHeader.nativeElement.scrollLeft =
          event.target.scrollLeft;
      },
      true
    );

    this.scrollPermession.nativeElement.addEventListener(
      'ps-scroll-x',
      (event: any) => {
        this.scrollPermessionHeader.nativeElement.scrollLeft =
          event.target.scrollLeft;
      },
      true
    );
  }

  ngOnDestroy() {
    this.scrollPracticeMaterial.nativeElement.removeEventListener(
      'ps-scroll-x',
      null
    );
    this.scrollPermession.nativeElement.removeEventListener(
      'ps-scroll-x',
      null
    );
  }

  listData = [];
  pathFile: string;
  public listDonVi: any;
  public fields = { text: 'name', value: 'id' };

  getListGroupuser() {
    this.comboboxService.getListGroupuser().subscribe((data: any) => {
      if (data.isStatus) {
        this.listUserGroup = data.data;
      }
    });

    this.comboboxService.getDonVi().subscribe({
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

  getUserById() {
    this.userService.getUserById(this.id).subscribe(
      (data) => {
        if (data.isStatus) {
          this.model = data.data;
          this.model.lockoutEnabled = this.model.lockoutEnabled.toString();

          if (data.data.avatar != null && data.data.avatar != '') {
            this.filedata = environment.apiUrl + data.data.avatar;
          }

          this.groupFunctions = data.data.listGroupFunction;
          this.groupSelectIndex = 0;
          for (let i = 0; i < this.groupFunctions.length; i++) {
            let checkCount = this.groupFunctions[i].checkCount;
            let length = this.groupFunctions[i].permissions.length;
            if (checkCount == 0) {
              this.groupFunctions[i].isIndeterminate = false;
            } else {
              if (checkCount < length) {
                this.groupFunctions[i].isIndeterminate = true;
              } else {
                this.groupFunctions[i].isIndeterminate = false;
                this.isSelectAll =
                  this.groupFunctions[i].checkCount ==
                  this.groupFunctions[i].permissions.length;
                this.groupFunctions[i].isChecked = this.isSelectAll;
              }
            }
          }
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  getPermission(groupuserid: string) {
    this.userService.getPermission(groupuserid).subscribe(
      (result) => {
        if (result.isStatus) {
          setTimeout(() => {
            this.groupFunctions = result.data;
          }, 200);
          this.groupSelectIndex = 0;
          for (let i = 0; i < this.groupFunctions.length; i++) {
            let checkCount = this.groupFunctions[i].checkCount;
            let length = this.groupFunctions[i].permissions.length;
            if (checkCount == 0) {
              this.groupFunctions[i].isIndeterminate = false;
            } else {
              if (checkCount < length) {
                this.groupFunctions[i].isIndeterminate = true;
              } else {
                this.groupFunctions[i].isIndeterminate = false;
                this.isSelectAll =
                  this.groupFunctions[i].checkCount ==
                  this.groupFunctions[i].permissions.length;
                this.groupFunctions[i].isChecked = this.isSelectAll;
              }
            }
          }
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  rowSelected($event: any) {
    this.listPermission = $event.data.permissions;
    this.model.permissions = this.listPermission;
  }

  changeGroupFunctionCheck(group: any, index: number) {
    group.permissions.forEach((permission: any) => {
      if (permission.isChecked && !group.isChecked) {
        group.checkCount--;
      }
      if (!permission.isChecked && group.isChecked) {
        group.checkCount++;
      }
      permission.isChecked = group.isChecked;
    });

    if (index == this.groupSelectIndex) {
      this.isSelectAll = group.isChecked;
    }
    this.groupFunctions[index].isIndeterminate = false;
  }

  selectPermission(permission: any) {
    if (!permission.isChecked) {
      this.groupFunctions[this.groupSelectIndex].checkCount--;
    } else {
      this.groupFunctions[this.groupSelectIndex].checkCount++;
    }

    let checkCount = this.groupFunctions[this.groupSelectIndex].checkCount;
    let length = this.groupFunctions[this.groupSelectIndex].permissions.length;
    if (checkCount == 0) {
      this.groupFunctions[this.groupSelectIndex].isChecked = false;
      this.groupFunctions[this.groupSelectIndex].isIndeterminate = false;
      this.isSelectAll = false;
    } else {
      if (checkCount < length) {
        this.groupFunctions[this.groupSelectIndex].isIndeterminate = true;
        this.isSelectAll = false;
      } else {
        this.groupFunctions[this.groupSelectIndex].isIndeterminate = false;
        this.isSelectAll =
          this.groupFunctions[this.groupSelectIndex].checkCount ==
          this.groupFunctions[this.groupSelectIndex].permissions.length;
        this.groupFunctions[this.groupSelectIndex].isChecked = this.isSelectAll;
      }
    }
  }

  //Thay đổi nhóm quyền
  changeGroupUser(event: any) {
    this.model.userGroupId = event;
    if (this.model.userGroupId) {
      this.getPermission(this.model.userGroupId);
    }
  }

  //Thay đổi loại tài khoản
  changeLoaiTK(event: any) {
    this.model.userGroupId = event;
    if (this.model.userGroupId) {
      this.getPermission(this.model.userGroupId);
    }
  }

  onFileChange($event: any) {
    this.fileProcess.onAFileChange($event);
    this.changeDetectorRef.detectChanges();
  }

  saveAndContinue() {
    this.save(true);
  }

  save(isContinue: boolean) {
    let regex = this.constant.validEmailRegEx;

    if (this.model.email) {
      if (!regex.test(this.model.email)) {
        this.messageService.showMessage('E-mail không hợp lệ!');
        return;
      }
    }

    this.model.listGroupFunction = this.groupFunctions;

    if (this.fileProcess.FileDataBase == null) {
      if (this.id) {
        this.update();
      } else {
        this.create(isContinue);
      }
    } else if (this.fileProcess.FileDataBase) {
      this.fileService
        .uploadFile(this.fileProcess.FileDataBase, 'User/Image')
        .subscribe({
          next: (result: any) => {
            this.model.avatar = result.data.fileUrl;
            if (this.id) {
              this.update();
            } else {
              this.create(isContinue);
            }
          },
          error: (error: any) => {
            this.messageService.showError(error);
          },
        });
    }
  }

  create(isContinue: any) {
    this.userService.createUser(this.model).subscribe(
      (result) => {
        if (result.isStatus) {
          this.messageService.showSuccess('Thêm mới tài khoản thành công!');
          if (isContinue) {
            this.isAction = true;
            this.clear();
          } else {
            this.closeModal(true);
          }
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.userService.updateUser(this.id, this.model).subscribe(
      (result) => {
        if (result.isStatus) {
          this.messageService.showSuccess('Cập nhập tài khoản thành công!');
          this.closeModal(true);
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.fileProcess.fileModel = {};
    this.fileProcess.FileDataBase = null;

    this.model = {
      userName: '',
      fullName: '',
      email: '',
      phoneNumber: '',
      anh: '',
      password: '',
      confirmationPassword: '',
      status: 1,
      description: '',
      isChecked: false,
      groupId: null,
      idChucVu: null,
    };
  }

  showComfirmDeleteFile() {
    this.messageService
      .showConfirm('Bạn có chắc muốn xóa ảnh này không?')
      .then((data) => {
        this.model.anh = null;
        this.filedata = null;
        this.fileProcess.fileModel = {};
        this.fileProcess.FileDataBase = null;
      });
  }

  deleteFile($event: any) {
    if (this.filedata != null ) {
      this.modelDelteFile.anh = this.filedata;
      // this.fileService.deleteFile(anh).subscribe(
      //   data => {
      //     if (data.statusCode == this.constant.StatusCode.Success) {
      //       this.model.anh = null;
      //       this.update();
      //     }
      //     else {
      //       this.messageService.showMessage(data.message,data.exception);
      //     }
      //   }
      //);
    } 
    
    if (this.fileProcess.FileDataBase != null) {
      this.filedata = null;
      this.fileProcess.fileModel.DataURL = null;
    }
  }

  closeModal(isOK: boolean) {
    if (this.fileProcess.fileModel.DataURL != undefined) {
      this.fileProcess.fileModel.DataURL = null;
    }
    this.router.navigate(['/nguoi-dung/tai-khoan']);
  }

  fieldTextType: boolean;
  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }

  showMK: boolean = false;
  showMKXN: boolean = false;
  toggleMK(type: number) {
    if (type == 1) this.showMK = !this.showMK;
    else if (type == 2) this.showMKXN = !this.showMKXN;
  }

  rowGroupSelected($event: any) {
    this.groupSelectIndex = $event.rowIndex;
    this.groupSelect = $event.data;
    //kiểm tra nếu collapsable thì set  Permission = []
    if (this.groupSelect.children == 0) {
      this.listPermission = $event.data.permissions;
    } else {
      this.listPermission = [];
    }

    //Kiểm tra để chek all
    var permissionsCheck = this.listPermission.filter((s) => s.isChecked);
    if (
      this.listPermission.length > 0 &&
      this.listPermission.length == permissionsCheck.length
    ) {
      this.checkAll = true;
    } else {
      this.checkAll = false;
    }
  }

  //Chuyển trạng thái check all
  selectAll() {
    this.listPermission.forEach((itemFunc) => {
      itemFunc.isChecked = this.checkAll;
    });

    var itemsChoose = this.listPermission.filter((a) => a.isChecked);
    this.changeDataSub(this.groupFunctions, itemsChoose.length);

    //refresh tree
    this.treegrid.refresh();
    setTimeout(() => {
      this.treegrid.selectRow(this.groupSelectIndex);
    }, 100);
  }

  checkItem(permission: any) {
    var itemsChoose = this.listPermission.filter((a) => a.isChecked);
    if (itemsChoose.length == this.listPermission.length) this.checkAll = true;
    else this.checkAll = false;

    this.changeDataSub(this.groupFunctions, itemsChoose.length);

    //refresh tree
    this.treegrid.refresh();
    setTimeout(() => {
      this.treegrid.selectRow(this.groupSelectIndex);
    }, 100);
  }

  changeDataSub(listSub: any[], totalChoose: number) {
    listSub.forEach((item) => {
      if (this.groupSelect.id == item.id) item.checkCount = totalChoose;

      if (item.children.length > 0)
        this.changeDataSub(item.children, totalChoose);
    });
  }
}
