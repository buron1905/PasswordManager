import { Entitymodel } from "./entity.model";

export interface PasswordModel extends Entitymodel {
  passwordName: string;
  userName: string;
  passwordDecrypted: string;
  url: string;
  notes: string;
  favorite: boolean;
}
