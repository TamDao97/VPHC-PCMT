import { Component, HostBinding, OnInit } from '@angular/core';
import { LayoutService } from '../../../../../layout';

export type NotificationsTabsType =
  | 'kt_topbar_notifications_1'
  | 'kt_topbar_notifications_2'
  | 'kt_topbar_notifications_3';

@Component({
  selector: 'app-notifications-inner',
  templateUrl: './notifications-inner.component.html',
})
export class NotificationsInnerComponent implements OnInit {
  @HostBinding('class') class =
    'menu menu-sub menu-sub-dropdown menu-column w-350px w-lg-375px';
  @HostBinding('attr.data-kt-menu') dataKtMenu = 'true';

  activeTabId: NotificationsTabsType = 'kt_topbar_notifications_1';
  alerts: Array<AlertModel> = defaultAlerts;
  logs: Array<LogModel> = defaultLogs;
  constructor() {}

  ngOnInit(): void {}

  setActiveTabId(tabId: NotificationsTabsType) {
    this.activeTabId = tabId;
  }
}

interface AlertModel {
  title: string;
  description: string;
  time: string;
  icon: string;
  state: 'primary' | 'danger' | 'warning' | 'success' | 'info';
}

const defaultAlerts: Array<AlertModel> = [
  {
    title: 'BĐBP tỉnh Quảng Ninh',
    description: 'Thu giữ: 42,686 Gram Ma túy tổng hợp; 21 Cái Điện thoại di động; 12 kg Chất gây nghiện...',
    time: '1 giờ',
    icon: 'icons/duotune/technology/teh008.svg',
    state: 'primary',
  },
  {
    title: 'BĐBP tỉnh Thanh Hóa',
    description: 'Thu giữ: 27.000 kg Đường cát, kính; 15.000 Bao Thuốc lá điếu; 13.500 Con Gia cầm giống...',
    time: '2 giờ',
    icon: 'icons/duotune/general/gen044.svg',
    state: 'danger',
  },
  {
    title: 'BĐBP tỉnh Kiên Giang',
    description: 'Thu giữ: 11,6 kg Pháo nổ; 22 Viên Đạn; 12 Khẩu Súng tự tạo; 66,02 kg Thuốc nổ...',
    time: '5 giờ',
    icon: 'icons/duotune/finance/fin006.svg',
    state: 'warning',
  },
  {
    title: 'BĐBP tỉnh Quảng Bình',
    description: 'Thu giữ: 42,686 Gram Ma túy tổng hợp; 21 Cái Điện thoại di động; 12 kg Chất gây nghiện...',
    time: '5 giờ',
    icon: 'icons/duotune/technology/teh008.svg',
    state: 'primary',
  },
  {
    title: 'BĐBP tỉnh Lào Cai',
    description: 'Thu giữ: 27.000 kg Đường cát, kính; 15.000 Bao Thuốc lá điếu; 13.500 Con Gia cầm giống...',
    time: '7 giờ',
    icon: 'icons/duotune/general/gen044.svg',
    state: 'danger',
  },
  {
    title: 'BĐBP tỉnh Cà Mau',
    description: 'Thu giữ: 11,6 kg Pháo nổ; 22 Viên Đạn; 12 Khẩu Súng tự tạo; 66,02 kg Thuốc nổ...',
    time: '8 giờ',
    icon: 'icons/duotune/finance/fin006.svg',
    state: 'warning',
  }
];

interface LogModel {
  code: string;
  state: 'success' | 'danger' | 'warning';
  message: string;
  time: string;
}

const defaultLogs: Array<LogModel> = [
  { code: 'Thông báo', state: 'success', message: 'Bạn đã xử lý xong vụ 093740316', time: '1 giờ' },
  { code: 'Nhắc nhở', state: 'danger', message: 'Đã có 2 vụ việc quá thời gian xử lý', time: '2 giờ' },
  {
    code: 'Cảnh báo',
    state: 'warning',
    message: 'Bạn nên hoàn thành xử lý các vụ việc: 093740316, 115015564',
    time: '2 ngày',
  }
];
