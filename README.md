# Sistema de Pedidos de E-Commerce

## Descripción
API REST para gestión de pedidos de una tienda en línea. El sistema permite a los usuarios crear pedidos, agregar productos y gestionar el estado de los mismos.

## Entidades del Dominio

### Product
- **Id**: Identificador único del producto
- **Name**: Nombre del producto
- **Description**: Descripción del producto
- **Price**: Precio unitario
- **StockQuantity**: Cantidad disponible en stock

### Order
- **Id**: Identificador único del pedido
- **CreatedAt**: Fecha de creación
- **State**: Estado del pedido (Pendiente, Pagado, Enviado)
- **Items**: Lista de ítems del pedido
- **Total**: Total calculado del pedido

### OrderItem
- **Id**: Identificador único del ítem
- **OrderId**: ID del pedido al que pertenece
- **ProductId**: ID del producto
- **Product**: Referencia al producto
- **Quantity**: Cantidad del producto
- **UnitPrice**: Precio unitario al momento de la compra
- **Subtotal**: Subtotal calculado

## Reglas de Negocio

### Regla Crítica 1: Productos de Solo Lectura
- **NO** se permite agregar, eliminar o modificar productos
- Solo se pueden consultar productos existentes

### Regla Crítica 2: Productos Únicos por Orden
- **NO** se permite agregar un producto que ya existe en una orden
- **NO** se permite eliminar productos de una orden

## Endpoints

### Productos

#### GET /api/products
Obtiene todos los productos disponibles.

**Respuesta:**
```json
[
  {
    "id": 1,
    "name": "Laptop HP Pavilion",
    "description": "Laptop de 15 pulgadas con procesador Intel i5",
    "price": 899.99,
    "stockQuantity": 10
  }
]
```

#### GET /api/products/{productId}
Obtiene un producto específico por su ID.

**Respuesta:**
```json
{
  "id": 1,
  "name": "Laptop HP Pavilion",
  "description": "Laptop de 15 pulgadas con procesador Intel i5",
  "price": 899.99,
  "stockQuantity": 10
}
```

### Pedidos

#### GET /api/orders
Obtiene todos los pedidos.

#### GET /api/orders/{orderId}
Obtiene un pedido específico por su ID.

#### POST /api/orders
Crea un nuevo pedido.

**Respuesta:**
```json
{
  "id": 1,
  "createdAt": "2024-01-15T10:30:00Z",
  "state": "Pendiente",
  "total": 0,
  "items": []
}
```

#### POST /api/orders/{orderId}/items
Agrega un ítem a un pedido existente.

**Body:**
```json
{
  "productId": 1,
  "quantity": 2
}
```

**Respuesta:**
```json
{
  "id": 1,
  "productId": 1,
  "productName": "Laptop HP Pavilion",
  "quantity": 2,
  "unitPrice": 899.99,
  "subtotal": 1799.98
}
```

#### PUT /api/orders/{orderId}/state
Actualiza el estado de un pedido.

**Body:**
```json
{
  "state": "Pagado"
}
```

**Estados válidos:**
- "Pendiente"
- "Pagado"
- "Enviado"

## Ejecución del Proyecto

1. **Clonar el repositorio**
2. **Navegar al directorio del proyecto**
3. **Ejecutar el proyecto:**
   ```bash
   dotnet run
   ```
4. **Acceder a la API:**
   - Swagger UI: https://localhost:7001/swagger
   - API Base URL: https://localhost:7001/api

## Pruebas

El archivo `APIPedidos.http` contiene ejemplos de requests para probar todos los endpoints de la API.

## Tecnologías Utilizadas

- .NET 9.0
- ASP.NET Core Web API
- C# 12
- OpenAPI/Swagger 