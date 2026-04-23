<script setup lang="ts">
import { ref, onMounted } from 'vue'
import ProductCard from '@/components/productComponents/ProductCard.vue'
import { DataView } from 'primevue'
import type { Product } from '@/shared/composables/product';

//datatable state
const items = ref<Product[]>([]);
const totalRecordCount = ref<number>(0);
const loading = ref<boolean>(true);
const pageNumber = ref<number>(1);

//amount of rows shown per page
const rows = ref<number>(10);


async function loadData(pageNumber: number, pageSize: number) {
  loading.value = true

  const response = await fetch(`/product/Paged?pageNumber=${pageNumber}&pageSize=${pageSize}`)
  if (response.ok) {
    const json = await response.json()

    totalRecordCount.value = json.totalRecordCount;
    rows.value = json.pageSize

    items.value = [];
    json.body.forEach((element: { id: string; name: string; price: number }) => {
      items.value.push({
        productId: element.id,
        name: element.name,
        price: element.price
      })
    });
  }

  loading.value = false
}

async function onPageChange(event: { page: number; rows: number; }) {
  pageNumber.value = event.page + 1;
  await loadData(event.page + 1, event.rows);
}

onMounted(async () => {
  await loadData(pageNumber.value, rows.value);
});
</script>

<template>
  <div class="productPageComponent">
    <h1>Products</h1>

    <div v-if="loading" class="loading">
      Fetching products...
    </div>

    <div v-else>

      <DataView :value="items" :lazy="true" :paginator="true" :rows="rows" :totalRecords="totalRecordCount"
        :loading="loading" @page="onPageChange">
        <template #list="slotProps">
          <div class="products-grid">
            <ProductCard v-for="(item, index) in slotProps.items" :key="index" :product="item" />
          </div>
        </template>
      </DataView>
    </div>
  </div>
</template>

<style scoped>
.productPageComponent {
  text-align: center;
}

.products-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 16px;
  margin-top: 20px;
}
</style>
