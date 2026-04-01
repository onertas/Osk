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
    $(this.modalSelector).on('hidden.bs.modal', () => {
      // Modal kapandıktan sonra body'e veya başka elemana focus ver
      $('body').focus();
    });

    $(this.modalSelector).on('shown.bs.modal', () => {
      // Modal açılınca ilk focusable elemana focus ver
      $(this.modalSelector)
        .find(
          'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
        )
        .first()
        .focus();
     

      const $dialog = $(this.modalSelector + ' .modal-dialog');
      const windowWidth = $(window).width();
      const windowHeight = $(window).height();
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
