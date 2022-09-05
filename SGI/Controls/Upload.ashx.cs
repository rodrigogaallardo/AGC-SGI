using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace SGI.Controls
{
    /// <summary>
    /// Descripción breve de Upload
    /// </summary>
    public class Upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";//"application/json";
            var r = new System.Collections.Generic.List<ViewDataUploadFilesResult>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            foreach (string file in context.Request.Files)
            {


                HttpPostedFile hpf = context.Request.Files[file] as HttpPostedFile;
                string FileName = string.Empty;
                if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
                {
                    string[] files = hpf.FileName.Split(new char[] { '\\' });
                    FileName = files[files.Length - 1];
                }
                else
                {
                    FileName = hpf.FileName;
                }

                try
                {
                    FileName = context.Request.QueryString["nrorandom"].ToString() + FileName;
                }
                catch (Exception)
                {
                    FileName = "";
                }

                if (hpf.ContentLength == 0)
                    continue;

                //string directorio = "c:\\Temporal"; //HttpContext.Current.Server.MapPath(context.Request["folder"]);
                string savedFileName = "c:\\Temporal\\" + FileName;

                //if (!Directory.Exists(directorio))
                //    Directory.CreateDirectory(directorio);

                hpf.SaveAs(savedFileName);


                r.Add(new ViewDataUploadFilesResult()
                {
                    //Thumbnail_url = savedFileName,
                    Name = FileName,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType
                });
                var uploadedFiles = new
                {
                    files = r.ToArray()
                };
                var jsonObj = js.Serialize(uploadedFiles);
                context.Response.Write(jsonObj.ToString());

                //redimiensiona la imagen a 150 x 150 pixels
                //ResizeImage(savedFileName, savedFileName, 150, 150);



            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static string ResizeImage(string strImgPath, string strImgOutputPath, int iWidth, int iHeight)
        {
            try
            {
                strImgPath = strImgPath.ToLower();
                strImgOutputPath = strImgOutputPath.ToLower();

                bool mismaImagen = strImgPath.Equals(strImgOutputPath);
                if (mismaImagen)
                {
                    strImgOutputPath = strImgPath + "___.jpg";
                }

                string[] extensiones = {
                                   ".jpg",
                                   ".png",
                                   ".bmp",
                                   ".gif"
                               };

                if (!extensiones.Contains(System.IO.Path.GetExtension(strImgPath)))
                    throw new Exception("Extensión no soportada");

                //Lee el fichero en un stream
                System.IO.Stream mystream = null;

                if (strImgPath.StartsWith("http"))
                {
                    System.Net.HttpWebRequest wreq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strImgPath);
                    System.Net.HttpWebResponse wresp = (System.Net.HttpWebResponse)wreq.GetResponse();
                    mystream = wresp.GetResponseStream();
                }
                else
                    mystream = File.OpenRead(strImgPath);

                // Cargo la imágen
                Bitmap imgToResize = new Bitmap(mystream);

                Size size = new Size(iWidth, iHeight);

                int sourceWidth = imgToResize.Width;
                int sourceHeight = imgToResize.Height;

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = ((float)size.Width / (float)sourceWidth);
                nPercentH = ((float)size.Height / (float)sourceHeight);

                if (nPercentH < nPercentW)
                    nPercent = nPercentH;
                else
                    nPercent = nPercentW;

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap b = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage((System.Drawing.Image)b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                g.Dispose();

                // We will store the correct image codec in this object
                ImageCodecInfo ici = GetEncoderInfo("image/jpeg"); ;
                // This will specify the image quality to the encoder
                EncoderParameter epQuality = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 99L);
                // Store the quality parameter in the list of encoder parameters
                EncoderParameters eps = new EncoderParameters(1);
                eps.Param[0] = epQuality;
                b.Save(strImgOutputPath, ici, eps);

                imgToResize.Dispose();
                mystream.Close();
                mystream.Dispose();
                b.Dispose();
                g.Dispose();

                if (mismaImagen)
                {
                    File.Delete(strImgPath);
                    File.Move(strImgOutputPath, strImgPath);
                }

                return strImgPath;
            }
            catch
            {
                throw;
            }
        }
        
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }

    public class ViewDataUploadFilesResult
    {
        public string Thumbnail_url { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
    }
}