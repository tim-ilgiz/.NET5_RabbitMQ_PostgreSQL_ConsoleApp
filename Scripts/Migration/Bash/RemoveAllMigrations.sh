#!/bin/bash 

cd ../../../SmsProcessingService.Persistence

#Реверт миграций
dotnet ef database update 0 -c ApplicationDbContext --startup-project ../WebApi -v

#Удаление миграций
dotnet ef migrations remove -c ApplicationDbContext --startup-project ../WebApi -v