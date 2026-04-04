import { ref, readonly } from "vue";

export interface User {
  name: string,
  email: string
}

const currentUser = ref<User | null>(null)
const initialized = ref(false)

export async function fetchCurrentUser(force = false): Promise<User | null> {
  if (initialized.value && !force) {
    return currentUser.value
  }

  const response = await fetch('/login/me', {
    credentials: "include"
  })
  if (response.status == 401 || !response.ok) {
    currentUser.value = null
  } else {
    const data = await response.json()
    currentUser.value = {
      name: data.name,
      email: data.email
    }
  }

  initialized.value = true
  return currentUser.value
}

export async function logout(): Promise<void> {
  await fetch('/login/logout', {
    credentials: 'include',
    method: 'POST'
  })

  await fetchCurrentUser(true)
}

export async function login(email: string, password: string): Promise<boolean> {

  const response = await fetch("/login/login", {
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

export function useUser() {
  return {
    currentUser: readonly(currentUser),
    fetchCurrentUser,
    logout,
    login
  }
}
