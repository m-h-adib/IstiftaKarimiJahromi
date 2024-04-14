import { Component } from '@angular/core';
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
}
