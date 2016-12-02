using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Afonsoft.Petz.Library
{
    public static class ExceptionExtensions
    {
        private static object locker = new object();

        /// <summary>
        /// Tratar a Exception para recuperar um string com todo o erro
        /// </summary>
        public static string Treatment(this Exception exception, bool Save = false)
        {
            string msgRetorno = "";

            string ExceptionMessages = string.Empty;
            string StackTraces = string.Empty;
            Exception TmpException = exception;

            if (TmpException != null)
            {
                while (TmpException != null)
                {
                    ExceptionMessages += TmpException.Message + Environment.NewLine;
                    string Traces = "";
                    System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(TmpException, true);
                    foreach (StackFrame stack in trace.GetFrames())
                    {
                        if (stack.GetFileLineNumber() > 0)
                        {
                            Traces += "FileName: " + stack.GetFileName() + Environment.NewLine;
                            Traces += "Metodo: " + stack.GetMethod().Name + Environment.NewLine;
                            Traces += "Line: " + stack.GetFileLineNumber() + Environment.NewLine;
                            Traces += "Column: " + stack.GetFileColumnNumber() + Environment.NewLine;
                        }
                    }
                    if (!String.IsNullOrEmpty(Traces))
                        StackTraces += TmpException.StackTrace + Environment.NewLine + Environment.NewLine + Traces + Environment.NewLine + Environment.NewLine;
                    else
                        StackTraces += TmpException.StackTrace + Environment.NewLine + Environment.NewLine;

                    TmpException = TmpException.InnerException;
                }
                msgRetorno = "Messages: " + ExceptionMessages + Environment.NewLine + " StackTraces: " + StackTraces;
                
                if (Save)
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
                lock (locker)
                {
                    string pathExe = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

                    if (pathExe.IndexOf("file:\\") >= 0)
                        pathExe = pathExe.Replace("file:\\", "");
                    if (pathExe.IndexOf("bin") >= 0)
                        pathExe = pathExe.Replace("\\bin", "");
                    string path = Path.Combine(pathExe, "logs", DateTime.Now.ToString("yyyy-MM-dd"));

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string FileName = Path.Combine(path, DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
                    using (StreamWriter sw = new StreamWriter(FileName, true, Encoding.UTF8))
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