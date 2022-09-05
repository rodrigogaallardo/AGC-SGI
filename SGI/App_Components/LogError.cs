using Elmah;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SGI
{
    public static class LogError
    {
        public static void WriteFile(Exception ex)
        {
            string logPath = "ErrorLog";
            string message = GetErrorMessage(ex);

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            //Elimina los archivos con mas de 3 días para mantener el directorio limpio.
            string[] lstArchs = Directory.GetFiles(logPath);
            foreach (string arch in lstArchs)
            {
                DateTime fechaCreacion = File.GetCreationTime(arch);
                if (fechaCreacion < DateTime.Now.AddDays(-30))
                    File.Delete(arch);
            }


            string filename = string.Format("{0}/{1}.log", logPath, DateTime.Now.ToString("yyyyMMdd-HHmmss.ff"));
            File.WriteAllText(filename, message);


        }
        public static string GetErrorMessage(Exception ex)
        {
            string ret = ex.Message;


            if (ex.InnerException != null)
                ret = ex.InnerException.Message;

            return ret;
        }

        public static string GetErrorMessage(Exception ex, string mensaje)
        {
            string ret = ex.Message;
            

            if (ex.InnerException != null)
                ret = ex.InnerException.Message;

            Elmah.Error err = new Elmah.Error();
            
            return ret;
        }
        public static void Write(Exception ex, string contextualMessage = null)
        {
            try
            {
                // log error to Elmah
                if (contextualMessage != null)
                {
                    // log exception with contextual information that's visible when 
                    // clicking on the error in the Elmah log
                    var annotatedException = new Exception(contextualMessage, ex);
                    ErrorSignal.FromCurrentContext().Raise(annotatedException, HttpContext.Current);
                }
                else
                {
                    ErrorSignal.FromCurrentContext().Raise(ex, HttpContext.Current);
                }

                // send errors to ErrorWS (my own legacy service)
                // using (ErrorWSSoapClient client = new ErrorWSSoapClient())
                // {
                //    client.LogErrors(...);
                // }
            }
            catch (Exception)
            {
                // uh oh! just keep going
            }
        }
    }
}