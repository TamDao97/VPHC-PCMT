import {
  ChangeDetectorRef,
  Component,
  OnInit,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from '../../common/constants';
import { FileProcess } from '../../common/file-process';
import { FileService } from '../../services/file.service';
import { MessageService } from '../../services/message.service';
import { NtsViewFileService } from '../../services/nts-view-file.service';
import { ComboboxCoreService } from '../../services/combobox-core.service';

@Component({
  selector: 'app-nts-view-file',
  templateUrl: './nts-view-file.component.html',
  styleUrls: ['./nts-view-file.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class NtsViewFileComponent implements OnInit {
  @ViewChild('pdfViewer', { static: false }) public pdfViewer: any;
  pdfSource: any = '';
  pdfSrc: string | undefined;
  safePdfUrl: SafeResourceUrl | undefined;
  images: any[] = [];
  isImg = false;
  id: any;
  fileContentResult: any;
  pathFile: string;
  nameFile: string;

  searchTemplateModel: any = {
    id: 0,
    totalItems: 0,
    name: '',
  };

  startIndex: number = 1;
  selectTemplateIndex: number = 0;
  height: number = 0;

  fileViewIndex: number = -1;
  fileViewIndexView: number = 0;

  constructor(
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private comboboxService: ComboboxCoreService,
    public messageService: MessageService,
    public fileProcess: FileProcess,
    private sanitizer: DomSanitizer,
    private fileService: FileService,
    private ntsViewFileService: NtsViewFileService,
    private changeDetectorRef: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.height = window.innerHeight - 250;

    
    if (this.pathFile) {
      this.viewFile(true, this.pathFile);
    } else {
      this.viewBlobFile(true, this.fileContentResult);
    }
  }

  viewFile(isNext: boolean, pathFile: string) {
    if (isNext) {
      this.fileViewIndex = this.fileViewIndex + 1;
    } else if (!isNext) {
      this.fileViewIndex = this.fileViewIndex - 1;
    }

    this.ntsViewFileService.getFile(pathFile).subscribe(
      (data) => {
        if (isNext) {
          this.fileViewIndexView = this.fileViewIndexView + 1;
        } else if (!isNext) {
          this.fileViewIndexView = this.fileViewIndexView - 1;
        }

        if (data.type.includes('pdf')) {
          this.isImg = false;
          const pdfBlob = new Blob([data], { type: 'application/pdf' });
          this.pdfSrc = URL.createObjectURL(pdfBlob);
          this.safePdfUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
            this.pdfSrc
          );
        } else if (data.type.includes('image')) {
          this.images = [];
          this.isImg = true;
          const blob = new Blob([data], { type: 'image/png' });
          var unsafeImg = window.URL.createObjectURL(blob);
          let img = this.sanitizer.bypassSecurityTrustUrl(unsafeImg);
          this.images.push(img);
        } else {
          this.downloadFile(pathFile, this.nameFile);
        }
      },
      (error) => {
        const blb = new Blob([error.error], { type: 'text/plain' });
        const reader = new FileReader();

        reader.onload = () => {
          this.messageService.showMessage(
            (reader.result?.toString() ?? '').replace('"', '').replace('"', '')
          );
        };
        // Start reading the blob as text.
        reader.readAsText(blb);
      }
    );
  }

  downloadFile(pathFile: string, nameFile: string) {
    let fileDowload = {
      pathFile: pathFile,
      nameFile: nameFile,
    };

    this.fileService.downloadFile(fileDowload).subscribe(
      (data) => {
        var blob = new Blob([data], { type: 'octet/stream' });
        var url = window.URL.createObjectURL(blob);
        this.fileProcess.downloadFileLink(url, nameFile);
      },
      (error) => {
        const blb = new Blob([error.error], { type: 'text/plain' });
        const reader = new FileReader();

        reader.onload = () => {
          this.messageService.showMessage(
            (reader.result?.toString() ?? '').replace('"', '').replace('"', '')
          );
        };
        // Start reading the blob as text.
        reader.readAsText(blb);
      }
    );
  }

  viewBlobFile(isNext: boolean, fileContent: any) {
    if (isNext) {
      this.fileViewIndex = this.fileViewIndex + 1;
    } else if (!isNext) {
      this.fileViewIndex = this.fileViewIndex - 1;
    }
    if (isNext) {
      this.fileViewIndexView = this.fileViewIndexView + 1;
    } else if (!isNext) {
      this.fileViewIndexView = this.fileViewIndexView - 1;
    }

    if (fileContent.type.includes('pdf')) {
      const pdfBlob = new Blob([fileContent], { type: 'application/pdf' });
      this.pdfSrc = URL.createObjectURL(pdfBlob);
      this.safePdfUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
        this.pdfSrc
      );
    } else if (fileContent.type.includes('image')) {
      this.images = [];
      this.isImg = true;
      const blob = new Blob([fileContent], { type: 'image/png' });
      var unsafeImg = window.URL.createObjectURL(blob);
      let img = this.sanitizer.bypassSecurityTrustUrl(unsafeImg);
      this.images.push(img);
    } else {
      this.downloadBlobFile(fileContent, this.nameFile);
    }
  }

  downloadBlobFile(fileContent: any, nameFile: string) {
    var blob = new Blob([fileContent], { type: 'octet/stream' });
    var url = window.URL.createObjectURL(blob);
    this.fileProcess.downloadFileLink(url, nameFile);
  }

  closeModal() {
    this.activeModal.close(false);
  }
}
