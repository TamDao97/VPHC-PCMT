import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
// import { CategoryService } from '../../service/category.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { Constants } from 'src/app/cores/shared/common/constants';

@Component({
  selector: 'app-category-create',
  templateUrl: './category-create.component.html',
  styleUrls: ['./category-create.component.scss']
})
export class CategoryCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    // private categoryService: CategoryService,
    private comboboxCoreService: ComboboxCoreService,
    public constant : Constants
  ) { }

  id: any;
  tableName: any;
  isAction: boolean = false;
  listIndex: any[] = [];
  listParent: any[] = [];
  modalInfo = {
    Title: 'Thêm mới danh mục',
  };

  categoryModel: any = {
    id: null,
    parentId: null,
    name: '',
    index: null,
    tableName: '',
  }

  ngOnInit(): void {
    this.categoryModel.tableName = this.tableName;    
    this.getListOder();
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật danh mục';
      this.getCategoryById();
    } else {
      this.modalInfo.Title = 'Thêm mới danh mục';
    }
  }
  getListOder() {
    // this.categoryService.getListOderTable(this.id, this.tableName).subscribe(
    //   result => {
    //     if (result.isStatus) {
    //       this.listIndex = result.data;
    //       if (!this.id) {
    //         this.categoryModel.index = this.listIndex[this.listIndex.length - 1];
    //       }
    //     }
    //   }, error => {
    //     this.messageService.showError(error);
    //   }
    // );
  }

  getCategoryById() {
    // this.categoryService.getCategoryTableInfo(this.id, this.categoryModel.tableName).subscribe(
    //   result => {
    //     if (result.isStatus) {
    //       this.categoryModel = result.data;
    //     }
    //   }, error => {
    //     this.messageService.showError(error);
    //   }
    // );
  }

  saveAndContinue() {
    this.save(true);
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.update();
    } else {
      this.create(isContinue);
    }
  }

  create(isContinue: any) {
    // this.categoryService.createCategoryTable(this.categoryModel).subscribe(
    //   result => {
    //     if (result.isStatus) {
    //       this.messageService.showSuccess('Thêm mới danh mục thành công!');
    //       if (isContinue) {
    //         this.isAction = true;
    //         this.clear();
    //       } else {
    //         this.closeModal(true);
    //       }
    //     } 
    //   },
    //   error => {
    //     this.messageService.showError(error);
    //   }
    // );
  }

  update() {
    this.categoryModel.tableName = this.tableName;
    // this.categoryService.updateCategoryTable(this.id, this.categoryModel).subscribe(
    //   result => {
    //     if (result.isStatus) {
    //       this.messageService.showSuccess('Cập nhập danh mục thành công!');
    //       this.closeModal(true);
    //     } 
    //   },
    //   error => {
    //     this.messageService.showError(error);
    //   }
    // );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  clear() {
    this.categoryModel = {
      id: null,
      parentId: null,
      name: '',
      order: null,
      tableName: this.categoryModel.tableName,
    }

    this.getListOder();
  }

}
