import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FunctionAutoService } from '../../../service/function-auto.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';

@Component({
  selector: 'app-function-auto-detail',
  templateUrl: './function-auto-detail.component.html',
  styleUrls: ['./function-auto-detail.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class FunctionAutoDetailComponent implements OnInit {
  constructor(
    private activeModal: NgbActiveModal,
    private router: Router,
    private route: ActivatedRoute,
    private messageService: MessageService,
    private service: FunctionAutoService,
    public constant: Constants,
    private dateUtils: DateUtils,
    private translate: TranslateService,
    private lgService: LanguageService
  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  isAction: boolean = false;
  id: string;

  slug: string;
  //Thông tin cấu hình chức năng bên ngoài của chức năng này
  modelManageDesign: any = {};
  //Thông tin cấu hình giao diện chức năng
  modelDesign: any = {};
  modelDateView: any = {};
  //Model chi tiêt thông tin
  model: any = {};

  //Tiêu đề chức năng
  titleForm: string = 'Chi tiết';

  ngOnInit(): void {
    //Lấy slug trên url
    if (!this.slug) this.slug = this.route.snapshot.paramMap.get('slug') ?? '';

    //Lấy id trên url
    if (!this.id) this.id = this.route.snapshot.paramMap.get('id') ?? '';

    this.getFunctionDesign();
  }

  //Lấy thông tin cấu hình giao diện để hiển thị
  getFunctionDesign() {
    this.service.getConfigDesign(this.slug).subscribe(
      (result) => {
        if (result.isStatus) {
          
          this.modelDesign = result.data;
          this.initForm();
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  //Khởi tạo form
  initForm() {
    this.titleForm = 'Chi tiết ' + this.modelDesign.functionName;

    this.getDetaiData();
  }

  //Lấy thông tin chi tiết
  getDetaiData() {
    this.service.getDetailById(this.slug, this.id).subscribe(
      (data) => {
        if (data.isStatus) {
          this.model = data.data;
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  //ĐÓng modal hoặc load lại trang
  closeModal(isOK: boolean) {
    if (this.modelDesign.detailWindowType == 1) {
      this.activeModal.close(isOK ? isOK : this.isAction);
    } else {
      this.router.navigate(['/system-config/function/manage/' + this.slug]);
    }
  }
}
