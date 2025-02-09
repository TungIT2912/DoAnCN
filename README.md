# **Dental Service Booking & Management System**

## **Overview**
This is a **dental service booking system** that allows users to browse dental services, book appointments with available dentists, and receive **email reminders** before their scheduled visits. The system also includes an **admin panel** for managing employees, tracking revenue, and handling patient invoices.

## **Key Features**
- **User Features:**
  - Browse available **dental services**.
  - **Book appointments** based on the dentist’s available shifts.
  - Receive **email reminders** one day before appointments.
  - Follow up with dentists and select services for treatment.

- **Admin Features:**
  - **Manage employee accounts**.
  - View **revenue reports** by year.
  - **Export & import** data for invoices and patient records.
  - Handle appointment scheduling conflicts.

## **Technology Stack**
- **Backend:** ASP.NET Core Web API
- **Frontend:** ASP.NET Core MVC with Razor Views
- **Database:** SQL Server, Entity Framework Core (EF Core)
- **Version Control:** GitHub
- **Other:** JavaScript, CSS

## **Installation & Setup**
### **Prerequisites**
- .NET SDK (Version 6.0+)
- SQL Server
- Visual Studio (Recommended)

### **Steps to Run the Project**
1. **Clone the repository:**
   ```bash
   git clone https://github.com/TungIT2912/DoAnCN.git
   cd DoAnCN
   ```
2. **Setup database:**
   - Go to ApplicationnDbContext in foder Data change
   ```ApplicationnDbContext
       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
      optionsBuilder
          .UseSqlServer("Server=YOUR_SERVER;Database=YOUR_NAME_DATABASE;Integrated Security=True;Trust Server Certificate=True")
          .EnableSensitiveDataLogging(); // Enable sensitive data logging for detailed exception information

      base.OnConfiguring(optionsBuilder);
  }
  ```
   - Update the connection string in `appsettings.json`.
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_NAME_DATABASE;Integrated Security=True;TrustServerCertificate=True;"
   }
   ```
   - Apply migrations and seed the database:
   add-migration name
   update-database
   ```
3. **Run the project:**
   ```bash
   dotnet run
   ```
4. Open the browser and go to:
   ```
   http://localhost:5000
   ```

## **Contributing**
Feel free to submit issues and pull requests if you'd like to contribute to this project!

## **License**
This project is licensed under the MIT License.

---
### 📌 **Author:**
- **Nguyen Thanh Tung**
- 📧 nguyenthanhtung29122003@gmail.com
- 🔗 [GitHub Profile](https://github.com/TungIT2912)
