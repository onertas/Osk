export class CreateStaffDto {
    code: string = '';
    branchId: string = '';
    healthFacilityId: string = '';
    count: number = 0;
    staffNo: number = 0;
    date: any = null;
    reason: string = '';
}

export class UpdateStaffDto {
    id: string = '';
    code: string = '';
    branchId: string = '';
    healthFacilityId: string = '';
    count: number = 0;
    staffNo: number = 0;
    date: any = null;
    reason: string = '';
}

export class ListStaffDto {
    id: string = '';
    code: string = '';
    branchId: string = '';
    branchName: string = '';
    healthFacilityId: string = '';
    healthFacilityName: string = '';
    count: number = 0;
    staffNo: number = 0;
    date: any = null;
    reason: string = '';
}

export class CreateTemporarayStaffDto {
    code: string = '';
    branchId: string = '';
    healthFacilityId: string = '';
    pmTypeId: string = '';
   
}

export class UpdateTemporarayStaffDto {
    id: string = '';
    code: string = '';
    branchId: string = '';
    healthFacilityId: string = '';
    pmTypeId: string = '';
   
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

}
