import { CreatePersonnelDto } from './../../dtos/personnel/CreatePersonnelDto';
import { Component, inject, OnInit } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { InputMaskModule } from 'primeng/inputmask';
import { SharedModule } from '../../modules/shared.module';
import { MultiSelectModule } from 'primeng/multiselect';
import { HttpApiService } from '../../services/http-api-service';

interface City {
  name: string;
  code: string;
}
@Component({
  selector: 'app-personnel',
  imports: [ButtonModule, InputMaskModule, DatePickerModule, SharedModule, MultiSelectModule],
  templateUrl: './personnel.html',
  styleUrl: './personnel.css',
})
export class Personnel implements OnInit {
  http = inject(HttpApiService);

  ngOnInit(): void {
    this.cities = [
      { name: 'New York', code: 'NY' },
      { name: 'Rome', code: 'RM' },
      { name: 'London', code: 'LDN' },
      { name: 'Istanbul', code: 'IST' },
      { name: 'Paris', code: 'PRS' },
    ];
  }
  date: Date = new Date();
  cities!: City[];

  selectedCities!: City[];

  newPersonnel: CreatePersonnelDto = new CreatePersonnelDto();
  Add() {
    this.newPersonnel.firstName = 'test';
    this.newPersonnel.lastName = 'test2';
    this.http.post('personnel/add', this.newPersonnel).subscribe((response) => {
      console.log(response);
    });
  }
}
