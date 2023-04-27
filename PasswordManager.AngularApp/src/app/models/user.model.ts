import { Entitymodel } from "./entity.model";

export interface UserModel extends Entitymodel {
  emailAddress: string;
  password: string;
  passwordHASH: string;
  emailConfirmed: boolean;
  twoFactorEnabled: boolean;
  twoFactorSecret: string;
}
