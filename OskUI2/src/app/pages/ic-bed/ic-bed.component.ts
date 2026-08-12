import { Component, inject, Input, Output, EventEmitter, OnInit, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule, Table } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { SelectModule } from 'primeng/select';
import { SharedModule } from '../../modules/shared.module';
import { Modal } from '../../components/modal/modal';
import { HttpApiService } from '../../services/http-api-service';
import { SwalService } from '../../services/swall.service';
import { ExcelService } from '../../services/excel.service';
import { ListIcBedDto } from '../../dtos/beds/list-ic-bed.dto';
import { CreateIcBedDto } from '../../dtos/beds/create-ic-bed.dto';
import { UpdateIcBedDto } from '../../dtos/beds/update-ic-bed.dto';
import { HfManagementListDto } from '../../dtos/healthFacility/hf-management-list.dto';

@Component({
  selector: 'app-ic-bed',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    DatePickerModule,
    InputTextModule,
    CheckboxModule,
    SelectModule,
    SharedModule,
    Modal
  ],
  templateUrl: './ic-bed.component.html',
})
export class IcBedComponent implements OnInit, OnChanges {
  @Input() healthFacilityId: string = '';
  @Input() isReadOnly: boolean = false;
  @Input() showFilters: boolean = true;
  @Output() totalBedsCount = new EventEmitter<number>();
  
  @ViewChild('dt') table!: Table;
  @ViewChild(Modal) modalCom: Modal | undefined;

  http = inject(HttpApiService);
  swal = inject(SwalService);
  excel = inject(ExcelService);

  beds: ListIcBedDto[] = [];
  allBeds: ListIcBedDto[] = [];
  filteredBeds: ListIcBedDto[] = [];
  showInactive: boolean = false;
  
  newBed: CreateIcBedDto = new CreateIcBedDto();
  updateBed: UpdateIcBedDto = new UpdateIcBedDto();

  bedSummary = {
    eriskin: 0,
    cocuk: 0,
    yenidogan: 0,
    ybToplam: 0,
    servis: 0,
    genelToplam: 0
  };

  // Filters
  bedFilterModel = {
    healthFacilityId: null as string | null,
    icBedType: null as number | null,
    icBedNameId: null as string | null,
    icBedRegLevel: null as number | null,
    icBedRegType: null as number | null,
    isActive: true as boolean | null,
    search: ''
  };

  // Lookup listeleri
  bedTypes: any[] = [];
  bedNames: any[] = [];
  filteredBedNames: any[] = []; // Used in Add/Edit modals
  regLevels: any[] = [];
  regTypes: any[] = [];
  facilities: HfManagementListDto[] = [];
  activeOptions = [
    { label: 'Hepsi', value: null },
    { label: 'Aktif', value: true },
    { label: 'Pasif', value: false }
  ];

