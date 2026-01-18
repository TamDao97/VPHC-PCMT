import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TreeGridComponent } from '@syncfusion/ej2-angular-treegrid';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CategoryService } from '../../service/category.service';
import { CategoryCreateComponent } from '../category-create/category-create.component';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { SearchGlobalService } from 'src/app/cores/shared/common/search-global.service';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { MenuOptions } from 'src/app/cores/shared/models/menu-options.model';
import { UserService } from 'src/app/_fake/services/user-service';
//import { LanguageService } from 'src/app/shared/services/language.service';

@Component({
  selector: 'app-category-manage',
  templateUrl: './category-manage.component.html',
  styleUrls: ['./category-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CategoryManageComponent implements OnInit {

  @ViewChild('grid')
  public grid!: TreeGridComponent;

  constructor(
    // public constant: Constants,
    private modalService: NgbModal,
    private fileProcess: FileProcess,
    private messageService: MessageService,
    private categoryService: CategoryService,
    private searchGlobalService: SearchGlobalService,
    private translate:TranslateService,
    //private lgService:LanguageService,
    private apiService: UserService
  ) {
    this._unsubscribeAll = new Subject();
    //this.translate.use(this.lgService.getLanguage());
  }

  _unsubscribeAll: Subject<any>;
  startIndex = 1;
  public searchSettingModel: Object = {};
  public toolbar: string[] = [];
  groupCategories: any[] = [];
  categories: any[] = [];
  height = 0;
  type = 0;
  name: string;

  searchModel: any = {
    pageNumber: 1,
    pageSize: 10,
    totalItems: 0,

    name: '',
    tableName: '',
    //groupCategoryId: null,
  }

  categoryModel: any = {
    id: null,
    name: '',
    order: null,
    tableName: '',
  }

  ngOnInit(): void {
    this.searchSettingModel = { hierarchyMode: 'Parent' };
    this.toolbar = ['Search'];
    this.height = window.innerHeight - 180;
    this.searchCategory();
    this.searchGlobalService.onDataChanged.pipe(takeUntil(this._unsubscribeAll)).subscribe(data => {
      if (data) {
        this.searchModel.name = data.name;
        this.searchModel.pageNumber = data.pageNumber;

        this.search();
      }
    });

    this.setToolbarConfig();
  }

  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this.searchGlobalService.setConfig(null);
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();
  }

  setToolbarConfig() {
    let meuOptions: MenuOptions = {
      isExcel: false,
      isPDF: false,
      isSearch: true,
      searchModel: this.searchModel,
      searchOptions: {
        FieldContentName: 'name',
        Placeholder: 'Tìm kiếm tên danh mục',
        Items: [
          // {
          //   FieldName: 'type',
          //   Name: 'Loại đơn vị tính',
          //   Type: 'ngselect',
          //   DisplayName: 'Name',
          //   ValueName: 'Id',
          //   Data: this.constant.listUnitType
          // }
        ]
      }
    };

    this.searchGlobalService.setConfig(meuOptions);
  }

  searchCategory() {
    this.categoryService.searchCategory().subscribe(
      (result: any) => {
        if (result.isStatus) {
          setTimeout(() => {
            this.groupCategories = result.data;
            if (this.groupCategories.length > 0) {
              this.grid.selectedRowIndex = 1;
            }
          }, 200);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  search() {
    if (this.type == 2) {
      this.categoryService.searchCategoryTable(this.searchModel).subscribe(
        (result: any) => {
          if (result.isStatus) {
            this.searchModel.totalItems = result.data.totalItems;
            this.categories = result.data.dataResults;
          }
          else {
            this.messageService.showMessage(result.message,result.exception);
          }
        }, error => {
          this.messageService.showError(error);
        }
      );
    }
  }

  onChange(event: any) {
    // = event.itemData.id;
  }

  rowSelected($event: any) {
    this.searchModel.tableName = $event.data.tableName;
    this.type = $event.data.type;
    this.name = $event.data.name;
    if (this.type == 2) {
      this.setToolbarConfig();
      this.search();
    }
  }

  showConfirmDelete(id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá danh mục này không?").then(
      (result:any) => {
        this.delete(id);
      }
    );
  }

  delete(id: string) {
    this.categoryService.deleteCategoryTable(id, this.searchModel.tableName).subscribe(
      result => {
        if (result.isStatus) {
          this.messageService.showSuccess('Xóa danh mục thành công!');
          this.search();
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.searchModel = {
      pageNumber: 1,
      pageSize: 10,
      totalItems: 0,

      name: '',
      tableName: this.searchModel.tableName,
      groupCategoryId: null,
    }
    this.search();
  }

  showCreateUpdate(id: string) {
    let activeModal = this.modalService.open(CategoryCreateComponent, { container: 'body', windowClass: 'category-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.tableName = this.searchModel.tableName;
    activeModal.result.then((result: any) => {
      if (result) {
        this.search();
      }
    });
  }

  onDrop(event: CdkDragDrop<string[]>) {
    let currentIndex = this.categories[event.currentIndex].order;
    let previousIndex = this.categories[event.previousIndex].order;
    this.categories[event.currentIndex].order = previousIndex;
    this.categories[event.previousIndex].order = currentIndex;
    moveItemInArray(this.categories, event.previousIndex, event.currentIndex);
    if (currentIndex < previousIndex) {
      this.categoryModel = this.categories[event.currentIndex];
    } else {
      this.categoryModel = this.categories[event.previousIndex];
    }

    this.update();
  }

  update() {
    this.categoryModel.tableName = this.searchModel.tableName;
    this.categoryService.updateCategoryTable(this.categoryModel.id, this.categoryModel).subscribe(
      result => {
        if (result.isStatus) {
          this.messageService.showSuccess('Cập nhập danh mục thành công!');
        } 
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  export() {
    this.categoryService.getExport().subscribe(data => {
      var blob = new Blob([data], { type: 'octet/stream' });
      var url = window.URL.createObjectURL(blob);
      this.fileProcess.downloadFileLink(url, "Danhmuc.zip");
    }, error => {
      const blb = new Blob([error.error], { type: "text/plain" });
      const reader = new FileReader();

      reader.onload = () => {
        //this.messageService.showMessage(reader.result.toString().replace('"', '').replace('"', ''));
      };
      // Start reading the blob as text.
      reader.readAsText(blb);
    });
  }
}
