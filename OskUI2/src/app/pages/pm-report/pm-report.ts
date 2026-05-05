import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpApiService } from '../../services/http-api-service';
import { SharedModule } from '../../modules/shared.module';
import { TableModule } from 'primeng/table';
import { SelectModule } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';

import * as XLSX from 'xlsx';

@Component({
  selector: 'app-pm-report',
  standalone: true,
  imports: [CommonModule, FormsModule, SharedModule, TableModule, SelectModule, InputTextModule, ButtonModule, DatePickerModule],
  templateUrl: './pm-report.html',
  styleUrl: './pm-report.css'
})
export class PmReportComponent implements OnInit {
  private http = inject(HttpApiService);

  // Filters
  filterModel = {
    pmTypeId: null as string | null,
    healthFacilityId: null as string | null,
    healthFacilityTypeId: null as string | null,
    isActive: null as boolean | null,
    titleId: null as string | null,
    branchId: null as string | null,
    search: ''
  };

  // Options for dropdowns
  pmTypes: any[] = [];
  facilities: any[] = [];
  facilityTypes: any[] = [];
  titles: any[] = [];
  branches: any[] = [];
  activeOptions = [
    { label: 'Hepsi', value: null },
    { label: 'Aktif (Devam Eden)', value: true },
    { label: 'Pasif (Ayrılmış)', value: false }
  ];

  // Table Data
  movements: any[] = [];
  totalRecords: number = 0;
  loading: boolean = true;
  lastEvent: any;

  ngOnInit(): void {
    this.loadFilterOptions();
  }

  loadFilterOptions() {
    this.http.get<any[]>('PmType/GetAll').subscribe(res => this.pmTypes = res.data || []);
    this.http.get<any[]>('HealthFacility/GetAll').subscribe(res => this.facilities = res.data || []);
    this.http.get<any[]>('HealthFacilityType/GetHealthFacilityTypes').subscribe(res => this.facilityTypes = res.data || []);
    this.http.get<any[]>('Title/GetAll').subscribe(res => this.titles = res.data || []);
    this.http.get<any[]>('Branch/GetAll').subscribe(res => this.branches = res.data || []);
  }

  loadReport(event?: any) {
    this.loading = true;
    this.lastEvent = event || this.lastEvent;

    const page = (this.lastEvent?.first / this.lastEvent?.rows) + 1 || 1;
    const pageSize = this.lastEvent?.rows || 10;

    const params: any = {
      page,
      pageSize,
      ...this.filterModel
    };

    this.http.get<any>('Pm/GetReport', params).subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.movements = res.data.items;
          this.totalRecords = res.data.totalCount;
        }
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  onFilterChange() {
    this.loadReport();
  }

  resetFilters() {
    this.filterModel = {
      pmTypeId: null,
      healthFacilityId: null,
      healthFacilityTypeId: null,
      isActive: null,
      titleId: null,
      branchId: null,
      search: ''
    };
    this.loadReport();
  }

  exportExcel() {
    const exportData = this.movements.map(m => ({
      'Personel': `${m.personnel?.firstName} ${m.personnel?.lastName}`,
      'TC Kimlik No': m.personnel?.identityNumber,
      'Kurum': m.healthFacility?.name,
      'Kurum Türü': m.healthFacility?.healthFacilityType?.name,
      'Hareket Tipi': m.pmType?.name,
      'Ünvan': m.branch?.title?.name,
      'Branş': m.branch?.name,
      'Başlama Tarihi': m.start ? new Date(m.start).toLocaleDateString('tr-TR') : '',
      'Ayrılış Tarihi': m.finish ? new Date(m.finish).toLocaleDateString('tr-TR') : 'Devam Ediyor',
      'Durum': m.finish ? 'Pasif' : 'Aktif',
      'SGK Durumu': m.isSgk ? 'Var' : 'Yok'
    }));

    const worksheet = XLSX.utils.json_to_sheet(exportData);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Hareket Raporu');
    
    // Column widths
    const wscols = [
      { wch: 25 }, // Personel
      { wch: 15 }, // TC
      { wch: 30 }, // Kurum
      { wch: 20 }, // Kurum Türü
      { wch: 20 }, // Hareket Tipi
      { wch: 20 }, // Ünvan
      { wch: 20 }, // Branş
      { wch: 15 }, // Başlama
      { wch: 15 }, // Ayrılış
      { wch: 10 }, // Durum
      { wch: 12 }  // SGK
    ];
    worksheet['!cols'] = wscols;

    XLSX.writeFile(workbook, `Personel_Hareket_Raporu_${new Date().getTime()}.xlsx`);
  }
}
