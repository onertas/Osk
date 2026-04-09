import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule, Table } from 'primeng/table';
import { CheckboxModule } from 'primeng/checkbox';
import { SharedModule } from '../../modules/shared.module';
import { Modal } from '../../components/modal/modal';
import { HttpApiService } from '../../services/http-api-service';
import { SwalService } from '../../services/swall.service';
import { CreatePmTypeDto } from '../../dtos/pmType/create-pm-type.dto';
import { ListPmTypeDto } from '../../dtos/pmType/list-pm-type.dto';
import { UpdatePmTypeDto } from '../../dtos/pmType/update-pm-type.dto';
import { Blank } from '../../components/blank/blank';
import { Section } from '../../components/section/section';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { GrupMenuComponent } from '../../components/grup-menu-component/grup-menu-component';

@Component({
  selector: 'app-pm-type',
  standalone: true,
  imports: [CommonModule, ButtonModule, InputTextModule, TableModule, CheckboxModule, SharedModule, Modal, Blank, Section, FormsModule, GrupMenuComponent],
  templateUrl: './pm-type.html'
})
export class PmTypeComponent implements OnInit {

  http = inject(HttpApiService);
  swal = inject(SwalService);

  @ViewChild('dt') table!: Table;
  @ViewChild(Modal) modalCom: Modal | undefined;

  newPmType: CreatePmTypeDto = new CreatePmTypeDto();
  updatePmType: UpdatePmTypeDto = new UpdatePmTypeDto();
  pmTypes: ListPmTypeDto[] = [];

  ngOnInit(): void {
    this.GetAll();
  }

  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.table.filterGlobal(value, 'contains');
  }

  GetAll() {
    this.http.get<ListPmTypeDto[]>('PmType/GetAll').subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.pmTypes = res.data;
        }
      }
    });
  }

  Add(form: any) {
    this.http.post('PmType/Add', this.newPmType).subscribe({
      next: (res) => {
        this.modalCom?.close('addPmTypeModal');
        form.resetForm();
        this.newPmType = new CreatePmTypeDto();
        this.GetAll();
        this.swal.showSuccess("Başarıyla eklendi");
      },
      error: (err) => {
        console.error('Error adding PmType', err);
      }
    });
  }

  Edit(item: ListPmTypeDto) {
    this.updatePmType = { ...item };
  }

  Update(form: any) {
    this.http.post('PmType/Update', this.updatePmType).subscribe({
      next: (res) => {
        this.modalCom?.close('editPmTypeModal');
        this.GetAll();
        this.swal.showSuccess("Başarıyla güncellendi");
      },
      error: (err) => {
        console.error('Error updating PmType', err);
      }
    });
  }

  Delete(id: string) {
    this.swal.showConfirmation("Silme İşlemi", "Bu kaydı silmek istediğinize emin misiniz?", () => {
      this.http.delete(`PmType/Delete?id=${id}`).subscribe({
        next: (res) => {
          this.GetAll();
          this.swal.showSuccess("Başarıyla silindi");
        },
        error: (err) => {
          console.error('Error deleting PmType', err);
        }
      });
    });
  }
}
