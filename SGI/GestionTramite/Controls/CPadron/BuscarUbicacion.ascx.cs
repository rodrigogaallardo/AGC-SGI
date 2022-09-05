using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls.CPadron
{
    public partial class BuscarUbicacion : System.Web.UI.UserControl
    {
        private int IdTratmiteTarea
        {
            get
            {
                int ret = 0;
                ret = Convert.ToInt32(Request.QueryString["id"]);

                //int.TryParse(Page.RouteData.Values["id"].ToString(), out ret);
                return ret;
            }
        }

        private int _id_cpadron_mod;
        public int id_cpadron_mod
        {
            get
            {
                if (_id_cpadron_mod == 0)
                {
                    int.TryParse(hid_id_cpadron.Value, out _id_cpadron_mod);
                }
                return _id_cpadron_mod;
            }
            set
            {
                hid_id_cpadron.Value = value.ToString();
                _id_cpadron_mod = value;
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

        public int id_cpadronUbicacion { get; set; }        

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
        }

        protected void btnBuscar1_Click(object sender, EventArgs e)
        {
            BuscarUbicaciones(1);
        }
        protected void btnBuscar2_Click(object sender, EventArgs e)
        {
            BuscarUbicaciones(2);
        }
        protected void btnBuscar3_Click(object sender, EventArgs e)
        {
            BuscarUbicaciones(3);
        }
        protected void btnBuscar4_Click(object sender, EventArgs e)
        {
            BuscarUbicaciones(4);
        }

        private void BuscarUbicaciones(int nrotab)
        {

            //Limpia y oculta los errores
            lstValidacionesUbicacion.Items.Clear();
            pnlValidacionIngresoUbicacion.Visible = false;

            DGHP_Entities db = new DGHP_Entities();

            DateTime FechaActual = DateTime.Now;

            switch (nrotab)
            {
                case 1:
                    int vNroPartida = int.Parse(txtNroPartida.Text.Trim());

                    if (optTipoPartidaHorizontal.Checked)
                    {
                        var query1 = (from ubic in db.Ubicaciones
                                      join phor in db.Ubicaciones_PropiedadHorizontal on ubic.id_ubicacion equals phor.id_ubicacion
                                      where phor.NroPartidaHorizontal == vNroPartida
                                      && (!ubic.VigenciaHasta.HasValue || ubic.VigenciaHasta > FechaActual)
                                      && ubic.baja_logica == false
                                      select new Model.Ubicacion
                                      {
                                          id_ubicacion = ubic.id_ubicacion,
                                          id_subtipoubicacion = ubic.id_subtipoubicacion,
                                          NroPartidaMatriz = ubic.NroPartidaMatriz,
                                          Seccion = ubic.Seccion,
                                          Manzana = ubic.Manzana,
                                          Parcela = ubic.Parcela,
                                          VigenciaDesde = ubic.VigenciaDesde,
                                          VigenciaHasta = ubic.VigenciaHasta
                                      }).Distinct();


                        result = query1.ToList();
                    }
                    else
                    {
                        var query1 = (from ubic in db.Ubicaciones
                                      where ubic.NroPartidaMatriz == vNroPartida
                                      && (!ubic.VigenciaHasta.HasValue || ubic.VigenciaHasta > FechaActual)
                                      && ubic.baja_logica == false
                                      select new Model.Ubicacion
                                      {
                                          id_ubicacion = ubic.id_ubicacion,
                                          id_subtipoubicacion = ubic.id_subtipoubicacion,
                                          NroPartidaMatriz = ubic.NroPartidaMatriz,
                                          Seccion = ubic.Seccion,
                                          Manzana = ubic.Manzana,
                                          Parcela = ubic.Parcela,
                                          VigenciaDesde = ubic.VigenciaDesde,
                                          VigenciaHasta = ubic.VigenciaHasta
                                      });

                        result = query1.ToList();
                    }
                    break;
                case 2:

                    int vNroPuerta = int.Parse(txtNroPuerta.Text.Trim());
                    int minvaluePuerta = 0;
                    int maxvaluePuerta = 0;
                    int parimpar = 0;   // 0 = par - 1 impar

                    int vcodigo_calle = int.Parse(ddlCalles.SelectedValue);
                    var query2 = (from ubic in db.Ubicaciones
                                  join puer in db.Ubicaciones_Puertas on ubic.id_ubicacion equals puer.id_ubicacion
                                  where puer.codigo_calle.Equals(vcodigo_calle) && puer.NroPuerta_ubic.Equals(vNroPuerta)
                                  && (!ubic.VigenciaHasta.HasValue || ubic.VigenciaHasta > FechaActual)
                                  && ubic.baja_logica == false
                                  select new Model.Ubicacion
                                  {
                                      id_ubicacion = ubic.id_ubicacion,
                                      id_subtipoubicacion = ubic.id_subtipoubicacion,
                                      NroPartidaMatriz = ubic.NroPartidaMatriz,
                                      Seccion = ubic.Seccion,
                                      Manzana = ubic.Manzana,
                                      Parcela = ubic.Parcela,
                                      VigenciaDesde = ubic.VigenciaDesde,
                                      VigenciaHasta = ubic.VigenciaHasta
                                  }).Distinct();

                    result = query2.ToList();
                    if (result.Count.Equals(0))
                    {

                        if (vNroPuerta % 100 == 0)
                        {
                            minvaluePuerta = vNroPuerta - 25;
                            maxvaluePuerta = vNroPuerta;
                        }
                        else
                        {
                            minvaluePuerta = Convert.ToInt32(Math.Floor(Convert.ToDecimal(vNroPuerta / 100)) * 100 + 1);
                            maxvaluePuerta = Convert.ToInt32(Math.Floor(Convert.ToDecimal(vNroPuerta / 100 + 1)) * 100);
                            if (minvaluePuerta < vNroPuerta - 34)
                                minvaluePuerta = vNroPuerta - 34;
                            if (maxvaluePuerta > vNroPuerta + 34)
                                maxvaluePuerta = vNroPuerta + 34;

                        }
                        parimpar = vNroPuerta % 2;

                        // se realiza la búsqueda en un rango de +30 y -30 

                        query2 = (from ubic in db.Ubicaciones
                                  join puer in db.Ubicaciones_Puertas on ubic.id_ubicacion equals puer.id_ubicacion
                                  where puer.codigo_calle.Equals(vcodigo_calle) && (puer.NroPuerta_ubic >= minvaluePuerta && puer.NroPuerta_ubic <= maxvaluePuerta)
                                  && puer.NroPuerta_ubic % 2 == parimpar
                                  && (!ubic.VigenciaHasta.HasValue || ubic.VigenciaHasta > FechaActual)
                                  && ubic.baja_logica == false
                                  orderby puer.NroPuerta_ubic
                                  select new Model.Ubicacion
                                  {
                                      id_ubicacion = ubic.id_ubicacion,
                                      id_subtipoubicacion = ubic.id_subtipoubicacion,
                                      NroPartidaMatriz = ubic.NroPartidaMatriz,
                                      Seccion = ubic.Seccion,
                                      Manzana = ubic.Manzana,
                                      Parcela = ubic.Parcela,
                                      VigenciaDesde = ubic.VigenciaDesde,
                                      VigenciaHasta = ubic.VigenciaHasta
                                  });

                        result = query2.ToList();

                        if (result.Count > 0)
                        {
                            List<Model.Ubicacion> ubicacionesSinDuplicados = new List<Model.Ubicacion>();

                            foreach (Model.Ubicacion ubicacion in query2.ToList())
                            {

                                if (ubicacionesSinDuplicados.Find(x => x.id_ubicacion == ubicacion.id_ubicacion) == null)
                                    ubicacionesSinDuplicados.Add(ubicacion);

                            }

                            result = ubicacionesSinDuplicados;
                        }
                    }
                    break;

                case 3:

                    int vSeccion = int.Parse(txtSeccion.Text);
                    string vManzana = txtManzana.Text.Trim();
                    string vParcela = txtParcela.Text.Trim();

                    var query3 = (from ubic in db.Ubicaciones
                                  where ubic.Seccion == vSeccion && ubic.Manzana == vManzana && ubic.Parcela == vParcela
                                  && (!ubic.VigenciaHasta.HasValue || ubic.VigenciaHasta > FechaActual)
                                  && ubic.baja_logica == false
                                  select new Model.Ubicacion
                                  {
                                      id_ubicacion = ubic.id_ubicacion,
                                      id_subtipoubicacion = ubic.id_subtipoubicacion,
                                      NroPartidaMatriz = ubic.NroPartidaMatriz,
                                      Seccion = ubic.Seccion,
                                      Manzana = ubic.Manzana,
                                      Parcela = ubic.Parcela,
                                      VigenciaDesde = ubic.VigenciaDesde,
                                      VigenciaHasta = ubic.VigenciaHasta
                                  });

                    result = query3.ToList();

                    break;
                case 4:

                    int vid_subtipoubicacion = int.Parse(ddlSubTipoUbicacion.SelectedValue);

                    var query4 = (from ubic in db.Ubicaciones
                                  where ubic.id_subtipoubicacion == vid_subtipoubicacion
                                  && (!ubic.VigenciaHasta.HasValue || ubic.VigenciaHasta > FechaActual)
                                  && ubic.baja_logica == false
                                  select new Model.Ubicacion
                                  {
                                      id_ubicacion = ubic.id_ubicacion,
                                      id_subtipoubicacion = ubic.id_subtipoubicacion,
                                      NroPartidaMatriz = ubic.NroPartidaMatriz,
                                      Seccion = ubic.Seccion,
                                      Manzana = ubic.Manzana,
                                      Parcela = ubic.Parcela,
                                      VigenciaDesde = ubic.VigenciaDesde,
                                      VigenciaHasta = ubic.VigenciaHasta
                                  });

                    result = query4.ToList();
                    break;
            }


            gridubicacion.DataSource = result;
            gridubicacion.DataBind();
            pnlResultados.Update();

            pnlContentBuscar.Visible = false;
            pnlGridResultados.Visible = true;
            switch (result.Count)
            {
                case 0:
                    lblCantResultados.Text = "";
                    break;
                case 1:
                    lblCantResultados.Text = "( " + result.Count + " resultado )";
                    break;
                default:
                    lblCantResultados.Text = "( " + result.Count + " resultados )";
                    break;

            }

            btnIngresarUbicacion.Visible = (result.Count > 0);


        }

        public void editar()
        {
            if (this.id_cpadronUbicacion != 0)
            {
                int IdCapdronUbicacion = this.id_cpadronUbicacion;

                _editar = true;

                DGHP_Entities db = new DGHP_Entities();
                var query = (from ubicCpadron in db.CPadron_Ubicaciones
                                             join ubic in db.Ubicaciones on ubicCpadron.id_ubicacion equals ubic.id_ubicacion
                                             where ubicCpadron.id_cpadronubicacion == IdCapdronUbicacion
                                             select new Model.Ubicacion
                                             {
                                                 id_ubicacion = ubic.id_ubicacion,
                                                 id_subtipoubicacion = ubic.id_subtipoubicacion,
                                                 NroPartidaMatriz = ubic.NroPartidaMatriz,
                                                 Seccion = ubic.Seccion,
                                                 Manzana = ubic.Manzana,
                                                 Parcela = ubic.Parcela,
                                                 VigenciaDesde = ubic.VigenciaDesde,
                                                 VigenciaHasta = ubic.VigenciaHasta
                                             });

                var result = query.ToList();

                gridubicacion.DataSource = result;
                gridubicacion.DataBind();
                pnlResultados.Update();
                pnlContentBuscar.Visible = false;
                pnlGridResultados.Visible = true;

                switch (result.ToList().Count)
                {
                    case 0:
                        lblCantResultados.Text = "";
                        break;
                    case 1:
                        lblCantResultados.Text = "( " + result.Count + " resultado )";
                        break;
                    default:
                        lblCantResultados.Text = "( " + result.Count + " resultados )";
                        break;
                }


                btnModificarUbicacion.Visible = true;
                btnIngresarUbicacion.Visible = false;
            }

        }
        protected void btnModificarUbicacion_Click(object sender, EventArgs e)
        {
            lstValidacionesUbicacion.Items.Clear();
            pnlValidacionIngresoUbicacion.Visible = false;

            GridViewRow row = gridubicacion.Rows[0];        // es la fila 0 porque rows devuelve las filas de la pagina actual y como la pagina es siempre 1 hay 1 sola fila.

            if (!TildoPuertas(row) && hid_tabselected.Value != "3")     // Si no tildo puertas y no es una ubicacion especial.
                lstValidacionesUbicacion.Items.Add("Debe tildar al menos 1 puerta.");

            ValidarPuertas(row, ref lstValidacionesUbicacion);

            if (lstValidacionesUbicacion.Items.Count.Equals(0))
            {
                DGHP_Entities db = new DGHP_Entities();
                int idubicacion = (int)gridubicacion.DataKeys[gridubicacion.Rows.Count - 1].Value;
                int id_cpadron = validar_estado == 0 ? id_cpadron_mod :  (from sgiTraTarea in db.SGI_Tramites_Tareas_CPADRON where sgiTraTarea.id_tramitetarea == IdTratmiteTarea select sgiTraTarea).FirstOrDefault().id_cpadron;
                int id_cpadronUbicacion = (from cpadronUbic in db.CPadron_Ubicaciones where cpadronUbic.id_ubicacion == idubicacion && cpadronUbic.id_cpadron == id_cpadron select cpadronUbic).FirstOrDefault().id_cpadronubicacion;
                db.CPadron_EliminarUbicacion(id_cpadron, id_cpadronUbicacion, validar_estado);
                lstValidacionesUbicacion.Items.Clear();
                pnlValidacionIngresoUbicacion.Visible = false;
                btnIngresarUbicacion_Click(sender, e);
                btnModificarUbicacion.Visible = false;
            }
            else
            {
                // mostrar resultados de las validaciones
                pnlValidacionIngresoUbicacion.Visible = true;
            }

        }
        protected void gridubicacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridubicacion.PageIndex = e.NewPageIndex;
            gridubicacion.DataSource = result;
            gridubicacion.DataBind();
            pnlResultados.Update();

        }


        protected void cmdPage(object sender, EventArgs e)
        {
            Button cmdPage = (Button)sender;
            gridubicacion.PageIndex = int.Parse(cmdPage.Text) - 1;
            gridubicacion.DataSource = result;
            gridubicacion.DataBind();
            pnlResultados.Update();

        }
        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            gridubicacion.PageIndex = gridubicacion.PageIndex - 1;

            gridubicacion.DataSource = result;
            gridubicacion.DataBind();
            pnlResultados.Update();

        }
        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            gridubicacion.PageIndex = gridubicacion.PageIndex + 1;
            gridubicacion.DataSource = result;
            gridubicacion.DataBind();
            pnlResultados.Update();
        }


        protected void gridubicacion_DataBound(object sender, EventArgs e)
        {
            GridView grid = gridubicacion;
            GridViewRow fila = (GridViewRow)grid.BottomPagerRow;

            if (fila != null)
            {
                Button btnAnterior = (Button)fila.Cells[0].FindControl("cmdAnterior");
                Button btnSiguiente = (Button)fila.Cells[0].FindControl("cmdSiguiente");

                if (grid.PageIndex == 0)
                    btnAnterior.Visible = false;
                else
                    btnAnterior.Visible = true;

                if (grid.PageIndex == grid.PageCount - 1)
                    btnSiguiente.Visible = false;
                else
                    btnSiguiente.Visible = true;


                // Ocultar todos los botones con Números de Página
                for (int i = 1; i <= 19; i++)
                {
                    Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                    btn.Visible = false;
                }


                if (grid.PageIndex == 0 || grid.PageCount <= 10)
                {
                    // Mostrar 10 botones o el máximo de páginas

                    for (int i = 1; i <= 10; i++)
                    {
                        if (i <= grid.PageCount)
                        {
                            Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                            btn.Text = i.ToString();
                            btn.Visible = true;
                        }
                    }
                }
                else
                {
                    // Mostrar 9 botones hacia la izquierda y 9 hacia la derecha
                    // o bien los que sea posible en caso de no llegar a 9

                    int CantBucles = 0;

                    Button btnPage10 = (Button)fila.Cells[0].FindControl("cmdPage10");
                    btnPage10.Visible = true;
                    btnPage10.Text = Convert.ToString(grid.PageIndex + 1);

                    // Ubica los 9 botones hacia la izquierda
                    for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                    {
                        CantBucles++;
                        if (i >= 0)
                        {
                            Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 - CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                        }

                    }

                    CantBucles = 0;
                    // Ubica los 9 botones hacia la derecha
                    for (int i = grid.PageIndex + 1; i <= grid.PageIndex + 9; i++)
                    {
                        CantBucles++;
                        if (i <= grid.PageCount - 1)
                        {
                            Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 + CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                        }
                    }



                }
                Button cmdPage;
                string btnPage = "";
                for (int i = 1; i <= 19; i++)
                {
                    btnPage = "cmdPage" + i.ToString();
                    cmdPage = (Button)fila.Cells[0].FindControl(btnPage);
                    if (cmdPage != null)
                        cmdPage.CssClass = "btn btn-default";

                }



                // busca el boton por el texto para marcarlo como seleccionado
                string btnText = Convert.ToString(grid.PageIndex + 1);
                foreach (Control ctl in fila.Cells[0].FindControl("pnlpager").Controls)
                {
                    if (ctl is Button)
                    {
                        Button btn = (Button)ctl;
                        if (btn.Text.Equals(btnText))
                        {
                            btn.CssClass = "btn btn-info";
                        }
                    }
                }

            }
        }


        protected void gridubicacion_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                HiddenField hid_id_ubicacion = (HiddenField)e.Row.FindControl("hid_id_ubicacion");
                DataList lstPuertas = (DataList)e.Row.FindControl("lstPuertas");
                CheckBoxList optPartidasHorizontales = (CheckBoxList)e.Row.FindControl("CheckBoxListPHorizontales");
                Panel pnlPartidasHorizontales = (Panel)e.Row.FindControl("pnlPartidasHorizontales");
                UpdatePanel updPuertas = (UpdatePanel)e.Row.FindControl("updPuertas");

                ScriptManager.RegisterStartupScript(updPuertas, updPuertas.GetType(), "init_JS_updPuertas", "init_JS_updPuertas();", true);

                int id_ubicacion = int.Parse(hid_id_ubicacion.Value);


                DGHP_Entities db = new DGHP_Entities();
                Model.Ubicacion ubicacion = (from ubic in db.Ubicaciones
                                             where ubic.id_ubicacion == id_ubicacion
                                             select new Model.Ubicacion
                                             {
                                                 id_ubicacion = ubic.id_ubicacion,
                                                 id_subtipoubicacion = ubic.id_subtipoubicacion,
                                                 NroPartidaMatriz = ubic.NroPartidaMatriz,
                                                 Seccion = ubic.Seccion,
                                                 Manzana = ubic.Manzana,
                                                 Parcela = ubic.Parcela,
                                                 VigenciaDesde = ubic.VigenciaDesde,
                                                 VigenciaHasta = ubic.VigenciaHasta
                                             }).First();


                lstPuertas.DataSource = ubicacion.GetPuertas();
                lstPuertas.DataBind();

                if (hid_tabselected.Value == "3")
                {
                    Panel pnlTipoUbicacion = (Panel)e.Row.FindControl("pnlTipoUbicacion");
                    Panel pnlSMP = (Panel)e.Row.FindControl("pnlSMP");
                    Label lblTipoUbicacion = (Label)e.Row.FindControl("lblTipoUbicacion");
                    Label lblSubTipoUbicacion = (Label)e.Row.FindControl("lblSubTipoUbicacion");
                    Label lblLocal = (Label)e.Row.FindControl("lblLocal");

                    var dataEsp = (from ubic in db.Ubicaciones
                                   join stu in db.SubTiposDeUbicacion on ubic.id_subtipoubicacion equals stu.id_subtipoubicacion
                                   join tu in db.TiposDeUbicacion on stu.id_tipoubicacion equals tu.id_tipoubicacion
                                   where ubic.id_ubicacion == id_ubicacion
                                   select new
                                   {
                                       tu.descripcion_tipoubicacion,
                                       stu.descripcion_subtipoubicacion
                                   }).FirstOrDefault();

                    if (dataEsp != null)
                    {
                        lblTipoUbicacion.Text = dataEsp.descripcion_tipoubicacion;
                        lblSubTipoUbicacion.Text = dataEsp.descripcion_subtipoubicacion;
                        lblLocal.Text = txtDescUbicacion.Text;
                    }

                    pnlTipoUbicacion.Visible = true;
                    pnlSMP.Visible = false;

                }


                var lstPartidasHorizontales = (from phor in db.Ubicaciones_PropiedadHorizontal
                                               where phor.id_ubicacion == id_ubicacion && (!phor.VigenciaHasta.HasValue || phor.VigenciaHasta < DateTime.Now)
                                               select new
                                               {
                                                   phor.id_propiedadhorizontal,
                                                   phor.id_ubicacion,
                                                   phor.NroPartidaHorizontal,
                                                   phor.Piso,
                                                   phor.Depto,
                                                   DescripcionCompleta = "Partida: " + (phor.NroPartidaHorizontal.HasValue ? phor.NroPartidaHorizontal.Value.ToString() : "N/A") +
                                                                (phor.Piso.Length > 0 ? " - Piso: " + phor.Piso : "") +
                                                                (phor.Depto.Length > 0 ? " - U.F.: " + phor.Depto : "")
                                               }).ToList();

                // Partidas Horizontales
                optPartidasHorizontales.DataValueField = "id_propiedadhorizontal";
                optPartidasHorizontales.DataTextField = "DescripcionCompleta";
                optPartidasHorizontales.DataSource = lstPartidasHorizontales;
                optPartidasHorizontales.DataBind();

                if (_editar && this.id_cpadronUbicacion != 0)
                {
                    TextBox txtOtros = (TextBox)e.Row.FindControl("txtOtros");
                    TextBox txtLocal = (TextBox)e.Row.FindControl("txtLocal");
                    TextBox txtDepto = (TextBox)e.Row.FindControl("txtDepto");
                    //TextBox txtTorre = (TextBox)e.Row.FindControl("txtTorre");

                    var query = (from ubicCpadron in db.CPadron_Ubicaciones
                                 where ubicCpadron.id_cpadronubicacion == this.id_cpadronUbicacion
                                 select ubicCpadron);

                    var resultCpadron = query.ToList();
                    string Otros = resultCpadron.FirstOrDefault().deptoLocal_cpadronubicacion ?? "";
                    string Local = resultCpadron.FirstOrDefault().Local ?? "";
                    string Depto = resultCpadron.FirstOrDefault().Depto ?? "";
                    string Torre = resultCpadron.FirstOrDefault().Torre ?? "";

                    txtOtros.Text = Otros.Trim();
                    txtLocal.Text = Local.Trim();
                    txtDepto.Text = Depto.Trim();
                    //txtTorre.Text = Torre.Trim();
                }
                pnlPartidasHorizontales.Style["display"] = (optPartidasHorizontales.Items.Count > 0 ? "block" : "none");

            }
        }

        private bool TildoPuertas(GridViewRow row)
        {
            bool ret = false;

            DataList lstPuertas = (DataList)row.FindControl("lstPuertas");

            foreach (DataListItem item in lstPuertas.Items)
            {
                CheckBox chkPuerta = (CheckBox)item.FindControl("chkPuerta");
                if (chkPuerta.Checked)
                {
                    ret = true;
                    break;
                }
            }

            return ret;

        }
        protected void btnNuevaBusquedar_Click(object sender, EventArgs e)
        {
            btnModificarUbicacion.Visible = false;
            pnlGridResultados.Visible = false;
            pnlContentBuscar.Visible = true;
            gridubicacion.PageIndex = 0;
            pnlResultados.Update();
        }

        protected void lnkAgregarOtraPuerta_Click(object sender, EventArgs e)
        {

            LinkButton lnk = (LinkButton)sender;
            int id_ubic_puerta_origen = int.Parse(lnk.CommandArgument);
            DataList lstPuertas = (DataList)lnk.Parent.Parent;
            List<Model.Ubicacion.Puerta> puertas = new List<Model.Ubicacion.Puerta>();
            Model.Ubicacion.Puerta puerta = new Model.Ubicacion.Puerta();
            List<bool> puertasTildadas = new List<bool>();
            List<int> NroPuertasModificadas = new List<int>();
            UpdatePanel updPuertas = (UpdatePanel)lstPuertas.Parent.Parent;

            ScriptManager.RegisterStartupScript(updPuertas, updPuertas.GetType(), "init_JS_updPuertas", "init_JS_updPuertas();", true);

            foreach (DataListItem item in lstPuertas.Items)
            {
                HiddenField hid_ubic_puerta = (HiddenField)item.FindControl("hid_ubic_puerta");
                CheckBox chkPuerta = (CheckBox)item.FindControl("chkPuerta");
                TextBox txtNroPuerta = (TextBox)item.FindControl("txtNroPuerta");

                int NroPuerta = 0;
                int.TryParse(txtNroPuerta.Text.Trim(), out NroPuerta);

                puertasTildadas.Add(chkPuerta.Checked);
                NroPuertasModificadas.Add(NroPuerta);

                puerta = Model.Ubicacion.GetPuerta(int.Parse(hid_ubic_puerta.Value));
                puertas.Add(puerta);

            }

            puerta = Model.Ubicacion.GetPuerta(id_ubic_puerta_origen);
            puertas.Add(puerta);
            NroPuertasModificadas.Add(puerta.NroPuerta_ubic);
            puertasTildadas.Add(false);

            lstPuertas.DataSource = puertas;
            lstPuertas.DataBind();

            foreach (DataListItem item in lstPuertas.Items)
            {
                CheckBox chkPuerta = (CheckBox)item.FindControl("chkPuerta");
                TextBox txtNroPuerta = (TextBox)item.FindControl("txtNroPuerta");

                chkPuerta.Checked = puertasTildadas[item.ItemIndex];
                txtNroPuerta.Text = NroPuertasModificadas[item.ItemIndex].ToString();
            }
        }

        // Devuelve true si hay puertas tildadas en la ubicacion
        protected void lstPuertas_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (Edicion)
            {
                DGHP_Entities db = new DGHP_Entities();
                var query = (from ubiCpadron in db.CPadron_Ubicaciones_Puertas
                             where ubiCpadron.id_cpadronubicacion == id_cpadronUbicacion select ubiCpadron);

                var Codigo = Convert.ToInt32((e.Item.FindControl("hid_codigo_calle") as HiddenField).Value);
                var Numero = Convert.ToInt32((e.Item.FindControl("hid_NroPuerta_ubic") as HiddenField).Value);

                if (query.Select(p => p.codigo_calle).Contains(Codigo) && query.Select(p => p.NroPuerta).Contains(Numero))
                    (e.Item.FindControl("chkPuerta") as CheckBox).Checked = true;
            }

        }
        protected void ddlTipoDeUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id_tipoubicacion = int.Parse(ddlTipoDeUbicacion.SelectedValue);
            if (id_tipoubicacion == 1) //Si es subte desabilito la validacion del local
                ReqtxtDescUbicacion.Enabled = false;
            else
                ReqtxtDescUbicacion.Enabled = true;

            CargarComboSubTipoUbicacion(id_tipoubicacion);
        }
        private void CargarComboTipoUbicacion()
        {

            DGHP_Entities db = new DGHP_Entities();
            var query = (from ent in db.TiposDeUbicacion
                         where ent.id_tipoubicacion > 0
                         orderby ent.descripcion_tipoubicacion
                         select ent);


            ddlTipoDeUbicacion.DataTextField = "descripcion_tipoubicacion";
            ddlTipoDeUbicacion.DataValueField = "id_tipoubicacion";
            ddlTipoDeUbicacion.DataSource = query.ToList();
            ddlTipoDeUbicacion.DataBind();

            int id_tipoubicacion = 0;
            if (int.TryParse(ddlTipoDeUbicacion.SelectedValue, out id_tipoubicacion))
                CargarComboSubTipoUbicacion(id_tipoubicacion);

            db.Dispose();

        }

        private void CargarComboSubTipoUbicacion(int id_tipoubicacion)
        {

            DGHP_Entities db = new DGHP_Entities();
            var query = (from ent in db.SubTiposDeUbicacion
                         where ent.id_tipoubicacion.Equals(id_tipoubicacion)
                         orderby ent.descripcion_subtipoubicacion
                         select ent);

            ddlSubTipoUbicacion.DataTextField = "descripcion_subtipoubicacion";
            ddlSubTipoUbicacion.DataValueField = "id_subtipoubicacion";
            ddlSubTipoUbicacion.DataSource = query.ToList();
            ddlSubTipoUbicacion.DataBind();
            db.Dispose();
        }

        // realiza las validaciones necesarias sobre las puertas tildadas e informa los errores en la lista pasada por referencia
        private void ValidarPuertas(GridViewRow row, ref BulletedList lst)
        {
            List<int[]> puertasSeleccionadas = new List<int[]>();
            DataList lstPuertas = (DataList)row.FindControl("lstPuertas");

            foreach (DataListItem item in lstPuertas.Items)
            {
                CheckBox chkPuerta = (CheckBox)item.FindControl("chkPuerta");
                if (chkPuerta.Checked)
                {
                    int NroRegistro = item.ItemIndex + 1;
                    HiddenField hid_codigo_calle = (HiddenField)item.FindControl("hid_codigo_calle");
                    HiddenField hid_NroPuerta_ubic = (HiddenField)item.FindControl("hid_NroPuerta_ubic"); //Número de Puerta registro de base de datos (original)
                    TextBox txtNroPuerta = (TextBox)item.FindControl("txtNroPuerta");
                    Label lblnombreCalle = (Label)item.FindControl("lblnombreCalle");

                    string strPuerta = lblnombreCalle.Text;
                    int codigo_calle = 0, NroPuerta = 0, NroPuertaOriginal = 0, minvaluePuerta = 0, maxvaluePuerta = 0;

                    int.TryParse(hid_codigo_calle.Value, out codigo_calle);
                    int.TryParse(hid_NroPuerta_ubic.Value, out NroPuertaOriginal);
                    int.TryParse(txtNroPuerta.Text, out NroPuerta);


                    if (NroPuertaOriginal % 100 == 0)
                    {
                        minvaluePuerta = NroPuertaOriginal - 99;
                        maxvaluePuerta = NroPuertaOriginal;
                    }
                    else
                    {
                        minvaluePuerta = Convert.ToInt32(Math.Floor(Convert.ToDecimal(NroPuertaOriginal / 100)) * 100 + 1);
                        maxvaluePuerta = Convert.ToInt32(Math.Floor(Convert.ToDecimal(NroPuertaOriginal / 100 + 1)) * 100);
                    }

                    if (NroPuerta < minvaluePuerta || NroPuerta > maxvaluePuerta)
                    {

                        lst.Items.Add(string.Format("En la puerta Nº {0} '{1}', la numeración debe encontrarse entre {2} y {3}.",
                                NroRegistro, lblnombreCalle.Text + " " + txtNroPuerta.Text.Trim(), minvaluePuerta, maxvaluePuerta));

                    }
                    else if (NroPuerta % 2 != NroPuertaOriginal % 2)
                    {
                        string strParImpar = "";

                        if (NroPuertaOriginal % 2 == 0)
                            strParImpar = "par";
                        else
                            strParImpar = "impar";


                        lst.Items.Add(
                                string.Format("En la puerta Nº {0} '{1}', la numeración debe ser un Nº {2} según el lado de la calle elegido.",
                                NroRegistro, lblnombreCalle.Text + " " + txtNroPuerta.Text, strParImpar
                                ));

                    }
                    else
                    {
                        if (puertasSeleccionadas.Find(x => x[0] == codigo_calle && x[1] == NroPuerta) == null)
                            puertasSeleccionadas.Add(new int[2] { codigo_calle, NroPuerta });
                        else
                            lst.Items.Add(
                                string.Format("Existe más de 1 puerta con la misma numeración en la misma calle, quite el tilde a la puerta Nº {0} '{1}'.",
                                NroRegistro, lblnombreCalle.Text + " " + txtNroPuerta.Text
                                ));

                    }
                }
            }
        }

        protected void btnIngresarUbicacion_Click(object sender, EventArgs e)
        {
            lstValidacionesUbicacion.Items.Clear();
            pnlValidacionIngresoUbicacion.Visible = false;

            GridViewRow row = gridubicacion.Rows[0];        // es la fila 0 porque rows devuelve las filas de la pagina actual y como la pagina es siempre 1 hay 1 sola fila.

            if (!TildoPuertas(row) && hid_tabselected.Value != "3")     // Si no tildo puertas y no es una ubicacion especial.
                lstValidacionesUbicacion.Items.Add("Debe tildar al menos 1 puerta.");

            ValidarPuertas(row, ref lstValidacionesUbicacion);

            if (lstValidacionesUbicacion.Items.Count.Equals(0))
            {
                ucAgregarUbicacionEventsArgs args = new ucAgregarUbicacionEventsArgs();
                HiddenField hid_id_ubicacion = (HiddenField)row.FindControl("hid_id_ubicacion");
                DataList lstPuertas = (DataList)row.FindControl("lstPuertas");
                CheckBoxList chkPartidasHorizontales = (CheckBoxList)row.FindControl("CheckBoxListPHorizontales");
                TextBox txtDepto = (TextBox)row.FindControl("txtDepto");
                TextBox txtLocal = (TextBox)row.FindControl("txtLocal");
                TextBox txtOtros = (TextBox)row.FindControl("txtOtros");

                string vDeptoLocalOtros = "";

                vDeptoLocalOtros = txtOtros.Text.Trim();

                if (txtDepto.Text.Trim().Length > 0)
                {
                    vDeptoLocalOtros += " Depto " + txtDepto.Text.Trim();
                    vDeptoLocalOtros = vDeptoLocalOtros.Trim();
                }

                if (txtLocal.Text.Trim().Length > 0)
                {
                    vDeptoLocalOtros += " Local " + txtLocal.Text.Trim();
                    vDeptoLocalOtros = vDeptoLocalOtros.Trim();
                }

                args.id_ubicacion = Convert.ToInt32(hid_id_ubicacion.Value);
                args.id_subtipoubicacion = 0;
                args.vDeptoLocalOtros = vDeptoLocalOtros;

                // Agrega los datos de las ubicaciones especiales
                if (hid_tabselected.Value.Equals("3") && ddlSubTipoUbicacion.SelectedValue.Length > 0)
                {
                    args.id_subtipoubicacion = int.Parse(ddlSubTipoUbicacion.SelectedValue);
                    args.local_subtipoubicacion = txtDescUbicacion.Text.Trim();
                }


                // agrega las calles seleccionadas
                foreach (DataListItem item in lstPuertas.Items)
                {
                    CheckBox chkPuerta = (CheckBox)item.FindControl("chkPuerta");
                    if (chkPuerta.Checked)
                    {
                        HiddenField hid_codigo_calle = (HiddenField)item.FindControl("hid_codigo_calle");
                        TextBox txtNroPuerta = (TextBox)item.FindControl("txtNroPuerta");
                        Label lblnombreCalle = (Label)item.FindControl("lblnombreCalle");
                        int codigo_calle = int.Parse(hid_codigo_calle.Value);
                        int NroPuerta = int.Parse(txtNroPuerta.Text.Trim());

                        ucPuerta puerta = new ucPuerta();
                        puerta.codigo_calle = codigo_calle;
                        puerta.NroPuerta = NroPuerta;

                        args.Puertas.Add(puerta);

                    }

                }

                //Agregar las partidas horizontales seleccionadas
                if (chkPartidasHorizontales.Items.Count > 0)
                {
                    foreach (ListItem chkPartidaHorizontal in chkPartidasHorizontales.Items)
                    {
                        if (chkPartidaHorizontal.Selected)
                        {
                            args.ids_propiedades_horizontales.Add(int.Parse(chkPartidaHorizontal.Value));
                        }
                    }


                }
                args.upd = updPanelBotones;

                // Llamar al evento.
                if (AgregarUbicacionClick != null)
                {
                    AgregarUbicacionClick(sender, ref args);
                    if (!args.Cancel)
                        Inicilizar_Control();
                }

            }
            else
            {
                // mostrar resultados de las validaciones
                pnlValidacionIngresoUbicacion.Visible = true;
            }
        }


    }
}