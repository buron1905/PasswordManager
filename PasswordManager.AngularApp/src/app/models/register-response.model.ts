import { UserModel } from "./user.model";

export interface RegisterResponseModel {
  isRegistrationSuccessful: boolean;
  errorMessage: string;
  user: UserModel;
}
