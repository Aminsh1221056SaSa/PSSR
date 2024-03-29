﻿using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PSSR.ServiceLayer.Logger
{
    public class LogParts
    {
        private const string EfCoreEventIdStartWith = "Microsoft.EntityFrameworkCore";

        [JsonConverter(typeof(StringEnumConverter))]
        public LogLevel LogLevel { get; private set; }

        public EventId EventId { get; private set; }

        public string EventString { get; private set; }

        public bool IsDb => EventId.Name?.StartsWith(EfCoreEventIdStartWith) ?? false;

        public LogParts(LogLevel logLevel, EventId eventId, string eventString)
        {
            LogLevel = logLevel;
            EventId = eventId;
            EventString = eventString;
        }

        public override string ToString()
        {
            return $"{LogLevel}: {EventString}";
        }

    }
}
