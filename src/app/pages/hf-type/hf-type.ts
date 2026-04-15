import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { HttpApiService } from '../../services/http-api-service';
import { SwalService } from '../../services/swall.service';
import { HealthFacilityTypeListDto } from '../../dtos/healthFacility/HealthFacilityTypeListDto';
import { TableModule } from "primeng/table";
import { Modal } from '../../components/modal/modal';

@Component({
  selector: 'app-hf-type',
  imports: [SharedModule, TableModule, Modal],
  templateUrl: './hf-type.html',
  styleUrl: './hf-type.css',
})
export class HfType implements OnInit {
@ViewChild(Modal) modalCom: Modal | undefined;
ngOnInit(): void {
  this.GetAll();
}
http= inject(HttpApiService);
swal = inject(SwalService);

typelist: HealthFacilityTypeListDto[] = [];
newType: HealthFacilityTypeListDto = new HealthFacilityTypeListDto();
updateType: HealthFacilityTypeListDto = new HealthFacilityTypeListDto();


  GetAll(){
  this.http.get<HealthFacilityTypeListDto[]>('healthfacilityType/GetHealthFacilityTypes').subscribe(res=>{
   if(res.success && res.data){
    this.typelist=res.data;
   }
  });
}

Add(form: any) {
  this.http.post('healthfacilityType/Add', this.newType).subscribe({
    next: (response) => {
      this.modalCom?.close('addHfTypeModal');
      form.resetForm();
      this.newType = new HealthFacilityTypeListDto();
      this.GetAll();
    },
    error: (err) => {
      console.error('Error adding HF type', err);
    }
  });
}

Edit(type: any) {
  this.updateType = { ...type };
}

Update(form: any) {
  this.http.post('healthfacilityType/Update', this.updateType).subscribe({
    next: (response) => {
      this.modalCom?.close('editHfTypeModal');
      this.GetAll();
    },
    error: (err) => {
      console.error('Error updating HF type', err);
    }
  });
}

  Delete(id: string) {
    this.swal.showConfirmation("Silmek istediğinize emin misiniz?", "Bu işlem geri alınamaz!", () => {
      this.http.post('healthfacilityType/Delete', `"${id}"`).subscribe({
        next: (response) => {
          this.GetAll();
        },
        error: (err) => {
          console.error("Error deleting HF type", err);
        }
      });
    });
  }
}
