import { useRegistrationData, type RegistrationError } from "@/shared/composables/registrationData";

export interface Seller {
  id: string|null,
  organizationName: string,
  commerceNumber: string,
  country: string,
  city: string,
  postalCode: string,
  address: string,
}

const { processRegistrationResponse } = useRegistrationData();

async function registerSeller(seller: Seller, firstUserId: string): Promise<string | RegistrationError[]> {
  const response = await fetch("/register/seller", {
    method: "POST",
    credentials: "include",
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      ...seller,
      userId: firstUserId
    })
  });

  return processRegistrationResponse(response);
}

export function useSeller() {
  return {
    registerSeller
  }
};
