
# BugBusters.Server

**BugBusters** is a company-specific developer panel designed for sharing issues and providing solutions. This application serves as a private version of StackOverflow, where employees within a company can ask questions, provide answers, and engage in community-driven problem-solving, without the concerns of public exposure.

## Features

- **User Management**: Registration, login, and profile management.
- **Roles**: Multiple user roles including Admin, Moderator, and User.
- **Q&A System**: Ask questions, answer them, and upvote/downvote content.
- **Admin Panel**: Admins can approve questions and answers before they go live.

## Technology Stack

- **Backend**: ASP.NET Core 7
- **Database**: SQL Server (MSSQL)
- **ORM**: Entity Framework Core (Code First, Migrations)
- **Logging**: Serilog
- **API Documentation**: Swagger
- **Authentication**: JWT
- **Mapping**: AutoMapper
- **Unit Testing**: NUnit, Moq
- **Architecture**: Clean Architecture, Repository Pattern
- **Best Practices**: Following industry standards and best practices

## Setup

### Prerequisites

- .NET 7 SDK
- SQL Server instance
- Node.js (for front-end setup if applicable)

### Installation

1. Clone this repository:
   ```bash
   git clone https://github.com/biswajitpanday/BugBusters.Server
   ```
2. Navigate to the solution directory and restore dependencies:
   ```bash
   cd BugBusters.Server
   dotnet restore
   ```
3. Apply migrations to set up the database schema:
   ```bash
   dotnet ef database update
   ```
4. Start the server:
   ```bash
   dotnet run
   ```

### Running Tests

To run the unit tests, use:
```bash
dotnet test
```

## API Documentation (Swagger)

The API is documented using Swagger, which is accessible once the server is running. Below is a screenshot of the Swagger UI:

![Swagger Screenshot](path-to-your-image/swagger-screenshot.png)

## Contributing

1. Fork the repository.
2. Create a new branch.
3. Implement your changes and create a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
