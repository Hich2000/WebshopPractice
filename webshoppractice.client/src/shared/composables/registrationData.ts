export interface PassWordError {
  code: string,
  description: string
}

export interface UserRegistrationData {
  name: string,
  email: string,
  password: string,
  error: PassWordError[] | null,
  success: string | null
}
