import { LoginModel } from "./login.model";

export interface RegisterModel extends LoginModel {
    confirmPassword: string;
  }
