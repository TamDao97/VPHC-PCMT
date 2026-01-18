import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { FormControl, Validators } from '@angular/forms';
import { Constants } from 'src/app/cores/shared/common/constants';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { AuthService } from '../../services/auth.service';
declare var $: any;

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ChangePasswordComponent implements OnInit, OnDestroy {
  private message: string = '';
  returnUrl: string = '';
  MatKhauCu: any;
  XacNhanMatKhauMoi: any;
  idChange: string;
  ischange = false;
  model: any = {
    Id: '',
    MatKhauCu: '',
    MatKhauMoi: '',
    XacNhanMatKhauMoi: '',
    isChange: false,
  };
  passwordNew: FormControl;
  enterPassword: FormControl;
  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private constant: Constants,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    private translate: TranslateService,
    private lgService: LanguageService
  ) {
    this.translate.use(this.lgService.getLanguage());
    this.passwordNew = new FormControl('', [
      Validators.required,
      Validators.minLength(6),
      Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$'),
    ]);
    this.enterPassword = new FormControl('', [
      Validators.required,
      Validators.minLength(6),
      Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$'),
    ]);
  }
  ngOnDestroy(): void {}

  ngOnInit() {
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });

    let user = localStorage.getItem('bvteCurrentUser');
    if (user) {
      this.model.id = JSON.parse(user).userid;
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(true);
  }

  ConfirmChangePassword() {
    this.messageService
      .showConfirm('Bạn có chắc muốn thay đổi mật khẩu không?')
      .then((data) => {
        this.ChangePassword();
      });
  }

  ChangePassword() {
    let user = localStorage.getItem('CurrentUser');
    if (!this.ischange) {
      if (user) {
        this.model.id = JSON.parse(user).userId;
      }
      this.model.isChange = false;
    } else {
      this.model.id = this.idChange;
      this.model.isChange = true;
    }
    this.authService.changePassword(this.model).subscribe({
      next: (result: any) => {
        if (result.isStatus) {
          this.messageService.showSuccess('Thay đổi mật khẩu thành công!');
          this.closeModal(true);
          if (!this.ischange) {
            this.authService.logout();
            this.router.navigate(['/auth/dang-nhap']);
          }
        }
      },
      error: (error: any) => {
        this.messageService.showError(error);
        this.model.MatKhauCu = null;
        this.model.MatKhauMoi = null;
        this.model.XacNhanMatKhauMoi = null;
      },
    });
  }

  showMKCu: boolean = false;
  showMKMoi: boolean = false;
  showMKXN: boolean = false;
  toggleMK(type: number) {
    if (type == 1) this.showMKCu = !this.showMKCu;
    else if (type == 2) this.showMKMoi = !this.showMKMoi;
    else if (type == 3) this.showMKXN = !this.showMKXN;
  }
}
