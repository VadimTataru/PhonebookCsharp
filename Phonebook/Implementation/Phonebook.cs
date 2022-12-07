using Phonebook.Models;

namespace Phonebook.Implementation
{
    /// <summary>
    /// Главный класс
    /// </summary>
    public class PhonebookImplementation
    {
        private const string filepath = "phonebook.txt";

        #region Events
        public Action<string, ConsoleColor> OnDataChange;
        #endregion

        #region Singleton
        /// <summary>
        /// Синглтон
        /// </summary>
        private static PhonebookImplementation? instance;

        private PhonebookImplementation()
        {
            if (!File.Exists(filepath))
                File.Create(filepath);

        }

        public static PhonebookImplementation GetInstance()
        {
            if (instance == null)
                instance = new PhonebookImplementation();
            return instance;
        }
        #endregion

        #region CRUD
        /// <summary>
        /// Ассинхронный метод записи нового контакта
        /// </summary>
        /// <param name="contact">На вход подаётся модель Contact</param>
        /// <returns>True - контакт записан, False - контакт не записан</returns>
        public async Task<bool> CreateContactAsync(Contact contact)
        {
            if (!File.Exists(filepath))
                File.Create(filepath);
            var contacts = await ReadContactAsync();

            if (contacts.Contains(contact))
                return false;

            using (StreamWriter sw = new StreamWriter(filepath, true))
            {
                await sw.WriteLineAsync($"{contact}");
                if (OnDataChange != null)
                    OnDataChange(contact.name, ConsoleColor.Green);
            }
            return true;
        }

        /// <summary>
        /// Ассинхронное получение контактов из файла
        /// </summary>
        /// <returns>Коллекция контактов</returns>
        public async Task<List<Contact>> ReadContactAsync()
        {
            if (!File.Exists(filepath))
                File.Create(filepath);
            var contacts = new List<Contact>();
            using (StreamReader reader = new StreamReader(filepath))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (line == null || line == String.Empty)
                        return contacts;
                    var contactData = line.Split(":");
                    contacts.Add(new Contact(
                        contactData[0],
                        contactData[1]
                        ));
                }
            }
            return contacts;
        }

        /// <summary>
        /// Ассинхронный метод изменения контакта
        /// </summary>
        /// <param name="id">Идентификатор контакта</param>
        /// <param name="contact">Заменяющие данные контакта</param>
        /// <returns>Nothing</returns>
        public async Task UpdateContact(int id, Contact contact)
        {
            var contacts = await ReadContactAsync();
            contacts[id] = contact;
            await RewriteFile(contacts);
            if (OnDataChange != null)
                OnDataChange(contacts[id].name, ConsoleColor.Yellow);
        }

        /// <summary>
        /// Удаляет контакт по идентификатору
        /// </summary>
        /// <param name="contactId">Идентификатор контакта</param>
        /// <returns>Another one (nothing)</returns>
        public async Task DeleteContact(int contactId)
        {
            var contacts = await ReadContactAsync();
            var contactToRemove = contacts[contactId];
            contacts.Remove(contactToRemove);
            await RewriteFile(contacts);
            if (OnDataChange != null)
                OnDataChange(contactToRemove.name, ConsoleColor.Red);
        }
        #endregion

        /// <summary>
        /// Вспомогательный метод перезаписи всех контактов
        /// </summary>
        /// <param name="contacts">Список контактов</param>
        /// <returns>And another one (nothing)</returns>
        private async Task RewriteFile(List<Contact> contacts)
        {
            using (StreamWriter sw = new StreamWriter(filepath, false))
            {
                foreach (var c in contacts)
                {
                    await sw.WriteLineAsync($"{c}");

                }
            }
        }
    }
}
