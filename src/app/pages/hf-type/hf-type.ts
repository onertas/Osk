import { Component, inject, OnInit } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { HttpApiService } from '../../services/http-api-service';
import { HealthFacilityTypeListDto } from '../../dtos/healthFacility/HealthFacilityTypeListDto';
import { TableModule } from "primeng/table";

@Component({
  selector: 'app-hf-type',
  imports: [SharedModule, TableModule],
  templateUrl: './hf-type.html',
  styleUrl: './hf-type.css',
})
export class HfType implements OnInit {
ngOnInit(): void {
  this.GetAll();
}
http= inject(HttpApiService);

typelist: HealthFacilityTypeListDto[] = [];


GetAll(){
  this.http.get<HealthFacilityTypeListDto[]>('healthfacilityType/GetHealthFacilityTypes').subscribe(res=>{
   if(res.success && res.data){
    this.typelist=res.data;
   }
  });
}

}
