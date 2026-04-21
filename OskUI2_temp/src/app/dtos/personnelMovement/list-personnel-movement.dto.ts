export class ListPersonnelMovementDto {
    id: string = '';
    start: Date = new Date();
    finish?: Date;
    contractStart?: Date;
    contractFinish?: Date;
    description: string = '';
    pmTypeId: string = '';
    pmType?: any;
    branchId: string = '';
    branch?: any;
    healthFacilityId: string = '';
    healthFacility?: any;
    afiliatedUnitId: string = '';
    personnelId: string = '';
    personnel?: any;
    isSgk: boolean = true;
}
