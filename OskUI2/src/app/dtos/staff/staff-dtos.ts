export class CreateStaffDto {
    code: string = '';
    branchId: string = '';
    healthFacilityId: string = '';
    count: number = 0;
}

export class UpdateStaffDto {
    id: string = '';
    code: string = '';
    branchId: string = '';
    healthFacilityId: string = '';
    count: number = 0;
}

export class ListStaffDto {
    id: string = '';
    code: string = '';
    branchId: string = '';
    branchName: string = '';
    healthFacilityId: string = '';
    healthFacilityName: string = '';
    count: number = 0;
}

export class CreateTemporarayStaffDto {
    code: string = '';
    branchId: string = '';
    healthFacilityId: string = '';
    pmTypeId: string = '';
    count: number = 0;
}

export class UpdateTemporarayStaffDto {
    id: string = '';
    code: string = '';
    branchId: string = '';
    healthFacilityId: string = '';
    pmTypeId: string = '';
    count: number = 0;
}

export class ListTemporarayStaffDto {
    id: string = '';
    code: string = '';
    branchId: string = '';
    branchName: string = '';
    healthFacilityId: string = '';
    healthFacilityName: string = '';
    pmTypeId: string = '';
    pmTypeName: string = '';
    count: number = 0;
}
