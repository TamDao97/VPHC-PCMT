import { SearchItemOptions } from "./search-item-options.model";

export class SearchOptions {

  /*
   * Tên Trường thông tin tìm kiếm ưu tiên   
   */
  FieldContentName: string;

  /*
   * Placeholder thông tin tìm kiếm ưu tiên
   */
  Placeholder: string;

  /*
   * Danh sách điều kiện tìm kiếm
   */
  Items: any[];
}