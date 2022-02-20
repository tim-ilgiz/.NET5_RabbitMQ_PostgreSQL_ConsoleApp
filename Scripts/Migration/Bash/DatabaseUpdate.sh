#!/bin/bash 

cd ../../../SmsProcessingService.Persistence
dotnet ef database update --startup-project ../WebApi -c ApplicationDbContext -v
