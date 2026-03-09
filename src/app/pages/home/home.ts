import { Component } from '@angular/core';
import { Blank } from "../../components/blank/blank";
import { Section } from "../../components/section/section";

@Component({
  selector: 'app-home',
  imports: [Blank, Section],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {

}
