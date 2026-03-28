<script setup>defineProps({
    id: Number,
    name: String,
    price: Number
  })</script>

<template>
  <div class="product-card">
    <img src="@/assets/logo.svg" />
    <h2 class="product-name">{{ name }}</h2>
    <p class="product-id">ID: {{ id }}</p>
    <p class="product-price">€{{ price.toFixed(2) }}</p>
    <p class="product-description clamp">
      Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore
    </p>
  </div>
</template>


<style scoped>
  .product-card {
    border: 1px solid #ddd;
    border-radius: 10px;
    padding: 16px;
    background: #fff;
    box-shadow: 0 2px 6px rgba(0,0,0,0.1);
  }

  .product-name {
    font-size: 1.2rem;
    margin-bottom: 8px;
  }

  .product-price {
    font-weight: bold;
    color: #2ecc71;
  }

  .product-description {
    font-size: 1.2rem;
  }

  .clamp {
    display: -webkit-box;
    -webkit-line-clamp: 4;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }
</style>
