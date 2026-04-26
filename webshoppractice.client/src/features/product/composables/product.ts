export type Product = {
  productId: string,
  name: string,
  price: number,
  sellerId: string
}


async function createProduct(name: string, price: string): Promise<boolean>
{
  const response = await fetch("/product", {
    method: "POST",
    credentials: "include",
    body: JSON.stringify({
      name: name,
      price: price
    })
  });

  return response.ok;
}

async function getProductInfo(id: string): Promise<Product | false>
{
  const response = await fetch(`/product/${id}`);

  try {
    const json = await response.json();
    const product: Product = {
      productId: json.id,
      name: json.name,
      price: json.price,
      sellerId: json.sellerId
    }

    return product;
  } catch {
    return false;
  }
}

async function updateProductInfo(id: string, name: string, price: string): Promise<boolean>
{
  const response = await fetch(`/product/${id}`, {
    method: "PATCH",
    credentials: "include",
    body: JSON.stringify({
      id: id,
      name: name,
      price: price
    })
  });

  return response.ok;
}

async function deleteProduct(id: string): Promise<boolean>
{
  const response = await fetch(`/product/${id}`, {
    method: "DELETE",
    credentials: "include",
  });

  return response.ok;
}

export function useProduct() {
  return {
    createProduct,
    getProductInfo,
    updateProductInfo,
    deleteProduct
  }
}
