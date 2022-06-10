
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public class AzureServiceBusMessageBus : IMessageBus
    {
        //connection string, a good practice would be if we put it in appjson
        private string connectionString = "Endpoint=sb://mangoservicerest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=AnNXoIDjWa0z45075VwJmcCDTuwtiHZpqd3T2ciV/bE=";

        public async Task PublishMessage(BaseMessage message, string topicName)
        {
            // creating a service bus client instance
            await using var client = new ServiceBusClient(connectionString);

            // creating a sender which we would use to send message to the specified topic
            ServiceBusSender sender = client.CreateSender(topicName);

            var jsonMessage = JsonConvert.SerializeObject(message);

            // final message as per the format, takes encoding type and correlation ID
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString() // random Correlation Id
            };

            await sender.SendMessageAsync(finalMessage);

            await client.DisposeAsync();
        }
    }
}
