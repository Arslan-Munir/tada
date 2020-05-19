import { HttpErrorResponse, HttpInterceptor, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor{
    intercept(
        req: import('@angular/common/http').HttpRequest<any>,
        next: import('@angular/common/http').HttpHandler)
        : import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {

        return next.handle(req).pipe(
            catchError((response) => {
                if (response.status === 401) {
                    return throwError(response.statusText);
                }

                if (response instanceof HttpErrorResponse) {
                    const applicationError = response.headers.get('Application-Error');
                    if (applicationError) {
                        return throwError(applicationError);
                    }

                    const serverError = response.error;
                    let modelStateErrors = '';
                    if (serverError && typeof serverError === 'object') {
                        for (const key in serverError.errors) {
                            if (serverError.errors[key]) {
                                modelStateErrors += serverError.errors[key] + '<br />';
                            }
                        }
                    }
                    return throwError(modelStateErrors || serverError || 'Server Error');
                }
            })
        );
    }
}

export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
};
