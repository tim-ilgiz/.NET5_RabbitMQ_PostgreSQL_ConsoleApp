@echo off
echo Скрипт применяет новую миграцию!
set /P name="Введите название миграции: "

cd ../../../SmsProcessingService.Persistence

dotnet ef migrations add %name% -c ApplicationDbContext --startup-project WebApi -v