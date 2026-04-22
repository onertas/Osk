import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Modal } from '../../components/modal/modal';
import { SharedModule } from '../../modules/shared.module';
import { PersonnelMovementComponent } from '../personnel-movement/personnel-movement.component';
import { IcBedComponent } from '../ic-bed/ic-bed.component';
import { StaffComponent } from '../staff/staff';
import { TemporarayStaffComponent } from '../temporaray-staff/temporaray-staff';
import { HttpApiService } from '../../services/http-api-service';
import { HfManagementListDto } from '../../dtos/healthFacility/hf-management-list.dto';

@Component({
  selector: 'app-hf-detail',
  standalone: true,
  imports: [SharedModule, PersonnelMovementComponent, IcBedComponent, StaffComponent, TemporarayStaffComponent],
  templateUrl: './hf-detail.html',
  styleUrl: './hf-detail.css',
})
export class HfDetailComponent implements OnInit {
  route = inject(ActivatedRoute);
  http = inject(HttpApiService);
  healthFacilityId: string = '';
  facility: HfManagementListDto = new HfManagementListDto();
  isLoading: boolean = true;

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.healthFacilityId = params.get('id') || '';
      if (this.healthFacilityId) {
        this.loadFacility();
      }
    });
  }

  loadFacility() {
    this.isLoading = true;
    this.http.get<HfManagementListDto>('HealthFacility/GetById', { id: this.healthFacilityId }).subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.facility = res.data;
        }
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }
}
