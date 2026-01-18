import { Component, ElementRef, OnInit, Renderer2, ViewChild, ViewEncapsulation } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NgbModal, NgbPopover } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { FunctionAutoCreateComponent } from '../function-auto-create/function-auto-create.component';
import { ActivatedRoute, Router } from '@angular/router';
import { FunctionAutoEditComponent } from '../function-auto-edit/function-auto-edit.component';
import { FunctionAutoDetailComponent } from '../function-auto-detail/function-auto-detail.component';
import { FunctionAutoService } from '../../../service/function-auto.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { DateUtils } from 'src/app/cores/shared/common/date-utils';
import { SearchGlobalService } from 'src/app/cores/shared/common/search-global.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { MenuOptions } from 'src/app/cores/shared/models/menu-options.model';
import { ContextMenuComponent, MenuEventArgs, MenuItemModel, NodeClickEventArgs, NodeSelectEventArgs, TreeViewComponent } from '@syncfusion/ej2-angular-navigations';

@Component({
  selector: 'app-function-auto-manage',
  templateUrl: './function-auto-manage.component.html',
  styleUrls: ['./function-auto-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FunctionAutoManageComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private router: Router,
    private modalService: NgbModal,
    private route: ActivatedRoute,
    private service: FunctionAutoService,
    public constant: Constants,
    private searchGlobalService: SearchGlobalService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private dateUtils: DateUtils,
    private elementRef: ElementRef,
    private renderer: Renderer2
  ) {
    this._unsubscribeAll = new Subject();
    this.translate.use(this.lgService.getLanguage());
  }

  slug: string;

  _unsubscribeAll: Subject<any>;
  searchModel: any = {
    pageSize: 10,
    pageNumber: 1
  }
  modelDateView: any = {}

  modelDesign: any = {}
  modelDesignTree: any = {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.slug = this.route.snapshot.paramMap.get('slug')??"";

      this.getFunctionDesign();

      this.searchGlobalService.onDataChanged.pipe(takeUntil(this._unsubscribeAll)).subscribe(data => {
        if (data) {
          this.modelDesign.filterDesigns.forEach((item:any) => {
            this.searchModel[item.columnName] = !data[item.columnName] ? null : data[item.columnName];
            if (item.filterControlType == 6) {
              this.modelDateView[item.columnName + "FromV"] = data[item.columnName + "FromV"];
              this.modelDateView[item.columnName + "ToV"] = data[item.columnName + "ToV"];
            }
          });
          this.searchModel['searchKeyword'] = !data['searchKeyword'] ? null : data['searchKeyword'];

          this.search();
        }
      });

      this.searchGlobalService.onRefreshData.pipe(takeUntil(this._unsubscribeAll)).subscribe(data => {
        if (data) {
          this.clearData();
        }
      });
    });
  }

  //Lấy thông tin cấu hình giao diện để hiển thị
  getFunctionDesign() {
    this.service.getConfigDesign(this.slug).subscribe(
      result => {
        if (result.isStatus) {
          this.modelDesign = result.data;
          this.setToolbarConfig();

          //Trường hợp layout treeview
          if (this.modelDesign.layoutType == 2) {
            //Loại bỏ tìm kiếm theo danh mục tree vì đã có cây thu mục bên ngoài
            this.modelDesign.filterDesigns = this.modelDesign.filterDesigns.filter((s:any) => s.linkTable != this.modelDesign.treeTableName);
            this.setToolbarConfig();

            this.getTreeData();
            this.getFunctionDesignTree(this.modelDesign.treeFunctionConfigId);
          }
        }
      },
      error => {
        this.messageService.showError(error);
      });
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
      isSearch: this.modelDesign.searchDisplay,
      searchModel: this.searchModel,
      searchOptions: {
        FieldContentName: 'searchKeyword',
        Placeholder: 'Tìm kiếm theo từ khóa',
        Items: []
      }
    };

    this.modelDesign.filterDesigns.forEach((itemFilter:any) => {
      if (itemFilter.filterControlType == 8) { //Select search
        meuOptions.searchOptions?.Items.push({
          FieldName: itemFilter.columnName,
          Name: itemFilter.displayName,
          Type: 'ngselect',
          DisplayName: "name",
          ValueName: "id",
          Data: itemFilter.linkData
        });
      }
      else if (itemFilter.filterControlType == 6) { //Select search
        meuOptions.searchOptions?.Items.push({
          Name: itemFilter.displayName,
          FieldNameTo: itemFilter.columnName + 'ToV',
          FieldNameFrom: itemFilter.columnName + 'FromV',
          Type: 'date',
        });
      }
      else {
        meuOptions.searchOptions?.Items.push({
          FieldName: itemFilter.columnName,
          Name: itemFilter.displayName,
          Type: 'text',
          Placeholder: ''
        });
      }
    });

    this.searchGlobalService.setConfig(meuOptions);
  }

  dataSearch: any[] = [];
  totalItems: any = 0;
  startIndex = 0;
  search() {
    this.modelDesign.filterDesigns.forEach((item: any) => {
      if (item.filterControlType == 6) {
        this.searchModel[item.columnName] = null;

        if (this.modelDateView[item.columnName + 'FromV'] != null) {
          this.searchModel[item.columnName] = this.dateUtils.convertObjectToDate(this.modelDateView[item.columnName + 'FromV']);
          this.searchModel[item.columnName + 'From'] = this.dateUtils.convertObjectToDate(this.modelDateView[item.columnName + 'FromV']);
        } else {
          this.searchModel[item.columnName + 'From'] = null;
        }

        if (this.modelDateView[item.columnName + 'ToV'] != null) {
          this.searchModel[item.columnName] = this.dateUtils.convertObjectToDate(this.modelDateView[item.columnName + 'ToV']);
          this.searchModel[item.columnName + 'To'] = this.dateUtils.convertObjectToDate(this.modelDateView[item.columnName + 'ToV']);
        } else {
          this.searchModel[item.columnName + 'To'] = null;
        }
      }
    });

    //Trường hợp layout treeview
    if (this.modelDesign.layoutType == 2) {
      this.setDataSearchByTreeviewCheck(this.checkedNodes);
    }

    this.service.search(this.slug, this.searchModel).subscribe((data: any) => {
      if (data.isStatus) {
        this.startIndex = ((this.searchModel.pageNumber - 1) * this.searchModel.pageSize + 1);
        this.dataSearch = data.data.dataResults;
        this.totalItems = data.data.totalItems;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  clearData() {
    this.searchModel = {
      pageSize: 10,
      pageNumber: 1
    }
    totalItems: 0;
    this.setToolbarConfig();
    this.search();
  }

  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá " + this.modelDesign.functionName + " này không?").then(
      data => {
        this.delete(Id);
      }
    );
  }

  delete(Id: string) {
    this.service.delete(this.slug, Id).subscribe(
      data => {
        if (data.isStatus) {
          this.messageService.showSuccess('Xóa ' + this.modelDesign.functionName + ' thành công!');
          this.search();
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  //Show chức năng thêm mới
  showCreate() {
    //Nếu là giao diện treeview và chưa chọn this.modelDesign.layoutType == 2
    if(this.modelDesign.layoutType == 2 && !this.selectedNode)
    {
      this.messageService.showWarning("Chưa chọn " + this.modelDesign.treeName);
      return;
    }

    if (this.modelDesign.createWindowType == 2) {
      //Mở trên page hiện tại
      this.router.navigate(['/system-config/function/create/' + this.slug]);
    }
    else if (this.modelDesign.createWindowType == 3) {
      //Mở sang tab mới
      window.open(this.router.createUrlTree(['/system-config/function/create/' + this.slug]).toString(), '_blank');
    }
    else if (this.modelDesign.createWindowType == 4) {
      //Mở trong cửa sổ mới
      const windowFeatures = 'width=' + window.innerWidth + ',height=' + window.innerHeight + ',left=0,top=0';
      window.open('/system-config/function/create/' + this.slug, '_blank', windowFeatures);
    }
    else {
      //Còn lại  mở popup
      let activeModal = this.modalService.open(FunctionAutoCreateComponent, { container: 'body', size: " " + this.modelDesign.createWindowWidth, windowClass: "app-function-auto-create", backdrop: 'static' })
      activeModal.componentInstance.slug = this.slug;
      activeModal.componentInstance.selectedNode = this.selectedNode;
      activeModal.componentInstance.modelManageDesign = this.modelDesign;
      activeModal.result.then((result) => {
        if (result) {
          this.search();
        }
      }, (reason) => {
      });
    }
  }

  //Show chức năng cập nhật
  showUpdate(id: string) {
    if (this.modelDesign.editWindowType == 2) {
      //Mở trên page hiện tại
      this.router.navigate(['/system-config/function/update/' + this.slug + "/" + id]);
    }
    else if (this.modelDesign.editWindowType == 3) {
      //Mở sang tab mới
      window.open(this.router.createUrlTree(['/system-config/function/update/' + this.slug + "/" + id]).toString(), '_blank');
    }
    else if (this.modelDesign.editWindowType == 4) {
      //Mở trong cửa sổ mới
      const windowFeatures = 'width=' + window.innerWidth + ',height=' + window.innerHeight + ',left=0,top=0';
      window.open('/system-config/function/update/' + this.slug + "/" + id, '_blank', windowFeatures);
    }
    else {
      //Còn lại  mở popup
      let activeModal = this.modalService.open(FunctionAutoEditComponent, { container: 'body', size: " " + this.modelDesign.createWindowWidth, windowClass: "app-function-auto-edit", backdrop: 'static' })
      activeModal.componentInstance.id = id;
      activeModal.componentInstance.slug = this.slug;
      activeModal.componentInstance.selectedNode = this.selectedNode;
      activeModal.componentInstance.modelManageDesign = this.modelDesign;
      activeModal.result.then((result) => {
        if (result) {
          this.search();
        }
      }, (reason) => {
      });
    }
  }

  //Show chức năng xem chi tiết
  showDetail(id: string) {
    if (this.modelDesign.detailWindowType == 2) {
      //Mở trên page hiện tại
      this.router.navigate(['/system-config/function/detail/' + this.slug + "/" + id]);
    }
    else if (this.modelDesign.detailWindowType == 3) {
      //Mở sang tab mới
      window.open(this.router.createUrlTree(['/system-config/function/detail/' + this.slug + "/" + id]).toString(), '_blank');
    }
    else if (this.modelDesign.detailWindowType == 4) {
      //Mở trong cửa sổ mới
      const windowFeatures = 'width=' + window.innerWidth + ',height=' + window.innerHeight + ',left=0,top=0';
      window.open('/system-config/function/detail/' + this.slug + "/" + id, '_blank', windowFeatures);
    }
    else {
      //Còn lại  mở popup
      let activeModal = this.modalService.open(FunctionAutoDetailComponent, { container: 'body', size: " " + this.modelDesign.createWindowWidth, windowClass: "app-function-auto-detail", backdrop: 'static' })
      activeModal.componentInstance.id = id;
      activeModal.componentInstance.slug = this.slug;
      activeModal.componentInstance.modelManageDesign = this.modelDesign;
    }
  }

  //#region  Start xử lý treeview bên trái
  @ViewChild('treeview')
  public tree?: TreeViewComponent;
  public dataTreeView: Object[] = [];

  //Lấy thông tin cấu hình giao diện quản lý cây thư mục
  getFunctionDesignTree(id: string) {
    this.service.getConfigDesign(id).subscribe({
    next:(result:any)   => {
        if (result.isStatus) {
          this.modelDesignTree = result.data;
        }
      },
      error: (error:any) => {
        this.messageService.showError(error);
      }});
  }

  //Lấy thông tin cấu hình giao diện để hiển thị
  getTreeData() {
    this.service.getTreeData(this.slug).subscribe(
      result => {
        if (result.isStatus) {
          this.dataTreeView = result.data;
          this.field = { dataSource: this.dataTreeView, id: 'id', parentID: 'parentId', text: 'text', hasChildren: 'hasChildren' };
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  selectedNode: any = null;
  public nodeSelected(e: NodeSelectEventArgs) {
    this.selectedNode = e.nodeData;
  };

  public field: Object = {};
  // set the CheckBox to the TreeView
  public showCheckBox: boolean = true;
  //set the checknodes to the TreeView
  public checkedNodes: string[] = [];
  public nodeChecked(args: any): void {
    this.checkedNodes = this.tree?.checkedNodes??[];
    this.setDataSearchByTreeviewCheck(this.checkedNodes);
    this.search();
  }

  private setDataSearchByTreeviewCheck(checkedNodeIds: string[]) {
    //Xử lý gán các node được check vào model tìm kiếm
    this.modelDesign.columnDesigns.forEach((itemDesign:any) => {
      if (this.modelDesignTree.tableName && itemDesign.linkTable == this.modelDesignTree.tableName) {
        this.searchModel[itemDesign.columnName] = checkedNodeIds;
      }
    });
  }

  @ViewChild('contentmenutree') contentmenutree?: ContextMenuComponent;

  public nodeclicked(args: NodeClickEventArgs) {
    if (args.event.which === 3) {
      (this.tree as TreeViewComponent).selectedNodes = [args.node.getAttribute('data-uid') as string];
    }
  }

  //Render the context menu with target as Treeview
  public menuItems: MenuItemModel[] = [
    { id: 'Add', text: 'Thêm mới', iconCss: 'fas fa-plus' },
    { id: 'Edit', text: 'Chỉnh sửa', iconCss: 'fas fa-edit' },
    { id: 'Delete', text: 'Xóa', iconCss: 'fas fa-times' }
  ];

  public index: number = 1;
  public menuclick(args: MenuEventArgs) {
    

    let targetNodeId: string = this.tree?.selectedNodes[0] as string;
    if (args.item.id == "Add") {
      //let nodeId: string = "tree_" + this.index;
      //let item: { [key: string]: Object } = { id: nodeId, name: "New Folder" };
      //this.tree?.addNodes([item], targetNodeId, null as any);
      //this.index++;
      //this.countries.push(item);
      //this.tree?.beginEdit(nodeId);
      this.showAddItemTree("", this.selectedNode);
    }
    else if (args.item.id == "Delete") {
      this.messageService.showConfirm("Bạn có chắc muốn xoá " + this.modelDesignTree.functionName + " này không?").then(
        data => {
          this.service.delete(this.modelDesignTree.slug, targetNodeId).subscribe(
            data => {
              if (data.isStatus) {
                this.messageService.showSuccess('Xóa ' + this.modelDesignTree.functionName + ' thành công!');
                this.tree?.removeNodes([targetNodeId]);
              }
            },
            error => {
              this.messageService.showError(error);
            });
        }
      );
    }
    else if (args.item.id == "Edit") {
      //this.tree?.beginEdit(targetNodeId);
      this.showUpdateItemTree(targetNodeId, this.selectedNode);
    }
  }

  showAddItemTree(id: string, treeSelectedNode: any) {
    let activeModal = this.modalService.open(FunctionAutoCreateComponent, { container: 'body', size: " " + this.modelDesign.createWindowWidth, windowClass: "app-function-auto-create", backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.selectedNode = treeSelectedNode;
    activeModal.componentInstance.modelManageDesign = this.modelDesign;
    activeModal.componentInstance.slug = this.modelDesignTree.slug;
    activeModal.result.then((result) => {
      if (result) {
        this.getTreeData();
      }
    }, (reason) => {
    });
  }

  showUpdateItemTree(id: string, treeSelectedNode: any) {
    let activeModal = this.modalService.open(FunctionAutoEditComponent, { container: 'body', size: " " + this.modelDesign.createWindowWidth, windowClass: "app-function-auto-create", backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.selectedNode = treeSelectedNode;
    activeModal.componentInstance.modelManageDesign = this.modelDesign;
    activeModal.componentInstance.slug = this.modelDesignTree.slug;
    activeModal.result.then((result) => {
      if (result) {
        this.getTreeData();
      }
    }, (reason) => {
    });
  }
  //#endregion End xử lý treeview
}
