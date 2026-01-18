import { Menu } from "./menu.model";
import { SearchOptions } from "./search-options.model";

export class MenuOptions {
  /*
    * Xuất excel
    * True: Có
    * False: Không
    */
  isExcel?: boolean;

  /*
   * Xuất PDF
   * True: Có
   * False: Không
   */
  isPDF?: boolean;

  /*
   * Tìm kiếm theo ngày
   * True: Có
   * False: Không
   */
  isSearchDate?: boolean;

  /*
   * Danh sách menu
   */
  menus?: Menu[];

  /*
   * Danh sách menu mở rộng
   */
  menuMores?: Menu[];

  /*
   * Tìm kiếm
   * True: Có
   * False: Không
   */
  isSearch?: boolean;

  /*
   * Model tìm kiếm
   */
  searchModel?: any;

  /*
   * Cấu hình điều kiện tìm kiếm
   */
  searchOptions?: SearchOptions;

  /**
   * Giao diện Dashboard
   */
  isDashboard?: boolean;
}