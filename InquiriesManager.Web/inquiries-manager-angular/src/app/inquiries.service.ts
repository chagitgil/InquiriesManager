import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MonthlyInquiriesReportRow } from './models/monthly-inquiries-report-row';

export interface Inquiry {
  id?: number;
  name: string;
  phone: string;
  email: string;
  departmentId: number;
  description: string;
  createdAt?: string;
}

@Injectable({ providedIn: 'root' })
export class InquiriesService {
  

  private apiUrl = 'https://localhost:7085/api/Inquiries'; // עדכן לכתובת ה-API שלך

  constructor(private http: HttpClient) {}

  sendInquiry(inquiry: Inquiry): Observable<any> {
    return this.http.post(this.apiUrl, inquiry);
  }

  getMonthlyReport(year: number, month: number): Observable < MonthlyInquiriesReportRow[] > {
      const params = new HttpParams().set('year', year).set('month', month);
      return this.http.get<MonthlyInquiriesReportRow[]>(
        `${this.apiUrl}/monthly`,
        { params }
      );
    }

 


  getAllInquiries() {
    return this.http.get<Inquiry[]>(this.apiUrl);
}

  deleteInquiry(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}