import type { MenuItem } from "primevue/menuitem";
import { useUser } from '@/composables/user'
import { router } from '@/main';
import { ref, type Ref } from "vue";

const { currentUser, logout } = useUser()

export interface NavItemList {
  mainMenuItems: readonly MenuItem[],
  userMenuItems: readonly MenuItem[]
}

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
  adminMenuItems.push({ label: "Admin Panel", command: () => router.push("/Placeholder") });
  mainMenuItems.push(...adminMenuItems);
}

const navItemList: Ref<NavItemList> = ref<NavItemList>({
  mainMenuItems: mainMenuItems,
  userMenuItems: userMenuItems
});

export function useNavItems() {
  return {
    navItemList: navItemList
  }
}
