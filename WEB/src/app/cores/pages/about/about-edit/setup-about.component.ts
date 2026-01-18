import {
  Component,
  ElementRef,
  OnInit,
  ViewEncapsulation,
} from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { AboutService } from '../service/about.service';
import { takeUntil } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { Constants } from '../../../shared/common/constants';
import { FileProcess } from '../../../shared/common/file-process';
import { MessageService } from '../../../shared/services/message.service';
import { FileService } from '../../../shared/services/file.service';
import { LanguageService } from '../../../shared/services/language.service';
// import { AnyMxRecord } from 'dns';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-setup-about',
  templateUrl: './setup-about.component.html',
  styleUrls: ['./setup-about.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class SetupAboutComponent implements OnInit {
  constructor(
    public constant: Constants,
    public fileProcess: FileProcess,
    public fileProcessDataSheet: FileProcess,
    // private fileService: FileService,
    private aboutService: AboutService,
    private messageService: MessageService,
    private translate: TranslateService,
    private lgService: LanguageService
  ) {}

  discoveryConfig: any = {};

  setConfig(language: string) {
    this.discoveryConfig = {
      plugins: [
        'image code',
        'visualblocks',
        'print preview',
        'table',
        'directionality',
        'link',
        'media',
        'codesample',
        'table',
        'charmap',
        'hr',
        'pagebreak',
        'nonbreaking',
        'anchor',
        'toc',
        'insertdatetime',
        'advlist',
        'lists',
        'textcolor',
        'wordcount',
        'imagetools',
        'contextmenu',
        'textpattern',
        'searchreplace visualblocks code fullscreen',
      ],
      language: language,
      // file_picker_types: 'file image media',
      automatic_uploads: true,
      toolbar:
        'undo redo | fontselect | fontsizeselect | bold italic forecolor backcolor |alignleft aligncenter alignright alignjustify alignnone | numlist | table | link | outdent indent',
      convert_urls: false,
      height: window.innerHeight - 350,
      auto_focus: false,
      plugin_preview_width: 1000,
      plugin_preview_height: 650,
      readonly: 0,
      content_style: 'body {font-size: 12pt;font-family: Arial;}',
      aligncenter: {
        selector: 'media',
        classes: 'center',
        styles: { display: 'block', margin: '0px auto', textAlign: 'center' },
      },
      // file_browser_callback: function RoxyFileBrowser(field_name, url, type, win) {
      //   //var roxyFileman = '/fileman/index.html';
      //   var roxyFileman = "https://nhantinsoft.vn:9566/fileServer/fileman/index.html";
      //   if (roxyFileman.indexOf("?") < 0) {
      //     roxyFileman += "?type=" + type;
      //   }
      //   else {
      //     roxyFileman += "&type=" + type;
      //   }
      //   roxyFileman += '&input=' + field_name + '&value=' + win.document.getElementById(field_name).value;
      //   if (tinymce.activeEditor.settings.language) {
      //     roxyFileman += '&langCode=' + tinymce.activeEditor.settings.language;
      //   }
      //   tinymce.activeEditor.windowManager.open({
      //     file: roxyFileman,
      //     title: 'Roxy Fileman',
      //     width: 850,
      //     height: 650,
      //     resizable: "yes",
      //     plugins: "media",
      //     inline: "yes",
      //     close_previous: "no"
      //   }, {
      //     window: win,
      //     input: field_name
      //   });
      //   return false;
      // },
      //setup: TinymceUserConfig.setup,
      // content_css: '/assets/css/custom_editor.css',
      images_upload_handler: (blobInfo: any, success: any, failure: any) => {
        // this.fileService.uploadFile(blobInfo.blob(), 'About').subscribe({
        //   next: (result: any) => {
        //     success(environment.apiUrl + result.data.fileUrl);
        //   },
        //   error: (error: any) => {
        //     return;
        //   },
        // });
      },
    };
  }

  height: number;
  model: any = {
    content: '',
  };

  ngOnInit(): void {
    this.setConfig('vi_VN');
    this.lgService.onLanguageChanged.pipe().subscribe((languageCode) => {
      if (languageCode) {
        this.translate.use(languageCode);
        this.setConfig(languageCode + '_' + languageCode.toUpperCase());
      }
    });

    this.getAbout();
  }

  getAbout() {
    this.aboutService.getAbout().subscribe({
      next: (data: any) => {
        if (data.isStatus) {
          this.model = data.data;
        }
      },
      error: (error: any) => {
        this.messageService.showError(error);
      },
    });
  }

  create() {
    this.aboutService.create(this.model).subscribe({
      next: (result: any) => {
        if (result.isStatus) {
          this.messageService.showSuccess('Chỉnh sửa giới thiệu thành công!');
        }
      },
      error: (error: any) => {
        this.messageService.showError(error);
      },
    });
  }
}
