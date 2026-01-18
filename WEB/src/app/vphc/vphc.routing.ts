import { Routes } from '@angular/router';

const PageVPHCRouting: Routes = [
  {
    path: 'ke-hoach',
    loadChildren: () => import('./ke-hoach/ke-hoach.module').then((m) => m.KeHoachModule),
  },
  {
    path: 'vphc',
    loadChildren: () => import('./vu-viec/vu-viec.module').then((m) => m.VuViecModule),
  },
  {
    path: 'tra-cuu',
    loadChildren: () => import('./tra-cuu/tra-cuu.module').then((m) => m.TraCuuModule),
  },
  {
    path: 'trang-chu',
    loadChildren: () => import('./trang-chu/trang-chu.module').then((m) => m.TrangChuModule),
  },
  {
    path: 'phan-tich',
    loadChildren: () => import('./xu-huong/xu-huong.module').then((m) => m.XuHuongModule),
  },
  {
    path: 'thong-ke',
    loadChildren: () => import('./thong-ke/thong-ke.module').then((m) => m.ThongKeModule),
  }
];

export { PageVPHCRouting };
