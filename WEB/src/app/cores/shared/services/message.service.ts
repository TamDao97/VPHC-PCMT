import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

import { MessageComponent } from '../component/message/message.component';
import { MessageconfirmComponent } from '../component/messageconfirm/messageconfirm.component';
import { NTSModalService } from '../services/ntsmodal.service';
import { MessageconfirmcodeComponent } from '../component/messageconfirmcode/messageconfirmcode.component';
import { LanguageService } from './language.service';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  constructor(
    private modalService: NgbModal,
    private ntsModalService: NTSModalService,
    public router: Router,
    private toastr: ToastrService,
    private translate: TranslateService,
    private lgService: LanguageService
  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  /*
   * type:
   * 0: Bình thường
   * 1: Lỗi
   */
  messageCurrent = '';
  showMessage(message: string, exception: string = '') {
    if (message != this.messageCurrent) {
      const activeModal = this.modalService.open(MessageComponent, {
        container: 'body',
        centered: true,
      });
      this.messageCurrent = message;
      activeModal.componentInstance.message = message;
      activeModal.componentInstance.exception = exception;
      activeModal.result.then(
        (result) => {
          this.ntsModalService.closeMultiModal();
          this.messageCurrent = '';
        },
        (reason) => {
          this.ntsModalService.closeMultiModal();
          this.messageCurrent = '';
        }
      );
    }
  }

  showListMessage(message: string[]) {
    const activeModal = this.modalService.open(MessageComponent, {
      container: 'body',
    });
    activeModal.componentInstance.messages = message;
    activeModal.result.then(
      (result) => {
        this.ntsModalService.closeMultiModal();
      },
      (reason) => {
        this.ntsModalService.closeMultiModal();
      }
    );
  }

  showError(error: any) {
    let message: string = '';
    if (error.status == 0) {
      message = 'Không thể kết nối được đến server, vui lòng kiểm tra lại';
    } else if (error.status == 400) {
      message =
        'Dữ liệu đầu vào không hợp lệ. Vui lòng kiểm tra các trường bắt buộc hoặc định dạng kiểu dữ liệu cho đúng.';
    } else if (error.status == 401) {
      localStorage.removeItem('CurrentUser');
      message = 'Bạn đã hết phiên làm việc. Bạn hãy đăng nhập lại để tiếp tục.';
      this.router.navigate(['/auth/dang-nhap']);
    } else {
      var messagecode = error.error.messageCode;
      this.translate.get('Message.' + messagecode).subscribe((res: string) => {
        message = res;
      });
    }

    this.showMessage(message);
  }

  showMessageErrorBlob(err: any) {
    var arrayBuffer;
    var fileReader = new FileReader();
    fileReader.onload = (event: any) => {
      arrayBuffer = event.target;
      let str = new TextDecoder().decode(arrayBuffer.result);
      this.showMessage(str);
    };
    fileReader.readAsArrayBuffer(err.error);
  }

  showConfirm(message: string): Promise<any> {
    return new Promise((resolve, reject) => {
      const activeModalConfirm = this.modalService.open(
        MessageconfirmComponent,
        { container: 'body', centered: true }
      );
      activeModalConfirm.componentInstance.message = message;
      activeModalConfirm.result.then(
        (result) => {
          this.ntsModalService.closeMultiModal();
          if (result) {
            resolve(result);
          }
          // else{
          //     reject(false);
          // }
        },
        (reason) => {
          reject('');
          this.ntsModalService.closeMultiModal();
        }
      );
    });
  }

  messageToastrCurrent = '';
  showSuccess(message: string, title?: string) {
    if (this.messageToastrCurrent != message) {
      this.messageToastrCurrent = message;
      const toastrRef = this.toastr.success(message, title);
      toastrRef.onHidden.subscribe(() => {
        this.messageToastrCurrent = '';
      });
      toastrRef.onTap.subscribe(() => {
        this.messageToastrCurrent = '';
      });
    }
  }

  showInfo(message: string, title?: string) {
    if (this.messageToastrCurrent != message) {
      this.messageToastrCurrent = message;
      const toastrRef = this.toastr.info(message, title);
      toastrRef.onHidden.subscribe(() => {
        this.messageToastrCurrent = '';
      });
      toastrRef.onTap.subscribe(() => {
        this.messageToastrCurrent = '';
      });
    } 
  }

  showWarning(message: string, title?: string) {
    if (this.messageToastrCurrent != message) {
      this.messageToastrCurrent = message;
      const toastrRef = this.toastr.warning(message, title);
      toastrRef.onHidden.subscribe(() => {
        this.messageToastrCurrent = '';
      });
      toastrRef.onTap.subscribe(() => {
        this.messageToastrCurrent = '';
      });
    }
  }

  showConfirmCode(message: string): Promise<any> {
    return new Promise((resolve, reject) => {
      const activeModalConfirm = this.modalService.open(
        MessageconfirmcodeComponent,
        { container: 'body' }
      );
      activeModalConfirm.componentInstance.message = message;
      activeModalConfirm.result.then(
        (result) => {
          this.ntsModalService.closeMultiModal();
          if (result) {
            resolve(result);
          } else {
            reject(false);
          }
        },
        (reason) => {
          reject('');
          this.ntsModalService.closeMultiModal();
        }
      );
    });
  }
}
