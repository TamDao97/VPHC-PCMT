import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { ImportExcelComponent } from '../component/import-excel/import-excel.component';
import { NTSModalService } from './ntsmodal.service';

@Injectable({
    providedIn: 'root'
})
export class ComponentService {

    constructor(private modalService: NgbModal, private ntsModalService: NTSModalService,
        public router: Router) {

    }

    showImportExcel(templatePath: string, isData:any): Observable<any> {
        var importExcel = new Observable(observer => {
            const activeModal = this.modalService.open(ImportExcelComponent, { container: 'body', backdrop: 'static' });
            activeModal.componentInstance.templatePath = templatePath;
            activeModal.componentInstance.isData = isData;
            activeModal.result.then((result) => {
                observer.next(result);
                observer.unsubscribe();
            }, (reason) => {
                observer.next(false);
                observer.unsubscribe();
            });
        });

        return importExcel;
    }
}
