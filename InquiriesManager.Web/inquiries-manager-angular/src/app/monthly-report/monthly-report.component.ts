import { Component, OnInit } from '@angular/core';
import { InquiriesService, Inquiry } from '../inquiries.service';
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
export class MonthlyReportComponent implements OnInit {
  displayedColumns: string[] = ['department', 'currentMonthCount', 'previousMonthCount', 'lastYearSameMonthCount'];
  rows: any[] = [];
  year: number = new Date().getFullYear();
  month: number = new Date().getMonth() + 1;

  // מפה של מחלקות לפי ID
  departments: { [key: number]: string } = {
    1: 'משאבי אנוש',
    2: 'מכירות',
    3: 'שירות לקוחות',
    4: 'תמיכה טכנית',
    5: 'הנהלה'
  };

  constructor(private inquiriesService: InquiriesService) {}

  ngOnInit() {
  }

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

  getDepartmentName(departmentId: number): string {
    return this.departments[departmentId] || 'לא ידוע';
  }
}
