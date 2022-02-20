#!/bin/bash 
	
echo Введите название миграции:
 
read name

sh RemoveBinAndObjFolders.sh

cd ../../../SmsProcessingService.Persistence

dotnet ef migrations add $name -c ApplicationDbContext --startup-project WebApi -v