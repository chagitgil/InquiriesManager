import { Component, OnInit } from '@angular/core';
import { InquiriesService, Inquiry } from '../inquiries.service';
import { departments } from '../models/departments';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';  

@Component({
  selector: 'app-monthly-report',
  imports: [
    CommonModule,
    MatCardModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    FormsModule
  ],
  templateUrl: './monthly-report.component.html',
  styleUrl: './monthly-report.component.scss'
})
// קומפוננטה לדוח פניות חודשי
export class MonthlyReportComponent implements OnInit {
  // עמודות הטבלה
  displayedColumns: string[] = ['department', 'currentMonthCount', 'previousMonthCount', 'lastYearSameMonthCount'];
  // נתוני הדוח
  rows: any[] = [];
  // שנה נבחרת
  year: number = new Date().getFullYear();
  // חודש נבחר
  month: number = new Date().getMonth() + 1;


  // בנאי הקומפוננטה
  constructor(private inquiriesService: InquiriesService) {}

  // אתחול הקומפוננטה
  ngOnInit() {
  }

  // טוען את נתוני הדוח מהשרת
  load() {
    this.inquiriesService.getMonthlyReport(this.year, this.month).subscribe({
      next: (data) => {
        this.rows = data.map(row => ({
          ...row,
          department: this.getDepartmentName(row.departmentId)
        }));
      },
      error: () => {
        this.rows = [];
      }
    });
  }

  // מחזיר את שם המחלקה לפי מזהה
  getDepartmentName(departmentId: number): string {
    return departments[departmentId] || 'לא ידוע';
  }
}
