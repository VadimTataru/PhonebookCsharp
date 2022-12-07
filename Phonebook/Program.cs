using Phonebook.Models;
using Phonebook.Implementation;

int command;

var isRunning = true;
var phonebook = PhonebookImplementation.GetInstance();
phonebook.OnDataChange += DisplayDataChange;
while (isRunning)
{
    Console.Clear();
    Console.WriteLine("1. Добавить контакт");
    Console.WriteLine("2. Просмотреть контакты");
    Console.WriteLine("3. Изменить контакт");
    Console.WriteLine("4. Удалить контакт");
    Console.WriteLine("0. Выход");

    if (!int.TryParse(Console.ReadLine(), out command))
    {
        Console.WriteLine("Ввод не корректен");
        continue;
    }

    switch (command)
    {
        //Добавляем контакт
        case 1:
            {
                Console.Clear();
                Console.WriteLine("Добавить контакт");
                Console.WriteLine("Введите имя");
                var name = Console.ReadLine();
                Console.WriteLine("Введите номер");
                var phone = Console.ReadLine();
                if (name == null || phone == null)
                    Console.WriteLine("Похоже, что вы не ввели одно из полей");
                else
                {
                    var answer = await phonebook.CreateContactAsync(new Contact(name, phone));
                    if (!answer)
                        Console.WriteLine("Не вышло =(\nПохоже такой контакт уже существует =/");
                }
                Console.WriteLine("Нажмите любую кнопку, чтобы выйти в меню");
                Console.ReadKey();
                break;
            }
        //Просматриваем контакты
        case 2:
            {
                Console.Clear();
                Console.WriteLine("Список контактов");
                var contactList = await phonebook.ReadContactAsync();
                if (contactList.Count > 0)
                    for (var i = 0; i < contactList.Count; i++)
                        Console.WriteLine($"{i + 1}. {contactList[i]}");
                else
                    Console.WriteLine("Здесь пусто D:");
                Console.WriteLine("Нажмите любую кнопку, чтобы выйти в меню");
                Console.ReadKey();
                break;
            }
        //Изменяем контакт
        case 3:
            {
                Console.Clear();
                Console.WriteLine("Изменение контакта");
                var contactList = await phonebook.ReadContactAsync();
                if (contactList.Count > 0)
                    for (var i = 0; i < contactList.Count; i++)
                        Console.WriteLine($"{i + 1}. {contactList[i]}");
                else
                    Console.WriteLine("Здесь пусто D:");

                Console.WriteLine("Выберите номер контакта");
                int id;
                if (!int.TryParse(Console.ReadLine(), out id))
                {
                    Console.WriteLine("Ввод не корректен");
                    continue;
                }

                Console.WriteLine("Введите имя");
                var name = Console.ReadLine();
                Console.WriteLine("Введите номер");
                var phone = Console.ReadLine();
                if (name == null || phone == null)
                    Console.WriteLine("Похоже, что вы не ввели одно из полей");
                else
                {
                    await phonebook.UpdateContact(id - 1, new Contact(name, phone));
                }
                Console.WriteLine("Нажмите любую кнопку, чтобы выйти в меню");
                Console.ReadKey();
                break;
            }
        //Удолить
        case 4:
            {
                Console.Clear();
                Console.WriteLine("Удаление контакта");

                var contactList = await phonebook.ReadContactAsync();
                if (contactList.Count > 0)
                    for (var i = 0; i < contactList.Count; i++)
                        Console.WriteLine($"{i + 1}. {contactList[i]}");
                else
                    Console.WriteLine("Здесь пусто D:");

                Console.WriteLine("Выберите номер контакта");
                int id;
                if (!int.TryParse(Console.ReadLine(), out id))
                {
                    Console.WriteLine("Ввод не корректен");
                    continue;
                }
                else if (id > contactList.Count || id <= 0)
                {
                    Console.WriteLine("Ввод не корректен");
                    continue;
                }

                await phonebook.DeleteContact((id - 1));
                Console.WriteLine("Нажмите любую кнопку, чтобы выйти в меню");
                Console.ReadKey();
                break;
            }
        //Выйти в главное меню
        case 0:
            {

                isRunning = false;
                break;
            }
    }
}

void DisplayDataChange(string message, ConsoleColor consoleColor)
{
    Console.ForegroundColor = consoleColor;
    Console.WriteLine(message);
    Console.ResetColor();
}