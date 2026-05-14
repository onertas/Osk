import { Component, inject, OnInit, ViewChild, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule, Table } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { SharedModule } from '../../modules/shared.module';
import { HttpApiService } from '../../services/http-api-service';
import { SwalService } from '../../services/swall.service';
import { Modal } from '../../components/modal/modal';
import { Blank } from '../../components/blank/blank';
import { Section } from '../../components/section/section';
import { CreateStaffDto, ListStaffDto, UpdateStaffDto } from '../../dtos/staff/staff-dtos';
import { ListBranchDto } from '../../dtos/branch/ListBranchDto';
import { HfManagementListDto } from '../../dtos/healthFacility/hf-management-list.dto';
import { ExcelService } from '../../services/excel.service';

@Component({
  selector: 'app-staff',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    TableModule, 
    InputTextModule, 
    ButtonModule, 
    SelectModule,
    DatePickerModule,
    SharedModule, 
    Modal, 
    Blank, 
    Section
  ],
  templateUrl: './staff.html'
})
export class StaffComponent implements OnInit, OnChanges {
  @Input() healthFacilityId: string = '';

  http = inject(HttpApiService);
  swal = inject(SwalService);
  excel = inject(ExcelService);

  @ViewChild('dt') table!: Table;
  @ViewChild(Modal) modalCom: Modal | undefined;

  staffList: ListStaffDto[] = [];
  staffSummary: any[] = [];
  branches: ListBranchDto[] = [];
  facilities: HfManagementListDto[] = [];

  newStaff: CreateStaffDto = new CreateStaffDto();
  updateStaff: UpdateStaffDto = new UpdateStaffDto();

  formatStaffNo(no: any): string {
    if (!no) return '-';
    return no.toString().padStart(4, '0');
  }

  private formatDate(date: any): string | null {
    if (!date) return null;
    const d = new Date(date);
    const month = '' + (d.getMonth() + 1);
    const day = '' + d.getDate();
    const year = d.getFullYear();

    return [year, month.padStart(2, '0'), day.padStart(2, '0')].join('-');
  }

  ngOnInit(): void {
    if (!this.healthFacilityId) {
      this.getAll();
    }
    this.loadBranches();
    this.loadFacilities();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['healthFacilityId'] && this.healthFacilityId) {
      this.getByFacilityId();
      this.getSummary();
      this.newStaff.healthFacilityId = this.healthFacilityId;
    }
  }

  getByFacilityId() {
    this.http.get<ListStaffDto[]>('Staff/GetByHealthFacilityId', { id: this.healthFacilityId }).subscribe({
      next: (res: any) => {
        if (res.success && res.data) {
          this.staffList = res.data.map((item: any) => ({
            ...item,
            code: item.code || ''
          }));
        }
      }
    });
  }

  getSummary() {
    if (!this.healthFacilityId) return;
    this.http.get<any[]>('Staff/GetStaffSummaryByHfId', { id: this.healthFacilityId }).subscribe({
      next: (res: any) => {
        if (res.success && res.data) {
          this.staffSummary = res.data;
        }
      }
    });
  }

  getAll() {
    this.http.get<ListStaffDto[]>('Staff/GetAll').subscribe({
      next: (res: any) => {
        if (res.success && res.data) {
          this.staffList = res.data.map((item: any) => ({
            ...item,
            code: item.code || '' // Null değerler filtrelemeyi bozmasın
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

  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    console.log("Filtering value:", value);
    if (this.table) {
      this.table.filterGlobal(value, 'contains');
    }
  }

  add(form: any) {
 

    const model = { ...this.newStaff };
    model.date = this.formatDate(model.date);

    this.http.post('Staff/Add', model).subscribe({
      next: (res: any) => {
        if (res.success) {
          this.swal.showSuccess("Başarıyla eklendi");
          this.modalCom?.close('addStaffModal');
          form.resetForm();
          this.newStaff = new CreateStaffDto();
          if (this.healthFacilityId) {
            this.newStaff.healthFacilityId = this.healthFacilityId;
            this.getByFacilityId();
            this.getSummary();
          } else {
            this.getAll();
          }
        }
      }
    });
  }

  edit(item: ListStaffDto) {
    this.updateStaff = {
      id: item.id,
      code: item.code,
      branchId: item.branchId,
      healthFacilityId: item.healthFacilityId,
      count: item.count,
      staffNo: item.staffNo,
      date: item.date ? new Date(item.date) : null,
      reason: item.reason
    };
  }

  update(form: any) {
    const model = { ...this.updateStaff };
    model.date = this.formatDate(model.date);

    this.http.post('Staff/Update', model).subscribe({
      next: (res: any) => {
        if (res.success) {
          this.swal.showSuccess("Başarıyla güncellendi");
          this.modalCom?.close('editStaffModal');
          if (this.healthFacilityId) {
            this.getByFacilityId();
            this.getSummary();
          } else {
            this.getAll();
          }
        }
      }
    });
  }

  delete(id: string) {
    this.swal.showConfirmation("Silmek istediğinize emin misiniz?", "Bu işlem geri alınamaz!", () => {
      this.http.post('Staff/Delete', `"${id}"`).subscribe({
        next: (res: any) => {
          if (res.success) {
            this.swal.showSuccess("Başarıyla silindi");
            if (this.healthFacilityId) {
              this.getByFacilityId();
              this.getSummary();
            } else {
              this.getAll();
            }
          }
        }
      });
    });
  }

  exportToExcel() {
    const dataToExport = this.staffList.map(s => ({
      'Kod': s.code,
      'Kuruluş': s.healthFacilityName,
      'Branş': s.branchName,
      'Sayı': s.count
    }));
    this.excel.exportToExcel(dataToExport, 'Kadro_Listesi');
  }
}
