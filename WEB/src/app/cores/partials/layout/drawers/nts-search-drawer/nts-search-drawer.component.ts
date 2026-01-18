import { AfterViewInit, Component, OnInit } from '@angular/core';
import { NtsToolbarSearchService } from './nts-search-drawer.service';
import { MenuOptions } from 'src/app/cores/shared/models/menu-options.model';
import { FormControl } from '@angular/forms';
import { NavigationEnd, Router } from '@angular/router';
import { debounceTime } from 'rxjs';

@Component({
  selector: 'app-nts-search-drawer',
  templateUrl: './nts-search-drawer.component.html',
})
export class NtsSearchDrawerComponent implements OnInit {
  constructor(
    private ntsToolbarSearchService: NtsToolbarSearchService,
    private router: Router
  ) { }
  public model: any = {};
  public meuOptions: MenuOptions = {};
  isSearchToolbar: boolean = false;
  isDashboard: boolean = false;
  keywordControl = new FormControl('');
  ngOnInit(): void {
    this.router.events.subscribe((event: any) => {
      if (event instanceof NavigationEnd) {
        this.isSearchToolbar = false;
        this.isDashboard = false;
      }
    });

    this.ntsToolbarSearchService.onChangedConfig.subscribe((data) => {
      this.isSearchToolbar = data ? data.isSearch : false;
      this.isDashboard = data ? data.isDashboard : false;
      if (data != null) {
        this.model = data.searchModel;
        this.meuOptions = data;
        if (this.isDashboard) {
          this.model.namBaoCao = new Date().getFullYear();
          this.model.namSoSanh = new Date().getFullYear() - 1;
        }
      }
    });

    this.ntsToolbarSearchService.onKeywordChanged.subscribe((data) => {
      if (data != null && this.meuOptions.searchOptions != null) {
        this.model[this.meuOptions.searchOptions?.FieldContentName] = data;
        this.onSearch(false);
      }
    });

    //Xử lý tìm kiếm theo từ khóa nhập
    this.keywordControl.valueChanges
      .pipe(debounceTime(500)) // Chờ 500ms sau khi nhập xong
      .subscribe((value) => {
        this.ntsToolbarSearchService.updateKeyword(value);
      });
  }

  public onSearch(close: boolean = true) {
    this.ntsToolbarSearchService.updateSearchModel(this.model);
    if (close) document.getElementById('kt_ntssearch_close')?.click();
  }

  public onRefresh() {
    this.ntsToolbarSearchService.refreshSearchModel({
      pageNumber: 1,
      pageSize: 15,
    });
  }

  onCloseDrawer() {
    // alert("Drawer đang đóng...");
    // Thêm logic xử lý ở đây
  }

  //#region -------Xử lý năm báo cáo trên trang chủ----------------
  public fields = { text: 'name', value: 'id' };
  listNamBC: any[] = [
    { name: '2026', id: 2026 },
    { name: '2025', id: 2025 },
    { name: '2024', id: 2024 },
    { name: '2023', id: 2023 },
    { name: '2022', id: 2022 },
    { name: '2021', id: 2021 },
    { name: '2020', id: 2020 },
  ];
  listNamSS: any[] = [
    { name: '2026', id: 2026 },
    { name: '2025', id: 2025 },
    { name: '2024', id: 2024 },
    { name: '2023', id: 2023 },
    { name: '2022', id: 2022 },
    { name: '2021', id: 2021 },
    { name: '2020', id: 2020 },
  ];
  // public model: any = {
  //   namBaoCao: new Date().getFullYear(),
  //   namSoSanh: new Date().getFullYear() - 1,
  // };
  public onValueChange(event: any) {
    this.ntsToolbarSearchService.updateSearchModel(this.model);
  }
  //#endregion
}
