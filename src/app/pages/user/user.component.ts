import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule, Table } from 'primeng/table';
import { MultiSelectModule } from 'primeng/multiselect';
import { SharedModule } from '../../modules/shared.module';
import { Modal } from '../../components/modal/modal';
import { HttpApiService } from '../../services/http-api-service';
import { SwalService } from '../../services/swall.service';
import { SystemCreateUserDto, SystemListUserDto, SystemUpdateUserDto } from '../../dtos/user/user-dtos';
import { ListRoleDto } from '../../dtos/role/role-dtos';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonModule, InputTextModule, TableModule, MultiSelectModule, SharedModule, Modal],
  templateUrl: './user.component.html'
})
export class UserComponent implements OnInit {
  http = inject(HttpApiService);
  swal = inject(SwalService);
  
  @ViewChild('dt') table!: Table;
  @ViewChild(Modal) modalCom: Modal | undefined;

  users: SystemListUserDto[] = [];
  availableRoles: ListRoleDto[] = [];
  
  createModel: SystemCreateUserDto = new SystemCreateUserDto();
  updateModel: SystemUpdateUserDto = new SystemUpdateUserDto();

  ngOnInit(): void {
    this.GetRoles();
    this.GetAll();
  }

  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.table.filterGlobal(value, 'contains');
  }

  ResetForm() {
    this.createModel = new SystemCreateUserDto();
  }

  GetRoles() {
    this.http.get<ListRoleDto[]>('role/getall').subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.availableRoles = res.data;
        }
      }
    });
  }

  GetAll() {
    this.http.get<SystemListUserDto[]>('user/getall').subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.users = res.data;
        }
      }
    });
  }

  Add(form: any) {
    this.http.post('user/add', this.createModel).subscribe({
      next: (res) => {
        if (res.success) {
          this.swal.showSuccess("Kullanıcı Eklendi");
          this.modalCom?.close('userAddModal');
          form.resetForm();
          this.GetAll();
        } else {
          this.swal.showError(res.message || "Ekleme başarısız");
        }
      }
    });
  }

  Edit(user: SystemListUserDto) {
    this.updateModel = {
      id: user.id,
      userName: user.userName,
      fullName: user.fullName,
      email: user.email,
      roles: [...user.roles], // Clone array
      newPassword: ''
    };
  }

  Update(form: any) {
    this.http.post('user/update', this.updateModel).subscribe({
      next: (res) => {
        if (res.success) {
          this.swal.showSuccess("Kullanıcı Güncellendi");
          this.modalCom?.close('userEditModal');
          this.GetAll();
        } else {
          this.swal.showError(res.message || "Güncelleme başarısız");
        }
      }
    });
  }

  Delete(id: string) {
    this.swal.showConfirmation("Silmek İstediğinize Emin Misiniz?", "Bu işlem geri alınamaz!", () => {
      this.http.delete(`user/delete?id=${id}`).subscribe({
        next: (res) => {
          if (res.success) {
            this.swal.showSuccess("Kullanıcı Silindi");
            this.GetAll();
          } else {
            this.swal.showError(res.message || "Silme işlemi başarısız");
          }
        }
      });
    });
  }
}
