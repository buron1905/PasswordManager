export interface UserModel {
  id: string;
  emailAddress: string;
  password: string;
  passwordHASH: string;
  emailConfirmed: boolean;
  twoFactorEnabled: boolean;
  twoFactorSecret: string;
  idt: string;
  udt: string;
  ddt: string;
  deleted: boolean;
}
