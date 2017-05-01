using NLog;
using STSCommon.Models;

namespace STSCommon.Extensions
{
    public static class LoggerExtenstions
    {
        public static void LogError(this Logger logger, ErrorReportItem errorReportItem, string message = "")
        {
            var info = new LogEventInfo(errorReportItem.Severity, "", message);
            info.Properties["Source"] = ExtractionSettings.Input;
            info.Properties["Severity"] = errorReportItem.Severity.ToString();
            info.Properties["Location"] = errorReportItem.Location;
            logger.Log(info);
        }

        public static void LogInfo(this Logger logger, ProcessingReportItem processingReportItem, string message = "")
        {
            var info = new LogEventInfo(LogLevel.Info, "", processingReportItem.UniqueId);
            info.Properties["Source"] = ExtractionSettings.Input;
            info.Properties["Type"] = processingReportItem.Type;
            info.Properties["Destination"] = processingReportItem.Destination;
            info.Properties["UniqueId"] = processingReportItem.UniqueId;
            info.Properties["Resources"] = processingReportItem.Resources;
            logger.Log(info);
        }
    }
}