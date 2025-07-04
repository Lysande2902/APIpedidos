namespace APIPedidos.Models;

public static class ValidationMessages
{
    // Productos
    public const string ProductNotFound = "El producto no fue encontrado";
    public const string ProductNameRequired = "El nombre del producto es obligatorio";
    public const string ProductNameLength = "El nombre del producto debe tener entre 3 y 100 caracteres";
    public const string ProductNameInvalid = "El nombre del producto no puede contener caracteres especiales";
    public const string ProductDescriptionRequired = "La descripción del producto es obligatoria";
    public const string ProductDescriptionLength = "La descripción del producto debe tener entre 10 y 500 caracteres";
    public const string ProductPriceRequired = "El precio del producto es obligatorio";
    public const string ProductPricePositive = "El precio del producto debe ser mayor a 0";
    public const string ProductPriceMax = "El precio del producto no puede exceder $999,999.99";
    public const string ProductStockRequired = "La cantidad en stock es obligatoria";
    public const string ProductStockPositive = "La cantidad en stock debe ser mayor o igual a 0";
    public const string ProductStockMax = "La cantidad en stock no puede exceder 999,999";
    public const string ProductNameDuplicate = "Ya existe un producto con ese nombre";
    public const string ProductInOrders = "No se puede eliminar el producto porque está en órdenes existentes";
    public const string ProductIdInvalid = "El ID del producto debe ser un número positivo";

    // Órdenes
    public const string OrderNotFound = "La orden no fue encontrada";
    public const string OrderIdInvalid = "El ID de la orden debe ser un número positivo";
    public const string OrderStateInvalid = "El estado de la orden debe ser: Pendiente, Pagado o Enviado";
    public const string OrderAlreadyShipped = "No se puede modificar ni eliminar una orden que ya fue enviada";
    public const string OrderAlreadyPaid = "No se puede modificar una orden que ya fue pagada";

    // Ítems de Orden
    public const string OrderItemQuantityRequired = "La cantidad del ítem es obligatoria";
    public const string OrderItemQuantityPositive = "La cantidad del ítem debe ser mayor a 0";
    public const string OrderItemQuantityMax = "La cantidad del ítem no puede exceder 999";
    public const string OrderItemProductExists = "El producto ya existe en esta orden";
    public const string OrderItemProductNotFound = "El producto especificado no existe";
    public const string OrderItemInsufficientStock = "No hay suficiente stock disponible para este producto";

    // Paginación
    public const string PageNumberInvalid = "El número de página debe ser mayor a 0";
    public const string PageSizeInvalid = "El tamaño de página debe estar entre 1 y 100";
    public const string PageNumberMax = "El número de página no puede exceder 10,000";

    // Autenticación
    public const string InvalidCredentials = "Usuario o contraseña incorrectos";
    public const string UsernameRequired = "El nombre de usuario es obligatorio";
    public const string PasswordRequired = "La contraseña es obligatoria";
    public const string TokenRequired = "El token de autenticación es requerido";
    public const string TokenInvalid = "El token de autenticación es inválido o ha expirado";

    // General
    public const string InvalidRequest = "La solicitud contiene datos inválidos";
    public const string ServerError = "Ha ocurrido un error interno del servidor";
    public const string ResourceNotFound = "El recurso solicitado no fue encontrado";
} 