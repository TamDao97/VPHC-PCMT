import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbModal, NgbPopoverConfig } from '@ng-bootstrap/ng-bootstrap';
import { ToolbarItems, TreeGridComponent } from '@syncfusion/ej2-angular-treegrid';
import { ClickEventArgs } from '@syncfusion/ej2-navigations';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FunctionConfigService } from '../../service/function-config.service';
import { CheckBoxSelectionService } from '@syncfusion/ej2-angular-dropdowns';
import { Constants } from 'src/app/cores/shared/common/constants';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { SearchGlobalService } from 'src/app/cores/shared/common/search-global.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { MenuOptions } from 'src/app/cores/shared/models/menu-options.model';

@Component({
  selector: 'app-function-manage',
  templateUrl: './function-manage.component.html',
  styleUrls: ['./function-manage.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [NgbPopoverConfig]
})
export class FunctionManageComponent implements OnInit {

  @ViewChild('grid')
  public grid!: TreeGridComponent;

  constructor(
    public constant: Constants,
    private messageService: MessageService,
    private functionService: FunctionConfigService,
    private searchGlobalService: SearchGlobalService,
    private comboboxService: ComboboxCoreService,
    private fileProcess: FileProcess,
    config: NgbPopoverConfig
  ) {
    this._unsubscribeAll = new Subject();
    config.autoClose = 'outside';
  }

  _unsubscribeAll: Subject<any>;
  startIndex = 1;
  groupCategories: any[] = [];
  height = 0;
  type = 0;
  name: string;
  //0: Xem chi tiết; 1: Thêm mới; 2: Chỉnh sửa
  action: any = 0;
  idUpdate: string = '';

  searchModel: any = {
    pageNumber: 1,
    pageSize: 10,
    totalItems: 0,
    name: '',
    tableName: ''
  }

  model: any;

  tableNanes: any[] = [];
  listSystemFunctionAuto: any[] = [];

  ngOnInit(): void {
    this.height = window.innerHeight - 180;
    this.initModel();
    this.search();

    this.setToolbarConfig();

    this.getIntData();
  }

  //Khởi tạo model
  initModel() {
    this.model = {
      tableName: null,
      functionName: '',
      slug: '',
      createWindowType: 1,
      editWindowType: 1,
      detailWindowType: 1,
      searchDisplay: true,
      filterDisplay: true,
      createDisplay: true,
      editDisplay: true,
      detailDisplay: true,
      deleteDisplay: true,
      importDisplay: false,
      exportDisplay: false,
      layoutType: 1,
      treeColumnsText: '',
      functionGroup: '',
      searchPermission: '',
      createPermission: '',
      editPermission: '',
      deletePermission: '',
      detailPermission: '',
      importPermission: '',
      exportPermission: '',
      functionDesigns: []
    }
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
      isSearch: false,
      searchModel: this.searchModel,
      searchOptions: {
        FieldContentName: 'name',
        Placeholder: 'Tìm kiếm tên đơn vị tính',
        Items: []
      }
    };

    this.searchGlobalService.setConfig(meuOptions);
  }

  getIntData() {
    this.functionService.getTableName().subscribe(
      (result: any) => {
        if (result.isStatus) {
          this.tableNanes = result.data;
        }
      }, error => {
        this.messageService.showError(error);
      }
    );

    this.comboboxService.getSystemFunctionAuto().subscribe(
      result => {
        if (result.isStatus) {
          this.listSystemFunctionAuto = result.data;
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getColumnTable(tableName: string) {
    this.functionService.getColumnTable(tableName).subscribe(
      (result: any) => {
        if (result.isStatus) {
          this.model.functionDesigns = result.data;
        }
      }, error => {
        this.model.functionDesigns = [];
        this.messageService.showError(error);
      }
    );
  }

  search() {
    this.functionService.searchFunction(this.searchModel).subscribe(
      (result: any) => {
        if (result.isStatus) {
          setTimeout(() => {
            this.groupCategories = result.data.dataResults
          }, 200);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  rowSelected($event: any) {
    this.searchModel.tableName = $event.data.tableName;
    this.type = $event.data.type;
    this.name = $event.data.name;
  }

  addNew() {
    this.action = 1;
    this.initModel();
  }

  showConfirmDelete(id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá cấu hình chức năng này không?").then(
      result => {
        this.delete(id);
      }
    );
  }

  delete(id: string) {
    this.functionService.deleteFunctionConfig(id).subscribe(
      result => {
        if (result.isStatus) {
          this.messageService.showSuccess('Xóa cấu hình chức năng thành công!');
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
      groupFunctionId: null,
    }
    this.search();
  }

  getUpdateById(id: string) {
    this.functionService.getUpdateFunctionConfigById(id).subscribe(
      result => {
        if (result.isStatus) {
          this.model = result.data;
          this.action = 2;
          this.idUpdate = id;

          if (this.model.layoutType == 2) {
            this.getComboxColumnTable(this.model.treeTableName);
          }
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  saveAndContinue() {
    this.save(true);
  }

  save(isContinue: boolean = false) {
    
    if (this.action == 1)//Thêm mới
    {
      this.functionService.createFunctionConfig(this.model).subscribe(
        result => {
          if (result.isStatus) {
            this.messageService.showSuccess('Thêm mới cấu hình chức năng thành công!');
            if (isContinue) {
              this.clear();
            } else {
              this.clear();
            }
            this.search();
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else if (this.action == 2) //Cập nhật
    {
      this.functionService.updateFunctionConfig(this.idUpdate, this.model).subscribe(
        result => {
          if (result.isStatus) {
            this.messageService.showSuccess('Cập nhật cấu hình chức năng thành công!');
            if (isContinue) {
              this.clear();
            } else {
            }
            this.search();
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
  }

  changeTable(event: any) {
    this.model.tableName = event;
    if (this.model.tableName) {
      this.getColumnTable(event);
    } else {
      this.model.functionDesigns = [];
    }
  }

  openTooltip(tooltip:any, greeting: string) {
    tooltip.close();
    tooltip.open({ greeting });
  }

  closeTooltip(tooltip:any) {
    tooltip.close();
  }

  clickHandler(args: ClickEventArgs): void {
    if (args.item.id === 'expandall') {
      this.grid.expandAll();
    }

    if (args.item.id === 'collapseall') {
      this.grid.collapseAll();
    }
  }

  onDrop(event: CdkDragDrop<string[]>, tableName: string) {
    //Xử lý order theo index của loại bảng nào
    if (tableName == "Table") {
      moveItemInArray(this.model.functionDesigns.sort((a:any, b:any) => {
        if (a.columnIndex < b.columnIndex) {
          return -1;
        } else if (a.columnIndex > b.columnIndex) {
          return 1;
        } else {
          return 0;
        }
      }), event.previousIndex, event.currentIndex);
    } else if (tableName == "Search") {
      moveItemInArray(this.model.functionDesigns.sort((a:any, b:any) => {
        if (a.columnIndex < b.columnIndex) {
          return -1;
        } else if (a.columnIndex > b.columnIndex) {
          return 1;
        } else {
          return 0;
        }
      }), event.previousIndex, event.currentIndex);
    }
    else if (tableName == "Filter") {
      moveItemInArray(this.model.functionDesigns.sort((a:any, b:any) => {
        if (a.filterIndex < b.filterIndex) {
          return -1;
        } else if (a.filterIndex > b.filterIndex) {
          return 1;
        } else {
          return 0;
        }
      }), event.previousIndex, event.currentIndex);
    }
    else if (tableName == "Create") {
      moveItemInArray(this.model.functionDesigns.sort((a:any, b:any) => {
        if (a.divCreateIndex < b.divCreateIndex) {
          return -1;
        } else if (a.divCreateIndex > b.divCreateIndex) {
          return 1;
        } else {
          return 0;
        }
      }), event.previousIndex, event.currentIndex);
    }
    else if (tableName == "Edit") {
      moveItemInArray(this.model.functionDesigns.sort((a:any, b:any) => {
        if (a.divEditIndex < b.divEditIndex) {
          return -1;
        } else if (a.divEditIndex > b.divEditIndex) {
          return 1;
        } else {
          return 0;
        }
      }), event.previousIndex, event.currentIndex);
    }
    else if (tableName == "Detail") {
      moveItemInArray(this.model.functionDesigns.sort((a:any, b:any) => {
        if (a.divDetailIndex < b.divDetailIndex) {
          return -1;
        } else if (a.divDetailIndex > b.divDetailIndex) {
          return 1;
        } else {
          return 0;
        }
      }), event.previousIndex, event.currentIndex);
    }

    //Xử lý cập nhật index theo bảng truyền xuống
    this.model.functionDesigns.forEach((item:any, index:number) => {
      if (tableName == "Table") {
        item.columnIndex = index;
        item.filterIndex = index;
        item.divCreateIndex = index;
        item.divEditIndex = index;
        item.divDetailIndex = index;
      } else if (tableName == "Search") {
        item.columnIndex = index;
      }
      else if (tableName == "Filter") {
        item.filterIndex = index;
      }
      else if (tableName == "Create") {
        item.divCreateIndex = index;
      }
      else if (tableName == "Edit") {
        item.divEditIndex = index;
      }
      else if (tableName == "Detail") {
        item.divDetailIndex = index;
      }
    });
  }

  //Sử dụng cấu hình trên chức năng thêm mới cho chỉnh sửa
  applyConfigAddToEdit() {
    this.model.editWindowType = this.model.createWindowType;
    this.model.editWindowWidth = this.model.createWindowWidth;
    this.model.functionDesigns.forEach((item:any, index:number) => {
      item.editDisplay = item.createDisplay;
      item.divEditIndex = item.divCreateIndex;
      item.editRequired = item.createRequired;
      item.editControlType = item.createControlType;
      item.divEditWidth = item.divCreateWidth;
      item.editControlHeight = item.createControlHeight;
    });
  }

  //Sử dụng cấu hình trên chức năng thêm mới cho chi tiết
  applyConfigAddToDetail() {
    this.model.detailWindowType = this.model.createWindowType;
    this.model.detailWindowWidth = this.model.createWindowWidth;
    this.model.functionDesigns.forEach((item:any, index:number) => {
      item.detailDisplay = item.createDisplay;
      item.divDetailIndex = item.divCreateIndex;
      item.detailRequired = item.createRequired;
      if (item.createControlType == 2)//Text area
      {
        item.detailControlType = 2; // Text more
      } else {
        item.detailControlType = 1; // Text
      }
      item.divDetailWidth = item.divCreateWidth;
      item.detailControlHeight = item.createControlHeight;
    });
  }

  //#region Xử lý cấu hình layout tree view
  public mode?: string = 'CheckBox';
  public sportsData: Object[] = [];
  // maps the appropriate column to fields property
  public fields: Object = { text: 'name', value: 'id' };
  // set placeholder to MultiSelect input element
  public placeholder: string = 'Select games';

  changeTreeTableName(event: any) {
    this.model.treeTableName = event;
    this.getComboxColumnTable(this.model.treeTableName);
  }

  getComboxColumnTable(tableName: string) {
    if (tableName) {
      this.functionService.getComboxColumnTable(tableName).subscribe(
        (result: any) => {
          if (result.isStatus) {
            this.sportsData = result.data;
            if (this.model.treeColumnsText)
              this.selectedTreeTextValues = this.model.treeColumnsText.split(';');
          }
        }, error => {
          this.model.functionDesigns = [];
          this.messageService.showError(error);
        }
      );
    } else {
      this.sportsData = [];
    }
  }

  selectedTreeTextValues = [];
  treeColumnsTextOnChange(args: any): void {
    this.selectedTreeTextValues = args.value;
    this.model.treeColumnsText = this.selectedTreeTextValues.join(';');
  }
  //#endregion
}
