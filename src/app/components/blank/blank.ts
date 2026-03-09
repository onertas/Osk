import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-blank',
  imports: [RouterLink],
  templateUrl: './blank.html',
  styleUrl: './blank.css',
})
export class Blank {
 @Input() routes:string[]=[]
  @Input() title:string=""
}
