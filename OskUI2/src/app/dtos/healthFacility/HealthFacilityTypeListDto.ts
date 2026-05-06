
export class HealthFacilityTypeListDto {
  id!: string;
  name!: string;
  code!: string;
  showBed: boolean = false;
  showDevice: boolean = false;
  showStaff: boolean = false;
  showTempStaff: boolean = false;
  showPm: boolean = false;
}
