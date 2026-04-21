export class CreatePmTypeDto {
    name: string = '';
    description: string = '';
    order: number = 0;
    isUsingStaff: boolean = false;
    isBeforeStartStaff: boolean = false;
    isManager: boolean = false;
    isFaaliyet2Control: boolean = false;
    isOnlyOneStatu: boolean = false;
    statusQuota: number = 0;
}
