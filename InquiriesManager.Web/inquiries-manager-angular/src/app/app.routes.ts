import { Routes } from '@angular/router';
import { InquiryFormComponent } from './inquiry-form/inquiry-form.component';
import { MonthlyReportComponent } from './monthly-report/monthly-report.component';
import { InquiriesListComponent } from './inquiries-list/inquiries-list.component';

export const routes: Routes = [
	{ path: '', component: InquiryFormComponent },
	{ path: 'inquiry-form', component: InquiryFormComponent },
	{ path: 'monthly-report', component: MonthlyReportComponent },
	{ path: 'inquiries-list', component: InquiriesListComponent }
];
