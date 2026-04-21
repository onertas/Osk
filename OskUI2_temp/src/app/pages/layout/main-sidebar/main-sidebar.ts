import { Component, DOCUMENT, Inject, Renderer2, ViewChild } from '@angular/core';
import { Modal } from '../../../components/modal/modal';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { GenericHttpService } from '../../../services/generic.http.service';
import { SwalService } from '../../../services/swall.service';
import { StringService } from '../../../services/string.service';
import { Menus } from '../../../constants/menu';
import { SharedModule } from '../../../modules/shared.module';
import { AuthService, CurrentUser } from '../../../services/auth.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-main-sidebar',
  imports: [SharedModule],
  standalone: true,
  templateUrl: './main-sidebar.html',
  styleUrl: './main-sidebar.css',
})
export class MainSidebar {
  @ViewChild(Modal) modalCom: Modal | undefined;
  user$: Observable<CurrentUser | null>;
   
  constructor(
  
    private router: Router,
    private http: GenericHttpService,
    private swallSer: SwalService,
    private renderer: Renderer2,
    public stringSer: StringService,
    private authService:AuthService,
    @Inject(DOCUMENT) private document: Document,
  ) {
    this.user$ = this.authService.user$;
  }
  //newmenu: MenuModel[] = [];
  menus = Menus;

  
  ngOnInit(): void {



    this.menus.forEach((element) => {
      let result =
        // tokenRoles.some((x) => element.roles.includes(x)) ||
        element.roles.includes('All');

      if (!result) {
        element.show = false;
      }

      element.subMenus.forEach((subElement) => {
        let result1 =
          //  tokenRoles.some((x) => subElement.roles.includes(x)) ||
          subElement.roles.includes('All');

        if (!result1) {
          subElement.show = false;
        }
      });
    });
  }

  menuac(event: Event): void {
    event.preventDefault();

    const link = event.currentTarget as HTMLElement; // <a> elementini verir
    const nextElement = link.nextElementSibling as HTMLElement | null;

    if (nextElement && nextElement.tagName === 'UL') {
      const isHidden = nextElement.style.display === 'none' || !nextElement.style.display;

      if (isHidden) {
        this.renderer.setStyle(nextElement, 'display', 'block');
      } else {
        this.renderer.setStyle(nextElement, 'display', 'none');
      }
    }
  }

  x(event: any) {
    if (this.document.body.classList.contains('sidebar-open')) {
      this.document.body.classList.remove('sidebar-open');
      this.document.body.classList.add('sidebar-closed', 'sidebar-collapse');
    }
  }
}