  ngOnInit(): void {
    this.GetAll();
    this.GetLookups();
    this.loadFacilities();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['healthFacilityId']) {
      this.bedFilterModel.healthFacilityId = this.healthFacilityId;
      this.GetAll();
    }
  }

  resetCreateModel() {
    this.newBed = new CreateIcBedDto();
    this.newBed.isActive = true;
    this.filteredBedNames = [];
  }

  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.bedFilterModel.search = value;
    this.applyBedFilters();
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

    this.excel.exportToExcel(dataToExport, 'Yogun_Bakim_Yataklari');
  }

  GetAll() {
    let url = 'IcBed/GetAll';
    if (this.healthFacilityId) {
      url = `IcBed/GetAllByHfId?healthFacilityId=${this.healthFacilityId}`;
      this.bedFilterModel.healthFacilityId = this.healthFacilityId;
    }

    this.http.get<ListIcBedDto[]>(url).subscribe(res => {
      if (res.success && res.data) {
        this.allBeds = res.data;
        this.applyBedFilters();
      }
    });
  }

  applyBedFilters() {
    this.filteredBeds = this.allBeds.filter(bed => {
      const matchHf = this.bedFilterModel.healthFacilityId === null || bed.healthFacilityId === this.bedFilterModel.healthFacilityId;
      const matchType = this.bedFilterModel.icBedType === null || bed.icBedType === this.bedFilterModel.icBedType;
      const matchName = this.bedFilterModel.icBedNameId === null || bed.icBedNameId === this.bedFilterModel.icBedNameId;
      const matchLevel = this.bedFilterModel.icBedRegLevel === null || bed.icBedRegLevel === this.bedFilterModel.icBedRegLevel;
      const matchRegType = this.bedFilterModel.icBedRegType === null || bed.icBedRegType === this.bedFilterModel.icBedRegType;
      const matchActive = this.bedFilterModel.isActive === null || bed.isActive === this.bedFilterModel.isActive;
      
      const searchStr = this.bedFilterModel.search.toLocaleLowerCase('tr-TR');
      const matchSearch = !searchStr || 
        (bed.icBedName?.toLocaleLowerCase('tr-TR').includes(searchStr)) ||
        (bed.icBedRegNumber?.toLocaleLowerCase('tr-TR').includes(searchStr)) ||
        (bed.healthFacilityName?.toLocaleLowerCase('tr-TR').includes(searchStr));

      return matchHf && matchType && matchName && matchLevel && matchRegType && matchActive && matchSearch;
    });

    this.calculateBedSummary();

    // Aktif yatakların toplam adedini hesapla (tüm data üzerinden)
    const totalActiveCount = this.allBeds
      .filter(b => b.isActive)
      .reduce((sum, current) => sum + (current.quantity || 0), 0);
    this.totalBedsCount.emit(totalActiveCount);
  }

  calculateBedSummary() {
    const activeBeds = this.filteredBeds.filter(b => b.isActive);

    this.bedSummary.eriskin = activeBeds
      .filter(b => b.icBedType === 1 || b.icBedTypeName?.toLocaleLowerCase('tr-TR').includes('erişkin'))
      .reduce((sum, b) => sum + (b.quantity || 0), 0);

    this.bedSummary.cocuk = activeBeds
      .filter(b => b.icBedType === 2 || b.icBedTypeName?.toLocaleLowerCase('tr-TR').includes('çocuk'))
      .reduce((sum, b) => sum + (b.quantity || 0), 0);

    this.bedSummary.yenidogan = activeBeds
      .filter(b => b.icBedType === 3 || b.icBedTypeName?.toLocaleLowerCase('tr-TR').includes('yenidoğan') || b.icBedTypeName?.toLocaleLowerCase('tr-TR').includes('yenidogan'))
      .reduce((sum, b) => sum + (b.quantity || 0), 0);

    this.bedSummary.ybToplam = this.bedSummary.eriskin + this.bedSummary.cocuk + this.bedSummary.yenidogan;

    this.bedSummary.servis = activeBeds
      .filter(b => b.icBedType === 4 || b.icBedTypeName?.toLocaleLowerCase('tr-TR').includes('servis'))
      .reduce((sum, b) => sum + (b.quantity || 0), 0);

    this.bedSummary.genelToplam = this.bedSummary.ybToplam + this.bedSummary.servis;
  }

  resetBedFilters() {
    this.bedFilterModel = {
      healthFacilityId: null,
      icBedType: null,
      icBedNameId: null,
      icBedRegLevel: null,
      icBedRegType: null,
      isActive: true,
      search: ''
    };
    this.applyBedFilters();
  }

  loadFacilities() {
    this.http.get<HfManagementListDto[]>('HealthFacility/GetAll').subscribe(res => {
      if (res.success && res.data) {
        this.facilities = res.data.filter(f => 
          f.typeName === 'Özel Hastane' || 
          f.typeName === 'Tıp Merkezi'
        );
      }
    });
  }

  GetLookups() {
    this.http.get<any[]>('IcBed/GetIcBedTypes').subscribe(res => this.bedTypes = res.data || []);
    this.http.get<any[]>('IcBed/GetIcBedNames').subscribe(res => this.bedNames = res.data || []);
    this.http.get<any[]>('IcBed/GetIcBedRegLevels').subscribe(res => this.regLevels = res.data || []);
    this.http.get<any[]>('IcBed/GetIcBedRegTypes').subscribe(res => this.regTypes = res.data || []);
  }

  // Used for Add/Edit modals specifically
  onTypeChange(event: any, mode: 'add' | 'edit') {
    const typeValue: number = (event !== null && typeof event === 'object' && 'value' in event)
      ? event.value
      : Number(event);

    if (!typeValue) {
      this.filteredBedNames = [];
      if (mode === 'add') {
        this.newBed.icBedNameId = '';
      }
      return;
    }

    this.http.get<any[]>(`IcBed/GetIcBedNames?typeValue=${typeValue}`).subscribe(res => {
      if (res.success && res.data && res.data.length > 0) {
        this.filteredBedNames = res.data;
        if (mode === 'add') {
          this.newBed.icBedNameId = this.filteredBedNames[0].id;
        } else if (mode === 'edit') {
          const exists = this.filteredBedNames.some(item => item.id === this.updateBed.icBedNameId);
          if (!exists) {
            this.updateBed.icBedNameId = this.filteredBedNames[0].id;
          }
        }
      } else {
        this.filteredBedNames = [];
        if (mode === 'add') {
          this.newBed.icBedNameId = '';
        }
      }
    });
  }

  Add(form: any) {
    if (this.healthFacilityId) {
      this.newBed.healthFacilityId = this.healthFacilityId;
    }

    this.newBed.isActive = true;

    if (!this.newBed.icBedNameId) {
      this.swal.showError('Lütfen önce Yatak Tipi seçiniz.');
      return;
    }

    this.http.post('IcBed/Add', this.newBed).subscribe(res => {
      if (res.success) {
        this.modalCom?.close('addIcBedModal');
        form.resetForm();
        this.newBed = new CreateIcBedDto();
        this.newBed.isActive = true;
        this.filteredBedNames = [];
        this.GetAll();
        this.swal.showSuccess('Eklendi');
      } else {
        this.swal.showError(res.message || 'Ekleme işlemi başarısız oldu.');
      }
    });
  }

  Edit(bed: ListIcBedDto) {
    this.updateBed = {
      id: bed.id,
      healthFacilityId: bed.healthFacilityId,
      icBedRegLevel: bed.icBedRegLevel,
      icBedRegType: bed.icBedRegType,
      quantity: bed.quantity,
      icBedRegDate: new Date(bed.icBedRegDate),
      icBedRegNumber: bed.icBedRegNumber,
      icBedNameId: bed.icBedNameId,
      icBedType: bed.icBedType,
      isActive: bed.isActive
    };
    this.onTypeChange(bed.icBedType, 'edit');
  }

  Update(form: any) {
    if (!this.updateBed.icBedNameId) {
      this.swal.showError('Geçerli bir yatak türü belirlenemedi.');
      return;
    }

    this.http.post('IcBed/Update', this.updateBed).subscribe(res => {
      if (res.success) {
        this.modalCom?.close('editIcBedModal');
        this.GetAll();
        this.swal.showSuccess('Güncellendi');
      } else {
        this.swal.showError(res.message || 'Güncelleme işlemi başarısız oldu.');
      }
    });
  }

  Delete(id: string) {
    this.swal.showConfirmation('Silmek istediğinize emin misiniz?', 'Bu işlem geri alınamaz!', () => {
      this.http.post('IcBed/Delete', `"${id}"`).subscribe(res => {
        if (res.success) {
          this.GetAll();
          this.swal.showSuccess('Silindi');
        }
      });
    });
  }

  cancelRegistration() {
    this.swal.showConfirmation('Tescil İptal', 'Bu yatağın tescilini iptal etmek istediğinize emin misiniz?', () => {
      this.updateBed.isActive = false;
      this.http.post('IcBed/Update', this.updateBed).subscribe(res => {
        if (res.success) {
          this.modalCom?.close('editIcBedModal');
          this.GetAll();
          this.swal.showSuccess('Tescil iptal edildi');
        } else {
          this.swal.showError(res.message || 'Tescil iptal işlemi başarısız oldu.');
        }
      });
    });
  }
}
