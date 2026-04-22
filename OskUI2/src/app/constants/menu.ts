export class MenuModel {
  name: string = '';
  isTitle: boolean = false;
  icon: string = '';
  url: string = '';
  roles: string[] = [];
  show: boolean = true;
  subMenus: MenuModel[] = [];
}

export const Menus: MenuModel[] = [
  {
    name: 'Ana Sayfa',
    isTitle: false,
    icon: 'fas fa-home',
    url: '/',
    roles: ['All'],
    show: true,
    subMenus: [],
  },
  {
    name: 'Sağlık Kuruluşları',
    isTitle: false,
    icon: 'fas fa-home',
    url: '/',
    roles: ['All'],
    show: true,
    subMenus: [],
  },

  {
    name: 'Yetkili İşlemleri',
    isTitle: false,
    icon: 'fa fa-leaf',
    url: '/',
    roles: ['All'],
    show: true,
    subMenus: [
      {
        name: 'Kuruluş Tipleri',
        icon: 'fa fa-chevron-right fa-xs',
        url: '/hf-types',
        isTitle: false,
        roles: ['All'],
        show: true,
        subMenus: [],
      },
      {
        name: 'Kuruluşlar',
        icon: 'fa fa-chevron-right fa-xs',
        url: '/hf-management',
        isTitle: false,
        roles: ['All'],
        show: true,
        subMenus: [],
      },
      {
        name: 'Personel',
        icon: 'fa fa-chevron-right fa-xs',
        url: '/personnel',
        isTitle: false,
        roles: ['All'],
        show: true,
        subMenus: [],
      },
      {
        name: 'Ünvanlar',
        icon: 'fa fa-chevron-right fa-xs',
        url: '/title',
        isTitle: false,
        roles: ['All'],
        show: true,
        subMenus: [],
      },
      {
        name: 'Branşlar',
        icon: 'fa fa-chevron-right fa-xs',
        url: '/branch',
        isTitle: false,
        roles: ['All'],
        show: true,
        subMenus: [],
      },
      {
        name: 'Personel Hareket Türleri',
        icon: 'fa fa-chevron-right fa-xs',
        url: '/pm-type',
        isTitle: false,
        roles: ['All'],
        show: true,
        subMenus: [],
      },
    ],
  },

  {
    name: 'Raporlar',
    isTitle: false,
    icon: '',
    url: '',
    roles: ['All'],
    show: true,
    subMenus: [
      {
        name: 'Kişi Bazlı Görev Raporu',
        icon: 'fa fa-chevron-right fa-xs',
        url: '/task-users',
        isTitle: false,
        roles: ['Admin', 'ProjeAdmin'],
        show: true,
        subMenus: [],
      },
      {
        name: 'Toplantı Raporu',
        icon: 'fa fa-chevron-right fa-xs',
        url: '/meet-report',
        isTitle: false,
        roles: ['Admin', 'TopnatiAdmin'],
        show: true,
        subMenus: [],
      },
      {
        name: 'Şikayet Raporu',
        icon: 'fa fa-chevron-right fa-xs',
        url: '/complaint-report',
        isTitle: false,
        roles: ['Admin', 'SikayetAdmin'],
        show: true,
        subMenus: [],
      },
    ],
  },
  {
    name: 'Sistem Ayarları',
    isTitle: false,
    icon: 'fas fa-cog',
    url: '/',
    roles: ['All'],
    show: true,
    subMenus: [
      {
        name: 'Kullanıcı Yönetimi',
        icon: 'fa fa-chevron-right fa-xs',
        url: '/users',
        isTitle: false,
        roles: ['All'],
        show: true,
        subMenus: [],
      },
      {
        name: 'Rol Yönetimi',
        icon: 'fa fa-chevron-right fa-xs',
        url: '/roles',
        isTitle: false,
        roles: ['All'],
        show: true,
        subMenus: [],
      },
    ],
  },
  // {
  //   name: 'Log İşlemleri',
  //   isTitle: false,
  //   icon: '',
  //   url: '/log',
  //   roles: ['ProjeAdmin'],
  //   show: true,
  //   subMenus: [],
  // },
];
