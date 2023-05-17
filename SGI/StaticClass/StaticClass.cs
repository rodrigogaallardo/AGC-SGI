using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using System.Reflection;
using RestSharp;
using System.Text;

namespace SGI.StaticClassNameSpace
{
    public static class StaticClass
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public static string Path_Temporal = @"C:\Temporal\";
    }

    public static class ChartColor
    {
        public static string verde = "#79c347";
        public static string rojo = "#FF5455";
        public static string amarillo = "#FFE600";
        public static string azul = "#070087";
        public static string[] colordoughnut = { "#79C346", "#FFC8C8", "#FF7878", "#FA0000", "#960000" };
        public static string[] colorpie = { "#79C347", "#FF5455", "#67C2EF" };
        public static string[] Colores = {"#f44336","#e91e63","#9c27b0","#673ab7","#3f51b5","#2196f3",
                            "#03a9f4","#00bcd4","#009688","#4caf50","#8bc34a","#cddc39","#ffeb3b","#ffc107",
                    "#ff9800","#ff5722","#795548","#9e9e9e","#607d8b","#f44336","#e91e63","#9c27b0","#673ab7","#3f51b5","#2196f3",
                            "#03a9f4","#00bcd4","#009688","#4caf50","#8bc34a","#cddc39","#ffeb3b","#ffc107",
                    "#ff9800","#ff5722","#795548","#9e9e9e","#607d8b","#f44336","#e91e63","#9c27b0","#673ab7","#3f51b5","#2196f3",
                            "#03a9f4","#00bcd4","#009688","#4caf50","#8bc34a","#cddc39","#ffeb3b","#ffc107",
                    "#ff9800","#ff5722","#795548","#9e9e9e","#607d8b","#f44336","#e91e63","#9c27b0","#673ab7","#3f51b5","#2196f3",
                            "#03a9f4","#00bcd4","#009688","#4caf50","#8bc34a","#cddc39","#ffeb3b","#ffc107",
                    "#ff9800","#ff5722","#795548","#9e9e9e","#607d8b","#f44336","#e91e63","#9c27b0","#673ab7","#3f51b5","#2196f3",
                            "#03a9f4","#00bcd4","#009688","#4caf50","#8bc34a","#cddc39","#ffeb3b","#ffc107",
                    "#ff9800","#ff5722","#795548","#9e9e9e","#607d8b","#f44336","#e91e63","#9c27b0","#673ab7","#3f51b5","#2196f3",
                            "#03a9f4","#00bcd4","#009688","#4caf50","#8bc34a","#cddc39","#ffeb3b","#ffc107",
                    "#ff9800","#ff5722","#795548","#9e9e9e","#607d8b"};
    }

    public enum MotivosNotificaciones
    {
        InicioHabilitación = 1,
        Observado = 2,
        Rechazado = 3,
        Aprobado = 4,
        AvisoCaducidadPróximoCaducar = 5,
        avisoCaducidad = 6,
        BajaDeSolicitud = 7,
        QRDisponible = 8,
        AnexoTecnicoAnulado = 9,
        SolicitudConfirmada = 10,
        LevantamientoDeRechazo = 11,
        AsignadoAlCalificador = 12

    }

    public enum TipoEmail
    {
        WebSGIAvisoCarátula = 5,
        WebSGICorrecciónSolicitud = 6,
        WebSGIRechazo = 7,
        WebSGIAprobacionDG = 8,
        Generico = 11,
        WebSGIRecuperoContraseña = 14,
        WebSGIBaja = 18,
    }



    public class Funciones
    {
        public static void ExportToExcel(DataTable dt, string destination)
        {
            using (var workbook = SpreadsheetDocument.Create(destination, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();

                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                uint sheetId = 1;
                if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                {
                    sheetId =
                        sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }

                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = dt.TableName };
                sheets.Append(sheet);

                DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                List<String> columns = new List<string>();
                foreach (System.Data.DataColumn column in dt.Columns)
                {
                    columns.Add(column.ColumnName);

                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (System.Data.DataRow dsrow in dt.Rows)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    foreach (String col in columns)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(CleanInvalidXmlChars(dsrow[col]?.ToString()));
                        newRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(newRow);
                }
            }
        }

        public static string CleanInvalidXmlChars(string text = "")
        {
            string re = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
            return System.Text.RegularExpressions.Regex.Replace(text, re, "");
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static void ExportDataSetToExcel(DataSet ds, string destination)
        {
            using (var workbook = SpreadsheetDocument.Create(destination, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();

                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                foreach (System.Data.DataTable table in ds.Tables)
                {

                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    uint sheetId = 1;
                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);

                    DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    List<String> columns = new List<string>();
                    foreach (System.Data.DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(headerRow);
                    foreach (System.Data.DataRow dsrow in table.Rows)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                        foreach (String col in columns)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(CleanInvalidXmlChars(dsrow[col].ToString())); //
                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                    }

                }
            }
        }

        public static void EliminarArchivosDirectorioTemporal()
        {
            //Elimina los archivos con mas de 3 días para mantener el directorio limpio.
            string[] lstArchs = System.IO.Directory.GetFiles(StaticClass.Path_Temporal);
            foreach (string arch in lstArchs)
            {
                DateTime fechaCreacion = System.IO.File.GetCreationTime(arch);
                if (fechaCreacion < DateTime.Now.AddDays(-3))
                    System.IO.File.Delete(arch);
            }
        }

        public static string GetErrorMessage(Exception ex)
        {
            string ret = ex.Message;
            Exception lex = ex;
            while (lex.InnerException != null)
            {
                lex = lex.InnerException;
            }

            if (lex != null)
                ret = lex.Message;

            return ret;
        }

        public static string GetDataFromClient(IRestClient client)
        {
            string ret = Environment.NewLine;
            if (client.Authenticator != null)
            {
                ret += " - Authenticator: " + client.Authenticator.ToString() + Environment.NewLine;
            }
            if (client.BaseUrl != null)
            {
                ret += " - Base URL: " + client.BaseUrl.ToString() + Environment.NewLine;
            }
            if (client.CachePolicy != null)
            {
                ret += " - Cache Policy Level: " + client.CachePolicy.Level + Environment.NewLine;
            }
            if (client.ClientCertificates != null && client.ClientCertificates.Count > 0)
            {
                ret += " - Listado de Certificates: " + Environment.NewLine;
                foreach (System.Security.Cryptography.X509Certificates.X509Certificate certif in client.ClientCertificates)
                {
                    ret += "    + Subject: " + certif.Subject +
                        " || Issuer: " + certif.Issuer +
                        " || Handle: " + certif.Handle +
                        " || GetCertHashString: " + certif.GetCertHashString() +
                        " || GetEffectiveDateString: " + certif.GetEffectiveDateString() +
                        " || GetExpirationDateString: " + certif.GetExpirationDateString() +
                        " || GetKeyAlgorithmParametersString: " + certif.GetKeyAlgorithmParametersString() +
                        " || GetPublicKeyString: " + certif.GetPublicKeyString() +
                        " || GetRawCertDataString: " + certif.GetRawCertDataString() +
                        " || GetSerialNumberString: " + certif.GetSerialNumberString() + Environment.NewLine;
                }
            }
            if (client.CookieContainer != null)
            {
                ret += " - CookieContainer: " + client.CookieContainer + Environment.NewLine;
            }
            ;
            if (client.DefaultParameters != null && client.DefaultParameters.Count() > 0)
            {
                ret += " - Listado de Default Parameters: " + Environment.NewLine;
                foreach (Parameter param in client.DefaultParameters)
                {
                    ret += "    + Name: " + param.Name +
                        " || Value: " + param.Value +
                        " || ContentType: " + param.ContentType +
                        " || Type: " + param.Type + Environment.NewLine;
                }
            }
            if (client.Encoding != null)
            {
                ret += " - Encoding: " + client.Encoding + Environment.NewLine;
            }
            return ret;
        }

        public static string GetDataFromRequest(IRestRequest request)
        {
            string ret = Environment.NewLine;
            if (request != null)
            {
                if (request.Files != null && request.Files.Count() > 0)
                {
                    ret += " - Listado de Files: " + Environment.NewLine;
                    foreach (FileParameter file in request.Files)
                    {
                        ret += "    + Name: " + file.FileName +
                            " || ContentLength: " + file.ContentLength +
                            " || ContentType: " + file.ContentType +
                            " || Name: " + file.Name +
                            " || Writer: " + file.Writer + Environment.NewLine;
                    }
                }
                ret += " - Method: " + request.Method + Environment.NewLine;
                if (request.Parameters != null && request.Parameters.Count() > 0)
                {
                    ret += " - Listado de Parameters: " + Environment.NewLine;
                    foreach (Parameter param in request.Parameters)
                    {
                        ret += "    + Name: " + param.Name +
                            " || Value: " + param.Value +
                            " || ContentType: " + param.ContentType +
                            " || Type: " + param.Type + Environment.NewLine;
                    }
                }
                ret += " - Resource: " + request.Resource + Environment.NewLine;
            }
            return ret;
        }

        public static string GetDataFromResponse(IRestResponse response)
        {
            string ret = Environment.NewLine;
            if (response != null)
            {
                ret += " - Response Content: " + Environment.NewLine;
                if (response.Content != null)
                {
                    ret += "    + Content Length: " + response.Content.Length + Environment.NewLine;
                }
                if (response.ContentEncoding != null)
                {
                    ret += "    + Encoding: " + response.ContentEncoding + Environment.NewLine;
                }
                ret += "    + Length: " + response.ContentLength + Environment.NewLine;
                if (response.ContentType != null)
                {
                    ret += "    + Type: " + response.ContentType + Environment.NewLine;
                }
                if (response.Cookies != null && response.Cookies.Count() > 0)
                {
                    ret += " - Response Cookies: " + Environment.NewLine;
                    foreach (RestResponseCookie cookie in response.Cookies)
                    {
                        ret += "    + Comment: " + cookie.Comment +
                           " || CommentUri: " + cookie.CommentUri +
                           " || Discard: " + cookie.Discard +
                           " || Expired: " + cookie.Expired +
                           " || Expires: " + cookie.Expires +
                           " || HttpOnly: " + cookie.HttpOnly +
                           " || Name: " + cookie.Name +
                           " || Path: " + cookie.Path +
                           " || Port: " + cookie.Port +
                           " || Secure: " + cookie.Secure +
                           " || TimeStamp: " + cookie.TimeStamp +
                           " || Value: " + cookie.Value +
                           " || Version: " + cookie.Version + Environment.NewLine;
                    }
                }
                if (response.ErrorException != null)
                {
                    ret += " - ErrorException: " + GetErrorMessage(response.ErrorException) + Environment.NewLine;
                }
                if (response.ErrorMessage != null)
                {
                    ret += " - ErrorMessage: " + response.ErrorMessage + Environment.NewLine;
                }
                if (response.Headers != null && response.Headers.Count() > 0)
                {
                    ret += " - Listado de Headers: " + Environment.NewLine;
                    foreach (Parameter param in response.Headers)
                    {
                        ret += "    + Name: " + param.Name +
                            " || Value: " + param.Value +
                            " || ContentType: " + param.ContentType +
                            " || Type: " + param.Type + Environment.NewLine;
                    }
                }
                if (response.RawBytes != null)
                {
                    ret += " - (UTF-8)RawBytes Length: " + Encoding.UTF8.GetString(response.RawBytes).Length + Environment.NewLine;
                }
                if (response.Request != null)
                {
                    ret += " - Request: " + Environment.NewLine;
                    if (response.Request.Files != null && response.Request.Files.Count() > 0)
                    {
                        ret += "    + Listado de Files: " + Environment.NewLine;
                        foreach (FileParameter file in response.Request.Files)
                        {
                            ret += "       * Name: " + file.FileName +
                                " || ContentLength: " + file.ContentLength +
                                " || ContentType: " + file.ContentType +
                                " || Name: " + file.Name +
                                " || Writer: " + file.Writer + Environment.NewLine;
                        }
                    }
                    ret += "    + Method: " + response.Request.Method + Environment.NewLine;
                    if (response.Request.Parameters != null && response.Request.Parameters.Count() > 0)
                    {
                        ret += "    + Listado de Parameters: " + Environment.NewLine;
                        foreach (Parameter param in response.Request.Parameters)
                        {
                            ret += "       * Name: " + param.Name +
                                " || Value: " + param.Value +
                                " || ContentType: " + param.ContentType +
                                " || Type: " + param.Type + Environment.NewLine;
                        }
                    }
                    ret += "    + Resource: " + response.Request.Resource + Environment.NewLine;
                }
                if (response.Server != null)
                {
                    ret += " - Server: " + response.Server + Environment.NewLine;
                }
                ret += " - Status Code: " + response.StatusCode + Environment.NewLine;
                if (response.StatusDescription != null)
                {
                    ret += " - Status Description: " + response.StatusDescription + Environment.NewLine;
                }
                ret += " - Status: " + response.ResponseStatus + Environment.NewLine;
                if (response.ResponseUri != null)
                {
                    ret += " - URI: " + response.ResponseUri + Environment.NewLine;
                }
            }
            return ret;
        }

    }









    }