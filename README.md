## Simple Seller APP
Aplikasi simple berbasis web dibangun untuk [Soal Tes Asesmen .NET Developer](https://docs.google.com/document/d/1zu_tdlNmCLWCMALsQwEnsYzu7d8AmV0E/edit)
### Teknologi yang digunakan

- **Database**: Microsoft SQL Server
- **Web MVC App**: .NET Core 8

### Prerequisites

Pastikan teknologi dibawah sudah terinstal di sistem:

- [.NET Core SDK (version 8)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Restore Database
Database pada aplikasi ini dibackup dengan format script file `db_Seller.sql` di lokasi direktori `simple-seller-app\database\db_Seller.sql`, pastikan sebelum run script ini, anda sudah membuat database dengan nama `db_seller`

### Web MVC App Setup (.NET Core 8)

1. Install .NET 8 (jika belum terinstal di sistem) dari official website: [.NET 8 Downloads](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

2. Navigate ke directori aplikasi:
```bash
cd simple-seller-app
```

3. Restore .NET Core package:
```bash
dotnet restore
```
4. Update `appsettings.json`dengan SQL Server connection string anda:

```json
"ConnectionStrings": {
    "SqlServer": "Server=namaserver];Database=db_seller;User Id=nama_user;Password=passwordakses;TrustServerCertificate=true;"
  },
```

5. build Identity dahulu, agar sistem Identity terpasang di sistem aplikasi:

```bash
dotnet ef migrations add InitialCreate --context AppIdentityDbContext
dotnet ef database update --context AppIdentityDbContext
```

5. build aplikasi:

```bash
dotnet build
```

6. Jalankan aplikasi:

```bash
dotnet run
```
