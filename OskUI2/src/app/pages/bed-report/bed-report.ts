import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { SelectModule } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { SharedModule } from '../../modules/shared.module';
import { HttpApiService } from '../../services/http-api-service';
import { ExcelService } from '../../services/excel.service';
import { ListIcBedDto } from '../../dtos/beds/list-ic-bed.dto';

@Component({
  selector: 'app-bed-report',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    SelectModule,
    InputTextModule,
    ButtonModule,
    SharedModule
  ],
  templateUrl: './bed-report.html',
  styleUrls: ['./bed-report.css']
})
export class BedReportComponent implements OnInit {
  private http = inject(HttpApiService);
  private excel = inject(ExcelService);

  // Filters
  filterModel = {
    healthFacilityId: null as string | null,
    icBedType: null as number | null,
    icBedRegLevel: null as number | null,
    icBedRegType: null as number | null,
    isActive: true as boolean | null,
    search: ''
  };

  // Lookups
  facilities: any[] = [];
  bedTypes: any[] = [];
  regLevels: any[] = [];
  regTypes: any[] = [];
  activeOptions = [
    { label: 'Hepsi', value: null },
    { label: 'Aktif', value: true },
    { label: 'Pasif', value: false }
  ];

  // Data
  allBeds: ListIcBedDto[] = [];
  filteredBeds: ListIcBedDto[] = [];
  loading: boolean = false;

  // Stats
  stats = {
    totalBeds: 0,
    activeBeds: 0,
    passiveBeds: 0,
    eriskin: 0,
    cocuk: 0,
    yenidogan: 0,
    servis: 0
  };

  ngOnInit(): void {
    this.loadLookups();
    this.loadData();
  }

  loadLookups() {
    this.http.get<any[]>('HealthFacility/GetAll').subscribe(res => this.facilities = res.data || []);
    this.http.get<any[]>('IcBed/GetIcBedTypes').subscribe(res => this.bedTypes = res.data || []);
    this.http.get<any[]>('IcBed/GetIcBedRegLevels').subscribe(res => this.regLevels = res.data || []);
    this.http.get<any[]>('IcBed/GetIcBedRegTypes').subscribe(res => this.regTypes = res.data || []);
  }

  loadData() {
    this.loading = true;
    this.http.get<ListIcBedDto[]>('IcBed/GetAll').subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.allBeds = res.data;
          this.applyFilters();
        }
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  applyFilters() {
    this.filteredBeds = this.allBeds.filter(bed => {
      const matchHf = this.filterModel.healthFacilityId === null || bed.healthFacilityId === this.filterModel.healthFacilityId;
      const matchType = this.filterModel.icBedType === null || bed.icBedType === this.filterModel.icBedType;
      const matchLevel = this.filterModel.icBedRegLevel === null || bed.icBedRegLevel === this.filterModel.icBedRegLevel;
      const matchRegType = this.filterModel.icBedRegType === null || bed.icBedRegType === this.filterModel.icBedRegType;
      const matchActive = this.filterModel.isActive === null || bed.isActive === this.filterModel.isActive;
      
      const searchStr = this.filterModel.search.toLocaleLowerCase('tr-TR');
      const matchSearch = !searchStr || 
        (bed.icBedName?.toLocaleLowerCase('tr-TR').includes(searchStr)) ||
        (bed.icBedRegNumber?.toLocaleLowerCase('tr-TR').includes(searchStr)) ||
        (bed.healthFacilityName?.toLocaleLowerCase('tr-TR').includes(searchStr));

      return matchHf && matchType && matchLevel && matchRegType && matchActive && matchSearch;
    });

    this.calculateStats();
  }

  calculateStats() {
    // Stats are calculated based on the FILTERED list to reflect current view
    this.stats.totalBeds = this.filteredBeds.reduce((sum, b) => sum + (b.quantity || 0), 0);
    this.stats.activeBeds = this.filteredBeds.filter(b => b.isActive).reduce((sum, b) => sum + (b.quantity || 0), 0);
    this.stats.passiveBeds = this.filteredBeds.filter(b => !b.isActive).reduce((sum, b) => sum + (b.quantity || 0), 0);
    
    // Type stats
    this.stats.eriskin = this.filteredBeds.filter(b => b.icBedType === 1).reduce((sum, b) => sum + (b.quantity || 0), 0);
    this.stats.cocuk = this.filteredBeds.filter(b => b.icBedType === 2).reduce((sum, b) => sum + (b.quantity || 0), 0);
    this.stats.yenidogan = this.filteredBeds.filter(b => b.icBedType === 3).reduce((sum, b) => sum + (b.quantity || 0), 0);
    this.stats.servis = this.filteredBeds.filter(b => b.icBedType === 4).reduce((sum, b) => sum + (b.quantity || 0), 0);
  }

  resetFilters() {
    this.filterModel = {
      healthFacilityId: null,
      icBedType: null,
      icBedRegLevel: null,
      icBedRegType: null,
      isActive: true,
      search: ''
    };
    this.applyFilters();
  }

  exportToExcel() {
    const dataToExport = this.filteredBeds.map(b => ({
      'Kuruluş': b.healthFacilityName,
      'Yatak Türü': b.icBedName,
      'Tip': b.icBedTypeName,
      'Tescil Seviyesi': b.icBedRegLevelName,
      'Tescil Türü': b.icBedRegTypeName,
      'Adet': b.quantity,
      'Tescil No': b.icBedRegNumber,
      'Tescil Tarihi': b.icBedRegDate ? new Date(b.icBedRegDate).toLocaleDateString('tr-TR') : '-',
      'Durum': b.isActive ? 'Aktif' : 'Pasif'
    }));

    this.excel.exportToExcel(dataToExport, 'Yatak_Raporu');
  }
}
