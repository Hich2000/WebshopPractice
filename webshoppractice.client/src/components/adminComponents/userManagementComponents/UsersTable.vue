<script setup lang="ts">
import { Button, Column, DataTable } from 'primevue';
import { onMounted, ref, type Ref } from 'vue';
import { type User } from '@/composables/user';
import { router } from '@/main';

//datatable state
const items = ref<User[]>([])
const totalRecordCount: Ref<number> = ref(0);
const loading = ref(true);

//amount of rows shown per page
const rows = ref(10);

async function loadData(pageNumber: number, pageSize: number) {
  loading.value = true;

  try {
    const response = await fetch(`/shopUser/paged?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
      method: "GET",
      credentials: "include"
    });
    if (!response.ok) {
      throw new Error('Failed to fetch data');
    }
    const data = await response.json();

    console.log(data);
    items.value = [];
    data.body.forEach((element: { userId: string; name: string; email: string; userLevel: string; }) => {
      items.value.push({
        id: element.userId,
        name: element.name,
        email: element.email,
        level: element.userLevel
      })
    });

    rows.value = data.pageSize;
    totalRecordCount.value = data.totalRecordCount


  } catch(error) {
    console.log(error);
  } finally {
    loading.value = false
  }
}

async function onPageChange(event: { page: number; rows: number; }) {
  await loadData(event.page+1, event.rows);
}

onMounted(async () => {
  await loadData(1, rows.value);
});
</script>

<template>
  <div class="form-div">
    <div style="width: 100%; text-align: start; padding: 1rem;" v-on:click="router.back()">
      <Button>back</Button>
    </div>

    <DataTable :value="items" :lazy="true" :paginator="true" :rows="rows" :totalRecords="totalRecordCount" :loading="loading" @page="onPageChange">
      <Column field="name" header="name" />
      <Column field="email" header="email" />
      <Column field="level" header="level" />
    </DataTable>
  </div>
</template>
