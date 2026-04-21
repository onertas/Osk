export class UpdateIcBedDto {
    id: string = '';
    healthFacilityId: string = '';
    icBedRegLevel: number = 0;
    icBedRegType: number = 0;
    quantity: number = 0;
    icBedRegDate: Date = new Date();
    icBedRegNumber: string = '';
    icBedNameId: string = '';
    isActive: boolean = true;
}
