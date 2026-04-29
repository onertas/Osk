export class UpdateHealthFacilityDto {
  id: string = '';
  healthFacilityTypeId: string = '';
  typeName: string = ''; // sadece görüntüleme için
  name: string = '';
  address?: string;
  phoneNumber?: string;
  email?: string;
  taxNumber?: string;
  corporationName?: string;
  observationBedCount: number = 0;
  totalBedCount: number = 0;
}
