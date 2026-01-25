import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

// import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

@Injectable({
    providedIn: 'root'
})
export class Constants {
    QLNV: number = 1;
    KTNVS: number = 0;
    minDate = { year: 1945, month: 1, day: 1 };

    ScrollConfig: PerfectScrollbarConfigInterface = {
        suppressScrollX: false,
        suppressScrollY: false,
        minScrollbarLength: 20,
        wheelPropagation: true
    };

    ScrollXConfig: PerfectScrollbarConfigInterface = {
        suppressScrollX: false,
        suppressScrollY: true,
        minScrollbarLength: 20,
        wheelPropagation: true
    };
    ScrollYConfig: PerfectScrollbarConfigInterface = {
        suppressScrollX: true,
        suppressScrollY: false,
        minScrollbarLength: 20,
        wheelPropagation: true
    };

    HttpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    FileHttpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'multipart/form-data' })
    };

    StatusCode = {
        Success: 1,
        Error: 2,
        Validate: 3
    };

    statusSearch: any = {
        search: 1,
        excel: 2,
        pdf: 3
    }

    data: any = {
        statusSearch: 1,
        searchData: {}
    }
    SearchDataType = {
        Sample: 1,
        QuestionType: 2,
    };
    Category_Ethnicities = "Ethnicities";
    Category_Religions = "Religions";
    Category_Provinces = "Provinces";
    Category_Province = "Provinces";
    Category_Districts = "Districts";
    Category_Communes = "Communes";
    Category_Occupation = "Occupation";
    // Giới tính
    Gender = [
        { Id: 1, Name: 'Nam', Checked: false },
        { Id: 2, Name: 'Nữ', Checked: false },
    ];
    SearchExpressionTypes: any[] = [
        { Id: 1, Name: '=' },
        { Id: 2, Name: '>' },
        { Id: 3, Name: '>=' },
        { Id: 4, Name: '<' },
        { Id: 5, Name: '<=' }
    ];
    Disable = [
        { Id: true, Name: 'Đang sử dụng', Checked: false, BadgeClass: 'badge-success', },
        { Id: false, Name: 'Không sử dụng', Checked: false, BadgeClass: 'badge-danger', },
    ];

    ListPageSize = [5, 10, 15, 20, 25, 30];
    PageSizeFours = [9, 12, 15, 18, 21, 24];

    validEmailRegEx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    listGioiTinh: any[] = [
        { Id: 1, Name: "Nam", BadgeClass: 'badge-success', },
        { Id: 2, Name: "Nữ", BadgeClass: 'badge-success', },
        { Id: 3, Name: "Khác", BadgeClass: 'badge-success', },
    ];
    DanhMuc: string = "DanhMuc";
    User_Name: string = "admin";
    User_Status = [
        { Id: false, Name: 'Đang hoạt động', Checked: false, BadgeClass: 'badge badge-light-success', },
        { Id: true, Name: 'Không hoạt động', Checked: false, BadgeClass: 'badge badge-light-danger', },
    ];

    UserHistory_Type: any = [
        { Id: 1, Name: 'Đăng nhập', Checked: false },
        { Id: 2, Name: 'Khai thác dữ liệu', Checked: false },
    ];

    b64EncodeUnicode(str: any) {
        return btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g,
            // function toSolidBytes(match, p1) {
            (match, p1) => {
                // console.debug('match: ' + match);
                return String.fromCharCode(("0x" + p1) as any);
            }));
    };

    //Loại control input
    ControlType: any = [
        { Id: 1, Name: 'Text box', Checked: false },
        { Id: 2, Name: 'Text area', Checked: false },
        { Id: 3, Name: 'Select', Checked: false },
        { Id: 4, Name: 'Check box', Checked: false },
        { Id: 5, Name: 'Radio', Checked: false },
        { Id: 6, Name: 'Datetime', Checked: false },
        { Id: 7, Name: 'Input number', Checked: false },
        { Id: 8, Name: 'Select search', Checked: false },
    ];

    //Kiểu mở cửa sổ giao diện
    WindowType: any = [
        { Id: 1, Name: 'Cửa sổ popup', Checked: false },
        { Id: 2, Name: 'Mở trên cửa sổ hiện tại', Checked: false },
        { Id: 3, Name: 'Mở sang tab mới', Checked: false },
        { Id: 4, Name: 'Mở sang cửa sổ mới', Checked: false }
    ];

    //Loại control hiển thị chi tiết
    ControlViewType: any = [
        { Id: 1, Name: 'Text', Checked: false },
        { Id: 2, Name: 'Text more', Checked: false }
    ];

    //Kiểu bố cục giao diện
    LayoutType: any = [
        { Id: 1, Name: 'Danh sách quản lý', Checked: false },
        { Id: 2, Name: 'Cây thư mục và danh sách', Checked: false }
    ];

    VuViec_TienTrinh = [
        { Id: 0, Name: 'Tiếp nhận, phát hiện', Checked: false, BadgeClass: 'badge badge-light-primary', },
        { Id: 1, Name: 'Lập biên bản', Checked: false, BadgeClass: 'badge badge-light-info', },
        { Id: 2, Name: 'Kiểm tra, xác minh', Checked: false, BadgeClass: 'badge badge-light-warning', },
        { Id: 3, Name: 'Xử lý, giải quyết', Checked: false, BadgeClass: 'badge badge-light-danger', },
        { Id: 4, Name: 'Kết thúc', Checked: false, BadgeClass: 'badge badge-light-success' },
    ];

    VuViec_XuLy = [
        { Id: '', Name: 'Đang xử lý', Checked: false, BadgeClass: 'fw-bolder text-success', },
        { Id: '1', Name: 'Xử phạt vi phạm hành chính', Checked: false, BadgeClass: 'fw-bolder text-danger', },
        { Id: '2', Name: 'Chuyển truy cứu trách nhiệm hình sự', Checked: false, BadgeClass: 'fw-bolder text-warning', },
        { Id: '3', Name: 'Áp dụng biện pháp thay thế nhắc nhở đối với người chưa thành niên', Checked: false, BadgeClass: 'fw-bolder text-info', },
        { Id: '4', Name: 'Chuyển cơ quan khác xử lý', Checked: false, BadgeClass: 'fw-bolder text-primary' },
        { Id: '5', Name: 'Không ra quyết định xử phạt vi phạm hành chính', Checked: false, BadgeClass: 'fw-bolder text-dark' },
    ];

    KeHoachKiemTra_QuyTrinh = [
        { Id: 1, Name: 'Soạn thảo', Checked: false, BadgeClass: 'badge badge-light-primary', },
        { Id: 2, Name: 'Đã trình', Checked: false, BadgeClass: 'badge badge-light-info', },
        { Id: 3, Name: 'Ban hành', Checked: false, BadgeClass: 'badge badge-light-warning', },
        // { Id: 3, Name: 'Xử lý, giải quyết', Checked: false, BadgeClass: 'badge badge-light-danger', },
        // { Id: 4, Name: 'Kết thúc', Checked: false, BadgeClass: 'badge badge-light-success' },
    ];
}

export enum TrangThaiKHKTEnum {
    // [Description("Soạn thảo")] 
    SoanThao = 1,
    // [Description("Đã trình")] 
    DaTrinh = 2,
    // [Description("Ban hành")] 
    BanHanh = 3,
}