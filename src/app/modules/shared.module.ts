import { CommonModule } from "@angular/common";

import { FormsModule } from "@angular/forms";
import { NgModule } from "@angular/core";

import { RouterLink, RouterLinkActive } from "@angular/router";

import { Blank } from "../components/blank/blank";
import { Section } from "../components/section/section";
import { FormvalidationDirective } from "../directives/formvalidation.directive";
import { GrupMenuComponent } from "../components/grup-menu-component/grup-menu-component";


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    Blank,
    RouterLink,
    RouterLinkActive,
    Section,
    FormsModule,
    FormvalidationDirective,
    GrupMenuComponent
  ],
  exports:[
    CommonModule,
    Blank,
    RouterLink,
    RouterLinkActive,
    Section,
    FormsModule,
    FormvalidationDirective,
    GrupMenuComponent
  ]
})
export class SharedModule { }
