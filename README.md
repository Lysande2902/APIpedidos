# Sistema de Pedidos E-Commerce API

API REST para gestión de pedidos de una tienda en línea con autenticación JWT y paginación.

## Características

- **Autenticación JWT**: Sistema de autenticación basado en tokens JWT
- **Paginación**: Endpoints paginados para productos y órdenes
- **Gestión de Productos**: CRUD completo con validaciones de negocio
- **Gestión de Órdenes**: Creación, modificación y cambio de estados
- **Reglas de Negocio**: Validaciones para mantener integridad de datos
- **Entity Framework Core**: Base de datos SQL Server con migraciones
- **Swagger/OpenAPI**: Documentación automática de la API

## Configuración

### Credenciales de Acceso
- **Usuario**: `admin`
- **Contraseña**: `admin123`

### Configuración JWT
- **Clave Secreta**: Configurada en `appsettings.json`
- **Expiración**: 24 horas por defecto
- **Emisor**: APIPedidos
- **Audiencia**: APIPedidosUsers

## Endpoints

### Autenticación

#### POST /api/auth/login
Obtener token JWT para autenticación.

**Request Body:**
```json
{
  "username": "admin",
  "password": "admin123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "expiresAt": "2024-01-15T10:30:00Z"
}
```

### Productos

#### GET /api/products
Obtener todos los productos (requiere autenticación).

#### GET /api/products/paginated?pageNumber=1&pageSize=10
Obtener productos paginados (requiere autenticación).

**Parámetros:**
- `pageNumber`: Número de página (default: 1)
- `pageSize`: Tamaño de página (default: 10, máximo: 100)

**Response:**
```json
{
  "items": [...],
  "totalCount": 150,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 15,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

#### GET /api/products/{id}
Obtener un producto específico (requiere autenticación).

#### POST /api/products
Crear un nuevo producto (requiere autenticación).

**Validaciones:**
- No se permiten nombres duplicados

#### PUT /api/products/{id}
Actualizar un producto existente (requiere autenticación).

**Validaciones:**
- No se permiten nombres duplicados

#### DELETE /api/products/{id}
Eliminar un producto (requiere autenticación).

**Validaciones:**
- No se puede eliminar si está en órdenes existentes

### Órdenes

#### GET /api/orders
Obtener todas las órdenes (requiere autenticación).

#### GET /api/orders/paginated?pageNumber=1&pageSize=10
Obtener órdenes paginadas (requiere autenticación).

#### GET /api/orders/{id}
Obtener una orden específica (requiere autenticación).

#### POST /api/orders
Crear una nueva orden (requiere autenticación).

#### POST /api/orders/{orderId}/items
Agregar un ítem a una orden (requiere autenticación).

**Request Body:**
```json
{
  "productId": 1,
  "quantity": 2
}
```

**Validaciones:**
- No se puede agregar un producto que ya esté en la orden
- La cantidad debe ser mayor a 0

#### PUT /api/orders/{orderId}/state
Cambiar el estado de una orden (requiere autenticación).

**Request Body:**
```json
{
  "state": "Pagado"
}
```

**Estados válidos:**
- `Pendiente`
- `Pagado`
- `Enviado`

## Reglas de Negocio

### Productos
1. **Nombres únicos**: No se permiten productos con nombres duplicados
2. **Eliminación restringida**: No se puede eliminar un producto que esté en órdenes existentes
3. **Stock**: El stock se actualiza automáticamente al agregar productos a órdenes

### Órdenes
1. **Estados**: Las órdenes tienen estados: Pendiente, Pagado, Enviado
2. **Ítems únicos**: No se puede agregar el mismo producto más de una vez a una orden
3. **Cálculo automático**: Los subtotales y totales se calculan automáticamente

## Base de Datos

### Entidades
- **Product**: Productos con nombre, descripción, precio y stock
- **Order**: Órdenes con fecha, estado y total
- **OrderItem**: Ítems de orden con producto, cantidad y precio unitario

### Relaciones
- Una orden puede tener múltiples ítems
- Un ítem pertenece a una orden y referencia un producto
- Restricción de eliminación en cascada para productos en órdenes

## Datos de Prueba

El sistema incluye datos de prueba:
- **150 productos** únicos con nombres incrementales
- **100 órdenes** con estados variados
- **Múltiples ítems** por orden

## Uso

### 1. Ejecutar la aplicación
```bash
dotnet run
```

### 2. Acceder a Swagger
```
https://localhost:7001/swagger
```

### 3. Autenticarse
1. Usar el endpoint `/api/auth/login` con las credenciales
2. Copiar el token del response
3. Hacer clic en "Authorize" en Swagger
4. Ingresar: `Bearer {token}`

### 4. Probar endpoints
- Todos los endpoints requieren autenticación
- Usar los endpoints paginados para grandes volúmenes de datos
- Seguir las reglas de negocio documentadas

## Tecnologías

- **.NET 9.0**
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **Swagger/OpenAPI**
- **ASP.NET Core Web API** 