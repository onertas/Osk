import { Component, inject } from '@angular/core';
import { HttpApiService } from '../../services/http-api-service';
import { ActivatedRoute } from '@angular/router';
import { HealthFacilityListDto } from '../../dtos/healthFacility/healthFacilityListDto';
import { SharedModule } from '../../modules/shared.module';
import { GrupMenuComponent } from '../../components/grup-menu-component/grup-menu-component';

@Component({
  selector: 'app-hf-home',
  imports: [SharedModule, GrupMenuComponent],
  templateUrl: './hf-home.html',
  styleUrl: './hf-home.css',
})
export class HfHome {
  private http = inject(HttpApiService);
  private route = inject(ActivatedRoute);
  public code: string | null = null;

  healthFacilities: HealthFacilityListDto[] = [];
  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.code = params['code'] ?? null;
      this.GetHealthFacilities();
    });
  }

  GetHealthFacilities() {
    this.http
      .get<HealthFacilityListDto[]>('healthfacility/GetHealthFacilities', { code: this.code })
      .subscribe((res) => {
        if (res.success && res.data) {
          this.healthFacilities = res.data;
        }
      });
  }
}
