export class SearchItemOptions {
  /*
  * Tên mô tả trường tìm kiếm
  */
  Name: string;

  /*
  * Tên trường tìm kiếm
  */
  FieldName: string;

  /*
  * Placeholder trường tìm kiếm
  */
  Placeholder: string;
  /*
  * select, text, number, numberYear, ngselect, date
  */
  Type: string;
  /*
  * Dữ liệu hiển thị cho danh sách
  */
  Data: any[];
  /*
  * Tên trường hiển thị trên select
  */
  DisplayName: string;
  /*
  * Giá trị được lấy khi chọn select
  */
  ValueName: string;
  /*
  * Trường từ ngày
  * Thời gian
  */
  FieldNameFrom: string;
  /*
  * Trường từ ngày
  * Thời gian
  */
  FieldNameTo: string;
  /*
  * Trường thông tin có quan hệ
  */
  IsRelation: boolean;
  /*
  * Vị trí trường quan hệ đến
  */
  RelationIndexTo: number;
  /*
  * Vị trí trường quan hệ từ
  */
  RelationIndexFrom: number | null;
  /*
  * Quyền
  */
  Permission: string;
  // /*
  // * Ẩn thông tin tìm kiếm
  // */
  Hidden: boolean = false;
  FieldNameType: string;
  DataType: number | null;
  Columns: any[]
  GetData: any;
  Index: number | null;
}