# AFIRegistrationAPI

AFIRegistrationAPI is a RESTful API built with **ASP.NET Core (.NET 8.0)**. It provides endpoints for managing customer registrations and policies. The API uses **SQLite** as the database backend, making it lightweight and easy to set up for development and testing.

## Features

- **Customer Registration**: Create and retrieve customer data.
- **Policy Management**: Retrieve and create insurance policy records.
- **SQLite Integration**: Lightweight database using Entity Framework Core, database first design.
- **Modular Structure**: Organized into Controllers, DTOs, Models, Repositories, and Mappers for maintainability.
 
## Project Structure

```bash
AFIRegistrationAPI/
├── Controllers/             
│   ├── CustomerController.cs
│   └── PolicyController.cs
├── DTO/                     
├── Database/                
│   └── database.db          # Pre-included SQLite database
├── Mappers/                 
├── Migrations/              
├── Models/ 
├── Postman/ 
├── Repositories/            
├── Properties/              
├── appsettings.json         
├── Program.cs               
├── AFIRegistrationAPI.csproj
└── AFIRegistrationAPI.sln   
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Installation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/codepig40/AFIRegistrationAPI.git
   cd AFIRegistrationAPI
   ```

2. **No database setup required**:

   A pre-configured SQLite database is already included in the `Database` folder as `database.db`. The connection string is ready to use:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=Database/database.db"
     }
   }
   ```

3. **Scaffold models from database (optional)**:

   If needed, you can regenerate the models using Entity Framework:

   ```bash
   dotnet ef dbcontext scaffold "Data Source=Database/database.db" Microsoft.EntityFrameworkCore.Sqlite -o Models -f
   ```

4. **Run the application**:

   ```bash
   dotnet run
   ```

   The API should be accessible at `https://localhost:7241` or `http://localhost:5089`.

## API Endpoints

### CustomerController

| Method | Endpoint   | Description               |
|--------|------------|---------------------------|
| GET    | /api/Customer           | Retrieve all customers    |
| POST   | /api/Customer/register  | Register a new customer   |

### PolicyController

| Method | Endpoint                                  | Description                          |
|--------|-------------------------------------------|--------------------------------------|
| GET    | /api/Policy                                          | Retrieve all policies                |
| GET    | /api/Policy/GetPolicyById/{id}                       | Get policy by internal ID            |
| GET    | /api/Policy/GetPolicyByReference/{policyReference}   | Get policy by reference string       |
| GET    | /api/Policy/GetPolicyByCustomerId/{id}               | Get policy by customer ID            |
| POST   | /api/Policy                                          | Create a new policy                  |


5. **Testing**:

This solution includes one or more test projects using [xUnit](https://xunit.net/).


   ```bash
   dotnet test
   ```



## Postman Testing

Postman Collection file for api/customer/register in Postman/AFIRegistrationAPI.postman_collection.json

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

