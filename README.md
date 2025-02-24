# Animal Inventory API

## 📌 Project Overview
The **Animal Inventory API** is a RESTful service that manages a list of stolen animals along with their respective locations. This API is designed using **ASP.NET Core** with **Entity Framework Core** as the ORM.

## 🚀 Features
- 📋 **Manage Animals**: Add, retrieve, and track animals in inventory.
- 📍 **Track Locations**: Store and retrieve stolen locations of animals.
- 🗄️ **Database Support**: Uses Entity Framework Core with a relational database.
- 📈 **Best Practices**: Implements API best practices, caching, and logging with Serilog.

## 🏗️ Technologies Used
- **ASP.NET Core** (Web API)
- **Entity Framework Core** (ORM)
- **SQL Server** (Database)
- **Serilog** (Logging)
- **Swagger** (API Documentation)

## 🏛️ Database Schema
### Animal Table
| Column      | Type    | Description |
|------------|--------|-------------|
| Id         | int    | Primary key |
| Name       | string | Animal name |
| Images     | string | Image URL |

### Location Table
| Column     | Type     | Description |
|------------|---------|-------------|
| Id        | int     | Primary key |
| Latitude  | double  | Latitude of stolen location |
| Longitude | double  | Longitude of stolen location |
| Address   | string  | Stolen location address |
| AnimalId  | int     | Foreign key referencing `Animal.Id` |

## 📌 API Endpoints
### 🐘 Animal Endpoints
| Method | Endpoint         | Description |
|--------|----------------|-------------|
| GET    | `/api/animals`  | Get all animals |
| GET    | `/api/animals/{id}` | Get an animal by ID |
| POST   | `/api/animals`  | Add a new animal |

### 📍 Location Endpoints
| Method | Endpoint             | Description |
|--------|---------------------|-------------|
| GET    | `/api/locations`     | Get all locations |
| GET    | `/api/locations/{id}` | Get a location by ID |

## 🔧 Setup and Installation
### Prerequisites
- .NET SDK 7 or later
- SQL Server
- Entity Framework Core CLI

### 🛠️ Steps to Run the Project
1. **Clone the Repository**
   ```sh
   git clone https://github.com/your-repo/animal-inventory-api.git
   cd animal-inventory-api
   ```
2. **Update Database Connection String**
   Update the `appsettings.json` file:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=AnimalDB;Trusted_Connection=True;"
   }
   ```
3. **Apply Migrations**
   ```sh
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
4. **Run the API**
   ```sh
   dotnet run
   ```
5. **Access API in Browser**
   Open `http://localhost:5000/swagger` to view API documentation.

## 📜 Example Request (JSON)
To add an animal, send a `POST` request to `/api/animals` with this JSON body:
```json
{
  "name": "Elephant",
  "images": "elephant.jpg",
  "location": {
    "latitude": -1.2921,
    "longitude": 36.8219,
    "address": "Nairobi National Park"
  }
}
```

## 🛠️ Running EF Migrations
To update the database with the latest schema:
```sh
dotnet ef migrations add UpdateSchema
```
```sh
dotnet ef database update
```

## 📜 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing
Contributions are welcome! Feel free to submit issues and pull requests.

---
🎯 **Happy Coding!** 🚀

