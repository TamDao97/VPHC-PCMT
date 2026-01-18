import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { L10n, registerLicense, setCulture } from '@syncfusion/ej2-base';

// Registering Syncfusion license key
registerLicense(
  'Ngo9BigBOggjHTQxAR8/V1NHaF5cWWdCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXpfdHRRR2NcV0Z/XUo='
);

if (environment.production) {
  enableProdMode();
}

L10n.load({
  'en': {
    grid: {
      EmptyRecord: 'Không có dữ liệu!',
    },
  },
});

 setCulture('en');

platformBrowserDynamic()
  .bootstrapModule(AppModule)
  .catch((err) => console.error(err));
