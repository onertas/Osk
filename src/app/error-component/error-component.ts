import { Component } from '@angular/core';
import { SharedModule } from '../modules/shared.module';

@Component({
  selector: 'app-error-component',
  imports: [SharedModule],
  templateUrl: './error-component.html',
  styleUrl: './error-component.css',
})
export class ErrorComponent {

}
