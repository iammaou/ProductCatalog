# Product & Category Management API

## Goal

To provide a clean, layered RESTful Web API for managing products and product categories, with full CRUD support, flexible querying (filtering, sorting, paging), and consistent, well-documented error handling — so any client (browser, mobile app, or external system) can build a full catalog experience backed by SQL Server.

---

## Architecture & Tech Stack

The solution uses a classic layered architecture and is split into three focused projects that keep business logic, HTTP concerns, and verification separate:

- **Service** — The domain & application layer. Holds the Entity Framework Core models (`Entities`), the `ApplicationDbContext`, Code First migrations, business-logic interfaces (`IProductService`, `IProductCategoryService`) and their implementations, DTOs, query-parameter definitions, and mapping logic.
- **WebAPI** — The presentation layer and runnable entry point. Contains the ASP.NET Core controllers, request/response DTOs, and global exception-handling middleware.
- **Tests** — Unit tests for the service layer, backed by an in-memory Entity Framework Core database.

### Project Stack Breakdown

- **Backend:** .NET 9, ASP.NET Core Web API
- **Database:** SQL Server 2022 (running in a Docker container)
- **ORM:** Entity Framework Core 9 with Code First Migrations
- **Mapping:** Dedicated mapper classes (`Service/Mappers`)
- **Validation:** Data Annotations
- **API Documentation:** Swagger / OpenAPI (Swashbuckle)
- **Containerization:** Docker & Docker Compose (database only)

### Key Architectural Features

- Full **CRUD** operations for both `Categories` and `Products`
- **Filtering** by category, price range (`MinPrice` / `MaxPrice`), and availability (`IsActive`, `IsInStock`, `StockQuantity`)
- **Sorting** by price, name, or creation date (ascending / descending)
- **Paging** via `Page` and `PageSize` query parameters
- **Global exception-handling** middleware producing consistent error responses
- **Dependency Injection** using the built-in .NET container
- **Async / await** throughout every layer
- **DTOs** returned to the client instead of raw EF Core entities
- Automated **migration + seed** on application startup

---

## How to Run

The API targets the .NET 9 SDK and expects SQL Server 2022 via Docker Compose. The WebAPI project applies pending migrations and seeds initial data automatically on startup, so minimal setup is required.

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Docker with Docker Compose (for SQL Server)
- _(Optional)_ The `dotnet-ef` tool, to manage migrations manually: `dotnet tool install --global dotnet-ef`

### Quick Start

1. **Clone the Repository**

   ```bash
   git clone git@github.com:iammaou/Product-and-Category-Management-API.git
   cd Product-and-Category-Management-API
   ```

2. **Start SQL Server in Docker**

   ```bash
   docker compose up -d
   ```

   This runs SQL Server 2022 on port `1433` and persists its data in a named Docker volume.

3. **Run the API**

   ```bash
   dotnet run --project WebAPI
   ```

   On startup the application automatically applies any pending migrations and seeds the initial categories and products.

4. **Access the Application**
   - **HTTP API:** http://localhost:5195
   - **HTTPS API:** https://localhost:7065
   - **Swagger UI (OpenAPI docs):** http://localhost:5195/swagger
   - **Health Check:** http://localhost:5195/health

---

## API Reference

All endpoints are grouped under `/api` and are fully documented and interactive via Swagger UI. The base URL is `http://localhost:5195` (or `https://localhost:7065`).

### Category Endpoints

| Method   | Route                  | Description                        | Success | Errors      |
| -------- | ---------------------- | ---------------------------------- | ------- | ----------- |
| `GET`    | `/api/categories`      | List categories (paginated)        | `200`   |             |
| `GET`    | `/api/categories/{id}` | Retrieve a single category by `id` | `200`   | `404`       |
| `POST`   | `/api/categories`      | Create a new category              | `201`   | `400`       |
| `PUT`    | `/api/categories/{id}` | Update an existing category        | `200`   | `400` `404` |
| `DELETE` | `/api/categories/{id}` | Delete a category                  | `204`   | `404` `409` |

- Category list supports paging via `page` (default `1`) and `pageSize` (default `10`).
- Deleting a category that still has products returns **`409 Conflict`**.

### Product Endpoints

| Method   | Route                | Description                        | Success | Errors      |
| -------- | -------------------- | ---------------------------------- | ------- | ----------- |
| `GET`    | `/api/products`      | List products (filter, sort, page) | `200`   | `400`       |
| `GET`    | `/api/products/{id}` | Retrieve a single product by `id`  | `200`   | `404`       |
| `POST`   | `/api/products`      | Create a new product               | `201`   | `400`       |
| `PUT`    | `/api/products/{id}` | Update an existing product         | `200`   | `400` `404` |
| `DELETE` | `/api/products/{id}` | Delete a product                   | `204`   | `404`       |

### HTTP Status Code Conventions

- `200 OK` — successful `GET` / `PUT` (returns the payload)
- `201 Created` — successful `POST` (returns the created resource and its `Location` header)
- `204 No Content` — successful `DELETE`
- `400 Bad Request` — validation failure or invalid query parameters
- `404 Not Found` — resource with the given `id` does not exist
- `409 Conflict` — deleting a category that still contains products

---

## Querying: Filtering, Sorting & Paging (Products)

The product list endpoint accepts a rich set of optional query parameters.

### Query Parameters

| Parameter       | Type      | Default | Description                                 |
| --------------- | --------- | ------- | ------------------------------------------- |
| `page`          | `int`     | `1`     | Page number (must be ≥ 1)                   |
| `pageSize`      | `int`     | `10`    | Items per page (1–100)                      |
| `categoryId`    | `guid`    | —       | Filter products belonging to a category     |
| `minPrice`      | `decimal` | —       | Minimum price (≥ 0)                         |
| `maxPrice`      | `decimal` | —       | Maximum price (≥ 0, must be ≥ `minPrice`)   |
| `isActive`      | `bool`    | —       | Filter by active status                     |
| `stockQuantity` | `int`     | —       | Filter by exact stock quantity (≥ 0)        |
| `isInStock`     | `bool`    | —       | `true` = stock > 0, `false` = out of stock  |
| `sortBy`        | `string`  | —       | One of `price`, `name`, `createdat`         |
| `isDescending`  | `bool`    | `false` | Sort in descending order when set to `true` |

## Error Handling

A global exception handler (`WebAPI/Handlers/GlobalExceptionHandler.cs`) intercepts unhandled exceptions and returns consistent JSON problem responses instead of leaking stack traces (in production, the `detail` property is omitted).

### Exception-to-Status Mapping

| Exception                     | Status Code | Title                        |
| ----------------------------- | ----------- | ---------------------------- |
| `ArgumentException`           | `400`       | Invalid request parameters   |
| `KeyNotFoundException`        | `404`       | Resource not found           |
| `UnauthorizedAccessException` | `401`       | Unauthorized access          |
| Anything else                 | `500`       | An unexpected error occurred |

### Example Error Response

```json
{
  "statusCode": 400,
  "title": "Invalid request parameters",
  "traceId": "0HLL8V0HQH1HQ:00000002",
  "detail": null,
  "timestamp": "2026-09-05T12:00:00.0000000Z"
}
```

Model-validation failures (from Data Annotations) are also normalized to a consistent `Validation failed` response with a per-field error list.

---

## Testing

The API can be tested with any HTTP client:

- **Swagger UI:** http://localhost:5195/swagger (human-friendly, interactive)
- **Postman:** a ready-made collection is included at `ProductCatalog.postman_collection.json`
- **`curl` / any REST client:** see the query examples above
