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
import { SwalService } from '../../services/swall.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-hf-detail',
  standalone: true,
  imports: [
    SharedModule, 
    PersonnelMovementComponent, 
    IcBedComponent,
    StaffComponent, 
    TemporarayStaffComponent,
  ],
  templateUrl: './hf-detail.html',
  styleUrl: './hf-detail.css',
})
export class HfDetailComponent implements OnInit {
  route = inject(ActivatedRoute);
  http = inject(HttpApiService);
  swal = inject(SwalService);
  authService = inject(AuthService);

  healthFacilityId: string = '';
  facility: HfManagementListDto = new HfManagementListDto();
  isAdmin: boolean = false;
  
  totalIcBeds: number = 0;
  isBedsLoaded: boolean = false;
  activeTab: string = 'home';

  ngOnInit(): void {
    // Kullanıcı bilgisi yüklendiğinde isAdmin güncelle
    this.authService.user$.subscribe(user => {
      this.isAdmin = user?.roles.includes('Admin') ?? false;
    });

    this.route.paramMap.subscribe(params => {
      this.healthFacilityId = params.get('id') || '';
      if (this.healthFacilityId) {
        this.loadFacility();
      }
    });
  }

  loadFacility() {
   
    this.http.get<HfManagementListDto>('HealthFacility/GetById', { id: this.healthFacilityId }).subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.facility = res.data;

          console.log(res.data)
        }
       
      }
      
    });
  }

  onBedsLoaded(count: number) {
    this.totalIcBeds = count;
    this.isBedsLoaded = true;
  }

  selectTab(tab: string) {
    this.activeTab = tab;
  }
}
