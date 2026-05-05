import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpApiService } from '../../services/http-api-service';
import { SharedModule } from '../../modules/shared.module';
import { TableModule } from 'primeng/table';
import { SelectModule } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { HfManagementListDto } from '../../dtos/healthFacility/hf-management-list.dto';
import { ExcelService } from '../../services/excel.service';

@Component({
  selector: 'app-hf-report',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    SharedModule, 
    TableModule, 
    SelectModule, 
    InputTextModule, 
    ButtonModule
  ],
  templateUrl: './hf-report.html',
})
export class HfReportComponent implements OnInit {
  private http = inject(HttpApiService);
  private excel = inject(ExcelService);

  // Filters
  filterModel = {
    healthFacilityTypeId: null as string | null,
    search: ''
  };

  // Options
  facilityTypes: any[] = [];

  // Table Data
  facilities: HfManagementListDto[] = [];
  totalRecords: number = 0;
  loading: boolean = false;
  
  page = 1;
  pageSize = 10;
  private searchTimer: any;

  ngOnInit(): void {
    this.loadFilterOptions();
  }

  loadFilterOptions() {
    this.http.get<any[]>('HealthFacilityType/GetHealthFacilityTypes').subscribe(res => {
      if (res.success && res.data) {
        this.facilityTypes = res.data;
        
        // Default olarak "Özel Hastane" seç
        const ozelHastane = this.facilityTypes.find(t => t.name === 'Özel Hastane');
        if (ozelHastane) {
          this.filterModel.healthFacilityTypeId = ozelHastane.id;
          this.search();
        }
      }
    });
  }

  onSearchInput() {
    clearTimeout(this.searchTimer);
    this.searchTimer = setTimeout(() => {
      this.search();
    }, 500);
  }

  search() {
    this.loading = true;
    
    // API'de tip ID'sine göre direkt filtreleme yoksa arama parametresini kullanalım
    // GetAllPaged içinde search parametresi hem isim hem de tip adı içinde arıyor.
    // Ancak daha kesin sonuç için tip adı ile arama yapabiliriz.
    
    let searchStr = this.filterModel.search;
    if (this.filterModel.healthFacilityTypeId) {
        const selectedType = this.facilityTypes.find(t => t.id === this.filterModel.healthFacilityTypeId);
        if (selectedType) {
            // Eğer search boşsa sadece tipi arat, doluysa ikisini birleştirme şansımız yok mevcut API ile 
            // ama en azından tipi aratabiliriz.
            if (!searchStr) searchStr = selectedType.name;
        }
    }

    this.http.get<any>('HealthFacility/GetAllPaged', {
      page: this.page,
      pageSize: this.pageSize,
      search: searchStr
    }).subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.facilities = res.data.items;
          this.totalRecords = res.data.totalCount;
        }
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  onPageChange(event: any) {
    this.page = (event.first / event.rows) + 1;
    this.pageSize = event.rows;
    this.search();
  }

  exportToExcel() {
    const dataToExport = this.facilities.map(f => ({
      'Kuruluş Adı': f.name,
      'Tür': f.typeName,
      'Şirket/Kurum': f.corporationName,
      'Telefon': f.phoneNumber,
      'E-posta': f.email,
      'Vergi No': f.taxNumber,
      'Adres': f.address,
      'Gözlem Yatak': f.observationBedCount,
      'Toplam Yatak': f.totalBedCount
    }));

    this.excel.exportToExcel(dataToExport, 'Saglik_Tesisleri_Raporu');
  }
}
