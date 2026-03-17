import { Modal } from './../../components/modal/modal';
import { CreatePersonnelDto } from './../../dtos/personnel/CreatePersonnelDto';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { InputMaskModule } from 'primeng/inputmask';
import * as XLSX from 'xlsx';
import { InputTextModule } from 'primeng/inputtext';
import { SharedModule } from '../../modules/shared.module';
import { MultiSelectModule } from 'primeng/multiselect';
import { TableModule, Table } from 'primeng/table';
import { HttpApiService } from '../../services/http-api-service';


interface City {
  name: string;
  code: string;
}

interface PersonnelItem {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
}
@Component({
  selector: 'app-personnel',
  imports: [ButtonModule, Modal,InputMaskModule, DatePickerModule, InputTextModule, SharedModule, MultiSelectModule, TableModule, Modal],
  templateUrl: './personnel.html',
  styleUrl: './personnel.css',
})
export class Personnel implements OnInit {
  @ViewChild('dt') table!: Table;
 @ViewChild(Modal) modalCom: Modal | undefined;
  http = inject(HttpApiService);

  personnel: PersonnelItem[] = [];


  onGlobalFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.table.filterGlobal(value, 'contains');
  }

  

  ngOnInit(): void {
    this.cities = [
      { name: 'New York', code: 'NY' },
      { name: 'Rome', code: 'RM' },
      { name: 'London', code: 'LDN' },
      { name: 'Istanbul', code: 'IST' },
      { name: 'Paris', code: 'PRS' },
    ];

    this.personnel = [
      { firstName: 'John', lastName: 'Doe', email: 'john.doe@example.com', phone: '123-456-7890' },
      { firstName: 'Anna', lastName: 'Smith', email: 'anna.smith@example.com', phone: '234-567-8901' },
      { firstName: 'Peter', lastName: 'Jones', email: 'peter.jones@example.com', phone: '345-678-9012' },
      { firstName: 'Maria', lastName: 'Garcia', email: 'maria.garcia@example.com', phone: '456-789-0123' },
      { firstName: 'Maria', lastName: 'Garcia', email: 'maria.garcia@example.com', phone: '456-789-0123' },
      { firstName: 'Maria', lastName: 'Garcia', email: 'maria.garcia@example.com', phone: '456-789-0123' },
      { firstName: 'Maria', lastName: 'Garcia', email: 'maria.garcia@example.com', phone: '456-789-0123' },
      { firstName: 'Maria', lastName: 'Garcia', email: 'maria.garcia@example.com', phone: '456-789-0123' },
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
