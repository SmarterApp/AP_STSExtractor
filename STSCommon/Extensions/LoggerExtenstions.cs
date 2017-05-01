using NLog;
using STSCommon.Models;

namespace STSCommon.Extensions
{
    public static class LoggerExtenstions
    {
        public static void Log(this Logger logger, ErrorReportItem errorReportItem)
        {
            var info = new LogEventInfo(LogLevel.Error, string.Empty, errorReportItem.Message);
            info.Properties["Source"] = ExtractionSettings.Input;
            info.Properties["Severity"] = errorReportItem.Severity;
            info.Properties["Location"] = errorReportItem.Location;
            info.Properties["Message"] = errorReportItem.Message;
            logger.Log(info);
        }

        public static void Log(this Logger logger, ProcessingReportItem processingReportItem)
        {
            var info = new LogEventInfo(LogLevel.Info, string.Empty, processingReportItem.UniqueId);
            info.Properties["Source"] = ExtractionSettings.Input;
            info.Properties["Type"] = processingReportItem.Type;
            info.Properties["Destination"] = processingReportItem.Destination;
            info.Properties["UniqueId"] = processingReportItem.UniqueId;
            info.Properties["Resources"] = processingReportItem.Resources;
            logger.Log(info);
        }
    }
}