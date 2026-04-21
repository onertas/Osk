import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule, Table } from 'primeng/table';
import { SharedModule } from '../../modules/shared.module';
import { Modal } from '../../components/modal/modal';
import { HttpApiService } from '../../services/http-api-service';
import { SwalService } from '../../services/swall.service';
import { CreateTitleDto } from '../../dtos/title/CreateTitleDto';
import { ListTitleDto } from '../../dtos/title/ListTitleDto';
import { UpdateTitleDto } from '../../dtos/title/UpdateTitleDto';

@Component({
  selector: 'app-title',
  imports: [ButtonModule, InputTextModule, TableModule, SharedModule, Modal],
  templateUrl: './title.html',
  styleUrl: './title.css',
})
export class TitleComponent implements OnInit {

  http = inject(HttpApiService);
  swal = inject(SwalService);
  @ViewChild('dt') table!: Table;
  @ViewChild(Modal) modalCom: Modal | undefined;

  newTitle: CreateTitleDto = new CreateTitleDto();
  updateTitle: UpdateTitleDto = new UpdateTitleDto();
  titles: ListTitleDto[] = [];

  ngOnInit(): void {
    this.GetAll();
  }

  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.table.filterGlobal(value, 'contains');
  }

  Add(form: any) {
    this.http.post('title/add', this.newTitle).subscribe({
      next: (response) => {
        this.modalCom?.close('addTitleModal');
        form.resetForm();
        this.newTitle = new CreateTitleDto();
        this.GetAll();
      },
      error: (err) => {
        console.error('Error adding title', err);
      }
    });
  }

  GetAll() {
    this.http.get<ListTitleDto[]>('title/getall').subscribe(res => {
      if (res.success && res.data) {
        this.titles = res.data;
      }
    });
  }

  Edit(title: any) {
    this.updateTitle = { ...title };
  }

  Update(form: any) {
    this.http.post('title/update', this.updateTitle).subscribe({
      next: (response) => {
        this.modalCom?.close('editTitleModal');
        this.GetAll();
      },
      error: (err) => {
        console.error('Error updating title', err);
      }
    });
  }

  Delete(id: string) {
    this.swal.showConfirmation("Silmek istediğinize emin misiniz?", "Bu işlem geri alınamaz!", () => {
      this.http.post('title/delete', `"${id}"`).subscribe({
        next: (response) => {
          this.GetAll();
        },
        error: (err) => {
          console.error("Error deleting title", err);
        }
      });
    });
  }
}
