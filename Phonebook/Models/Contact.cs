using System;
namespace Phonebook.Models
{
    /// <summary>
    /// Модель контактов
    /// </summary>
    public class Contact
    {
        public string name;
        public string phoneNumber;

        public Contact(string name, string phoneNum)
        {
            this.name = name;
            this.phoneNumber = phoneNum;
        }

        public override string ToString()
        {
            return $"{name}:{phoneNumber}";
        }
    }
}

