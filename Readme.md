<b>Installation</b>:
<br><br>
1- Download the project<br>
2- Open the project<br>
3- Verify if you have installed .NET Core 2.2, if not go to SDK https://dotnet.microsoft.com/download/visual-studio-sdks?utm_source=getdotnetsdk&utm_medium=referral and select whether x64 SDK or x86 SDK depending your windows<br>
4- Add in the SQL Server Object Explorer into your MSSQLLocalDB a database with name "snacksstore" without authentication<br>
5- Finally run PM> update-database<br>
<br><br>
<b>To initialize data</b><br>
1-set the user by Roles:<br>
-First record must be Admin: He can edit the products, buy and see logs,<br>
-Second record must be users: They just buy products.<br>
2-Set the type of products<br>
3-Set the users in the table users and their roles(You must use the RoleID field to set the role regarding data in Roles table) to manage or buy products.

