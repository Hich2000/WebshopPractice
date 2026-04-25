export interface RegistrationResponse<T> {
  success: boolean,
  message: string,
  errors: string[],
  data: T
}


export async function processRegistrationResponse(response: Response): Promise<RegistrationResponse<string>> {
  const registrationResponse: RegistrationResponse<string> = {
    success: false,
    message: '',
    errors: [],
    data: ''
  }

  switch (response.status) {
    case 200: {
      registrationResponse.success = true;
      registrationResponse.message = 'Registration successful.';
      registrationResponse.data = await response.text();
      break;
    }
    case 400: {
      const errorJson = await response.json();

      registrationResponse.message = 'Registration failed.';
      registrationResponse.errors = errorJson.forEach((element: string) => {
        return element;
      });
      break;
    }
    default: {
      registrationResponse.message = 'An unknown error occurred.';
      break;
    }
  }

  return registrationResponse
}
