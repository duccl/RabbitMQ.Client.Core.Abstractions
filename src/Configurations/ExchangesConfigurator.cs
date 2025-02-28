﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Core.Interfaces;
using RabbitMQ.Client.Core.Options;

namespace RabbitMQ.Client.Core.Configurations
{
    ///<summary>
    /// This class is responsible for configurate all exchanges defined on appsetting.json
    /// See <see cref="ExchangesOption">
    ///</summary>
    public class ExchangesConfigurator: IExchangesConfigurator
    {
        private ConnectionFactory ConnectionFactory;
        private ILogger<ExchangesConfigurator> _logger;
        private readonly ExchangesOption _exchangesOption;
        public ExchangesConfigurator(
            IOptions<RabbitMQConnectionOptions> connectionOptions,
            IOptions<ExchangesOption> exchangesOption, 
            ILogger<ExchangesConfigurator> logger)
        {
            ConnectionFactory = new ConnectionFactory
            {
                HostName = connectionOptions.Value.HostName,
                Port = connectionOptions.Value.Port,
                UserName = connectionOptions.Value.UserName,
                Password = connectionOptions.Value.Password,
                VirtualHost = connectionOptions.Value.VirtualHost
            };
            _exchangesOption = exchangesOption.Value;
            _logger = logger;
            Setup();
        }

        ///<summary>
        /// Setup the Exchanges defined at config file
        /// No need to call it, since <see cref="RabbitMQ.Client.Core.Extensions.ServiceCollectionExtensions.AddMQExchanges(Microsoft.Extensions.DependencyInjection.IServiceCollection)"> extension does
        ///</summary>
        public void Setup()
        {
            using var connection = ConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            _logger.LogDebug("Starting to create exchanges");
            foreach (var exchange in _exchangesOption.Exchanges)
            {
                _logger.LogDebug($"{exchange.ExchangeName}@{exchange.Type} being created");
                channel.ExchangeDeclare(
                    exchange: exchange.ExchangeName,
                    type: exchange.Type,
                    durable: exchange.Durable,
                    autoDelete: exchange.AutoDelete,
                    arguments: exchange.Arguments
                );
            }
            _logger.LogDebug("Done to create exchanges");

        }
    }
}
