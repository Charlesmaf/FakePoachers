import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ToastMessage } from '../../domains/models/toastmessage';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private toastSubject = new BehaviorSubject<ToastMessage | null>(null);
  toast$ = this.toastSubject.asObservable();
  showToast(message: string, type: 'success' | 'danger' | 'warning' | 'info') {
    this.toastSubject.next({ message, type });

    setTimeout(() => {
      this.toastSubject.next(null);
    }, 2000);
  }
}
