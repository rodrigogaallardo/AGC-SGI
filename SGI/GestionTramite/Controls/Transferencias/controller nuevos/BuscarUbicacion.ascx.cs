using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls.Transferencias
{
    public partial class BuscarUbicacion : System.Web.UI.UserControl
    {
        /*private int IdTratmiteTarea
        {
            get
            {
                int ret = 0;
                ret = Convert.ToInt32(Request.QueryString["id"]);
                return ret;
            }
        }

        private int _id_solicitud_mod;
        public int id_solicitud_mod
        {
            get
            {
                if (_id_solicitud_mod == 0)
                {
                    int.TryParse(hid_id_solicitud.Value, out _id_solicitud_mod);
                }
                return _id_solicitud_mod;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
                _id_solicitud_mod = value;
            }
        }

        private int _validar_estado;
        public int validar_estado
        {
            get
            {
                if (_validar_estado == 0)
                {
                    int.TryParse(hid_validar_estado.Value, out _validar_estado);
                }
                return _validar_estado;
            }
            set
            {
                hid_validar_estado.Value = value.ToString();
                _validar_estado = value;
            }
        }

        public class ucPuerta
        {
            public int codigo_calle { get; set; }
            public int NroPuerta { get; set; }
        }
        public class ucAgregarUbicacionEventsArgs : EventArgs
        {
            public int id_ubicacion { get; set; }
            public Nullable<int> id_subtipoubicacion { get; set; }
            public string local_subtipoubicacion { get; set; }
            public string vDeptoLocalOtros { get; set; }
            public List<int> ids_propiedades_horizontales = new List<int>();
            public List<ucPuerta> Puertas = new List<ucPuerta>();
            public UpdatePanel upd { get; set; }
            public bool Cancel { get; set; }    // se utilizar para saber si se cancelo o no luego del llamado.
        }

        public int id_transfUbicacion { get; set; }

        private static List<Model.Ubicacion> result = new List<Model.Ubicacion>();
        private static string _OnCerrarClick = "";

        public delegate void EventHandlerCerrar(object sender, EventArgs e);
        public event EventHandlerCerrar CerrarClick;

        public delegate void EventHandlerAgregarUbicacion(object sender, ref ucAgregarUbicacionEventsArgs e);
        public event EventHandlerAgregarUbicacion AgregarUbicacionClick;


        protected void btnCerrar_Click(object sender, EventArgs e)
        {

            Inicilizar_Control();

            if (!string.IsNullOrEmpty(_OnCerrarClick))
            {
                ((BasePage)this.Page).EjecutarScript(updBuscarUbicacion, OnCerrarClientClick);
            }


            if (CerrarClick != null)
                CerrarClick(sender, e);
        }
        bool _editar = false;

        public bool Edicion
        {
            get { return _editar; }
        }
        public string OnCerrarClientClick
        {
            get
            {
                return _OnCerrarClick;
            }
            set
            {
                _OnCerrarClick = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updBuscarUbicacion, updBuscarUbicacion.GetType(), "init_JS_updBuscarUbicacion", "init_JS_updBuscarUbicacion();", true);

            }
            if (!IsPostBack)
            {
                CargarCalles();
                CargarComboTipoUbicacion();
            }
        }

        private void Inicilizar_Control()
        {
            btnModificarUbicacion.Visible = false;
            btnIngresarUbicacion.Visible = true;
            pnlGridResultados.Visible = false;
            pnlContentBuscar.Visible = true;
            gridubicacion.PageIndex = 0;
            txtSeccion.Text = "";
            txtManzana.Text = "";
            txtParcela.Text = "";
            ddlSubTipoUbicacion.ClearSelection();
            ddlTipoDeUbicacion.ClearSelection();
            optTipoPartidaMatriz.Checked = true;
            txtNroPartida.Text = "";
            ddlCalles.ClearSelection();
            txtNroPuerta.Text = "";
            txtDescUbicacion.Text = "";
            pnlResultados.Update();
            hid_tabselected.Value = "1";
        }

        private void CargarCalles()
        {
            DGHP_Entities db = new DGHP_Entities();

            var lstCalles = (from calle in db.Calles
                             select new
                             {
                                 calle.Codigo_calle,
                                 calle.NombreOficial_calle
                             }).Distinct().OrderBy(x => x.NombreOficial_calle).ToList();

            ddlCalles.DataSource = lstCalles;
            ddlCalles.DataTextField = "NombreOficial_calle";
            ddlCalles.DataValueField = "Codigo_calle";
            ddlCalles.DataBind();

            ddlCalles.Items.Insert(0, "");

            db.Dispose();
        }*/
    }
}