import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Message } from '../models/Message';
import { PaginatedResult } from '../models/pagination';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getUsers(page?, itemsPerPage?, userParams?, likesParams?): Observable<PaginatedResult<User[]>> {
    const paginatedResult = new PaginatedResult<User[]>();
    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('currentPage', page);
      params = params.append('itemsPerPage', itemsPerPage);
    }

    if (userParams != null) {
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }

    if (likesParams === 'likers') {
      params = params.append('likers', 'true');
    }

    if (likesParams === 'likees') {
      params = params.append('likees', 'true');
    }

    return this.http.get<User[]>(this.baseUrl + 'users', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination')) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
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

  sendLike(likerId: number, likeeId: number) {
    return this.http.post(this.baseUrl + 'users/' + likerId + '/like/' + likeeId, {} );
  }

  getMessages(userId: number, page?, itemsPerPage?, messagesType?) {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();

    let params = new HttpParams();
    params = params.append('messagesType', messagesType);

    if (page != null && itemsPerPage != null) {
      params = params.append('currentPage', page);
      params = params.append('itemsPerPage', itemsPerPage);
    }

    return this.http.get<Message[]>(this.baseUrl + 'messages/user/' + userId, { observe: 'response', params }).
      pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('pagination'));
          }
          return paginatedResult;
        })
      );
  }

  getMessageThread(id: number, receiverId: number) {
    return this.http.get<Message[]>(this.baseUrl + 'messages/' + id + '/thread/' + receiverId);
  }

  sendMessage(userId: number, message: Message) {
    return this.http.post(this.baseUrl + 'messages/user/' + userId, message);
  }

  deleteMessage(messageId: number, userId: number) {
    return this.http.put(this.baseUrl + 'messages/' + messageId + '/user/' + userId, {});
  }

  markAsRead(messageId: number, userId: number) {
    return this.http.put(this.baseUrl + 'messages/' + messageId + '/read/user/' + userId, {}).subscribe();
  }
}
