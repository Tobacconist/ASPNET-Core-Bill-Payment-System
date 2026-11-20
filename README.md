# BillPaymentSystem

A bill payment web app. A customer pays a bill by entering the bill number and the payment date, and a late fee is added automatically when the payment is overdue. An admin can list, add and delete bills.

ASP.NET Core MVC (.NET 10), Entity Framework Core, SQL Server LocalDB.

## Technical decisions

- The late fee is 5% of the bill amount per day and is calculated in `Fatura.GecikmeliTutar`. It does not depend on the controller, so it can be unit tested on its own.
- Amounts are stored as `decimal(18,2)`, and the total including the late fee is rounded to the nearest kuruş.
- Operations that change data, such as payment and delete, use POST with an antiforgery token.
- The admin panel is protected with cookie authentication. There is no Identity setup and no extra table. A single admin account is read from configuration, and the password is stored as a `PasswordHasher` hash, never as plain text.
- The add-bill form uses `[Bind]` so only the expected fields are accepted.

## Running locally

Requirements: .NET 10 SDK and SQL Server LocalDB.

```powershell
cd BillPaymentSystem
dotnet tool restore
dotnet ef database update
dotnet run
```

The app listens on `http://localhost:5288`. The admin login page is `/Account/Login`. The development username and password are in `appsettings.Development.json`.

## Tests

The late fee calculation is tested with xUnit in the `BillPaymentSystem.Tests` project. No database is needed. Run it from the solution folder:

```powershell
dotnet test
```

## Other environments

In any other environment, set the `AdminAuth__Username` and `AdminAuth__PasswordHash` environment variables (or use User Secrets). Override the connection string with the `ConnectionStrings:FaturaDb` key.
