import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Photo } from 'src/app/models/photo';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Input() photos: Photo[];
  @Output() getMemberPhotoChange = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  hasAnotherDropZoneOver = false;
  baseUrl = environment.apiUrl;
  response = '';

  constructor(private authService: AuthService, private userService: UserService, private alert: AlertifyService) { }

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  setMainPhoto(photo: Photo) {
    const currentMainPhoto = this.photos.filter(p => p.isMain === true)[0];
    currentMainPhoto.isMain = false;
    photo.isMain = true;
    this.userService.setMainPhoto(photo.id, this.authService.decodeToken().nameid)
      .subscribe(() => {
        this.authService.changeMemberPhoto(photo.url);
        this.authService.currentUser.photoUrl = photo.url;
        localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
        this.alert.success('Main photo updated!');
      }, error => {
        this.alert.error(error);
      });
  }

  deletePhoto(id: number) {
    this.alert.confirm('Are you sure you want to delete this photo?', () => {
      const photoIndex = this.photos.findIndex(p => p.id === id);
      const photoToDelete = this.photos[photoIndex];
      this.photos.splice(photoIndex, 1);
      this.alert.success('Photo has been deleted');

      this.userService.deletePhoto(id, this.authService.decodeToken().nameid)
        .subscribe(() => {}, error => {
            this.photos.splice(photoIndex, 0, photoToDelete);
            this.alert.error('System error! Failed to delete photo.');
        });
    });
  }

  private initializeUploader() {
    this.uploader = new FileUploader({
      url:
        this.baseUrl +
        'photo/' +
        'user/' +
        this.authService.decodeToken().nameid,
      authToken: 'Bearer ' + localStorage.getItem('token'),
      disableMultipart: false,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, header) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo: Photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };

        this.photos.push(photo);

        if (photo.isMain) {
          this.authService.changeMemberPhoto(photo.url);
          this.authService.currentUser.photoUrl = photo.url;
          localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
        }
      }
    };
  }
}
