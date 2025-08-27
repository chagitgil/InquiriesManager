import { Component, OnInit } from '@angular/core';
import { InquiriesService, Inquiry } from '../inquiries.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-inquiries-list',
  imports: [
    CommonModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './inquiries-list.component.html',
  styleUrl: './inquiries-list.component.scss'
})
export class InquiriesListComponent implements OnInit {
  displayedColumns: string[] = ['name', 'phone', 'email', 'departmentName', 'createdAt', 'description', 'actions'];
  inquiries: Inquiry[] = [];

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
    this.loadInquiries();
  }

  loadInquiries() {
    this.inquiriesService.getAllInquiries().subscribe({
      next: (data) => {
        this.inquiries = data.map(inquiry => ({
          ...inquiry,
          departmentName: this.getDepartmentName(inquiry.departmentId)
        }));
      },
      error: () => {
        this.inquiries = [];
      }
    });
  }

  getDepartmentName(departmentId: number): string {
    return this.departments[departmentId] || 'לא ידוע';
  }

  deleteInquiry(id: number) {
    if (confirm('האם אתה בטוח שברצונך למחוק פנייה זו?')) {
      this.inquiriesService.deleteInquiry(id).subscribe({
        next: () => {
          this.loadInquiries();
        },
        error: () => {
          alert('אירעה שגיאה במחיקת הפנייה');
        }
      });
    }
  }
}
