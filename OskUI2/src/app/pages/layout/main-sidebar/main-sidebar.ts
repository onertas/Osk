import { Component, DOCUMENT, Inject, Renderer2, ViewChild } from '@angular/core';
import { Modal } from '../../../components/modal/modal';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { GenericHttpService } from '../../../services/generic.http.service';
import { SwalService } from '../../../services/swall.service';
import { StringService } from '../../../services/string.service';
import { Menus, MenuModel } from '../../../constants/menu';
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
    this.getHealthFacilityTypes();

    // Kullanıcı bilgisi yüklenince menüleri filtrele
    this.authService.user$.subscribe(user => {
      if (user) {
        this.filterMenusByRole(user.roles);
      }
    });
  }

  /**
   * Kullanıcının rollerine göre menüleri filtreler.
   * 'All' rolü herkes tarafından görülebilir,
   * diğer roller ise kullanıcının rollerine sahip olmasını gerektirir.
   */
  private filterMenusByRole(userRoles: string[]) {
    this.menus.forEach((menu) => {
      const hasAccess =
        menu.roles.includes('All') ||
        userRoles.some(r => menu.roles.includes(r));

      menu.show = hasAccess;

      menu.subMenus.forEach((sub) => {
        const subAccess =
          sub.roles.includes('All') ||
          userRoles.some(r => sub.roles.includes(r));

        sub.show = subAccess;
      });
    });
  }

  getHealthFacilityTypes() {
    this.http.get<any>("HealthFacilityType/GetHealthFacilityTypes", {}, (res: any) => {
      const hfMenu = this.menus.find(x => x.name === 'Sağlık Kuruluşları');
      if (hfMenu && res.data) {
        hfMenu.subMenus = res.data.map((type: any) => {
          return {
            name: type.name,
            isTitle: false,
            icon: 'fas fa-hospital',
            url: `/hf-list/${type.code}`,
            roles: ['All'],
            show: true,
            subMenus: []
          };
        });
      }
    });
  }

  menuac(event: Event): void {
    event.preventDefault();
    event.stopPropagation();

    const link = event.currentTarget as HTMLElement;
    const nextElement = link.nextElementSibling as HTMLElement | null;

    if (nextElement && nextElement.tagName === 'UL') {
      const isHidden = nextElement.style.display === 'none' || !nextElement.style.display;

      // Diğer tüm açık menüleri kapat
      const allTreeviews = this.document.querySelectorAll('.nav-treeview');
      allTreeviews.forEach((treeview: any) => {
        if (treeview !== nextElement) {
          this.renderer.setStyle(treeview, 'display', 'none');
          // Eğer üst li elementinde 'menu-open' gibi bir class varsa onu da kaldırabilirsiniz
          this.renderer.removeClass(treeview.parentElement, 'menu-open');
        }
      });

      // Tıklanan menüyü aç veya kapat
      if (isHidden) {
        this.renderer.setStyle(nextElement, 'display', 'block');
        this.renderer.addClass(link.parentElement, 'menu-open');
      } else {
        this.renderer.setStyle(nextElement, 'display', 'none');
        this.renderer.removeClass(link.parentElement, 'menu-open');
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
