@echo off
echo Скрипт обновляет базу данных!

cd ../../../SmsProcessingService.Persistence

dotnet ef database update -c ApplicationDbContext --startup-project WebApi -v