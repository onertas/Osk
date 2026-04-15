import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule, Table } from 'primeng/table';
import { SharedModule } from '../../modules/shared.module';
import { Modal } from '../../components/modal/modal';
import { HttpApiService } from '../../services/http-api-service';
import { SwalService } from '../../services/swall.service';
import { CreateBranchDto } from '../../dtos/branch/CreateBranchDto';
import { ListBranchDto } from '../../dtos/branch/ListBranchDto';
import { ListTitleDto } from '../../dtos/title/ListTitleDto';
import { UpdateBranchDto } from '../../dtos/branch/UpdateBranchDto';

@Component({
  selector: 'app-branch',
  imports: [ButtonModule, InputTextModule, TableModule, SharedModule, Modal],
  templateUrl: './branch.html',
  styleUrl: './branch.css',
})
export class BranchComponent implements OnInit {

  http = inject(HttpApiService);
  swal = inject(SwalService);
  @ViewChild('dt') table!: Table;
  @ViewChild(Modal) modalCom: Modal | undefined;

  newBranch: CreateBranchDto = new CreateBranchDto();
  updateBranch: UpdateBranchDto = new UpdateBranchDto();
  branches: ListBranchDto[] = [];
  titles: ListTitleDto[] = [];

  branchTypes = [
    { value: 0, label: 'Yok' },
    { value: 1, label: 'Ana Dal' },
    { value: 2, label: 'Yan Dal' }
  ];

  ngOnInit(): void {
    this.GetAll();
    this.GetTitles();
  }

  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.table.filterGlobal(value, 'contains');
  }

  Add(form: any) {
    this.newBranch.branchTypeId = Number(this.newBranch.branchTypeId); // Ensure it's passed as number
    this.http.post('branch/add', this.newBranch).subscribe({
      next: (response) => {
        this.modalCom?.close('addBranchModal');
        form.resetForm();
        this.newBranch = new CreateBranchDto();
        this.GetAll();
      },
      error: (err) => {
        console.error('Error adding branch', err);
      }
    });
  }

  GetAll() {
    this.http.get<ListBranchDto[]>('branch/getall').subscribe(res => {
      if (res.success && res.data) {
        this.branches = res.data;
      }
    });
  }

  GetTitles() {
    this.http.get<ListTitleDto[]>('title/getall').subscribe(res => {
      if (res.success && res.data) {
        this.titles = res.data;
      }
    });
  }

  getBranchTypeName(typeId: number): string {
    const type = this.branchTypes.find(x => x.value === typeId);
    return type ? type.label : typeId.toString();
  }

  Edit(branch: any) {
    this.updateBranch = { ...branch };
  }

  Update(form: any) {
    this.updateBranch.branchTypeId = Number(this.updateBranch.branchTypeId);
    this.http.post('branch/update', this.updateBranch).subscribe({
      next: (response) => {
        this.modalCom?.close('editBranchModal');
        this.GetAll();
      },
      error: (err) => {
        console.error('Error updating branch', err);
      }
    });
  }

  Delete(id: string) {
    this.swal.showConfirmation("Silmek istediğinize emin misiniz?", "Bu işlem geri alınamaz!", () => {
      this.http.post('branch/delete', `"${id}"`).subscribe({
        next: (response) => {
          this.GetAll();
        },
        error: (err) => {
          console.error("Error deleting branch", err);
        }
      });
    });
  }
}
