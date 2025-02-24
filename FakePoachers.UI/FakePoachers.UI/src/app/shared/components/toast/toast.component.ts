import { Component } from '@angular/core';
import { ToastService } from '../../../core/services/toast.service';
import { CommonModule } from '@angular/common';
import { ToastMessage } from '../../../domains/models/toastmessage';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css',
})
export class ToastComponent {
  toast: ToastMessage | null = null;

  constructor(private toastService: ToastService) {
    this.toastService.toast$.subscribe((toast) => {
      this.toast = toast;
    });
  }
}
