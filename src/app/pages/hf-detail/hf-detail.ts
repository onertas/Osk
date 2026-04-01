import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Modal } from '../../components/modal/modal';
import { SharedModule } from '../../modules/shared.module';
import { PersonnelMovementComponent } from '../personnel-movement/personnel-movement.component';

@Component({
  selector: 'app-hf-detail',
  standalone: true,
  imports: [SharedModule, PersonnelMovementComponent],
  templateUrl: './hf-detail.html',
  styleUrl: './hf-detail.css',
})
export class HfDetailComponent implements OnInit {
  route = inject(ActivatedRoute);
  healthFacilityId: string = '';

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.healthFacilityId = params.get('id') || '';
    });
  }
}
