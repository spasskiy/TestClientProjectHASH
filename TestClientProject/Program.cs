using System;
using System.Threading.Tasks;

namespace TestClientProject
{
    internal class Program
    {
        //Пароль нужно будет в каждый клиент отдельно подключать. Вероятно через конфиг.
        static string _password = "12345";
        static async Task Main(string[] args)
        {
            // Создаем клиента с указанным IP-адресом и портом
            Client client = new Client("127.0.0.14", 10555);

            try
            {
                // Подключаемся к серверу
                await client.ConnectAsync();

                // Отправляем логин на сервер
                string username = "user1"; // Пример логина
                await client.SendMessageAsync($"login:{username}");

                // Получаем ответ от сервера (ожидается : 401 Unauthorized: соль )
                string response = await client.ReceiveMessageAsync();
                Console.WriteLine($"Ответ от сервера: {response}");

                // Проверяем ответ от сервера
                if (response.StartsWith("401 Unauthorized: "))
                {
                    HashingService hashingService = new HashingService();
                    string hashedPassword = hashingService.HashPassword(_password, response.Substring(18));

                    // Отправляем хэшированный пароль на сервер
                    await client.SendMessageAsync(hashedPassword);

                    response = await client.ReceiveMessageAsync();
                    Console.WriteLine($"Ответ от сервера: {response}");


                    Console.WriteLine("Успешно залогинились. Завершаем сеанс.");

                }
                else
                {
                    Console.WriteLine("Неверный ответ от сервера. Подключение будет закрыто.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            finally
            {
                // Отключаем клиента
                client.Disconnect();
                Console.WriteLine("Сеанс окончен.");
            }
        }

    }
}
