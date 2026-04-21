export class CreateRoleDto {
  name: string = '';
}

export class UpdateRoleDto extends CreateRoleDto {
  id: string = '';
}

export class ListRoleDto {
  id: string = '';
  name: string = '';
}
