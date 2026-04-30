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
}
