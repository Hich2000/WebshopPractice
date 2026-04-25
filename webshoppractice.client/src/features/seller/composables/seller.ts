import { processRegistrationResponse, type RegistrationResponse } from "@/shared/composables/registrationResponse";

export enum SellerStatus {
  Pending,
  Verified,
  Rejected,
  Suspended
}
export interface Seller {
  id: string | null,
  organizationName: string,
  commerceNumber: string,
  country: string,
  city: string,
  postalCode: string,
  address: string,
  verified: SellerStatus | null,
}

export interface SellerRegistrationData {
  organizationName: string,
  commerceNumber: string,
  country: string,
  city: string,
  postalCode: string,
  address: string,
  errors: string[] | null,
  success: string | null
}


async function registerSeller(seller: Seller, firstUserId: string): Promise<RegistrationResponse<string>> {
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

async function deleteSeller(id: string): Promise<boolean> {
  const response = await fetch(`/seller/${id}`, {
    credentials: "include",
    method: "DELETE"
  });

  return response.ok
}


export function useSeller() {
  return {
    registerSeller,
    deleteSeller
  }
};
