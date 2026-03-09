import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { catchError, Observable, throwError } from 'rxjs';

import { api } from '../constants/static';
import { Result } from '../dtos/result';

@Injectable({
  providedIn: 'root'
})
export class HttpApiService {
private baseUrl: string = api
  constructor(private http: HttpClient) {}

  private handleError(error: any) {
    console.error('HTTP Error:', error);
    return throwError(() => error);
  }

  // 🔹 GET metodu, query parametre destekli
  get<T>(
    endpoint: string,
    params?: { [key: string]: any },
    headers?: HttpHeaders
  ): Observable<Result<T>> {
    const url = `${this.baseUrl}/${endpoint}`;
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach((key) => {
        if (params[key] !== null && params[key] !== undefined) {
          httpParams = httpParams.set(key, params[key].toString());
        }
      });
    }

    return this.http
      .get<Result<T>>(url, { headers, params: httpParams,withCredentials: true })
      .pipe(catchError(this.handleError));
  }

  // 🔹 POST metodu, body parametreli
  post<T>(
    endpoint: string,
    body: any,
    params?: { [key: string]: any },
    headers?: HttpHeaders
  ): Observable<Result<T>> {
    const url = `${this.baseUrl}/${endpoint}`;
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach((key) => {
        if (params[key] !== null && params[key] !== undefined) {
          httpParams = httpParams.set(key, params[key].toString());
        }
      });
    }

    return this.http
      .post<Result<T>>(url, body, { headers, params: httpParams,withCredentials: true })
      .pipe(catchError(this.handleError));
  }
}
