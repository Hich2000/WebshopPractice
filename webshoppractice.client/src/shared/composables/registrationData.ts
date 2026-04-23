export interface RegistrationError {
  code: string,
  description: string
}

export interface UserRegistrationData {
  name: string,
  email: string,
  password: string,
  error: RegistrationError[] | null,
  success: string | null
}

export interface SellerRegistrationData {
  organizationName: string,
  commerceNumber: string,
  country: string,
  city: string,
  postalCode: string,
  address: string,
  error: RegistrationError[] | null,
  success: string | null
}
