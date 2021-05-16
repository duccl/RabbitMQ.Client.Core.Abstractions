﻿using RabbitMQ.Client.Core.Interfaces;
using System.Collections.Generic;

namespace RabbitMQ.Client.Core.Options
{
    public class ExchangeOption: IExchangeOption
    {
        public string ExchangeName { get; set; }
        public string Type { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public IDictionary<string, object> Arguments { get; set; }
    }
}
