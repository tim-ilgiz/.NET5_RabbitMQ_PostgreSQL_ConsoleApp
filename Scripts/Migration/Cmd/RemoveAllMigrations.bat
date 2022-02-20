@echo off

cd ../../../SmsProcessingService.Persistence

echo Реверт миграций!
dotnet ef database update 0 -c ApplicationDbContext --startup-project WebApi

echo Удаление миграций!
dotnet ef migrations remove -c ApplicationDbContext --startup-project WebApi