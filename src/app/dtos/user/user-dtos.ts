export class SystemCreateUserDto {
  userName: string = '';
  fullName: string = '';
  email: string = '';
  password: string = '';
  roles: string[] = [];
}

export class SystemUpdateUserDto {
  id: string = '';
  userName: string = '';
  fullName: string = '';
  email: string = '';
  roles: string[] = [];
  newPassword?: string;
}

export class SystemListUserDto {
  id: string = '';
  userName: string = '';
  fullName: string = '';
  email: string = '';
  roles: string[] = [];
}
