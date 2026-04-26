import { processRegistrationResponse, type RegistrationResponse } from "@/shared/composables/registrationResponse";
import { ref, type Ref } from "vue";

export enum SellerStatus {
  Pending,
  Verified,
  Rejected,
  Suspended
}
export interface Seller {
  id: string,
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

const currentSeller: Ref<Seller> = ref({
  id: '',
  organizationName: '',
  commerceNumber: '',
  country: '',
  city: '',
  postalCode: '',
  address: '',
  verified: SellerStatus.Pending,
});
const initialized = ref(false)

async function fetchSellerData(force: boolean = false): Promise<Seller | null> {
  if (initialized.value && !force) {
    return currentSeller.value
  }

  const response = await fetch(`/seller/me`, {
    credentials: "include"
  })

  if (!response.ok) {
    return null;
  }

  const json = await response.json();

  currentSeller.value = {
    id: json.id,
    organizationName: json.organizationName,
    commerceNumber: json.commerceNumber,
    country: json.country,
    city: json.city,
    postalCode: json.postalCode,
    address: json.address,
    verified: json.verified
  }

  return currentSeller.value
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

async function updateSellerInfo(updateForm: SellerRegistrationData) {

  currentSeller.value.organizationName = updateForm.organizationName;
  currentSeller.value.commerceNumber = updateForm.commerceNumber;
  currentSeller.value.country = updateForm.country;
  currentSeller.value.city = updateForm.city;
  currentSeller.value.postalCode = updateForm.postalCode;
  currentSeller.value.address = updateForm.address;

  console.log(currentSeller.value);
  const response = await fetch(`/seller/${currentSeller.value.id}`, {
    method: "PATCH",
    credentials: "include",
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(currentSeller.value)
  });

  if (!response.ok) {
    return false
  }

  await fetchSellerData();
  return true;
}


export function useSeller() {
  return {
    currentSeller,
    registerSeller,
    deleteSeller,
    fetchSellerData,
    updateSellerInfo
  }
};
