using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Afonsoft.Petz.Library
{
    public static class ExceptionExtensions
    {
        private static readonly object Locker = new object();

        /// <summary>
        /// Tratar a Exception para recuperar um string com todo o erro
        /// </summary>
        public static string Treatment(this Exception exception, bool save = false)
        {
            string msgRetorno = "";

            string exceptionMessages = string.Empty;
            string stackTraces = string.Empty;
            Exception tmpException = exception;

            if (tmpException != null)
            {
                while (tmpException != null)
                {
                    exceptionMessages += tmpException.Message + Environment.NewLine;
                    string traces = "";
                    var trace = new StackTrace(tmpException, true);
                    foreach (StackFrame stack in trace.GetFrames())
                    {
                        if (stack.GetFileLineNumber() > 0)
                        {
                            traces += "FileName: " + stack.GetFileName() + Environment.NewLine;
                            traces += "Metodo: " + stack.GetMethod().Name + Environment.NewLine;
                            traces += "Line: " + stack.GetFileLineNumber() + Environment.NewLine;
                            traces += "Column: " + stack.GetFileColumnNumber() + Environment.NewLine;
                        }
                    }
                    if (!String.IsNullOrEmpty(traces))
                        stackTraces += tmpException.StackTrace + Environment.NewLine + Environment.NewLine + traces + Environment.NewLine + Environment.NewLine;
                    else
                        stackTraces += tmpException.StackTrace + Environment.NewLine + Environment.NewLine;

                    tmpException = tmpException.InnerException;
                }
                msgRetorno = "Messages: " + exceptionMessages + Environment.NewLine + " StackTraces: " + stackTraces;
                
                if (save)
                    SaveException(exception);

                Trace.WriteLine(msgRetorno);
                Debug.WriteLine(msgRetorno);
            }
            return msgRetorno;
        }

        /// <summary>
        /// Salvar um log do erro dentro do diretório da aplicação em uma pasta LOGS
        /// </summary>
        public static void SaveException(this Exception exception)
        {
            if (exception != null)
            {
                lock (Locker)
                {
                    string pathExe = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

                    if (pathExe.IndexOf("file:\\", StringComparison.Ordinal) >= 0)
                        pathExe = pathExe.Replace("file:\\", "");
                    if (pathExe.IndexOf("bin", StringComparison.Ordinal) >= 0)
                        pathExe = pathExe.Replace("\\bin", "");
                    string path = Path.Combine(pathExe, "logs", DateTime.Now.ToString("yyyy-MM-dd"));

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fileName = Path.Combine(path, DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
                    using (StreamWriter sw = new StreamWriter(fileName, true, Encoding.UTF8))
                    {
                        sw.WriteLine("======================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "========================");
                        sw.WriteLine(exception.Treatment());
                        sw.WriteLine("==============================================");
                        sw.WriteLine("");
                    }
                }
            }
        }
    }
}