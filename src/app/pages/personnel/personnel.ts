import { Component, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { InputMaskModule } from 'primeng/inputmask';
import { SharedModule } from '../../modules/shared.module';
import { MultiSelectModule } from 'primeng/multiselect';

interface City {
    name: string,
    code: string
}
@Component({
  selector: 'app-personnel',
  imports: [ButtonModule,InputMaskModule,DatePickerModule,SharedModule,MultiSelectModule],
  templateUrl: './personnel.html',
  styleUrl: './personnel.css',
})
export class Personnel implements OnInit {
  ngOnInit(): void {
     this.cities = [
            {name: 'New York', code: 'NY'},
            {name: 'Rome', code: 'RM'},
            {name: 'London', code: 'LDN'},
            {name: 'Istanbul', code: 'IST'},
            {name: 'Paris', code: 'PRS'}
        ];
  }
date: Date =new Date();
  cities!: City[];

    selectedCities!: City[];
Add(){


}

}
