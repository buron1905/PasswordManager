export interface AuthResponseModel {
  isAuthSuccessful: boolean;
  emailVerified: boolean;
  isTfaEnabled: boolean;
  errorMessage: string;
  jweToken: string;
  expirationDateTime: string;
}
