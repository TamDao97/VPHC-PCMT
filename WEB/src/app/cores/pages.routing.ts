import { Routes } from '@angular/router';

const PagesRouting: Routes = [
  {
    path: 'danh-muc',
    loadChildren: () => import('./pages/category/category.module').then((m) => m.CategoryModule),
  },
  {
    path: 'gioi-thieu',
    loadChildren: () => import('./pages/about/about.module').then((m) => m.AboutModule),
  },
  {
    path: 'he-thong',
    loadChildren: () => import('./pages/user/user.module').then((m) => m.UserModule),
  },
];

export { PagesRouting };
