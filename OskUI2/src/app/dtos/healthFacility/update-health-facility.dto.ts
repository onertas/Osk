export class UpdateHealthFacilityDto {
  public id: string = '';
  public healthFacilityTypeId: string = '';
  public typeName: string = ''; // sadece görüntüleme için
  public name: string = '';
  public address?: string;
  public phoneNumber?: string;
  public email?: string;
  public taxNumber?: string;
  public corporationName?: string;
  public observationBedCount: number = 0;
  public totalBedCount: number = 0;
  public openingDate?: Date;
  public upperHealthFacilityId: string = '00000000-0000-0000-0000-000000000000';
  public hfStatus: number = 1;
}
