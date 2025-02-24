import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { GoogleMapsModule } from '@angular/google-maps';
import { RouterLink, RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';
import { LoadingService } from './core/services/load.service';
import { ToastComponent } from './shared/components/toast/toast.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    GoogleMapsModule,
    CommonModule,
    ToastComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  isLoading$: Observable<boolean>;
  constructor(private loadingService: LoadingService) {
    this.isLoading$ = this.loadingService.loading$;
  }
  title = 'FakePoachers.UI';
}
