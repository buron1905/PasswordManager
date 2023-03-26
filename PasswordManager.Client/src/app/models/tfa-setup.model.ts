export interface TfaSetup {
  isTfaEnabled: boolean;
  code: string;
  authenticatorKey: string;
  qrCodeSetupImageUrl: string;
}
