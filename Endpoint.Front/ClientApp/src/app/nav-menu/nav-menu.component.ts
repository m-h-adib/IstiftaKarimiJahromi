import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { StoreService } from '../StoreService';
import { Router } from '@angular/router';
import { GetAccessService } from '../_services/identity/getAccessService';
import { GetMenuAccessService } from '../_services/identity/getMenuAccessService';
import { GetMenuAccessDto } from '../_models/login/getMenuAccessDto';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded: boolean = false;
  Menus: GetMenuAccessDto[] = [];
  showCloseDialog: string = "";
  @ViewChild('toggleButton') toggleButton: ElementRef | undefined;
  @ViewChild('toggleButton1') toggleButton1: ElementRef | undefined;
  @ViewChild('toggleButton2') toggleButton2: ElementRef | undefined;
  @ViewChild('menu') menu: ElementRef | undefined;

  constructor(private renderer: Renderer2,
    private _store: StoreService,
    private _getAccessService: GetAccessService,
    private _getMenuAccessService: GetMenuAccessService,
    private router: Router) {
    this.renderer.listen('window', 'click', (e: Event) => {
      /**
       * Only run when toggleButton is not clicked
       * If we don't check this, all clicks (even on the toggle button) gets into this
       * section which in the result we might never see the menu open!
       * And the menu itself is checked here, and it's where we check just outside of
       * the menu and button the condition abbove must close the menu
       */
      if (e.target !== this.toggleButton!.nativeElement && e.target !== this.toggleButton1!.nativeElement && e.target !== this.toggleButton2!.nativeElement && e.target !== this.menu!.nativeElement) {
        this.collapse();

      }
    });
  }

  ngOnInit(): void {
    this._getMenuAccessService.GetMenuAccess().subscribe(p => {
      this.Menus = p.data;
    })

    this._getAccessService.GetMenuAccess().subscribe(p => {
      localStorage.setItem('access', JSON.stringify(p.data));
    })
  }

  collapse() {
    this.isExpanded = false;
  }


  toggle() {
    console.log(this.isExpanded);
    this.isExpanded = !this.isExpanded;
  }

  showLogoutDialog(): void {
    console.log(this.showCloseDialog);
    if (this.showCloseDialog == "") {
      this.showCloseDialog = "show"
    } else {
      this.showCloseDialog = "";
    }

    this.collapse();
  }

  logout(): void {
    this.showCloseDialog = "";
    localStorage.removeItem("token");
    localStorage.removeItem("name");
    localStorage.removeItem("status");
    this._store.setLoginStatus(false);
    this.router.navigate(["/login"]);
  }

  getrouterparams(key: string, value: string) {
    return { [key]: value };
  }

}
