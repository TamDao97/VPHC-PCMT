import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../../service/user.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {

  constructor(
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private comboboxService: ComboboxCoreService
  ) { }

  thongTinPopup = {
    Tieude: '',
    LuuTxt: ''
  };

  diachihopthu: string;
  maxacnhan: string;
  step = 1;

  ngOnInit(): void {
    this.thongTinPopup.Tieude = "Quên mật khẩu";
    if (this.step = 1) {
      this.thongTinPopup.LuuTxt = "Lấy mã";
    } else {
      this.thongTinPopup.LuuTxt = "Lấy mật khẩu";
    }
  }

  LayMaXacNhan() {
    // this.authenticationService.getOTP(this.diachihopthu).subscribe(
    //   result => {
    //     if (result.isStatus) {
    //       if (result.data) {
    //         this.messageService.showSuccess("Mã OTP đã được gửi vào email của bạn!");
    //         this.step = 2;
    //       }
    //     }       
    //   }, error => {
    //     this.messageService.showError(error);
    //   }
    // );
  }

  QuenMatKhau() {
    // this.authenticationService.forgotPassword(this.diachihopthu, this.maxacnhan).subscribe(
    //   result => {
    //     if (result.isStatus) {
    //       if (result.data) {
    //         this.messageService.showSuccess("Mật khẩu mới đã được gửi vào email của bạn!");
    //         this.closeModal();
    //       }
    //     }
    //   }, error => {
    //     this.messageService.showError(error);
    //   }
    // );
  }

  closeModal() {
    this.activeModal.close();
  }
}
