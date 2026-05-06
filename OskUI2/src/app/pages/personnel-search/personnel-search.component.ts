import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpApiService } from '../../services/http-api-service';
import { Blank } from '../../components/blank/blank';
import { Section } from '../../components/section/section';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-personnel-search',
  standalone: true,
  imports: [CommonModule, FormsModule, Blank, Section, TableModule],
  templateUrl: './personnel-search.component.html'
})
export class PersonnelSearchComponent {
  http = inject(HttpApiService);

  searchText: string = '';
  searchResults: any[] = [];
  selectedPersonnel: any = null;
  personnelMovements: any[] = [];
  
  loadingSearch: boolean = false;
  loadingDetails: boolean = false;
  searchPerformed: boolean = false;

  onSearch() {
    if (!this.searchText || this.searchText.trim().length < 3) {
      this.searchResults = [];
      return;
    }

    this.selectedPersonnel = null;
    this.personnelMovements = [];
    this.loadingSearch = true;
    this.searchPerformed = true;

    this.http.get<any[]>('Personnel/Search', { query: this.searchText }).subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.searchResults = res.data;
        } else {
          this.searchResults = [];
        }
        this.loadingSearch = false;
      },
      error: (err) => {
        console.error('Search error:', err);
        this.loadingSearch = false;
      }
    });
  }

  selectPersonnel(personnel: any) {
    this.selectedPersonnel = personnel;
    this.loadMovements(personnel.id);
  }

  loadMovements(personnelId: string) {
    this.loadingDetails = true;
    this.http.get<any[]>('Pm/GetByPersonnelId', { personnelId: personnelId }).subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.personnelMovements = res.data;
        } else {
          this.personnelMovements = [];
        }
        this.loadingDetails = false;
      },
      error: (err) => {
        console.error('Load movements error:', err);
        this.loadingDetails = false;
      }
    });
  }
}
