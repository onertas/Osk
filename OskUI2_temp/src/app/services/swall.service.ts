import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class SwalService {
  constructor() {}
  showError(text: string) {
    Swal.fire({
      title: 'Hata Oluştu',
      text: text,
      timer: 3000,
      showConfirmButton: false,
      toast: true,
      position: 'bottom-right',
      icon: 'error',
      background: '#c0392b',
      color: 'white',
      iconColor: 'white',
    });
  }
  showSuccess(text: string) {
    Swal.fire({
      title: 'İşlem Başarılı',
      text: text,
      timer: 3000,
      showConfirmButton: false,
      toast: true,
      position: 'bottom-right',
      icon: 'success',
      background: '#27ae60',
      color: 'white',
      iconColor: 'white',
    });
  }
  showWarning(text: string) {
    Swal.fire({
      title: 'Uyarı',
      text: text,
      timer: 3000,
      showConfirmButton: false,
      toast: true,
      position: 'bottom-right',
      icon: 'warning',
      background: '#8e44ad',
      color: 'white',
      iconColor: 'white',
    });
  }
  showConfirmation(title: string, text: string, callBack: () => void) {
    Swal.fire({
      title: title,
      html:text ,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      cancelButtonText:"İptal",
      confirmButtonText: 'Onayla',
    }).then((result) => {
      if (result.isConfirmed) {
        callBack();
      }
    });
  }

  showSwallMessage(title: string, text: string) {
    Swal.fire({
      title: title,
      icon: 'info',
      html: text,

      confirmButtonText: `
    <i class="fa fa-power-off"></i> Tamam !
  `,
    });
  }
}
