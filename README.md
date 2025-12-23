# Ticket System

Небольшая система управления тикетами, построенная на **ASP.NET Core**, **PostgreSQL** и **Apache Kafka**.

Проект состоит из двух сервисов:

- **TicketService** — основной сервис для работы с тикетами;
- **NotificationService** — сервис-слушатель Kafka, отвечающий за уведомления.

---

## TicketService

REST API для управления тикетами.

### Основные возможности

- Управление проектами;
- Управление пользователями;
- CRUD для тикетов;
- Назначение исполнителя;
- Изменение статуса и приоритета;
- Публикация доменных событий в Kafka.

### Доменные сущности

- **Project**;
- **Ticket**;
- **User**;
- **TicketStatus** (справочник);
- **TicketPriority** (справочник).

### События Kafka

TicketService публикует события:

- `ticket.created`;
- `ticket.updated`;
- `ticket.assigned`.

Каждое событие передаётся в виде DTO и сериализуется в JSON.

## NotificationService

Фоновый сервис, который:

- Подписывается на Kafka-топики;
- Десериализует события;
- Отправляет уведомления через `INotificationSender`.

В текущей реализации уведомления **логируются в консоль**, но архитектура позволяет легко добавить:
- Email;
- Telegram;
- Slack;
- Webhooks.

## Технологии

- **.NET 8**;
- **ASP.NET Core**;
- **Entity Framework Core**;
- **PostgreSQL**;
- **Swagger**;
- **Apache Kafka**;
- **Docker**.

## Запуск

В директории решения необходимо выполнить команду: docker-compose up --build --detach

Будут запущены:
- PostgreSQL;
- Zookeeper;
- Kafka;
- Kafka UI;
- TicketService;
- NotificationService.

## Доступные URL

- Swagger: http://localhost:8080/swagger;
- Kafka UI: http://localhost:8081.
