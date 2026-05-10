SETUP INSTRUCTIONS
==================

1. Open SQL Server Management Studio (SSMS)
2. Connect to your local SQL Server instance
3. Note the exact Server Name shown in the login dialog
4. Update appsettings.json → ConnectionStrings → DefaultConnection
   replacing the Server= value with your actual server name
5. In SSMS, open and run: DatabaseCreate.sql
   (This creates FarmToTableDB and all 8 tables)
6. In SSMS, open and run: SeedData.sql
   (This populates all tables with sample data)
7. In Visual Studio 2022, open FarmToTable.sln
8. Press F5 to run
9. Navigate to https://localhost:{port}

COMMON SERVER NAME VALUES:
- Full SQL Server:  .  or  localhost  or  AHMED\MSSQLSERVER
- SQL Server Express:  .\SQLEXPRESS  or  AHMED\SQLEXPRESS
- LocalDB:  (localdb)\MSSQLLocalDB
