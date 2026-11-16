import { Component, HostListener } from '@angular/core';
import { StoreService } from './StoreService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';

  loggedIn?: any = '';

  ngOnInit(): void {
    this.loggedIn = localStorage.getItem("token");

    if (this.loggedIn != "" && this.loggedIn != null) {
      this.store.setLoginStatus(true);
    }

    if (localStorage.getItem("status") == "Admin") {
      this.store.setIsAdmin(true);
    }
    else {
      this.store.setIsAdmin(false);
    }

  }

  constructor(public store: StoreService) { }

  @HostListener('document:keydown', ['$event'])
  onDocumentKeydown(event: KeyboardEvent): void {
    if (event.key !== 'Escape') {
      return;
    }

    const openModals = document.querySelectorAll<HTMLElement>('.modal.fade.show');
    const activeModal = openModals[openModals.length - 1];

    if (!activeModal) {
      return;
    }

    const closeButton = activeModal.querySelector<HTMLElement>('[data-bs-dismiss="modal"]');

    if (!closeButton) {
      return;
    }

    event.preventDefault();
    event.stopPropagation();
    closeButton.click();
  }
}
