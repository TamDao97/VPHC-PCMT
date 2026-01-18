import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-nts-pagination',
  templateUrl: './nts-pagination.component.html',
  styleUrls: ['./nts-pagination.component.scss'],
})
export class NtsPaginationComponent {
  @Input() totalRecords: number = 0; // Tổng số bản ghi
  @Input() pageSize: number = 15; // Kích thước trang
  @Input() pageNumber: number = 1; // Trang hiện tại
  totalPages: any = 1;
  @Input() listPageSize: number[] = [15, 30, 45]; // Danh sách số trang có thể chọn

  @Output() pageChange = new EventEmitter<any>(); // Sự kiện thay đổi trang

  ngOnInit(): void {
    this.totalPages = Math.ceil(this.totalRecords / this.pageSize);
  }

  onPageChange(page: number, requestType: any): void {
    if (requestType === 'paging') {
      {
        if (page) {
          this.pageNumber = page;
          this.pageChange.emit({ requestType: requestType, page: page });
        } else {
          this.pageNumber = 1;
        }
      }
    } else if (requestType === 'pagesize') {
      this.totalPages = Math.ceil(this.totalRecords / this.pageSize);
      this.pageChange.emit({ requestType: requestType, pageSize: page });
      this.pageNumber = 1;
    }
  }
}
