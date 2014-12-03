namespace Sporty.Common
{
    public static class Logger
    {
        //public const string FlatFileWithoutRethrowPolicy = "FlatFileWithoutRethrowing";
        //public const string FlatFileWithRethrowPolicy = "FlatFileWithRethrowing";

        //private static LogWriter writer;
        //private static LoggingSettings logConfig;

        //public static void InitializeLogWriter()
        //{
        //    LogWriterFactory factory = new LogWriterFactory();
        //    writer = factory.Create();
        //}

        ///// <summary>
        ///// Logging messages in info.log file (timestamp & message)
        ///// </summary>
        ///// <param name="info"></param>
        //public static void LogInfo(string info)
        //{
        //    LogEntry logEntry = new LogEntry();
        //    logEntry.Message = info;
        //    logEntry.Categories.Add(Constants.LogTypes.Info.ToString());

        //    if (writer == null)
        //    {
        //        InitializeLogWriter();
        //    }

        //    writer.Write(logEntry);
        //}

        ///// <summary>
        ///// Logging messages in exception.log file (trace info and message)
        ///// After logging same exception is thrown again
        ///// </summary>
        ///// <param name="exceptionToCatch"></param>
        //public static void LogExceptionThrow(Exception exceptionToCatch)
        //{
        //    LogException(exceptionToCatch, FlatFileWithRethrowPolicy);
        //}

        ///// <summary>
        ///// Logging messages in exception.log file (trace info and message)
        ///// </summary>
        ///// <param name="exceptionToCatch"></param>
        //public static void LogException(Exception exceptionToCatch)
        //{
        //    LogException(exceptionToCatch, FlatFileWithoutRethrowPolicy);
        //}


        /// <summary>
        /// Logging messages in exception.log file (trace info and message)
        /// and sending mail with exception message to mail adress - if sending mail is enabled
        /// </summary>
        /// <param name="exceptionToCatch"></param>
        /// <param name="policy"></param>
        //private static void LogException(Exception exceptionToCatch, string policy)
        //{
        //    // string subject = "Exception";
        //    string messageBody = "Message: " + exceptionToCatch.Message;
        //    messageBody += "\n\n\nStack Trace: \n\n" + exceptionToCatch.StackTrace;
        //    if (exceptionToCatch.InnerException != null)
        //    {
        //        messageBody += "\n\nMessage: " + exceptionToCatch.InnerException.Message;
        //        messageBody += "\n\n\nInner Stack Trace: \n\n" + exceptionToCatch.InnerException.StackTrace;
        //    }

        //    /* mail sending of exceptions
        //     * for later implementation
        //    if (AppSettingsHelper.IsEnabledSendingMail())
        //    {
        //        Mailer.SendMail(subject, messageBody);
        //    }
        //    */

        //    bool rethrow = ExceptionPolicy.HandleException(exceptionToCatch, policy);
        //    if (rethrow)
        //    {
        //        throw exceptionToCatch;
        //    }
        //}

        /// <summary>
        /// Logging messages in exception.log file (trace info and message)
        /// Logs common explanation for exception and original exception as inner exception
        /// </summary>
        /// <param name="exceptionToCatch"></param>
        //public static void LogException(String exceptionExplanation, Exception exceptionToCatch)
        //{
        //    Exception detExc = new Exception(exceptionExplanation, exceptionToCatch);
        //    LogException(detExc, FlatFileWithoutRethrowPolicy);
        //}
    }
}