export class UpdatePersonnelMovementDto {
    id: string = '';
    start: Date = new Date();
    finish?: Date;
    contractStart?: Date;
    contractFinish?: Date;
    description: string = '';
    pmTypeId: string = '';
    branchId: string = '';
    healthFacilityId: string = '';
    afiliatedUnitId: string = '';
    personnelId: string = '';
    isSgk: boolean = true;
}
