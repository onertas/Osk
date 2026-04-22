import { Component, inject, OnInit, ViewChild } from '@angular/core';
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
import { CreateStaffDto, ListStaffDto, UpdateStaffDto } from '../../dtos/staff/staff-dtos';
import { ListBranchDto } from '../../dtos/branch/ListBranchDto';
import { HfManagementListDto } from '../../dtos/healthFacility/hf-management-list.dto';

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
    SharedModule, 
    Modal, 
    Blank, 
    Section
  ],
  templateUrl: './staff.html'
})
export class StaffComponent implements OnInit {
  http = inject(HttpApiService);
  swal = inject(SwalService);

  @ViewChild('dt') table!: Table;
  @ViewChild(Modal) modalCom: Modal | undefined;

  staffList: ListStaffDto[] = [];
  branches: ListBranchDto[] = [];
  facilities: HfManagementListDto[] = [];

  newStaff: CreateStaffDto = new CreateStaffDto();
  updateStaff: UpdateStaffDto = new UpdateStaffDto();

  ngOnInit(): void {
    this.getAll();
    this.loadBranches();
    this.loadFacilities();
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
    this.http.post('Staff/Add', this.newStaff).subscribe({
      next: (res: any) => {
        if (res.success) {
          this.swal.showSuccess("Başarıyla eklendi");
          this.modalCom?.close('addStaffModal');
          form.resetForm();
          this.newStaff = new CreateStaffDto();
          this.getAll();
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
      count: item.count
    };
  }

  update(form: any) {
    this.http.post('Staff/Update', this.updateStaff).subscribe({
      next: (res: any) => {
        if (res.success) {
          this.swal.showSuccess("Başarıyla güncellendi");
          this.modalCom?.close('editStaffModal');
          this.getAll();
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
            this.getAll();
          }
        }
      });
    });
  }
}
