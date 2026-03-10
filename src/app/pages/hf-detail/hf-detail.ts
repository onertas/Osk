import { Component } from '@angular/core';
import { Modal } from '../../components/modal/modal';
import { SharedModule } from '../../modules/shared.module';
import { Personnel } from '../personnel/personnel';

@Component({
  selector: 'app-hf-detail',
  standalone: true,
  imports: [SharedModule, Modal,Personnel],
  templateUrl: './hf-detail.html',
  styleUrl: './hf-detail.css',
})
export class HfDetailComponent {
  ngOnInit(): void {
    this.GetPersonnelList();
    // this.GetHealthFacilityList();
  }
  GetPersonnelList() {}
}
