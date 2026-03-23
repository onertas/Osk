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
import { TableModule, Table } from 'primeng/table';
import { HttpApiService } from '../../services/http-api-service';
import { ListPersonnelDto } from '../../dtos/personnel/ListPersonnelDto';
import { SwalService } from '../../services/swall.service';





@Component({
  selector: 'app-personnel',
  imports: [ButtonModule, Modal, InputMaskModule, DatePickerModule, InputTextModule, SharedModule, MultiSelectModule, TableModule, Modal],
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
  personnel: ListPersonnelDto[] = [];
  branches: any[] = [];
  //#endregion

  //#region METHODS
  ngOnInit(): void {
    this.GetAll();
    this.GetBranches();
  }

  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.table.filterGlobal(value, 'contains');
  }
  Add(form: any) {

if(!this.newPersonnel.personnelBranches || this.newPersonnel.personnelBranches.length == 0){
 this.swal.showWarning("Lütfen en az bir branş seçin");
  return;
}

    this.http.post('personnel/add', this.newPersonnel).subscribe({
      next: (response) => {

        this.modalCom?.close('ac');
        form.resetForm();
        this.newPersonnel = new CreatePersonnelDto();
        this.GetAll();
      },
      error: (err) => {
        console.error('Error adding personnel', err);
      }
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
  //#endregion

}
