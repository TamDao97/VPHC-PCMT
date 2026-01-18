import { Injectable } from '@angular/core';

import { MessageService, } from '../services/message.service';
import { DownloadService, } from '../services/download.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class FileProcess {
    constructor(private messageService: MessageService,
        private downloadService: DownloadService) {

    }

    filesModel: any[] = [];
    totalbyte: number = 0;
    FilesDataBase: any[] = [];
    fileModel: any = {};
    FileDataBase: any = null;
    onFileChange(event: any) {
        if (event.target.files && event.target.files.length > 0) {
            let files = event.target.files;

            for (let i = 0; i < files.length; i++) {
                const element = files[i];
                this.totalbyte += element.size;
                if (element.size < 5242880) {
                    this.FilesDataBase.push(element);
                    let reader = new FileReader();
                    reader.readAsDataURL(element);
                    reader.onload = () => {
                        var filer = {
                            Name: element.name,
                            DataURL: reader.result,
                            Size: element.size
                        }
                        this.filesModel.push(filer);
                    };
                    reader.onprogress = (data) => {
                        if (data.lengthComputable) {
                            var progress = parseInt(((data.loaded / data.total) * 100).toString(), 10);
                            console.log(progress);
                        }
                    }
                }
                else {
                    this.messageService.showMessage('File ảnh chỉ được phép có size < 5MB!');
                }
            }
        }
    }

    getFileOnFileChange(event: any) {
        let files = [];
        if (event.target.files && event.target.files.length > 0) {
            files = event.target.files;
            for (let i = 0; i < files.length; i++) {
                const element = files[i];

                if (element.size < 2147483647) {

                }
                else {
                    this.messageService.showMessage('File chỉ được phép có size < 1GB!');
                    element.splice(files[i], 1);
                }
            }

        }

        return files;
    }

    onAFileChange(event: any) {
        if (event.target.files && event.target.files.length > 0) {
            let files = event.target.files;
            this.totalbyte = files[0].size;
            this.FileDataBase = files[0];
            if (this.totalbyte < 5242880) {
                let reader = new FileReader();
                reader.readAsDataURL(files[0]);
                reader.onload = () => {
                    var filer = {
                        Name: files[0].name,
                        DataURL: reader.result,
                        Size: files[0].size
                    }
                    this.fileModel = (filer);
                };
                reader.onprogress = (data) => {
                    if (data.lengthComputable) {
                        var progress = parseInt(((data.loaded / data.total) * 100).toString(), 10);
                        console.log(progress);
                    }
                }
            }
            else {
                this.messageService.showMessage('File ảnh chỉ được phép có size < 5MB!');
            }
        }
    }

    fileExist(urlToFile: any) {
        var xhr = new XMLHttpRequest();
        xhr.open('HEAD', urlToFile, false);
        xhr.send();
        if (xhr.status == 404) {
            console.log("file not found");
            return false;
        } else {
            return true;
        }
    }

    removeFiles(file: any) {
        let index = this.filesModel.indexOf(file);
        if (index > -1) {
            this.filesModel.splice(index, 1);
            this.FilesDataBase.splice(index, 1);
        }
    }

    removeFile() {
        this.fileModel = {};
        this.FileDataBase = null;
    }

    /**
     * Download file theo link
     * @param path: Đường dẫn file cần download
     */
    downloadFileLink(path: string, fileName: string) {
        var link = document.createElement("a");
        link.href = path;

        fileName = fileName.replace('\\', '-');
        fileName = fileName.replace('/', '-');
        fileName = fileName.replace(':', '-');
        fileName = fileName.replace('?', '-');
        fileName = fileName.replace('*', '-');
        fileName = fileName.replace('"', '-');
        fileName = fileName.replace('<', '-');
        fileName = fileName.replace('>', '-');
        fileName = fileName.replace('|', '-');

        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }

    /**
     * Download 1 file - blob
     * @param path: Đường dẫn file cần download
     * @param fileName: Tên file zip download về
     */
    downloadFile(path: string, fileName: string) {
        this.downloadService.downloadFile({ PathFile: path, NameFile: fileName }).subscribe(data => {
            var blob = new Blob([data], { type: 'octet/stream' });
            var url = window.URL.createObjectURL(blob);
            this.downloadFileLink(url, fileName);
        }, error => {
            const blb = new Blob([error.error], { type: "text/plain" });
            const reader = new FileReader();

            reader.onload = () => {

                if (reader.result)
                    this.messageService.showMessage(reader.result.toString().replace('"', '').replace('"', ''));
            };
            // Start reading the blob as text.
            reader.readAsText(blb);
        });
    }

    /**
     * Downloaf nhiều file - blob
     * @param fileName: Tên file zip download về
     * @param files: Danh sách đường dẫn file cần download
     */
    downloadFiles(fileName: string, files: any[]) {
        this.downloadService.downloadFiles({ folderName: fileName, files: files }).subscribe(data => {
            var blob = new Blob([data], { type: 'octet/stream' });
            var url = window.URL.createObjectURL(blob);
            this.downloadFileLink(url, fileName.indexOf('.zip') == -1 ? fileName + '.zip' : fileName);
        }, error => {
            if (error.status == 0 || error.status == 500) {
                this.messageService.showError(error);
            } else {
                const blb = new Blob([error.error], { type: "text/plain" });
                const reader = new FileReader();

                reader.onload = () => {
                    if (reader.result)
                        this.messageService.showMessage(reader.result.toString().replace('"', '').replace('"', ''));
                };
                // Start reading the blob as text.
                reader.readAsText(blb);
            }
        });
    }

    /**
     * Đọc nội dung 1 file khi upload có hạn chế size
     * @param event : Thông tin file được chọn
     */
    readDataFileIOnUploadSize(event: any): Observable<any> {
        const fileObservable = new Observable(observer => {

            if (event.target.files && event.target.files.length > 0) {
                for (let i = 0; i < event.target.files.length; i++) {
                    let element = event.target.files[i];
                    if (element.size < 5242880) {
                        let reader = new FileReader();
                        reader.readAsDataURL(element);
                        reader.onload = () => {
                            var filer = {
                                Name: element.name,
                                DataURL: reader.result,
                                Size: element.size
                            }

                            observer.next({ File: element, Data: filer.DataURL, Name: element.name });
                            observer.unsubscribe();
                        };
                        reader.onprogress = (data) => {

                        }
                    } else {
                        this.messageService.showMessage('File chỉ được phép có size < 5MB!');
                        observer.error(false);
                        observer.unsubscribe();
                    }
                }
            }
            else {
                this.messageService.showMessage('Không có file nào được chọn');
                observer.error(false);
                observer.unsubscribe();
            }
        });

        return fileObservable;
    }

    /**
     * Đọc nội dung 1 file khi upload có hạn chế size
     * @param event : Thông tin file được chọn
     */
    readDataFileIOnUpload(event: any): Observable<any> {
        const fileObservable = new Observable(observer => {

            if (event.target.files && event.target.files.length > 0) {
                for (let i = 0; i < event.target.files.length; i++) {
                    let element = event.target.files[i];

                    let reader = new FileReader();
                    reader.readAsDataURL(element);
                    reader.onload = () => {
                        var filer = {
                            Name: element.name,
                            DataURL: reader.result,
                            Size: element.size
                        }

                        observer.next({ File: element, Data: filer.DataURL, Name: element.name });
                        observer.unsubscribe();
                    };
                    reader.onprogress = (data) => {

                    }
                }
            }
            else {
                this.messageService.showMessage('Không có file nào được chọn');
                observer.error(false);
                observer.unsubscribe();
            }
        });

        return fileObservable;
    }

    /**
    * Đọc nội dung 1 file khi upload có hạn chế size
    * @param event : Thông tin file được chọn
    */
    readDataFileIOnUploadByFile(file: any): Observable<any> {
        const fileObservable = new Observable(observer => {
            let reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => {
                var filer = {
                    Name: file.name,
                    DataURL: reader.result,
                    Size: file.size
                }

                observer.next({ File: file, Data: filer.DataURL, Name: file.name });
                observer.unsubscribe();
            };
            reader.onprogress = (data) => {

            }
        });

        return fileObservable;
    }
}
