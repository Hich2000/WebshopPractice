<script setup lang="ts">
import { Button, Column, DataTable } from 'primevue';
import { onMounted, ref, type Ref } from 'vue';
import { useConfirm } from 'primevue/useconfirm'
import { ConfirmPopup } from 'primevue';
import { type Product, useProduct } from '@/features/product/composables/product';

const { deleteProduct } = useProduct();

//datatable state
const items = ref<Product[]>([]);
const totalRecordCount: Ref<number> = ref(0);
const loading = ref<boolean>(true);
const pageNumber = ref<number>(1);

//amount of rows shown per page
const rows = ref<number>(10);

async function loadData(pageNumber: number, pageSize: number) {
  loading.value = true;

  try {
    const response = await fetch(`/product/paged?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
      method: "GET",
      credentials: "include"
    });
    if (!response.ok) {
      throw new Error('Failed to fetch data');
    }
    const data = await response.json();

    items.value = [];
    data.body.forEach((element: { id: string; name: string; price: number; sellerId: string; }) => {
      items.value.push({
        productId: element.id,
        name: element.name,
        price: element.price,
        sellerId: element.sellerId
      })
    });

    rows.value = data.pageSize;
    totalRecordCount.value = data.totalRecordCount

  } catch (error) {
    console.log(error);
  } finally {
    loading.value = false
  }
}

async function onPageChange(event: { page: number; rows: number; }) {
  pageNumber.value = event.page + 1;
  await loadData(event.page + 1, event.rows);
}

const confirm = useConfirm()
// eslint-disable-next-line @typescript-eslint/no-explicit-any
const confirmDelete = (event: any, deleteIndex: number) => {
  confirm.require({
    target: event.currentTarget,
    message: 'Are you sure you want to delete this user?',
    icon: 'pi pi-exclamation-triangle',
    acceptLabel: 'Yes',
    rejectLabel: 'No',
    accept: async () => {
      const toDelete = items.value[deleteIndex];
      loading.value = true;
      await deleteProduct(toDelete!.productId);
      await loadData(pageNumber.value, rows.value);
    },
    reject: () => {
      console.log('Cancelled')
    }
  })
}

onMounted(async () => {
  await loadData(pageNumber.value, rows.value);
});
</script>

<template>
  <DataTable :value="items" :lazy="true" :paginator="true" :rows="rows" :totalRecords="totalRecordCount"
    :loading="loading" @page="onPageChange">
    <Column field="name" header="name" />
    <Column field="price" header="price" />
    <Column header="Delete">
      <template #body="slotProps">
        <Button icon="pi pi-trash" color="red" @click="(event) => confirmDelete(event, slotProps.index)"
          severity="danger" />
        <ConfirmPopup />
      </template>
    </Column>
  </DataTable>
</template>
