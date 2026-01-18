import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TreeGridComponent } from '@syncfusion/ej2-angular-treegrid';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { TemplateService } from '../../service/template.service';
import { TemplateCreateComponent } from '../template-create/template-create.component';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { SearchGlobalService } from 'src/app/cores/shared/common/search-global.service';
import { MenuOptions } from 'src/app/cores/shared/models/menu-options.model';

@Component({
  selector: 'app-template-manage',
  templateUrl: './template-manage.component.html',
  styleUrls: ['./template-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TemplateManageComponent implements OnInit {

  @ViewChild('grid')
  public grid!: TreeGridComponent;

  constructor(
    public constant: Constants,
    private modalService: NgbModal,
    private searchGlobalService: SearchGlobalService,
    private translate: TranslateService,
    private lgService: LanguageService,
    private templateService: TemplateService,
  ) {
    this._unsubscribeAll = new Subject();
    this.translate.use(this.lgService.getLanguage());
  }

  _unsubscribeAll: Subject<any>;
  startIndex = 1;
  public searchSettingModel: Object = {};
  public toolbar: string[] = [];
  files: any[] = [];
  userId: string
  height = 0;
  type = 0;

  searchModel: any = {
    pageNumber: 1,
    pageSize: 10,
    totalItems: 0,
    name: '',
    code: '',
    orderBy: '',
    orderType: ''
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
    this.searchGlobalService.onDataChanged.pipe(takeUntil(this._unsubscribeAll)).subscribe(data => {
      if (data) {
        this.searchModel.name = data.name;
        this.searchModel.code = data.code;
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
        Placeholder: 'Tìm kiếm biểu mẫu',
        Items: [         
          {
            FieldName: 'code',
            Name: 'Số biểu mẫu',
            Type: 'text',
            DisplayName: 'code',
            ValueName: 'code',
            Placeholder: 'Số biểu mẫu'
          }
        ]
      }
    };

    this.searchGlobalService.setConfig(meuOptions);
  }
  clickOrderBy(orderBy: any){
    this.searchModel.orderBy = orderBy;
    if(this.searchModel.orderType =="ASC"){
      this.searchModel.orderType = "DESC";
    }else{
      this.searchModel.orderType = "ASC";
    }   
    this.search();
  }
  search() {
    this.templateService.SearchFileTemplate(this.searchModel).subscribe(
      (data: any) => {
        if (data.isStatus) {
          this.startIndex = ((this.searchModel.pageNumber - 1) * this.searchModel.pageSize + 1);
          this.files = data.data.dataResults;
          this.searchModel.totalItems = data.data.totalItems;
        }
      }
    );

  }
  showCreateUpdate(id: string) {
    let activeModal = this.modalService.open(TemplateCreateComponent, { container: 'body', windowClass: 'group-user-create', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    }, (reason) => {
    });
  }
 
}
