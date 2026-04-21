import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule, Table } from 'primeng/table';
import { SharedModule } from '../../modules/shared.module';
import { Modal } from '../../components/modal/modal';
import { HttpApiService } from '../../services/http-api-service';
import { SwalService } from '../../services/swall.service';
import { CreateRoleDto, ListRoleDto, UpdateRoleDto } from '../../dtos/role/role-dtos';

@Component({
  selector: 'app-role',
  standalone: true,
  imports: [ButtonModule, InputTextModule, TableModule, SharedModule, Modal],
  templateUrl: './role.component.html'
})
export class RoleComponent implements OnInit {
  http = inject(HttpApiService);
  swal = inject(SwalService);
  
  @ViewChild('dt') table!: Table;
  @ViewChild(Modal) modalCom: Modal | undefined;

  roles: ListRoleDto[] = [];
  createModel: CreateRoleDto = new CreateRoleDto();
  updateModel: UpdateRoleDto = new UpdateRoleDto();

  ngOnInit(): void {
    this.GetAll();
  }

  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.table.filterGlobal(value, 'contains');
  }

  ResetForm() {
    this.createModel = new CreateRoleDto();
  }

  GetAll() {
    this.http.get<ListRoleDto[]>('role/getall').subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.roles = res.data;
        }
      }
    });
  }

  Add(form: any) {
    this.http.post('role/add', this.createModel).subscribe({
      next: (res) => {
        if (res.success) {
          this.swal.showSuccess("Rol Eklendi");
          this.modalCom?.close('roleAddModal');
          form.resetForm();
          this.GetAll();
        } else {
          this.swal.showError(res.message || "Ekleme başarısız");
        }
      }
    });
  }

  Edit(role: ListRoleDto) {
    this.updateModel = {
      id: role.id,
      name: role.name
    };
  }

  Update(form: any) {
    this.http.post('role/update', this.updateModel).subscribe({
      next: (res) => {
        if (res.success) {
          this.swal.showSuccess("Rol Güncellendi");
          this.modalCom?.close('roleEditModal');
          this.GetAll();
        } else {
          this.swal.showError(res.message || "Güncelleme başarısız");
        }
      }
    });
  }

  Delete(id: string) {
    this.swal.showConfirmation("Silmek İstediğinize Emin Misiniz?", "Bu işlem geri alınamaz!", () => {
      this.http.delete(`role/delete?id=${id}`).subscribe({
        next: (res) => {
          if (res.success) {
            this.swal.showSuccess("Rol Silindi");
            this.GetAll();
          } else {
            this.swal.showError(res.message || "Silme işlemi başarısız");
          }
        }
      });
    });
  }
}
