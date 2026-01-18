import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../../service/user.service';
import { TranslateService } from '@ngx-translate/core';
import { Constants } from 'src/app/cores/shared/common/constants';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { FileService } from 'src/app/cores/shared/services/file.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class UserInfoComponent implements OnInit {
  constructor(
    public constant: Constants,
    public fileProcess: FileProcess,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private fileService: FileService,
    private userService: UserService,
    private translate: TranslateService,
    private lgService: LanguageService
  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  modalInfo = {
    Title: '',
    SaveText: '',
  };

  model: any = {
    id: '',
    userName: '',
    fullName: '',
    email: '',
    phoneNumber: '',
    imageLink: '',
    password: '',
    description: '',
    isChecked: false,
  };

  id: any;
  isAction: boolean = false;
  filedata: any = null;
  isCheck = '';

  modelDelteFile: any = {
    avatar: '',
  };
  user: any | null;

  ngOnInit(): void {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    this.fileProcess.fileModel = {};
    this.fileProcess.FileDataBase = null;
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật thông tin';
      this.modalInfo.SaveText = 'Lưu';
      this.getUserInfo();
    }
  }

  onFileChange($event: any) {
    this.fileProcess.onAFileChange($event);
  }

  showComfirmDeleteFile() {
    this.messageService
      .showConfirm('Bạn có chắc muốn xóa ảnh này không?')
      .then((data) => {
        this.model.imageLink = null;
        this.filedata = null;
        this.fileProcess.fileModel = {};
        this.fileProcess.FileDataBase = null;
      });
  }

  getUserInfo() {
    this.userService.getUserInfo(this.id).subscribe(
      (result) => {
        if (result.isStatus) {
          this.model = result.data;
          this.isCheck = this.model.idChucVu;

          if (result.data.avatar != null && result.data.avatar != '') {
            this.filedata = environment.apiUrl + result.data.avatar;
          }
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    if (this.fileProcess.FileDataBase == null) {
      this.userService.updateUserInfo(this.id, this.model).subscribe(
        (result) => {
          if (result.isStatus) {
            this.messageService.showSuccess('Cập nhập tài khoản thành công!');

            this.user = JSON.parse(localStorage.getItem('CurrentUser') ?? '');
            if (this.user) {
              this.user.fullName = this.model.fullName;
              this.user.avatar = this.model.avatar;
            }
            localStorage.setItem('CurrentUser', JSON.stringify(this.user));

            this.closeModal(true);
          }
        },
        (error) => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.fileService
        .uploadFile(this.fileProcess.FileDataBase, 'User/Image')
        .subscribe(
          (result) => {
            this.model.avatar = result.data.fileUrl;
            this.userService.updateUserInfo(this.id, this.model).subscribe(
              (result) => {
                if (result.isStatus) {
                  this.messageService.showSuccess(
                    'Cập nhập tài khoản thành công!'
                  );

                  this.user = JSON.parse(
                    localStorage.getItem('CurrentUser') ?? ''
                  );
                  if (this.user) {
                    this.user.fullName = this.model.fullName;
                    this.user.avatar = this.model.avatar;
                  }
                  localStorage.setItem(
                    'CurrentUser',
                    JSON.stringify(this.user)
                  );

                  this.closeModal(true);
                }
              },
              (error) => {
                this.messageService.showError(error);
              }
            );
          },
          (error) => {
            this.messageService.showError(error);
          }
        );
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
