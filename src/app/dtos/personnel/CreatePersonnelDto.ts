export class CreatePersonnelDto {
  identityNumber: string="";
  firstName: string="";
  lastName: string="";
  email: string="";
  phoneNumber: string="";
  isBanned: boolean=false;
  banReason?: string
  personnelBranches?: string[]; // Guid -> string olarak tutulur
}