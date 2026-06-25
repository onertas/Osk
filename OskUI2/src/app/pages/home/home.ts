import { Component, inject, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { Blank } from '../../components/blank/blank';
import { GenericHttpService } from '../../services/generic.http.service';
import { ChartModule } from 'primeng/chart';

@Component({
  selector: 'app-home',
  imports: [CommonModule, Blank, ChartModule],
  providers: [DatePipe],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private http = inject(GenericHttpService);

  // Summary data
  totalFacilities = 0;
  totalPersonnel = 0;
  totalIcBeds = 0;

  // Detail arrays
  facilityCounts: any[] = [];
  personnelCounts: any[] = [];
  icBedCounts: any[] = [];
  tempStaffAlerts: any[] = [];
  expiredContractAlerts: any[] = [];
  suspendedFacilityAlerts: any[] = [];

  // Chart data
  facilityChartData: any;
  facilityChartOptions: any;

  personnelChartData: any;
  personnelChartOptions: any;

  icBedChartData: any;
  icBedChartOptions: any;

  // Animation
  isLoaded = false;

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard() {
    this.http.get<any>('Dashboard/GetSummary', {}, (res: any) => {
      const data = res.data;
      this.totalFacilities = data.totalFacilities;
      this.totalPersonnel = data.totalPersonnel;
      this.totalIcBeds = data.totalIcBeds;
      this.facilityCounts = data.facilityCounts;
      this.personnelCounts = data.personnelCounts;
      this.icBedCounts = data.icBedCounts;

      this.buildFacilityChart();
      this.buildPersonnelChart();
      this.buildIcBedChart();

      setTimeout(() => (this.isLoaded = true), 100);
    });

    this.http.get<any>('Dashboard/GetTempStaffAlerts', {}, (res: any) => {
      this.tempStaffAlerts = res.data ?? [];
    });

    this.http.get<any>('Dashboard/GetExpiredContractAlerts', {}, (res: any) => {
      this.expiredContractAlerts = res.data ?? [];
    });

    this.http.get<any>('Dashboard/GetSuspendedFacilityAlerts', {}, (res: any) => {
      this.suspendedFacilityAlerts = res.data ?? [];
    });
  }

  private buildFacilityChart() {
    const colors = [
      '#6366f1', '#8b5cf6', '#a78bfa', '#c4b5fd',
      '#818cf8', '#7c3aed', '#5b21b6', '#4c1d95',
    ];

    this.facilityChartData = {
      labels: this.facilityCounts.map((f: any) => f.typeName),
      datasets: [
        {
          data: this.facilityCounts.map((f: any) => f.count),
          backgroundColor: colors.slice(0, this.facilityCounts.length),
          borderWidth: 0,
          hoverOffset: 12,
        },
      ],
    };

    this.facilityChartOptions = {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          position: 'bottom',
          labels: {
            color: '#64748b',
            padding: 16,
            usePointStyle: true,
            pointStyleWidth: 10,
            font: { size: 12, family: "'Inter', sans-serif" },
          },
        },
        tooltip: {
          backgroundColor: 'rgba(15, 23, 42, 0.9)',
          titleFont: { size: 13, family: "'Inter', sans-serif" },
          bodyFont: { size: 12, family: "'Inter', sans-serif" },
          padding: 12,
          cornerRadius: 8,
          boxPadding: 6,
        },
      },
      cutout: '65%',
    };
  }

  private buildPersonnelChart() {
    const colors = [
      'rgba(59, 130, 246, 0.85)',
      'rgba(16, 185, 129, 0.85)',
      'rgba(245, 158, 11, 0.85)',
      'rgba(239, 68, 68, 0.85)',
      'rgba(139, 92, 246, 0.85)',
      'rgba(236, 72, 153, 0.85)',
      'rgba(20, 184, 166, 0.85)',
      'rgba(251, 146, 60, 0.85)',
    ];

    this.personnelChartData = {
      labels: this.personnelCounts.map((p: any) => p.titleName),
      datasets: [
        {
          label: 'Personel Sayısı',
          data: this.personnelCounts.map((p: any) => p.count),
          backgroundColor: colors.slice(0, this.personnelCounts.length),
          borderColor: colors.slice(0, this.personnelCounts.length).map((c: string) => c.replace('0.85', '1')),
          borderWidth: 1,
          borderRadius: 8,
          borderSkipped: false,
        },
      ],
    };

    this.personnelChartOptions = {
      responsive: true,
      maintainAspectRatio: false,
      indexAxis: 'y',
      plugins: {
        legend: { display: false },
        tooltip: {
          backgroundColor: 'rgba(15, 23, 42, 0.9)',
          titleFont: { size: 13, family: "'Inter', sans-serif" },
          bodyFont: { size: 12, family: "'Inter', sans-serif" },
          padding: 12,
          cornerRadius: 8,
        },
      },
      scales: {
        x: {
          grid: { color: 'rgba(148, 163, 184, 0.1)' },
          ticks: {
            color: '#64748b',
            font: { size: 11, family: "'Inter', sans-serif" },
          },
        },
        y: {
          grid: { display: false },
          ticks: {
            color: '#334155',
            font: { size: 12, family: "'Inter', sans-serif", weight: '500' },
          },
        },
      },
    };
  }

  private buildIcBedChart() {
    const colors = [
      'rgba(99, 102, 241, 0.85)',
      'rgba(16, 185, 129, 0.85)',
      'rgba(245, 158, 11, 0.85)',
      'rgba(239, 68, 68, 0.85)',
    ];

    this.icBedChartData = {
      labels: this.icBedCounts.map((b: any) => b.bedTypeName),
      datasets: [
        {
          label: 'Yatak Sayısı',
          data: this.icBedCounts.map((b: any) => b.count),
          backgroundColor: colors.slice(0, this.icBedCounts.length),
          borderColor: colors.slice(0, this.icBedCounts.length).map((c: string) => c.replace('0.85', '1')),
          borderWidth: 2,
          borderRadius: 8,
          borderSkipped: false,
        },
      ],
    };

    this.icBedChartOptions = {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: { display: false },
        tooltip: {
          backgroundColor: 'rgba(15, 23, 42, 0.9)',
          titleFont: { size: 13, family: "'Inter', sans-serif" },
          bodyFont: { size: 12, family: "'Inter', sans-serif" },
          padding: 12,
          cornerRadius: 8,
        },
      },
      scales: {
        x: {
          grid: { display: false },
          ticks: {
            color: '#334155',
            font: { size: 12, family: "'Inter', sans-serif", weight: '500' },
          },
        },
        y: {
          grid: { color: 'rgba(148, 163, 184, 0.1)' },
          ticks: {
            color: '#64748b',
            font: { size: 11, family: "'Inter', sans-serif" },
          },
        },
      },
    };
  }
}
