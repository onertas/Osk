export class ListPersonnelDto {
    id: string = '';
    identityNumber: string = '';
    firstName: string = '';
    lastName: string = '';
    email: string = '';
    phoneNumber: string = '';
    isBanned: boolean = false;
    title: string = '';
    branches: string[] = [];
    branchIds: string[] = [];
}
