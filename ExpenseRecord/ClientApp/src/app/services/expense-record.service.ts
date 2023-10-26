import { Inject, Injectable } from '@angular/core';
import { Observable, of, throwError, catchError } from 'rxjs';
import { ExpenseRecord } from '../models/ExpenseRecord';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams, HttpParamsOptions } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders(
    {
      'Content-Type': 'application/json',
      Authorization: 'my-token'
    }
  )
}

@Injectable({
  providedIn: 'root'
})
export class ExpenseRecordService {
  private baseUrl: string = "https://localhost:7081/api/v1/records";

  constructor(private http: HttpClient) {
    this.http = http;
  }

  getAll(): Observable<ExpenseRecord[]> {
    const api: string = `${this.baseUrl}/all`;
    return this.http.get<ExpenseRecord[]>(api).pipe(catchError(this.errorHandler));
  }

  createOne(body: ExpenseRecord): Observable<ExpenseRecord> {
    return this.http.post<ExpenseRecord>(this.baseUrl, body).pipe(catchError(this.errorHandler));
  }

  deleteOne(id: string): Observable<any> {
    const api: string = `${this.baseUrl}/${id}`;
    return this.http.delete<any>(api).pipe(catchError(this.errorHandler));
  }

  errorHandler(error: HttpErrorResponse) {
    if (error.status === 400) {
      console.error('bad request, please retry')
    } else if (error.status === 500) {
      console.error("server error")
    } else if (error.status === 404) {
      console.error("the item doesn't exist")
    }

    return throwError(() => new Error('something is wrong, please try again'))
  };
}
