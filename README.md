# ListToDOTask
Проек для створення власних задач.
Використовуються власні REST API  для створення, редагування, видалення та перегляду задач. Документація API була створена за допомогою OpenAPI.
Проект складається із двох програм:
1. Перша програма це власне Web API  для використання задач.

-Створив проек WEbAPI;

-Створив клас моделі для задач, які будуть зберігатися в БД;

-Підключив за допомогою EF БД;

-За допомогою NSwagStudio згенерував клас WebApiTODO.cs для роботи із задачами;
2. Друга програма це по суті користувацький інтерфейс. В ній користувач може проводити дії із задачами і переглядати вже створені.

-Створив проект ASP.NET Core MVC;

-Додав клас WebApiTODO.cs для роботи із задачами;

-Створив клас для моделі списків, які зберігають задачі та розумні списки, які не залежать від користувача;

-Також в класі AllLists.cs зберігаються додаткові поля для зберігання назви робочого(в якому знаходяться задачі з якими проводяться певні дії) списку, та зміна для отримання всіх спмсків один раз.

-Створив контролер для написання логіки роботи із списками та  задачами;

-Створив Views для користувацького інтерфейсу;

Для роботи другої програми потрібно запустити першу програму, запустивши програму можна використовувати другу програму, для маніпуляції даними і використання другої програми для планування задач.

Задачі поділені по категорія : розумні і користувацькі. Розумними задачами керує сама програма і користувач не має до них доступу(тільки перегляд). Користувацькі задачі можна редагувати та проводити з ними інші дія. Такаж можна створювати свій список задач об'єднюючи задачі за певною характеристикою.

Задачі можуть бути муль-задачами, якщо вони відмічень як мульт і коли в них співпадає заголовок.

Із видаленням списку із задачами всі задачі також видаляються.

В програмі також присутнє soft-delete, яке відбувається коли користувач помічає задачу як виконану.
