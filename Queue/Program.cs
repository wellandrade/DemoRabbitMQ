using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreateQueue
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
                    /* Declarar um tipo de cabecalho para o envio da mensagem, dps da pra tratar o tipo de mensagem que foi enviado pelo cabecalho no consumer  */
                    var property = channel.CreateBasicProperties();
                    property.Headers = new Dictionary<string, object>{
                        { "content-type", "application/json" }
                    };

                    /* A criacao da fila tanto no Queue quanto no Consumer deve ter a mesma configuracao */
                    channel.QueueDeclare("hello", false, false, false, null);

                    string message = "Hello " + DateTime.Now;

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", "hello", null, body);
                }
            }

            Console.WriteLine("Fila populada com sucesso");

            Console.ReadKey();
        }
    }
}
