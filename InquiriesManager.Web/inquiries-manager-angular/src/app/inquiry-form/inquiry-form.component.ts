import { Component } from '@angular/core';
import { departments } from '../models/departments';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { InquiriesService, Inquiry } from '../inquiries.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-inquiry-form',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule
  ],
  templateUrl: './inquiry-form.component.html',
  styleUrl: './inquiry-form.component.scss'
})
export class InquiryFormComponent {
  inquiryForm: FormGroup;
  departments = Object.entries(departments).map(([id, name]) => ({ id: Number(id), name }));

  constructor(private fb: FormBuilder, private inquiriesService: InquiriesService) {
    this.inquiryForm = this.fb.group({
      name: ['', Validators.required],
      phone: ['', [Validators.required, Validators.pattern(/^0[0-9]{9,10}$/)]],
      email: ['', [Validators.required, Validators.email]],
      departmentId: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  // שליחת הטופס לשרת
  onSubmit() {
    if (this.inquiryForm.valid) {
      const inquiry: Inquiry = this.inquiryForm.value;
      this.inquiriesService.sendInquiry(inquiry).subscribe({
        next: () => {
          alert('הפנייה נשלחה בהצלחה!');
          this.inquiryForm.reset();
          this.inquiryForm.markAsPristine();
          this.inquiryForm.markAsUntouched();
          Object.keys(this.inquiryForm.controls).forEach(key => {
            this.inquiryForm.get(key)?.setErrors(null);
          });
        },
        error: () => {
          alert('אירעה שגיאה בשליחת הפנייה');
        }
      });
    }
  }
}
