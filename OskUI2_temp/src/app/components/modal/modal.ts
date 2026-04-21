import { Component, Input } from '@angular/core';
declare var $: any;
@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [],
  templateUrl: './modal.html',
  styleUrl: './modal.css',
})
export class Modal {
  @Input() title: string = '';
  @Input() name: string = '';
  @Input() modalSize: 'lg' | 'sm' | 'xl' = 'lg';
  @Input() isCloseButton: boolean = true;
  @Input() isDrag: boolean = false;

  private modalSelector: string = '';
  constructor() {}

  ngOnInit(): void {}
  ngAfterViewInit(): void {
    this.modalSelector = '#' + this.name;
    $(this.modalSelector).on('hide.bs.modal', () => {
      // Modal kapanmaya başladığı anda odağı modal dışına çek.
      // Bu sayede "vurgulanan eleman gizli bir konteyner içinde kaldı" uyarısı engellenir.
      $('body').attr('tabindex', '-1').focus();
    });

    $(this.modalSelector).on('hidden.bs.modal', () => {
      // Modal tamamen kapandığında odağı body'e sabitle ve tabindex'i temizle
      $('body').focus().removeAttr('tabindex');
    });

    $(this.modalSelector).on('shown.bs.modal', () => {
      // Modal açılınca ilk focusable elemana focus ver
      const firstFocusable = $(this.modalSelector).find(
        'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
      ).first();
      
      if (firstFocusable.length) {
        firstFocusable.focus();
      } else {
        $(this.modalSelector).focus();
      }

      const $dialog = $(this.modalSelector + ' .modal-dialog');
      const windowWidth = $(window).width();
      const dialogWidth = $dialog.outerWidth();
      const dialogHeight = $dialog.outerHeight();

      $dialog.css({
       // position: 'computed',
       left: (windowWidth - dialogWidth) / 2 + 'px',
        top: '28px',
        margin: 0,
      });
    });

    //  if (!this.isDrag) {
    //    return
    //   }
    ($(this.modalSelector + ' .modal-dialog') as any).draggable({
      handle: '.modal-header',
     containment: 'window', // Pencere sınırları içinde tutar
      scroll: false, // Scroll hareketini engeller
      cursor: 'move', // İmleç şeklini değiştirir
      opacity: 1.0, // Sürükleme sırasında opaklık,
    });
  }

  ngOnDestroy(): void {
    if (this.modalSelector) {
      // Event listenerları kaldır
      $(this.modalSelector).off('hidden.bs.modal');
      $(this.modalSelector).off('shown.bs.modal');
    }
  }

  close(value: string) {
    $('#' + value).modal('hide');
  }

  open(value: string) {
    $('#' + value).modal('show');
  }
}
