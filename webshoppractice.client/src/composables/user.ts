import { ref, readonly } from "vue";

export interface User {
  id: string,
  name: string,
  email: string,
  level: string
}

export interface PassWordError {
  code: string,
  description: string
}

function isPasswordErrorArray(data: unknown): data is PassWordError[] {
  return Array.isArray(data) &&
    data.every(
      (item) =>
        typeof item.code === 'string' &&
        typeof item.description === 'string'
    );
}

const currentUser = ref<User | null>(null)
const initialized = ref(false)

export async function fetchCurrentUser(force = false): Promise<User | null> {
  if (initialized.value && !force) {
    return currentUser.value
  }

  const response = await fetch('/me', {
    credentials: "include"
  })
  if (response.status == 401 || !response.ok) {
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

  console.log(currentUser.value);
  initialized.value = true
  return currentUser.value
}

export async function logout(): Promise<void> {
  await fetch('/logout', {
    credentials: 'include',
    method: 'POST'
  })

  await fetchCurrentUser(true)
}

export async function login(email: string, password: string): Promise<boolean> {

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

export async function register(name: string, email: string, password: string): Promise<true | PassWordError[]> {
  const response = await fetch("/register/user", {
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

  if (response.status == 409) {
    const responseErrors = await response.json();
    if (isPasswordErrorArray(responseErrors)) {
      const passwordErrors = responseErrors;
      return passwordErrors;
    } else {
      return [
        {
          code: "unkownError",
          description: "An unknown error has occurred."
        }
      ]
    }
  }

  return true;
}

export async function updateInfo(id: string, name: string, email: string): Promise<boolean> {
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

export async function changeMyPassword(oldPassword: string, newPassword: string, verifyNewPassword: string): Promise<true | PassWordError[]> {

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

  if (response.status == 409) {
    const responseErrors = await response.json();
    if (isPasswordErrorArray(responseErrors)) {
      const passwordErrors = responseErrors;
      return passwordErrors;
    } else {
      return [
        {
          code: "unkownError",
          description: "An unknown error has occurred."
        }
      ]
    }
  }

  return true;
}

export function useUser() {
  return {
    currentUser: readonly(currentUser),
    fetchCurrentUser,
    logout,
    login,
    register,
    updateInfo,
    changeMyPassword
  }
}
