import { LoginModel } from "./login.model";

export interface LoginTfaModel extends LoginModel {
  code: string;
  }
