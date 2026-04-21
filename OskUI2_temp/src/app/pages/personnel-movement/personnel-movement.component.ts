import { Component, inject, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TableModule, Table } from 'primeng/table';
import { SharedModule } from '../../modules/shared.module';
import { Modal } from '../../components/modal/modal';
import { CheckboxModule } from 'primeng/checkbox';
import { HttpApiService } from '../../services/http-api-service';
import { SwalService } from '../../services/swall.service';
import { CreatePersonnelMovementDto } from '../../dtos/personnelMovement/create-personnel-movement.dto';
import { ListPersonnelMovementDto } from '../../dtos/personnelMovement/list-personnel-movement.dto';
import { UpdatePersonnelMovementDto } from '../../dtos/personnelMovement/update-personnel-movement.dto';
import { ListPmTypeDto } from '../../dtos/pmType/list-pm-type.dto';

@Component({
  selector: 'app-personnel-movement',
  standalone: true,
  imports: [ButtonModule, DatePickerModule, InputTextModule, SelectModule, TableModule, SharedModule, Modal, CheckboxModule],
  templateUrl: './personnel-movement.component.html'
})
export class PersonnelMovementComponent implements OnInit, OnChanges {
  @Input() healthFacilityId: string = '';

  http = inject(HttpApiService);
  swal = inject(SwalService);

  @ViewChild('dt') table!: Table;
  @ViewChild(Modal) modalCom: Modal | undefined;

  newMovement: CreatePersonnelMovementDto = new CreatePersonnelMovementDto();
  updateMovement: UpdatePersonnelMovementDto = new UpdatePersonnelMovementDto();
  movements: ListPersonnelMovementDto[] = [];

  pmTypes: ListPmTypeDto[] = [];
  branches: any[] = [];
  filteredBranches: any[] = [];
  personnelList: any[] = [];

  private searchSubject = new Subject<string>();

  ngOnInit() {
    this.GetPmTypes();
    this.GetBranches();

    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(query => {
      this.searchPersonnel(query);
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['healthFacilityId'] && this.healthFacilityId) {
      this.GetAll();
      this.newMovement.healthFacilityId = this.healthFacilityId;
    }
  }

  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.table.filterGlobal(value, 'contains');
  }

  GetAll() {
    if (!this.healthFacilityId) return;

    this.http.get<ListPersonnelMovementDto[]>('Pm/GetAll').subscribe({
      next: (res) => {
        if (res.success && res.data) {
          // Filter movements locally if backend doesn't filter by healthFacilityId
          this.movements = res.data.filter(x => x.healthFacilityId === this.healthFacilityId);
        }
      }
    });
  }

  GetPmTypes() {
    this.http.get<ListPmTypeDto[]>('PmType/GetAll').subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.pmTypes = res.data;
        }
      }
    });
  }

  GetBranches() {
    this.http.get<any[]>('Branch/GetAll').subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.branches = res.data;
          this.filteredBranches = res.data;
        }
      }
    });
  }

  onPersonnelChange(event: any) {
    const selectedPersonId = event.value;
    const selectedPerson = this.personnelList.find(p => p.id === selectedPersonId);

    if (selectedPerson && selectedPerson.branches && selectedPerson.branches.length > 0) {
      // Assuming selectedPerson.branches contains branch names or data that we can use to filter.
      // E.g., if selectedPerson.branches is a list of strings [ "Kardiyoloji" ]
      this.filteredBranches = this.branches.filter(b => selectedPerson.branches.includes(b.name));
    } else {
      this.filteredBranches = [];
    }

    if (!this.filteredBranches.find(b => b.id === this.newMovement.branchId)) {
      this.newMovement.branchId = '';
    }
  }

  onPersonnelSearch(event: any) {

    const query = event.filter || event.query || '';
    this.searchSubject.next(query);
  }

  searchPersonnel(query: string) {
    if (!query || query.length < 2) {
      this.personnelList = [];
      return;
    }

    this.http.get<any[]>("Personnel/Search", { query }).subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.personnelList = res.data;
        }
      }
    });
  }

  Add(form: any) {
    this.newMovement.healthFacilityId = this.healthFacilityId;

    this.http.post('Pm/Add', this.newMovement).subscribe({
      next: (response) => {
        if (response.success) {
          this.modalCom?.close('pmModal');
          form.resetForm();
          this.newMovement = new CreatePersonnelMovementDto();
          this.newMovement.healthFacilityId = this.healthFacilityId;
          this.GetAll();
          this.swal.showSuccess("Başarıyla kaydedildi");
        } else {
          this.swal.showError(response.message || "Kaydetme işlemi başarısız");
        }
      },
      error: (err) => {
        console.error('Error adding PM', err);
        this.swal.showError(err.error?.message || "Sunucu hatası oluştu");
      }
    });
  }

  Edit(pm: any) {
    this.updateMovement = { ...pm };
    // Optionally fetch specific personnel to populate dropdown
    if (pm.personnelId) {
      this.searchPersonnel(pm.personnel?.firstName || '');
      this.newMovement.branchId = pm.branchId;
    }
  }

  Update(form: any) {
    this.http.post('Pm/Update', this.updateMovement).subscribe({
      next: (response) => {
        if (response.success) {
          this.modalCom?.close('pmEditModal');
          this.GetAll();
          this.swal.showSuccess("Başarıyla güncellendi");
        } else {
          this.swal.showError(response.message || "Güncelleme işlemi başarısız");
        }
      },
      error: (err) => {
        console.error('Error updating PM', err);
        this.swal.showError(err.error?.message || "Sunucu hatası oluştu");
      }
    });
  }

  Delete(id: string) {
    this.swal.showConfirmation("Silmek istediğinize emin misiniz?", "Bu işlem geri alınamaz!", () => {
      this.http.post('Pm/Delete', `"${id}"`).subscribe({
        next: (response) => {
          if (response.success) {
            this.GetAll();
            this.swal.showSuccess("Silme işlemi başarılı");
          } else {
            this.swal.showError(response.message || "Silme işlemi başarısız");
          }
        },
        error: (err) => {
          console.error("Error deleting PM", err);
          this.swal.showError(err.error?.message || "Sunucu hatası");
        }
      });
    });
  }
}
