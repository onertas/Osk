export class CreatePersonnelMovementDto {
    start: Date = new Date();
    finish?: Date;
    contractStart?: Date;
    contractFinish?: Date;
    description: string = '';
    pmTypeId: string = '';
    branchId: string = '';
    healthFacilityId: string = '';
    afiliatedUnitId: string = "00000000-0000-0000-0000-000000000000"; // default empty guid if required
    personnelId: string = '';
    isSgk: boolean = true;
}
