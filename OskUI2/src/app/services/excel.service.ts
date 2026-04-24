import { Injectable } from '@angular/core';
import * as XLSX from 'xlsx';

@Injectable({
  providedIn: 'root',
})
export class ExcelService {
  constructor() {}

  /**
   * Verilen veriyi Excel dosyası olarak indirir.
   * @param data Dışa aktarılacak dizi (JSON formatında)
   * @param fileName Kaydedilecek dosya adı (uzantı eklenmez)
   * @param sheetName Excel sayfasının adı
   */
  exportToExcel(data: any[], fileName: string, sheetName: string = 'Sayfa1'): void {
    if (!data || data.length === 0) {
      console.warn('Dışa aktarılacak veri bulunamadı.');
      return;
    }

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(data);
    const workbook: XLSX.WorkBook = {
      Sheets: { [sheetName]: worksheet },
      SheetNames: [sheetName],
    };

    // Dosyayı oluştur ve indir
    XLSX.writeFile(workbook, `${fileName}_${new Date().getTime()}.xlsx`);
  }
}
