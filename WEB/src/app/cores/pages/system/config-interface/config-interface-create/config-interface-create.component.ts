import {
  Component,
  ElementRef,
  OnInit,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import {
  RowDDService,
  SelectionService,
  TreeGridComponent,
} from '@syncfusion/ej2-angular-treegrid';
import { Constants } from 'src/app/cores/shared/common/constants';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { ComboboxCoreService } from 'src/app/cores/shared/services/combobox-core.service';
import { FileService } from 'src/app/cores/shared/services/file.service';
import { LanguageService } from 'src/app/cores/shared/services/language.service';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { ConfigInterfaceService } from '../../service/configInterface.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-config-interface-create',
  templateUrl: './config-interface-create.component.html',
  styleUrls: ['./config-interface-create.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [RowDDService, SelectionService],
})
export class ConfigInterfaceCreateComponent implements OnInit {
  constructor(
    private messageService: MessageService,
    public constant: Constants,
    private translate: TranslateService,
    private lgService: LanguageService,
    private comboboxService: ComboboxCoreService,
    public fileProcess: FileProcess,
    private fileService: FileService,
    private configInterfaceService: ConfigInterfaceService
  ) {
    this.translate.use(this.lgService.getLanguage());
  }

  @ViewChild('treegrid')
  public treegrid: TreeGridComponent;
  @ViewChild('fileInputFolder')
  myInputFolderVariable!: ElementRef;
  templatePathFolder: string = '';

  @ViewChild('fileInputIcon')
  myInputVariable!: ElementRef;
  templatePath: string = '';
  fileToUploadLogo: any;
  fileToUploadIcon: any;
  modalInfo = {
    Title: 'Cấu hình hệ thống',
    SaveText: 'Lưu',
  };
  urlLogo = '';
  urlIcon = '';
  isData: boolean = false;
  model: any = {
    softwareName: '',
    isUseMultiLanguage: false,
    isUseCaptcha: false,
    filePathLogo: '',
    filePathIcon: '',
    isShowLogoTopBar: false,
    logo: 'test',
    menuType: 1,
    croppedImage: '',
  };

  ngOnInit(): void {
    this.getConfig();
    //Hứng sự kiện thay đổi ngôn ngữ để load lại Component
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
      }
    });
  }
  getConfig() {
    this.configInterfaceService.getConfig().subscribe(
      (data: any) => {
        if (data.data) {
          this.model = data.data;
          this.urlLogo = environment.apiUrl + this.model.filePathLogo;
          this.urlIcon = environment.apiUrl + this.model.filePathIcon;
          localStorage.setItem('configInterface', JSON.stringify(data.data));
        }
      },
      (error) => {
        this.messageService.showError(error);
      }
    );
  }
  createOrUpdate() {
    this.configInterfaceService.createOrUpdate(this.model).subscribe({
      next: (data: any) => {
        if (this.model.id) {
          this.messageService.showSuccess('Cập nhật thành công');
        } else {
          this.messageService.showSuccess('Thêm mới thành công');
        }
        localStorage.removeItem('configInterface');
        this.imageChangedEvent = '';
        this.getConfig();
      },
      error: (error: any) => {
        this.messageService.showError(error);
      },
    });
  }
  save() {
    if (this.fileToUploadIcon) {
      this.fileService
        .uploadFile(this.fileToUploadIcon, 'ConfigInterface/Icon')
        .subscribe({
          next: (result: any) => {
            this.model.filePathIcon = result.data.fileUrl;
            this.createOrUpdate();
          },
          error: (error: any) => {
            this.messageService.showError(error);
          },
        });
    } else {
      this.createOrUpdate();
    }
  }
  previewIcon: any;
  selectedFile: File;
  handleFileInputIcon($event: any) {
    this.fileProcess.onAFileChange($event);
    this.fileToUploadIcon = this.fileProcess.FileDataBase;
  }
  showLogo() {
    window.open(this.urlLogo, '_blank');
  }
  showIcon() {
    window.open(this.urlIcon, '_blank');
  }

  imageChangedEvent: any = '';
  fileChangeEvent(event: any): void {
    this.imageChangedEvent = event;
  }
  imageCropped(e: any) {
    this.model.croppedImage = e.base64;
  }
}
