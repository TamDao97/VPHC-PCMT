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
  selector: 'app-function-auto-edit',
  templateUrl: './function-auto-edit.component.html',
  styleUrls: ['./function-auto-edit.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class FunctionAutoEditComponent implements OnInit {
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
  //Node tree select trên treeview của chức năng bên ngoài
  selectedNode: any;
  //Thông tin cấu hình giao diện chức năng
  modelDesign: any = {};
  modelDateView: any = {};
  //Model cập nhật thông tin
  model: any = {};

  //Tiêu đề chức năng
  titleForm: string = 'Chỉnh sửa';

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
    this.titleForm = 'Chỉnh sửa ' + this.modelDesign.functionName;

    this.getDataUpdate();
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

  //Lấy thông tin cập nhật
  getDataUpdate() {
    this.service.getUpdateById(this.slug, this.id).subscribe(
      (data) => {
        if (data.isStatus) {
          this.model = data.data;
          this.modelDesign.updateDesigns.forEach((item: any) => {
            if (item.editControlType == 6) {
              if (this.model[item.columnName] != null) {
                this.modelDateView[item.columnName + 'V'] =
                  this.dateUtils.convertDateToObject(
                    this.model[item.columnName]
                  );
              } else {
                this.modelDateView[item.columnName + 'V'] = null;
              }
            }

            //Nếu liên kết và id kiểu int thì chuyển sang string để đồng nhất với id dữ liệu combox
            if (item.isLink && item.dataType == 'Int') {
              this.model[item.columnName] = '' + this.model[item.columnName];
            }
          });
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  //Lưu thông tin
  save() {
    this.modelDesign.editDesigns.forEach((item: any) => {
      if (item.controlType == 6) {
        if (this.modelDateView[item.columnName + 'V'] != null) {
          this.model[item.columnName] = this.dateUtils.convertObjectToDate(
            this.modelDateView[item.columnName + 'V']
          );
        } else {
          this.model[item.columnName] = null;
        }
      }
    });

    //Call api cập nhật
    this.service.update(this.slug, this.id, this.model).subscribe(
      (result) => {
        if (result.isStatus) {
          this.activeModal.close(true);
          this.messageService.showSuccess(
            'Cập nhật ' + this.modelDesign.functionName + ' thành công!'
          );
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }

  //ĐÓng modal hoặc load lại trang
  closeModal(isOK: boolean) {
    if (this.modelDesign.editWindowType == 1) {
      this.activeModal.close(isOK ? isOK : this.isAction);
    } else {
      this.router.navigate(['/system-config/function/manage/' + this.slug]);
    }
  }
}
