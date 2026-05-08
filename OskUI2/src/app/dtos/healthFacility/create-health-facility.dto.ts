export class CreateHealthFacilityDto {
  healthFacilityTypeId: string = '';
  name: string = '';
  address?: string;
  phoneNumber?: string;
  email?: string;
  taxNumber?: string;
  corporationName?: string;
  observationBedCount: number = 0;
  totalBedCount: number = 0;
  openingDate?: Date;
  upperHealthFacilityId: string = '00000000-0000-0000-0000-000000000000';
  hfStatus: number = 1;
}
