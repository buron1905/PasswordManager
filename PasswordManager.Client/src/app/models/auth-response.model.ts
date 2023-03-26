export interface AuthResponseModel {
  isAuthSuccessful: boolean;
  isTfaEnabled: boolean;
  errorMessage: string;
  jweToken: string;
  expirationDateTime: string;
}
