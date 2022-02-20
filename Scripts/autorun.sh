#!/bin/bash
set -e

ls

cd Migration/Bash

echo "Удаляем папки bin и obj"

sh RemoveBinAndObjFolders.sh || exit 1

echo "Применяем миграцию базы даннных..."

sh DatabaseUpdate.sh || (echo "Ошибка! Проверьте строку подключения к базе данных. Возможно, указан не localhost в качестве хоста и порт задан не внешний." && exit 1)

echo "Миграция успешно применена!"