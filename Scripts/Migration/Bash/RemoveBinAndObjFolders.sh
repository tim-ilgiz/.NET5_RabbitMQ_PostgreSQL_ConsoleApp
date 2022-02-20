#!/bin/bash
cd ../../..

find . -iname "bin" | xargs rm -rf || (echo "Ошибка! Не удалось удалить папки bin."; exit 1)
find . -iname "obj" | xargs rm -rf || (echo "Ошибка! Не удалось удалить папки obj."; exit 1)

echo "Папки bin и obj успешно удалены!"

dotnet clean