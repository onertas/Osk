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
}
