export class HfManagementListDto {
  id: string = '';
  name: string = '';
  address?: string;
  phoneNumber?: string;
  email?: string;
  taxNumber?: string;
  corporationName?: string;
  observationBedCount: number = 0;
  totalBedCount: number = 0;
  healthFacilityTypeId: string = '';
  typeName: string = '';
  openingDate?: Date;
  showBed: boolean = false;
  showDevice: boolean = false;
  showStaff: boolean = false;
  showTempStaff: boolean = false;
  showPm: boolean = false;
}
