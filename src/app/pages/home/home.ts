import { Component, inject, OnInit } from '@angular/core';
import { Blank } from '../../components/blank/blank';
import { Section } from '../../components/section/section';
import { HttpApiService } from '../../services/http-api-service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  imports: [Blank, Section],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  http = inject(HttpClient);
  ngOnInit(): void {}
  sonuc: any;

  ask() {
  const url = 'https://n8n.onertas.com/webhook/mevzuat-soru';
  const body = {
    pdfUrl: 'https://www.mevzuat.gov.tr/MevzuatMetin/yonetmelik/7.5.42353.pdf',
    soru: 'Tabela standartları nelerdir?',
  };

  // withCredentials: false yaparak sunucunun 'true' dönme zorunluluğunu ortadan kaldırın
  this.http.post<any>(url, body, { withCredentials: true})
    .subscribe({
      next: (res) => { this.sonuc = res; console.log('Başarılı:', res); },
      error: (err) => { console.error('Hata:', err); }
    });
  }
}
