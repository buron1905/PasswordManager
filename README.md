# Password Manager
Mobile and web application

Supervisor: Ing. Jan Janoušek</br>
Student:    Daniel Buroň

Web: https://passwordmanagerwebapi.azurewebsites.net/

Pro vytvoření databáze je možno využít connection stringu v appsettings a nad projektem Persistance pomocí Package Manager Console spustit příkaz update-database.
Druhou možností je připojit se na databázi Azure pomocí následujícího connection stringu: "Data Source=tcp:passwordmanager-webapidbserver.database.windows.net,1433;Initial Catalog=PasswordManager.WebAPI_db;User Id=bachelorthesis2023@passwordmanager-webapidbserver;Password=8aTPDDzXEzEVVy"

Doporučuji otevřít solution PasswordManager.sln, jež obsahuje všechny projekty. Následně se dá vybrat spuštění mobilní aplikace nebo webové aplikace. Spuštěním webové aplikace se spustí ASP.NET i Angular.
Na webu se pak dá najít na: https://localhost:4200/

