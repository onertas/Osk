import { Component, inject, Input, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { SelectModule } from 'primeng/select';
import { SharedModule } from '../../modules/shared.module';
import { Modal } from '../../components/modal/modal';
import { HttpApiService } from '../../services/http-api-service';
import { SwalService } from '../../services/swall.service';
import { ListIcBedDto } from '../../dtos/beds/list-ic-bed.dto';
import { CreateIcBedDto } from '../../dtos/beds/create-ic-bed.dto';
import { UpdateIcBedDto } from '../../dtos/beds/update-ic-bed.dto';

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
export class IcBedComponent implements OnInit {
  @Input() healthFacilityId: string = '';
  @ViewChild(Modal) modalCom: Modal | undefined;

  http = inject(HttpApiService);
  swal = inject(SwalService);

  beds: ListIcBedDto[] = [];
  newBed: CreateIcBedDto = new CreateIcBedDto();
  updateBed: UpdateIcBedDto = new UpdateIcBedDto();

  // Lookups
  bedTypes: any[] = [];
  bedNames: any[] = [];
  filteredBedNames: any[] = [];
  regLevels: any[] = [];
  regTypes: any[] = [];

  selectedType: number | null = null;
  selectedUpdateType: number | null = null;

  ngOnInit(): void {
    if (this.healthFacilityId) {
      this.GetAll();
    }
    this.GetLookups();
  }

  GetAll() {
    this.http.get<ListIcBedDto[]>(`IcBed/GetAllByHfId?healthFacilityId=${this.healthFacilityId}`).subscribe(res => {
      if (res.success && res.data) {
        this.beds = res.data;
      }
    });
  }

  GetLookups() {
    this.http.get<any[]>('IcBed/GetIcBedTypes').subscribe(res => {
      if (res.success && res.data) this.bedTypes = res.data;
    });
    this.http.get<any[]>('IcBed/GetIcBedRegLevels').subscribe(res => {
      if (res.success && res.data) this.regLevels = res.data;
    });
    this.http.get<any[]>('IcBed/GetIcBedRegTypes').subscribe(res => {
      if (res.success && res.data) this.regTypes = res.data;
    });
    // Initial fetch of all names for reference if needed, or we fetch on type change
    this.http.get<any[]>('IcBed/GetIcBedNames').subscribe(res => {
      if (res.success && res.data) this.bedNames = res.data;
    });
  }

  onTypeChange(typeValue: any, mode: 'add' | 'edit') {
    const value = typeValue?.value || typeValue;
    this.http.get<any[]>(`IcBed/GetIcBedNames?typeValue=${value}`).subscribe(res => {
      if (res.success && res.data) {
        this.filteredBedNames = res.data;
      }
    });
  }

  Add(form: any) {
    this.newBed.healthFacilityId = this.healthFacilityId;
    this.http.post('IcBed/Add', this.newBed).subscribe(res => {
      if (res.success) {
        this.modalCom?.close('addIcBedModal');
        form.resetForm();
        this.newBed = new CreateIcBedDto();
        this.selectedType = null;
        this.GetAll();
        this.swal.showSuccess("Eklendi");
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
      isActive: bed.isActive
    };
    this.selectedUpdateType = bed.icBedType;
    this.onTypeChange(bed.icBedType, 'edit');
  }

  Update(form: any) {
    this.http.post('IcBed/Update', this.updateBed).subscribe(res => {
      if (res.success) {
        this.modalCom?.close('editIcBedModal');
        this.GetAll();
        this.swal.showSuccess("Güncellendi");
      }
    });
  }

  Delete(id: string) {
    this.swal.showConfirmation("Silmek istediğinize emin misiniz?", "Bu işlem geri alınamaz!", () => {
      this.http.post('IcBed/Delete', `"${id}"`).subscribe(res => {
        if (res.success) {
          this.GetAll();
          this.swal.showSuccess("Silindi");
        }
      });
    });
  }
}
