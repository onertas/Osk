import { Modal } from './../../components/modal/modal';
import { CreatePersonnelDto } from './../../dtos/personnel/CreatePersonnelDto';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { InputMaskModule } from 'primeng/inputmask';
import * as XLSX from 'xlsx';
import { InputTextModule } from 'primeng/inputtext';
import { SharedModule } from '../../modules/shared.module';
import { MultiSelectModule } from 'primeng/multiselect';
import { SelectModule } from 'primeng/select';
import { TableModule, Table } from 'primeng/table';
import { HttpApiService } from '../../services/http-api-service';
import { ListPersonnelDto } from '../../dtos/personnel/ListPersonnelDto';
import { SwalService } from '../../services/swall.service';
import { UpdatePersonnelDto } from '../../dtos/personnel/UpdatePersonnelDto';





@Component({
  selector: 'app-personnel',
  imports: [ButtonModule, Modal, InputMaskModule, DatePickerModule, InputTextModule, SharedModule, MultiSelectModule, SelectModule, TableModule, Modal],
  templateUrl: './personnel.html',
  styleUrl: './personnel.css',
})
export class Personnel implements OnInit {

  //#region DEPENDENCIES AND VARIABLES
  http = inject(HttpApiService);
  swal=inject(SwalService);
  @ViewChild('dt') table!: Table;
  @ViewChild(Modal) modalCom: Modal | undefined;

  date: Date = new Date();
  newPersonnel: CreatePersonnelDto = new CreatePersonnelDto();
  updatePersonnelModel: UpdatePersonnelDto = new UpdatePersonnelDto();
  personnel: ListPersonnelDto[] = [];
  branches: any[] = [];
  titles: any[] = [];
  filteredBranches: any[] = [];
  selectedTitleId: string = "";
  //#endregion

  //#region METHODS
  ngOnInit(): void {
    this.GetAll();
    this.GetBranches();
    this.GetTitles();
  }

  ResetForm() {
    this.newPersonnel = new CreatePersonnelDto();
    this.selectedTitleId = "";
    this.filteredBranches = [];
  }

  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.table.filterGlobal(value, 'contains');
  }
  Add(form: any) {
    if (!this.newPersonnel.personnelBranches || this.newPersonnel.personnelBranches.length == 0) {
      this.swal.showWarning("Lütfen en az bir branş seçin");
      return;
    }

    this.http.post('personnel/add', this.newPersonnel).subscribe({
      next: (response) => {
        if (response.success) {
          this.swal.showSuccess("Eklendi");
          this.modalCom?.close('acAdd');
          form.resetForm();
          this.newPersonnel = new CreatePersonnelDto();
          this.GetAll();
        } else {
          this.swal.showError(response.message || "Bir hata oluştu");
        }
      }
    });
  }

  Update(form: any) {
      
    if (!this.updatePersonnelModel.personnelBranches || this.updatePersonnelModel.personnelBranches.length == 0) {
      this.swal.showWarning("Lütfen en az bir branş seçin");
      return;
    }


     
         this.http.post('personnel/update', this.updatePersonnelModel).subscribe({
      next: (response) => {
        if (response.success) {
          this.swal.showSuccess("Güncellendi");
          this.modalCom?.close('acEdit');
        
          this.GetAll();
        } else {
          this.swal.showError(response.message || "Bir hata oluştu");
        }
      }
    });
      
      
 
  }

  Edit(person: ListPersonnelDto) {

    this.updatePersonnelModel = {
      id: person.id,
      identityNumber: person.identityNumber,
      firstName: person.firstName,
      lastName: person.lastName,
      email: person.email,
      phoneNumber: person.phoneNumber,
      isBanned: person.isBanned,
      birthDate: person.birthDate ? new Date(person.birthDate) : undefined,
      personnelBranches: [...person.branchIds]
    };

    // Find the titleId from the first branch if available
    if (person.branchIds && person.branchIds.length > 0) {
      const firstBranch = this.branches.find(b => b.id === person.branchIds[0]);
      if (firstBranch) {
        this.selectedTitleId = firstBranch.titleId;
        this.onTitleChange(this.selectedTitleId);
      }
    } else {
      this.selectedTitleId = "";
      this.filteredBranches = [];
    }
  }

  Delete(id: string) {
    this.swal.showConfirmation("Silmek istediğinize emin misiniz?", "Bu işlem geri alınamaz!", () => {
      this.http.delete(`personnel/delete?id=${id}`).subscribe(res => {
        if (res.success) {
          this.swal.showSuccess("Silindi");
          this.GetAll();
        }
      });
    });
  }

  GetAll() {
    this.http.get<ListPersonnelDto[]>('personnel/getall').subscribe(res => {

      if (res.success && res.data) {
        this.personnel = res.data;
       
      }
    });
  }

  GetBranches() {
    this.http.get<any[]>('branch/getall').subscribe(res => {
      if (res.success && res.data) {
        this.branches = res.data;
      }
    });
  }

  GetTitles() {
    this.http.get<any[]>('title/getall').subscribe(res => {
      if (res.success && res.data) {
        this.titles = res.data;
      }
    });
  }

  onTitleChange(titleId: string) {
    this.filteredBranches = this.branches.filter(b => b.titleId === titleId);
    const validBranchIds = this.filteredBranches.map(b => b.id);

    if (this.newPersonnel.personnelBranches) {
      this.newPersonnel.personnelBranches = this.newPersonnel.personnelBranches.filter(id => validBranchIds.includes(id));
    }

    if (this.updatePersonnelModel.personnelBranches) {
      this.updatePersonnelModel.personnelBranches = this.updatePersonnelModel.personnelBranches.filter(id => validBranchIds.includes(id));
    }
  }
  //#endregion

}
