import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
// קומפוננטת השורש של האפליקציה
export class AppComponent {
  // משתנה כותרת האפליקציה
  title = 'inquiries-manager-angular';
}
