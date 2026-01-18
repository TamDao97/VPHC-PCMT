import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TemplateService } from '../../service/template.service';
import { Constants } from 'src/app/cores/shared/common/constants';
import { MessageService } from 'src/app/cores/shared/services/message.service';
import { FileProcess } from 'src/app/cores/shared/common/file-process';
import { FileService } from 'src/app/cores/shared/services/file.service';

@Component({
  selector: 'app-template-create',
  templateUrl: './template-create.component.html',
  styleUrls: ['./template-create.component.scss']
})
export class TemplateCreateComponent implements OnInit {

  constructor(
    public constant: Constants,
    private templateService: TemplateService,
    private messageService: MessageService,
    private fileProcess: FileProcess,    
    private fileService: FileService,
    private activeModal: NgbActiveModal,
  ) { }

  id: any;
  model = {
    name: '',
    description: '',
    code: '',
    updatedAt: '',
    path: '',
  }
  modeldownload: any = {

  };
  isAction: boolean = false;
  modalInfo = {
    Title: '',
  };

  ngOnInit(): void {
    if(this.id){
      this.modalInfo.Title = "Cập nhật biểu mẫu";
      this.getTemplateById();     
    }else{      
      this.modalInfo.Title = "Tạo biểu mẫu";
      this.model.updatedAt = new Date().toDateString();
    }
  }
  onFileSelected(event:any): void {
    this.fileProcess.FileDataBase = event.target.files[0];   
  } 

  save() {
    if(!this.id && !this.fileProcess.FileDataBase){
      this.create();    
    }else if(this.id && !this.fileProcess.FileDataBase) {
      this.update();
    }
    else if(this.fileProcess.FileDataBase) {
      this.fileService.uploadFile(this.fileProcess.FileDataBase, 'Template/Download').subscribe(
        result => {
          this.model.path = result.data.fileUrl;    
          if(!this.id){
            this.create();
          }else{
            this.update();
          }
        },
        error => {
          this.messageService.showError(error);
        });
     
    }
    
  }
  getTemplateById(){
    this.templateService.getFileTemplateById(this.id).subscribe(
      (data: any) => {
        this.model = data.data;
        this.modeldownload.pathFile = data.data.pathFile;
        this.model.path = data.data.pathFile;
      }, error => {
        this.messageService.showError(error);
      });
  }



  create(){
    this.templateService.createFileTemplate(this.model).subscribe(
      (data: any) => {
        this.messageService.showMessage("Thêm mới thành công");
        this.activeModal.close(true);
      },
      error => {
        this.messageService.showError(error);
      });
    
  }

  update(){
    this.templateService.updateFileTemplate(this.id,this.model).subscribe(
      (data: any) => {
        this.messageService.showMessage("Cập nhật thành công");
        this.activeModal.close(true);
      }, error => {
        this.messageService.showError(error);
      });
  }
  download(){
    this.fileService.downloadFile(this.modeldownload).subscribe(data => {
      var blob = new Blob([data], { type: 'octet/stream' });
      var url = window.URL.createObjectURL(blob);
      this.fileProcess.downloadFileLink(url, 'filedownload.docx');
    }, error => {
      const blb = new Blob([error.error], { type: "text/plain" });
      const reader = new FileReader();

      reader.onload = () => {
        this.messageService.showMessage((reader.result?.toString()??"").replace('"', '').replace('"', ''));
      };
      // Start reading the blob as text.
      reader.readAsText(blb);
    });
  }
  close(){
    this.activeModal.close(true);
  }
}
