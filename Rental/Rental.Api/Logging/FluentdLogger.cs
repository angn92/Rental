using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Fluentd;

namespace Rental.Api.Logging
{
    public static class FluentdLogger
    {
        private const string Host = "localhost";
        private const int Port = 24224;
        private const string Tag = "Rental";

        public static LoggerConfiguration Fluentd(this LoggerSinkConfiguration loggerSinkConfiguration,
                    FluentdSinkOptions options = null, LogEventLevel logEventLevel = LogEventLevel.Information)
        {
            var sink = new FluentdSink(options ?? new FluentdSinkOptions(Host, Port, Tag));

            return loggerSinkConfiguration.Sink(sink, logEventLevel);
        }

        public static LoggerConfiguration Fluentd(this LoggerSinkConfiguration loggerSinkConfiguration,
            string host, int port, string tag, LogEventLevel logEventLevel = LogEventLevel.Information)
        {
            var sink = new FluentdSink(new FluentdSinkOptions(host, port, tag));

            return loggerSinkConfiguration.Sink(sink, logEventLevel);
        }
    } 
}
