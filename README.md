# Bill Payment & Penalty Calculation System

> A web-based utility bill management platform developed to streamline customer payments and administrative tasks.

This project was built during my software engineering internship to apply backend development concepts using **ASP.NET Core MVC** and **Entity Framework Core**. The core objective was to design a robust system that handles financial logic, specifically identifying overdue bills and calculating late fee penalties automatically.

## Key Features & Learning Outcomes

* **Automated Penalty Logic:** Programmed a business rule in the Controller that calculates overdue days and applies a dynamic 5% daily penalty rate to unpaid bills past their due date.
* **MVC Architecture:** Structured the application clearly with Models (`Fatura`), Razor Page Views, and dedicated Controllers (`BaslangicController`) to separate concerns and ensure maintainability.
* **Database Management:** Utilized **Entity Framework Core** for seamless database interactions. Configured a `DbContext` with initial seed data to test the application logic effectively.
* **Admin Dashboard:** Created an administrative interface to perform full CRUD operations (Add, View, Delete) on utility bills (Electricity, Water, Internet).

## Built With

* C# & ASP.NET Core MVC
* Entity Framework Core
* SQL Server (LocalDB)
* HTML/CSS (Bootstrap)

##  Development Process & Observations

One of the key challenges during development was ensuring the accurate integration of the database using Entity Framework Core. After writing a proper context class and configuring the connection string, the data flow became highly efficient. Debugging the logic for the overdue payment calculations helped me significantly improve my problem-solving skills and understanding of real-world financial application constraints.

## How to Run Locally

1. Clone the repository to your local machine.
2. Open the solution in **Visual Studio**.
3. Ensure the `Microsoft.EntityFrameworkCore.SqlServer` NuGet package is installed.
4. Run the project (`F5`). Entity Framework Core will automatically handle the database creation and seed initial data.
