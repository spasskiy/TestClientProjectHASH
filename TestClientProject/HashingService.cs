using System;
using System.Text;
using Konscious.Security.Cryptography;

namespace TestClientProject
{
    internal class HashingService
    {
        public string HashPassword(string password, string salt)
        {
            // Преобразуем соль в байты
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            // Хэшируем пароль с использованием Argon2
            using (var hasher = new Argon2i(Encoding.UTF8.GetBytes(password)))
            {
                hasher.Salt = saltBytes;
                hasher.DegreeOfParallelism = 2; // Уровень параллелизма
                hasher.MemorySize = 256; // Размер памяти в килобайтах
                hasher.Iterations = 2; // Количество итераций

                // Возвращаем хэш пароля в виде строки
                byte[] hash = hasher.GetBytes(32); // Длина хэша 32 байта
                return Convert.ToBase64String(hash);
            }
        }
    }
}
