using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace TwoQueues
{
    class Program
    {
        static void Main()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    /* Criacao da fila */
                    CreateQueue(channel, "fila1");
                    CreateQueue(channel, "fila2");

                    /* Criacao do exchange */
                    CreateExchange(channel, "exchange1");

                    /* Associando a fial para um exchange, com uma routing key */
                    channel.QueueBind("fila1", "exchange1", "aaa");
                    channel.QueueBind("fila2", "exchange1", "bbb");

                    /* Enviando mensagem somente para a fila1, a o routing key definida */
                    SendMessageToQueueOne(channel, "exchange1", "aaa");

                    /* Enviando mensagem somente para a fila2, a o routing key definida */
                    SendMessageToQueueTwo(channel, "exchange1", "bbb");
                }
            }

            Console.WriteLine("Processado");
            Console.ReadKey();
        }

        private static void CreateExchange(IModel channel, string exchangeName)
        {
            channel.ExchangeDeclare(exchangeName, "topic");
        }

        private static void CreateQueue(IModel channel, string queueName)
        {
            channel.QueueDeclare(queueName, true, false, false, null);
        }

        private static void SendMessageToQueueOne(IModel channel, string exchangeName, string routingKey)
        {
            string message = "Fila 1 " + DateTime.Now;

            int i = 0;

            while (i < 10)
            {
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchangeName, routingKey, null, body);

                i++;
            }
        }

        private static void SendMessageToQueueTwo(IModel channel, string exchangeName, string routingKey)
        {
            string message = "Fila 1 " + DateTime.Now;

            int i = 0;

            while (i < 77)
            {
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchangeName, routingKey, null, body);

                i++;
            }
        }

    }
}
