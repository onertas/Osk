import { Component, inject, OnInit, ViewChild, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule, Table } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { SharedModule } from '../../modules/shared.module';
import { HttpApiService } from '../../services/http-api-service';
import { SwalService } from '../../services/swall.service';
import { Modal } from '../../components/modal/modal';
import { Blank } from '../../components/blank/blank';
import { Section } from '../../components/section/section';
import { CreateTemporarayStaffDto, ListTemporarayStaffDto, UpdateTemporarayStaffDto } from '../../dtos/staff/staff-dtos';
import { ListBranchDto } from '../../dtos/branch/ListBranchDto';
import { HfManagementListDto } from '../../dtos/healthFacility/hf-management-list.dto';
import { ListPmTypeDto } from '../../dtos/pmType/list-pm-type.dto';

@Component({
  selector: 'app-temporaray-staff',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    TableModule, 
    InputTextModule, 
    ButtonModule, 
    SelectModule,
    SharedModule, 
    Modal, 
    Blank, 
    Section
  ],
  templateUrl: './temporaray-staff.html'
})
export class TemporarayStaffComponent implements OnInit, OnChanges {
  @Input() healthFacilityId: string = '';

  http = inject(HttpApiService);
  swal = inject(SwalService);

  @ViewChild('dt') table!: Table;
  @ViewChild(Modal) modalCom: Modal | undefined;

  tempStaffList: ListTemporarayStaffDto[] = [];
  branches: ListBranchDto[] = [];
  facilities: HfManagementListDto[] = [];
  pmTypes: ListPmTypeDto[] = [];

  newTempStaff: CreateTemporarayStaffDto = new CreateTemporarayStaffDto();
  updateTempStaff: UpdateTemporarayStaffDto = new UpdateTemporarayStaffDto();

  ngOnInit(): void {
    if (!this.healthFacilityId) {
      this.getAll();
    }
    this.loadBranches();
    this.loadFacilities();
    this.loadPmTypes();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['healthFacilityId'] && this.healthFacilityId) {
      this.getByFacilityId();
      this.newTempStaff.healthFacilityId = this.healthFacilityId;
    }
  }

  getByFacilityId() {
    this.http.get<ListTemporarayStaffDto[]>('TemporarayStaff/GetByHealthFacilityId', { id: this.healthFacilityId }).subscribe({
      next: (res: any) => {
        if (res.success && res.data) {
          this.tempStaffList = res.data.map((item: any) => ({
            ...item,
            code: item.code || ''
          }));
        }
      }
    });
  }

  getAll() {
    this.http.get<ListTemporarayStaffDto[]>('TemporarayStaff/GetAll').subscribe({
      next: (res: any) => {
        if (res.success && res.data) {
          this.tempStaffList = res.data.map((item: any) => ({
            ...item,
            code: item.code || ''
          }));
        }
      }
    });
  }

  loadBranches() {
    this.http.get<ListBranchDto[]>('Branch/GetAll').subscribe({
      next: (res: any) => {
        if (res.success && res.data) {
          this.branches = res.data;
        }
      }
    });
  }

  loadFacilities() {
    this.http.get<HfManagementListDto[]>('HealthFacility/GetAll').subscribe({
      next: (res: any) => {
        if (res.success && res.data) {
          this.facilities = res.data;
        }
      }
    });
  }

  loadPmTypes() {
    this.http.get<ListPmTypeDto[]>('PmType/GetAll').subscribe({
      next: (res: any) => {
        if (res.success && res.data) {
          this.pmTypes = res.data;
        }
      }
    });
  }

  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    console.log("Filtering value (temp staff):", value);
    if (this.table) {
      this.table.filterGlobal(value, 'contains');
    }
  }

  add(form: any) {
    const exists = this.tempStaffList.some(
      s => s.healthFacilityId === this.newTempStaff.healthFacilityId && 
           s.branchId === this.newTempStaff.branchId && 
           s.pmTypeId === this.newTempStaff.pmTypeId
    );
    if (exists) {
      this.swal.showWarning("Bu kurum için aynı tipte bu kadro zaten eklenmiş.");
      return;
    }

    this.http.post('TemporarayStaff/Add', this.newTempStaff).subscribe({
      next: (res: any) => {
        if (res.success) {
          this.swal.showSuccess("Başarıyla eklendi");
          this.modalCom?.close('addTempStaffModal');
          form.resetForm();
          this.newTempStaff = new CreateTemporarayStaffDto();
          if (this.healthFacilityId) {
            this.newTempStaff.healthFacilityId = this.healthFacilityId;
            this.getByFacilityId();
          } else {
            this.getAll();
          }
        }
      }
    });
  }

  edit(item: ListTemporarayStaffDto) {
    this.updateTempStaff = {
      id: item.id,
      code: item.code,
      branchId: item.branchId,
      healthFacilityId: item.healthFacilityId,
      pmTypeId: item.pmTypeId,
      count: item.count
    };
  }

  update(form: any) {
    this.http.post('TemporarayStaff/Update', this.updateTempStaff).subscribe({
      next: (res: any) => {
        if (res.success) {
          this.swal.showSuccess("Başarıyla güncellendi");
          this.modalCom?.close('editTempStaffModal');
          if (this.healthFacilityId) {
            this.getByFacilityId();
          } else {
            this.getAll();
          }
        }
      }
    });
  }

  delete(id: string) {
    this.swal.showConfirmation("Silmek istediğinize emin misiniz?", "Bu işlem geri alınamaz!", () => {
      this.http.post('TemporarayStaff/Delete', `"${id}"`).subscribe({
        next: (res: any) => {
          if (res.success) {
            this.swal.showSuccess("Başarıyla silindi");
            if (this.healthFacilityId) {
              this.getByFacilityId();
            } else {
              this.getAll();
            }
          }
        }
      });
    });
  }
}
