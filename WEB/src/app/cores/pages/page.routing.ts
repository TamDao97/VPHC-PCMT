import { Routes } from '@angular/router';

const PageRouting: Routes = [
  {
    path: 'danh-muc',
    loadChildren: () => import('./category/category.module').then((m) => m.CategoryModule),
  },
  {
    path: 'gioi-thieu',
    loadChildren: () => import('./about/about.module').then((m) => m.AboutModule),
  },
  {
    path: 'nguoi-dung',
    loadChildren: () => import('./user/user.module').then((m) => m.UserModule),
  },
  {
    path: 'bieu-mau',
    loadChildren: () => import('./template/template.module').then((m) => m.TemplateModule),
  },
  {
    path: 'he-thong',
    loadChildren: () => import('./system/system.module').then((m) => m.SystemModule),
  }
];

export { PageRouting };
