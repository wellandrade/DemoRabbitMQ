using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ConsumerQueue
{
    class Program
    {
        static void Main()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    /* A criacao da fila tanto no Queue quanto no Consumer deve ter a mesma configuracao */
                    channel.QueueDeclare("hello", false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);

                    int i = 0;

                    while (i < 732032)
                    {
                        consumer.Received += (mode, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);

                            Console.WriteLine("Received {0}", message);
                        };

                        /* Autoack definido como true, dessa forma só vai tirar a mensagem da fila de ela for processada com sucesso
                            se acontecer algum erro, essa mensagem vai retornar para a fila de processamento
                        */
                        channel.BasicConsume("hello", true, consumer);
                    }
                }
            }

            Console.WriteLine("Finalizado");

            Console.ReadKey();
        }
    }
}
