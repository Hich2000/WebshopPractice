import { ref, readonly } from "vue";

export interface User {
  name: string,
  username: string
}

const currentUser = ref<User|null>(null)
const initialized = ref(false)

export async function fetchCurrentUser(force = false): Promise<User|null> {
  if (initialized.value && !force) {
    return currentUser.value
  }

  const response = await fetch('/login/me')
  if (response.status == 401 || !response.ok) {
    currentUser.value = null
  } else {
    const data = await response.json()
    currentUser.value = {
      name: data.name,
      username: data.username
    }
  }

  initialized.value = true
  return currentUser.value
}

export function useUser() {
  return {
    currentUser: readonly(currentUser),
    fetchCurrentUser
  }
}
