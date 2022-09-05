using SGI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.ABM
{
    public partial class CargaInstructivoHab : BasePage
    {
        private DGHP_Entities db = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {

                ScriptManager.RegisterStartupScript(updPnlInstructivoCPadron, updPnlInstructivoCPadron.GetType(), "init_Js_updPnlInstructivoCPadron", "init_Js_updPnlInstructivoCPadron();", true);
                ScriptManager.RegisterStartupScript(updPnlInstructivoTransf, updPnlInstructivoTransf.GetType(), "init_Js_updPnlInstructivoTransf", "init_Js_updPnlInstructivoTransf();", true);
                ScriptManager.RegisterStartupScript(updPnlInstructivoHab, updPnlInstructivoHab.GetType(), "init_Js_updPnlInstructivoHab", "init_Js_updPnlInstructivoHab();", true);
                ScriptManager.RegisterStartupScript(updPnlInstructivoAXTecnico, updPnlInstructivoAXTecnico.GetType(), "init_Js_updPnlInstructivoAXTecnico", "init_Js_updPnlInstructivoAXTecnico();", true);
            }

            updPnlInstructivoCPadron.Visible = cargaInstrCPadron;
            updPnlInstructivoTransf.Visible = cargaInstrTransf;
            cargarDescarArchivo();


        }
        #region permisos

        private bool cargaInstrCPadron = true;
        private bool cargaInstrTransf = true;
        private bool cargaInstrHab = true;
        private bool cargaInstrAXTecnico = true;

        #endregion

        private void cargarDescarArchivo()
        {
            this.db = new DGHP_Entities();
            if (cargaInstrCPadron)
            {
                var obj = (
                        from instructivosRel in db.Instructivos
                        where instructivosRel.cod_instructivo == "DGHyP_Consulta_Padron"
                        select new
                        {
                            instructivosRel.id_instructivo,
                            instructivosRel.id_file

                        }).FirstOrDefault();


                if (obj == null || obj.id_file == 0)
                    HyperLinkInst.Visible = false;
                else
                    HyperLinkInst.Visible = true;
                string pageid = obj != null ? obj.id_instructivo.ToString() : "0";

                HyperLinkInst.NavigateUrl = "~/Reportes/DescargarInstructivo.aspx?id=" + pageid;
            }
            if (cargaInstrTransf)
            {
                var obj = (
                        from instructivosRel in db.Instructivos
                        where instructivosRel.cod_instructivo == "DGHyP_Transferencias"
                        select new
                        {
                            instructivosRel.id_instructivo,
                            instructivosRel.id_file

                        }).FirstOrDefault();


                if (obj == null || obj.id_file == 0)
                    HyperLinkProf.Visible = false;
                else
                    HyperLinkProf.Visible = true;
                string pageid = obj != null ? obj.id_instructivo.ToString() : "0";

                HyperLinkProf.NavigateUrl = "~/Reportes/DescargarInstructivo.aspx?id=" + pageid;
            }
            if (cargaInstrHab)
            {
                var obj = (
                        from instructivosRel in db.Instructivos
                        where instructivosRel.cod_instructivo == "DGHyP_Habilitaciones"
                        select new
                        {
                            instructivosRel.id_instructivo,
                            instructivosRel.id_file

                        }).FirstOrDefault();


                if (obj == null || obj.id_file == 0)
                    lnkDescargarInstHab.Visible = false;
                else
                    lnkDescargarInstHab.Visible = true;
                string pageid = obj != null ? obj.id_instructivo.ToString() : "0";

                lnkDescargarInstHab.NavigateUrl = "~/Reportes/DescargarInstructivo.aspx?id=" + pageid;
            }
            if (cargaInstrAXTecnico)
            {
                var obj = (
                        from instructivosRel in db.Instructivos
                        where instructivosRel.cod_instructivo == "DGHyP_Anexo_Tecnico"
                        select new
                        {
                            instructivosRel.id_instructivo,
                            instructivosRel.id_file

                        }).FirstOrDefault();


                if (obj == null || obj.id_file == 0)
                    HyperLink5.Visible = false;
                else
                    HyperLink5.Visible = true;
                string pageid = obj != null ? obj.id_instructivo.ToString() : "0";

                HyperLink5.NavigateUrl = "~/Reportes/DescargarInstructivo.aspx?id=" + pageid;
            }
            db.Dispose();

        }

        private Byte[] cargarPDF(string arch)
        {
            FileStream fs = new FileStream(arch, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Byte[] pdfBytes = br.ReadBytes((Int32)fs.Length);
            br.Close();
            fs.Close();

            if (pdfBytes.Length == 0)
                throw new Exception("El documento está vacio.");


            if (pdfBytes.Length > 8000000)
                throw new Exception("El tamaño máximo permitido es de 8 MB");

            return pdfBytes;
        }

        #region "Propiedades"

        private string _nombreArchivoFisico2;
        public string NombreArchivoFisico2
        {
            get
            {
                if (string.IsNullOrEmpty(_nombreArchivoFisico2))
                    _nombreArchivoFisico2 = Constants.PathTemporal + this.RandomArchivo2 + hid_filename2.Value;
                return _nombreArchivoFisico2;
            }
            set
            {
                _nombreArchivoFisico2 = value;
            }
        }

        private string _nombreArchivoOriginal2;
        public string NombreArchivoOriginal2
        {
            get
            {
                if (string.IsNullOrEmpty(_nombreArchivoOriginal2))
                {
                    _nombreArchivoOriginal2 = hid_filename2.Value;
                }
                return _nombreArchivoOriginal2;
            }
            set
            {
                _nombreArchivoOriginal2 = value;
            }

        }

        private string _randomArchivo2;
        public string RandomArchivo2
        {
            get
            {
                if (string.IsNullOrEmpty(_randomArchivo2))
                    _randomArchivo2 = hid_filename_random2.Value;
                return _randomArchivo2;
            }
            set
            {
                _randomArchivo2 = value;
            }
        }
        private string _nombreArchivoFisico;
        public string NombreArchivoFisico
        {
            get
            {
                if (string.IsNullOrEmpty(_nombreArchivoFisico))
                    _nombreArchivoFisico = Constants.PathTemporal + this.RandomArchivo + hid_filename.Value;
                return _nombreArchivoFisico;
            }
            set
            {
                _nombreArchivoFisico = value;
            }
        }

        private string _nombreArchivoFisico3;
        public string NombreArchivoFisico3
        {
            get
            {
                if (string.IsNullOrEmpty(_nombreArchivoFisico3))
                    _nombreArchivoFisico3 = Constants.PathTemporal + this.RandomArchivo3 + hid_filename3.Value;
                return _nombreArchivoFisico3;
            }
            set
            {
                _nombreArchivoFisico3 = value;
            }
        }

        private string _nombreArchivoOriginal3;
        public string NombreArchivoOriginal3
        {
            get
            {
                if (string.IsNullOrEmpty(_nombreArchivoOriginal3))
                {
                    _nombreArchivoOriginal3 = hid_filename3.Value;
                }
                return _nombreArchivoOriginal3;
            }
            set
            {
                _nombreArchivoOriginal3 = value;
            }

        }

        private string _randomArchivo3;
        public string RandomArchivo3
        {
            get
            {
                if (string.IsNullOrEmpty(_randomArchivo3))
                    _randomArchivo3 = hid_filename_random3.Value;
                return _randomArchivo3;
            }
            set
            {
                _randomArchivo3 = value;
            }
        }
        private string _nombreArchivoOriginal;
        public string NombreArchivoOriginal
        {
            get
            {
                if (string.IsNullOrEmpty(_nombreArchivoOriginal))
                {
                    _nombreArchivoOriginal = hid_filename.Value;
                }
                return _nombreArchivoOriginal;
            }
            set
            {
                _nombreArchivoOriginal = value;
            }

        }

        private string _randomArchivo;
        public string RandomArchivo
        {
            get
            {
                if (string.IsNullOrEmpty(_randomArchivo))
                    _randomArchivo = hid_filename_random.Value;
                return _randomArchivo;
            }
            set
            {
                _randomArchivo = value;
            }
        }

        private string _nombreArchivoFisico5;
        public string NombreArchivoFisico5
        {
            get
            {
                if (string.IsNullOrEmpty(_nombreArchivoFisico5))
                    _nombreArchivoFisico5 = Constants.PathTemporal + this.RandomArchivo5 + hid_filename5.Value;
                return _nombreArchivoFisico5;
            }
            set
            {
                _nombreArchivoFisico5 = value;
            }
        }

        private string _nombreArchivoOriginal5;
        public string NombreArchivoOriginal5
        {
            get
            {
                if (string.IsNullOrEmpty(_nombreArchivoOriginal5))
                {
                    _nombreArchivoOriginal5 = hid_filename5.Value;
                }
                return _nombreArchivoOriginal5;
            }
            set
            {
                _nombreArchivoOriginal5 = value;
            }

        }

        private string _randomArchivo5;
        public string RandomArchivo5
        {
            get
            {
                if (string.IsNullOrEmpty(_randomArchivo5))
                    _randomArchivo5 = hid_filename_random5.Value;
                return _randomArchivo5;
            }
            set
            {
                _randomArchivo5 = value;
            }
        }

        #endregion

        private void EliminarDocumento(string arch)
        {
            try
            {
                System.IO.File.Delete(arch);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

        }

        protected void btnComenzarCargaArchivo_Click(object sender, EventArgs e)
        {

            try
            {
                Guid userid = Functions.GetUserId();

                this.db = new DGHP_Entities();

                Byte[] pdfBytes = cargarPDF(this.NombreArchivoFisico);

                string nombreProy = "DGHyP_Consulta_Padron";
                this.db.Instructivo_Agregar_Actualizar(nombreProy, pdfBytes, userid);

                this.db.Dispose();
                cargarDescarArchivo();
                updPnlInstructivoCPadron.Update();

                string mensaje = "El documento \"" + this.NombreArchivoOriginal + "\" se ha adjuntado correctamente.";
                mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);

            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();

                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updPnlCargarArchivo, "showfrmError();");

            }

            EliminarDocumento(this.NombreArchivoFisico);//borra el archivo del disco

        }


        protected void btnComenzarCargaArchivo2_Click(object sender, EventArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();

                this.db = new DGHP_Entities();

                Byte[] pdfBytes = cargarPDF(this.NombreArchivoFisico2);


                string nombreProy = "DGHyP_Transferencias";
                this.db.Instructivo_Agregar_Actualizar(nombreProy, pdfBytes, userid);

                this.db.Dispose();
                cargarDescarArchivo();
                updPnlInstructivoTransf.Update();

                string mensaje = "El documento \"" + this.NombreArchivoOriginal2 + "\" se ha adjuntado correctamente.";
                mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);

            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();

                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updPnlCargarArchivo2, "showfrmError();");
            }

            EliminarDocumento(this.NombreArchivoFisico2);//borra el archivo del disco

        }

        protected void btnComenzarCargaArchivo3_Click(object sender, EventArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();

                this.db = new DGHP_Entities();

                Byte[] pdfBytes = cargarPDF(this.NombreArchivoFisico3);

                string nombreProy = "DGHyP_Habilitaciones";
                this.db.Instructivo_Agregar_Actualizar(nombreProy, pdfBytes, userid);

                this.db.Dispose();
                cargarDescarArchivo();
                updPnlInstructivoHab.Update();

                string mensaje = "El documento \"" + this.NombreArchivoOriginal3 + "\" se ha adjuntado correctamente.";
                mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);

            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();

                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updPnlCargarArchivo3, "showfrmError();");
            }

            EliminarDocumento(this.NombreArchivoFisico3);//borra el archivo del disco

        }

        protected void btnComenzarCargaArchivo5_Click(object sender, EventArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();

                this.db = new DGHP_Entities();

                Byte[] pdfBytes = cargarPDF(this.NombreArchivoFisico5);

                string nombreProy = "DGHyP_Anexo_Tecnico";
                this.db.Instructivo_Agregar_Actualizar(nombreProy, pdfBytes, userid);

                this.db.Dispose();
                cargarDescarArchivo();
                updPnlInstructivoAXTecnico.Update();

                string mensaje = "El documento \"" + this.NombreArchivoOriginal5 + "\" se ha adjuntado correctamente.";
                mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);

            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();

                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updPnlCargarArchivo5, "showfrmError();");
            }

            EliminarDocumento(this.NombreArchivoFisico5);//borra el archivo del disco

        }

    }
}

