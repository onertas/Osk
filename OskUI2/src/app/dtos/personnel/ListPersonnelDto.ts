export class ListPersonnelDto {
    id: string = '';
    identityNumber: string = '';
    firstName: string = '';
    lastName: string = '';
    email: string = '';
    phoneNumber: string = '';
    isBanned: boolean = false;
    birthDate?: string;
    title: string = '';
    branches: string[] = [];
    branchIds: string[] = [];
}
