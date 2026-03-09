import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { api } from '../constants/static';
import { ErrorService } from './error-service';

@Injectable({
  providedIn: 'root',
})
export class GenericHttpService {
  private apiUrl = api;

  constructor(
    private http: HttpClient,
    private ErrorService: ErrorService,
  ) {}

  get<T>(
    endpoint: string,
    params: { [key: string]: any },
    callBack: (res: T) => void,
    errorCallBack?: () => void,
  ) {
    let httpParams = new HttpParams();

    if (params) {
      Object.keys(params).forEach((key) => {
        const value = params[key];

        // Tarih değerlerini ISO formatına çevir
        if (value instanceof Date) {
          httpParams = httpParams.set(key, value.toISOString());
        }
        // Object kontrolü
        else if (value !== null && typeof value === 'object') {
          httpParams = httpParams.set(key, JSON.stringify(value));
        }
        // Diğer değerler için normal string dönüşümü
        else {
          httpParams = httpParams.set(key, value?.toString() ?? '');
        }
      });
    }

    let httpHeaders = new HttpHeaders();

    this.http
      .get<T>(`${this.apiUrl}/${endpoint}`, {
        params: httpParams,
        headers: httpHeaders,
      })
      .subscribe({
        next: (res) => {
          if (res) {
            callBack(res);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.ErrorService.errorHandler(err);
          if (errorCallBack) {
            errorCallBack();
          }
        },
      });
  }

  post<T>(
    endpoint: string,
    body: any,
    callBack: (res: T) => void,
    headers?: { [key: string]: string },
    errorCallBack?: () => void,
    responseType: 'json' | 'blob' = 'json', // <-- bunu ekle
  ) {
    let httpHeaders = new HttpHeaders();
    if (headers) {
      Object.keys(headers).forEach((key) => {
        httpHeaders = httpHeaders.set(key, headers[key]);
      });
    }

    this.http
      .post<T>(`${this.apiUrl}/${endpoint}`, body, {
        headers: httpHeaders,
        responseType: responseType as any, // <-- burada da belirt
      })
      .subscribe({
        next: (res) => {
          if (res) {
            callBack(res);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.ErrorService.errorHandler(err);
          if (errorCallBack) {
            errorCallBack();
          }
        },
      });
  }

  put<T>(endpoint: string, body: any, headers?: { [key: string]: string }): Observable<T> {
    let httpHeaders = new HttpHeaders();
    if (headers) {
      Object.keys(headers).forEach((key) => {
        httpHeaders = httpHeaders.set(key, headers[key]);
      });
    }

    return this.http.put<T>(`${this.apiUrl}/${endpoint}`, body, {
      headers: httpHeaders,
    });
  }

  deleteOrn<T>(endpoint: string, headers?: { [key: string]: string }): Observable<T> {
    let httpHeaders = new HttpHeaders();
    if (headers) {
      Object.keys(headers).forEach((key) => {
        httpHeaders = httpHeaders.set(key, headers[key]);
      });
    }

    return this.http.delete<T>(`${this.apiUrl}/${endpoint}`, {
      headers: httpHeaders,
    });
  }

  delete<T>(
    endpoint: string,
    params: { [key: string]: any },
    callBack: (res: T) => void,
    errorCallBack?: () => void,
  ) {
    let httpParams = new HttpParams();

    if (params) {
      Object.keys(params).forEach((key) => {
        const value = params[key];

        // Tarih değerlerini ISO formatına çevir
        if (value instanceof Date) {
          httpParams = httpParams.set(key, value.toISOString());
        }
        // Object kontrolü
        else if (value !== null && typeof value === 'object') {
          httpParams = httpParams.set(key, JSON.stringify(value));
        }
        // Diğer değerler için normal string dönüşümü
        else {
          httpParams = httpParams.set(key, value?.toString() ?? '');
        }
      });
    }

    let httpHeaders = new HttpHeaders();

    this.http
      .delete<T>(`${this.apiUrl}/${endpoint}`, {
        params: httpParams,
        headers: httpHeaders,
      })
      .subscribe({
        next: (res) => {
          if (res) {
            callBack(res);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.ErrorService.errorHandler(err);
          if (errorCallBack) {
            errorCallBack();
          }
        },
      });
  }
  getFile(
    endpoint: string,
    params: { [key: string]: any },
    callBack: (res: Blob) => void,
    errorCallBack?: () => void,
  ) {
    let httpParams = new HttpParams();

    if (params) {
      Object.keys(params).forEach((key) => {
        const value = params[key];
        if (value instanceof Date) {
          httpParams = httpParams.set(key, value.toISOString());
        } else if (value !== null && typeof value === 'object') {
          httpParams = httpParams.set(key, JSON.stringify(value));
        } else {
          httpParams = httpParams.set(key, value?.toString() ?? '');
        }
      });
    }

    let httpHeaders = new HttpHeaders();

    this.http
      .get(`${this.apiUrl}/${endpoint}`, {
        params: httpParams,
        headers: httpHeaders,
        responseType: 'blob', // <-- burada önemli
      })
      .subscribe({
        next: (res: Blob) => {
          callBack(res);
        },
        error: (err: HttpErrorResponse) => {
          this.ErrorService.errorHandler(err);
          if (errorCallBack) {
            errorCallBack();
          }
        },
      });
  }
}
