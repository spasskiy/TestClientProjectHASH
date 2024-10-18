using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestClientProject
{
    public class Client
    {
        private readonly string _serverIp;
        private readonly int _serverPort;
        private TcpClient _tcpClient;

        public Client(string serverIp = "127.0.0.1", int serverPort = 10555)
        {
            _serverIp = serverIp;
            _serverPort = serverPort;
        }

        // Метод для подключения к серверу
        public async Task ConnectAsync()
        {
            try
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(_serverIp, _serverPort);
                Console.WriteLine("Подключено к серверу.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
                throw;
            }
        }

        // Метод для отправки данных на сервер
        public async Task SendMessageAsync(string message)
        {
            try
            {
                if (_tcpClient == null || !_tcpClient.Connected)
                {
                    Console.WriteLine("Соединение с сервером не установлено.");
                    return;
                }

                NetworkStream stream = _tcpClient.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
                Console.WriteLine($"Отправлено сообщение: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке данных: {ex.Message}");
                throw;
            }
        }

        // Метод для получения ответа от сервера
        public async Task<string> ReceiveMessageAsync()
        {
            try
            {
                if (_tcpClient == null || !_tcpClient.Connected)
                {
                    Console.WriteLine("Соединение с сервером не установлено.");
                    return null;
                }

                NetworkStream stream = _tcpClient.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);               
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении данных: {ex.Message}");
                throw;
            }
        }

        // Метод для отключения от сервера
        public void Disconnect()
        {
            if (_tcpClient != null && _tcpClient.Connected)
            {
                _tcpClient.Close();
                Console.WriteLine("Отключено от сервера.");
            }
        }
    }
}
