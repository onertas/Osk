import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule, Table } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PaginatorModule, PaginatorState } from 'primeng/paginator';
import { Tooltip } from 'primeng/tooltip';
import { SharedModule } from '../../modules/shared.module';
import { Modal } from '../../components/modal/modal';
import { Blank } from '../../components/blank/blank';
import { Section } from '../../components/section/section';
import { GrupMenuComponent } from '../../components/grup-menu-component/grup-menu-component';
import { HttpApiService } from '../../services/http-api-service';
import { SwalService } from '../../services/swall.service';
import { HfManagementListDto } from '../../dtos/healthFacility/hf-management-list.dto';
import { UpdateHealthFacilityDto } from '../../dtos/healthFacility/update-health-facility.dto';
import { CreateHealthFacilityDto } from '../../dtos/healthFacility/create-health-facility.dto';

@Component({
  selector: 'app-hf-management',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    PaginatorModule,
    Tooltip,
    SharedModule,
    Modal,
    Blank,
    Section,
    GrupMenuComponent,
  ],
  templateUrl: './hf-management.html',
})
export class HfManagementComponent implements OnInit {
  http = inject(HttpApiService);
  swal = inject(SwalService);

  @ViewChild(Modal) modalCom: Modal | undefined;

  // Tablo verisi
  facilities: HfManagementListDto[] = [];
  totalRecords = 0;
  loading = false;

  // Sayfalama
  page = 1;
  pageSize = 10;
  rowsPerPageOptions = [5, 10, 20, 50];

  // Arama (sunucu taraflı)
  searchText = '';
  private searchTimer: any;

  // HF Tipleri (dropdown için)
  hfTypes: { id: string; name: string }[] = [];

  // HF Durumları (SmartEnum'dan gelen sabitler)
  hfStatuses = [
    { value: 1, name: 'Aktif' },
    { value: 0, name: 'Kapanma' },
    { value: 3, name: 'Taşınma' },
    { value: 4, name: 'Ruhsat Askı' },
    { value: 5, name: 'Faaliyet Durdurma' }
  ];

  // Düzenleme modeli
  updateModel: UpdateHealthFacilityDto = new UpdateHealthFacilityDto();

  // Ekleme modeli
  createModel: CreateHealthFacilityDto = new CreateHealthFacilityDto();

  // Üst Kurumlar (Tıp Merkezi ve Özel Hastaneler)
  upperFacilities: HfManagementListDto[] = [];

  ngOnInit(): void {
    this.loadHfTypes();
    this.load();
    this.loadUpperFacilities();
  }

  loadUpperFacilities() {
    this.http.get<HfManagementListDto[]>('HealthFacility/GetAll').subscribe({
      next: (res: any) => {
        if (res.success && res.data) {
          this.upperFacilities = res.data.filter((f: any) => 
            f.typeName === 'Tıp Merkezi' || f.typeName === 'Özel Hastane'
          );
        }
      },
    });
  }

  load() {
    this.loading = true;
    this.http
      .get<any>(
        `HealthFacility/GetAllPaged?page=${this.page}&pageSize=${this.pageSize}&search=${this.searchText}`
      )
      .subscribe({
        next: (res) => {
          if (res.success && res.data) {
            this.facilities = res.data.items;
            this.totalRecords = res.data.totalCount;
          }
          this.loading = false;
        },
        error: () => (this.loading = false),
      });
  }

  loadHfTypes() {
    this.http.get<any>('HealthFacilityType/GetHealthFacilityTypes').subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.hfTypes = res.data.map((t: any) => ({ id: t.id, name: t.name }));
        }
      },
    });
  }

  onSearch(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    
    // En az 3 karakter veya boş olma durumu
    if (value.length > 0 && value.length < 3) return;

    clearTimeout(this.searchTimer);
    this.searchTimer = setTimeout(() => {
      this.searchText = value;
      this.page = 1;
      this.load();
    }, 600);
  }

  onPageChange(event: PaginatorState) {
    this.page = (event.page ?? 0) + 1;
    this.pageSize = event.rows ?? 10;
    this.load();
  }

  Edit(item: HfManagementListDto) {
    this.updateModel = new UpdateHealthFacilityDto();
    Object.assign(this.updateModel, item);
  }

  Update(form: any) {
    if (form.invalid) return;
    
    // Temizlik: Durum aktif/taşınma vb. ise tarihleri temizle
    if (this.updateModel.hfStatus !== 0 && this.updateModel.hfStatus !== 4 && this.updateModel.hfStatus !== 5) {
      this.updateModel.statusDate = undefined;
      this.updateModel.suspensionEndDate = undefined;
    } else if (this.updateModel.hfStatus === 0) {
      this.updateModel.suspensionEndDate = undefined;
    }

    this.http.post('HealthFacility/Update', this.updateModel).subscribe({
      next: () => {
        this.modalCom?.close('editHfModal');
        this.load();
        this.swal.showSuccess('Başarıyla güncellendi');
      },
    });
  }

  Create(form: any) {
    if (form.invalid) return;

    // Temizlik: Durum aktif/taşınma vb. ise tarihleri temizle
    if (this.createModel.hfStatus !== 0 && this.createModel.hfStatus !== 4 && this.createModel.hfStatus !== 5) {
      this.createModel.statusDate = undefined;
      this.createModel.suspensionEndDate = undefined;
    } else if (this.createModel.hfStatus === 0) {
      this.createModel.suspensionEndDate = undefined;
    }

    this.http.post('HealthFacility/Add', this.createModel).subscribe({
      next: () => {
        this.modalCom?.close('addHfModal');
        this.load();
        this.swal.showSuccess('Kuruluş başarıyla eklendi');
        this.createModel = new CreateHealthFacilityDto();
        form.resetForm();
      },
    });
  }

  resetCreateModel() {
    this.createModel = new CreateHealthFacilityDto();
  }

  Delete(id: string) {
    this.swal.showConfirmation(
      'Silmek istediğinize emin misiniz?',
      'Bu kuruluşu sildiğinizde geri alamazsınız.',
      () => {
        this.http.post('HealthFacility/Delete', `"${id}"`).subscribe({
          next: (res: any) => {
            if (res.success) {
              this.load();
              this.swal.showSuccess('Başarıyla silindi');
            } else {
              this.swal.showError(res.message || 'Silinemedi');
            }
          },
          error: (err: any) => {
            if (err.error && err.error.message) {
              this.swal.showError(err.error.message);
            } else {
              this.swal.showError('Kuruluş silinirken bir hata oluştu.');
            }
          }
        });
      }
    );
  }
}
