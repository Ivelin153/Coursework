import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FileModel } from '../../_models/File';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {
  @Input() files: FileModel[];
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  baseUrl = environment.apiUrl;

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'fileUpload/' + this.authService.decodedToken.nameid,
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedMimeType: ['application/csv', 'text/csv', 'application/vnd.ms-excel', 'text/plain'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: FileModel = JSON.parse(response);
        const file = {
          id: res.id,
          fileName: res.fileName,
          dateAdded: res.dateAdded
        };
        this.files.push(file);
      }
    };
  }

  deleteFile(id: number) {
    this.alertify.confirm('Are you sure you want to delete this file?', () => {
      this.userService.deleteFile(this.authService.decodedToken.nameid, id).subscribe(() => {
        this.files.splice(this.files.findIndex(p => p.id === id), 1);
        this.alertify.success('File has been deleted!');
      }, error => {
        this.alertify.error('Failed to delete the file!');
      });
    });
  }
}
