import { ref, readonly } from "vue";
import { processRegistrationResponse, type RegistrationResponse } from "./registrationResponse";

export interface User {
  id: string,
  name: string,
  email: string,
  level: string
}

export interface UserRegistrationData {
  name: string,
  email: string,
  password: string,
  errors: string[] | null,
  success: string | null
}

const currentUser = ref<User | null>(null)
const initialized = ref(false)

async function fetchCurrentUser(force: boolean = false): Promise<User | null> {
  if (initialized.value && !force) {
    return currentUser.value
  }

  const response = await fetch('/me', {
    credentials: "include"
  })
  if (!response.ok) {
    currentUser.value = null
  } else {
    const data = await response.json()
    currentUser.value = {
      id: data.id,
      name: data.name,
      email: data.email,
      level: data.level
    }
  }

  initialized.value = true
  return currentUser.value
}

async function logout(): Promise<void> {
  await fetch('/logout', {
    credentials: 'include',
    method: 'POST'
  })

  await fetchCurrentUser(true)
}

async function login(email: string, password: string): Promise<boolean> {

  const response = await fetch("/login", {
    method: 'POST',
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      email: email,
      password: password
    })
  })

  if (!response.ok || response.status == 401) {
    return false
  }

  await fetchCurrentUser(true)
  return true
}

async function updateInfo(id: string, name: string, email: string): Promise<boolean> {
  const response = await fetch(`/shopUser/${id}`, {
    method: "PATCH",
    credentials: "include",
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      userId: id,
      email: email,
      name: name
    })
  });

  if (!response.ok || response.status == 401) {
    return false
  }

  return true;
}

async function changeMyPassword(oldPassword: string, newPassword: string, verifyNewPassword: string)
  : Promise<boolean> {

  const response = await fetch('/register/changeOwnPassword', {
    method: "PATCH",
    credentials: "include",
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      oldPassword: oldPassword,
      newPassword: newPassword,
      verifyNewPassword: verifyNewPassword
    })
  });

  return response.ok;
}


async function registerCustomer(name: string, email: string, password: string): Promise<RegistrationResponse<string>> {
  const response = await registerUser(
    "/register/user",
    name,
    email,
    password
  );

  return processRegistrationResponse(response);
}

async function registerAdmin(name: string, email: string, password: string): Promise<RegistrationResponse<string>> {
  const response = await registerUser(
    "/register/admin",
    name,
    email,
    password
  );

  return processRegistrationResponse(response);
}

async function registerUser(endpoint: string, name: string, email: string, password: string): Promise<Response> {
  return await fetch(endpoint, {
    method: "POST",
    credentials: "include",
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      name: name,
      email: email,
      password: password
    })
  });
}

async function deleteUser(id: string): Promise<boolean> {
  const response = await fetch(`/shopUser/${id}`, {
    credentials: "include",
    method: "DELETE"
  });

  return response.ok
}

export function useUser() {
  return {
    currentUser: readonly(currentUser),
    fetchCurrentUser,
    registerCustomer,
    registerAdmin,
    logout,
    login,
    updateInfo,
    changeMyPassword,
    deleteUser
  }
}
