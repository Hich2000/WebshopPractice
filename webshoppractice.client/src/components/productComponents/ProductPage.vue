<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import ProductCard from '@/components/productComponents/ProductCard.vue'

type Products = {
  id: number
  name: string
  price: number
}[]

const route = useRoute()

const loading = ref<boolean>(false)
const post = ref<Products | null>(null)

async function fetchData() {
  post.value = null
  loading.value = true

  const response = await fetch('product')
  if (response.ok) {
    post.value = await response.json()
    loading.value = false
  }
}

onMounted(fetchData)

watch(
  () => route.fullPath,
  fetchData
)
</script>

<template>
  <div class="productPageComponent">
    <h1>Products</h1>

    <div v-if="loading" class="loading">
      Fetching products...
    </div>

    <div v-else-if="post">
      <div class="products-grid">
        <ProductCard v-for="product in post" :key="product.id" :id="product.id" :name="product.name"
          :price="product.price" />
      </div>
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
