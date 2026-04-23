import type { MenuItem } from "primevue/menuitem";
import { useUser } from '@/shared/composables/user'
import { router } from '@/main';
import { ref } from "vue";

const { currentUser, logout } = useUser()

export interface NavItemList {
  mainMenuItems: MenuItem[],
  userMenuItems: MenuItem[]
}

const navItemList = ref<NavItemList>({
  mainMenuItems: [],
  userMenuItems: []
})

async function buildNavList(): Promise<NavItemList> {
  //start with empty list and construct it as we go along.
  const mainMenuItems: MenuItem[] = [];
  const userMenuItems: MenuItem[] = [
    {
      label: "Profile",
      command: () => router.push('/Profile')
    },
    {
      label: "Logout",
      command: async () => {
        await logout()
        router.push('/')
      }
    }
  ];

  //customer specific items
  const customerMenuItems: MenuItem[] = [];
  customerMenuItems.push({ label: "Home", command: () => router.push('/') });
  mainMenuItems.push(...customerMenuItems)

  //admin specific items
  if (currentUser.value?.level === "Admin") {
    const adminMenuItems: MenuItem[] = [];
    adminMenuItems.push({ label: "Admin Panel", command: () => router.push("/Admin") });
    mainMenuItems.push(...adminMenuItems);
  }

  navItemList.value = {
    mainMenuItems: mainMenuItems,
    userMenuItems: userMenuItems
  }

  return navItemList.value;
}

export function useNavItems() {
  return {
    navItemList: navItemList,
    buildNavList
  }
}
