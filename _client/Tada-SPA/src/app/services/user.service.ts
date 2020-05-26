import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'users');
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }

  update(id: number, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user);
  }

  setMainPhoto(photoId: number, userId: number) {
    return this.http.put(this.baseUrl + 'photo/' + photoId + '/user/' + userId + '/setmain', {});
  }

  deletePhoto(photoId: number, userId: number) {
    return this.http.delete(this.baseUrl + 'photo/' + photoId + '/user/' + userId);
  }
}
