#!/bin/bash
set -e

export UID=$(id -u)
export GID=$(id -g)

echo "Запуск принудительной очистки..."
cd docker-compose

# docker-compose down || (echo "Ошибка! Не удалось остановить сервисы."; exit 1)
# docker system prune 
# docker volume prune

# echo "Принудительная очистка завершена!"

sleep 2

echo "Запуск сервисов..."

docker-compose up -d || (echo "Ошибка! Не удалось запустить сервисы."; exit 1)

echo "Сервисы успешно запущены!"

sleep 2

echo "Запуск применения миграции и добавление данных для синхронизации сервера базы данных 'dbpostgres'."

cd ../Scripts

sh autorun.sh || exit 1

echo "Миграция успешно применена и данные для синхронизации добавлены!"

sleep 2

echo "Инфраструктура успешно запущена!"