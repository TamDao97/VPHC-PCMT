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
  selector: 'app-function-auto-create',
  templateUrl: './function-auto-create.component.html',
  styleUrls: ['./function-auto-create.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class FunctionAutoCreateComponent implements OnInit {
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

  slug: string;
  //Thông tin cấu hình chức năng bên ngoài của chức năng này
  modelManageDesign: any = {};
  //Node tree select trên treeview của chức năng bên ngoài
  selectedNode: any;
  //Thông tin cấu hình giao diện chức năng
  modelDesign: any = {};
  modelDateView: any = {};
  //Model thêm mới thông tin
  model: any = {};

  titleForm: string = 'Thêm mới';

  ngOnInit(): void {
    //Lấy slug trên url
    if (!this.slug) this.slug = this.route.snapshot.paramMap.get('slug') ?? '';

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
    this.titleForm = 'Thêm mới ' + this.modelDesign.functionName;

    //Trường hợp giao diện quản lý bên ngoài dạng tree view
    if (this.modelManageDesign.layoutType == 2) {
      //Loại bỏ control chọn danh mục treeview trên giao diện
      this.modelDesign.createDesigns = this.modelDesign.createDesigns.filter(
        (s: any) => s.linkTable != this.modelManageDesign.treeTableName
      );

      //Sét giá trị node treeview select cho model
      this.modelDesign.columnDesigns.forEach((itemDesign: any) => {
        if (
          this.modelManageDesign.treeTableName &&
          itemDesign.linkTable == this.modelManageDesign.treeTableName
        ) {
          this.model[itemDesign.columnName] = this.selectedNode.id;
        }
      });
    }
  }

  //Lưu dữ liệu
  save(isContinue: boolean) {
    this.modelDesign.createDesigns.forEach((item: any) => {
      if (item.createControlType == 6) {
        if (this.modelDateView[item.columnName + 'V'] != null) {
          this.model[item.columnName] = this.dateUtils.convertObjectToDate(
            this.modelDateView[item.columnName + 'V']
          );
        } else {
          this.model[item.columnName] = null;
        }
      }
    });

    //Call api lưu dữ liệu
    this.service.create(this.slug, this.model).subscribe(
      (result) => {
        if (result.isStatus) {
          this.messageService.showSuccess(
            'Thêm mới ' + this.modelDesign.functionName + ' thành công!'
          );
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

  //Lưu và tiếp tục
  saveAndContinue() {
    this.save(true);
  }

  //Đóng modal hoặc load lại trang
  closeModal(isOK: boolean) {
    if (this.modelDesign.createWindowType == 1) {
      this.activeModal.close(isOK ? isOK : this.isAction);
    } else {
      this.router.navigate(['/system-config/function/manage/' + this.slug]);
    }
  }

  //Clear dữ liệu khi lưu và tiếp tục
  clear() {
    this.model = {};
    //Trường hợp giao diện quản lý bên ngoài dạng tree view
    if (this.modelManageDesign.layoutType == 2) {
      //Sét giá trị node treeview select cho model
      this.modelDesign.columnDesigns.forEach((itemDesign: any) => {
        if (
          this.modelManageDesign.treeTableName &&
          itemDesign.linkTable == this.modelManageDesign.treeTableName
        ) {
          this.model[itemDesign.columnName] = this.selectedNode.id;
        }
      });
    }
  }
}
