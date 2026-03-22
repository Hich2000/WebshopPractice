<script lang="ts">
  import ProductCard from '@/components/productComponents/ProductCard.vue'
  import { defineComponent } from 'vue';

  type Products = {
    id: number,
    name: string,
    price: number
  }[];

  interface Data {
    loading: boolean,
    post: null | Products
  }

  export default defineComponent({
    data(): Data {
      return {
        loading: false,
        post: null
      };
    },
    async created() {
      // fetch the data when the view is created and the data is
      // already being observed
      await this.fetchData();
    },
    watch: {
      // call again the method if the route changes
      '$route': 'fetchData'
    },
    methods: {
      async fetchData() {
        this.post = null;
        this.loading = true;

        var response = await fetch('product');
        if (response.ok) {
          this.post = await response.json();
          this.loading = false;
        }
      }
    },
    components: {
      ProductCard
    }
  });
</script>

<template>
  <div class="productPageComponent">
    <h1>Products</h1>

    <div v-if="loading" class="loading">
      Fetching products...
    </div>

    <div v-else-if="post" class="content">
      <div class="products-grid">
        <ProductCard v-for="product in post"
                     :key="product.id"
                     :id="product.id"
                     :name="product.name"
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
    grid-template-columns: repeat(4, 1fr); /* 5 cards per row */
    gap: 16px; /* spacing between cards */
    justify-items: center; /* center each card in its column */
    margin-top: 20px;
  }
</style>
