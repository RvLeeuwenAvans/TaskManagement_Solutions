# TaskManagement

TaskManagement is a project consisting of three main components:

- **API (.NET)**: A mock backend built with ASP.NET Core, featuring JWT authentication and Swagger documentation.
- **Mobile App (MAUI)**: A mobile app for .NET MAUI focused on task management.
- **CMS (Blazor)**: A Blazor Server app with MudBlazor for managing mocked data such as users and objects.

---

## Installation Guide

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- Rider or Visual Studio 2022 with MAUI and Blazor workloads.
- Android Emulator or physical test device (Windows required).
- Running MariaDB server:  
  [Download MariaDB 11.8.2 MSI (64-bit)](https://mariadb.org/download/?t=mariadb&p=mariadb&r=11.8.2&os=windows&cpu=x86_64&pkg=msi&mirror=mirhosting-nl)

---

## Setup

### Repository and Build
1. Clone this repository.
2. Build all projects in the solution.

### API Setup
1. Create a database in MariaDB with the name: `taskservice`.
2. Adjust the connection string in `TaskManagement.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=taskservice;Uid=root;Pwd=ROOT;"
}
```

3. Update the database with Entity Framework (this must be run in the TaskManagement.API project):

```bash
dotnet ef database update --project TaskManagement.Infrastructure --startup-project TaskManagement.API --context TaskManagementDatabaseContext
```

4. Seed the database:

```bash
dotnet run --seed
```

The API should now be running and the database contains test data.

### **Running Unit Tests for the API:**
Open and run the `TaskManagement.Tests` solution in the API folder.

---

### MAUI App Setup

1. Ensure you have a working Android emulator or physical test device.
2. The MAUI app works on Android and as a Windows app (via WinUI).
3. Make sure the API is running when testing the app.
4. **Android emulator**: adjust the API endpoint in  
   `TaskManagement.MobileApp/Properties/appsettings.json` to connect to localhost via the emulator, probably like this:
```json
{
  "ApiSettings": {
    "BaseUrl": "http://10.0.2.2:5104/api/"
  }
}
```
5. **Windows**: adjust the API endpoint in  
   `TaskManagement.MobileApp/Properties/appsettings.json` to connect to localhost, probably like this:
```json
{
  "ApiSettings": {
    "BaseUrl": "http://127.0.0.1:5104/api/"
  }
}
```
6. The application should now work when you run the solution.
    * For test data, consult the seeders.

#### **Running UI/Device Tests for the API:**
Consult the readme in `TaskManagement.Mobile.Tests`.

---

### Blazor CMS Setup

* The CMS is based on: https://mudblazor.com/getting-started/installation#using-templates.
* And initialized with: https://github.com/MudBlazor/Templates.

If issues arise, you can consult both documents above.

1. Make sure the API is running when testing the app.
2. Adjust the API URI in `TaskManagement.CMS/Properties/appsettings.json` to point to the endpoint where the API is running.
```json
{
  "ApiSettings": {
    "BaseUrl": "http://127.0.0.1:5104/api/"
  }
}
```
3. Build and run the project.
4. The application should launch itself directly in your default browser. If this doesn't happen, something went wrong.
    1. See troubleshooting below.

#### Troubleshooting:
* If the application builds but doesn't launch directly, clear the previous build and perform a clean build.
