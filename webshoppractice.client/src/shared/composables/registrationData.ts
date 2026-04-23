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

function isRegistrationsErrorArray(data: unknown): data is RegistrationError[] {
  return Array.isArray(data) &&
    data.every(
      (item) =>
        typeof item.code === 'string' &&
        typeof item.description === 'string'
    );
}

async function processRegistrationResponse(response: Response): Promise<string | RegistrationError[]> {
  if (!response.ok) {
    const responseErrors = await response.json();
    if (isRegistrationsErrorArray(responseErrors)) {
      const registrationErrors = responseErrors;
      return registrationErrors;
    } else {
      console.log(responseErrors);
      return [
        {
          code: "unkownError",
          description: "An unknown error has occurred."
        }
      ]
    }
  }

  try {
    const json = await response.json();
    return json.id;
  } catch (error) {
    console.log(error);
    return "success";
  }
}

export function useRegistrationData() {
  return {
    processRegistrationResponse
  }
}
