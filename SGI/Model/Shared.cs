using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGI.Model
{
    public class Shared
    {
        /*Dado dos fechas, retorna la cantidad de dias habiales entre las mismas*/
        public static int GetBusinessDays(DateTime start, DateTime end)
        {
            int count = 0;
            DateTime starTime = start;
            while ((end - start).TotalMinutes > 1440)
            {
                if ((start.DayOfWeek != DayOfWeek.Saturday) && (start.DayOfWeek != DayOfWeek.Sunday))
                    count++;
                start = start.AddDays(1);
            }
            DGHP_Entities db = new DGHP_Entities();
            int feriadosCount = (from hab in db.SGI_Feriados where hab.Fecha >= starTime && hab.Fecha <= end select hab).Count();
            count -= feriadosCount;
            return count;
        }

        public class clsProfesional
        {
            public int id_profesional { get; set; }
            public int? id_concejo { get; set; }
            public string concejo { get; set; }
            public string concejoGConsejo { get; set; }
            public string nro_matricula { get; set; }
            public string nombre_apellido { get; set; }
            public string direccion { get; set; }
            public string cuit { get; set; }
            public string email { get; set; }
            public string perfiles { get; set; }
            public List<aspnet_Roles> perfilesList { get; set; }
            public Guid? userId { get; set; }
            public string usuario { get; set; }
            public string subperfil { get; set; }
            public string DadoBaja { get; set; }
            //Agreados
            public string NroPuerta { get; set; }
            public string Piso { get; set; }
            public string Depto { get; set; }
            public int? IdTipoDocumento { get; set; }
            public string TipoDocumento { get; set; }
            public int? NroDocumento { get; set; }
            public string Localidad { get; set; }
            public string Provincia { get; set; }
            public string Sms { get; set; }
            public string Telefono { get; set; }
            public string Inhibido { get; set; }
            public List<Rel_UsuariosProf_Roles_Clasificacion> subperfilesList { get; set; }
        }

        public class clsElevadoresGeneral
        {
            public int id_dgubicacion { get; set; }
            public int? id_estado_pago { get; set; }
            public string direccion { get; set; }
            public string boleta_emitida { get; set; }
            public string estado_boleta { get; set; }
            public int id_elevador { get; set; }
            public string propietario { get; set; }
            public int? id_empasc { get; set; }
            public string RazonSocial_empasc { get; set; }
            public int anio_vigencia { get; set; }
            public int id_estado_aceptacion { get; set; }
            public string aceptacionxeca { get; set; }
            public string estado_aceptacion { get; set; }
            public DateTime? fecha_estado_aceptacion { get; set; }
            public string estado_elevador { get; set; }
        }

        public class clsElevadoresxEmpresaxEstados
        {
            public int id_dgubicacion { get; set; }
            public string direccion { get; set; }
            public int? id_empasc { get; set; }
            public string RazonSocial_empasc { get; set; }
            public int anio_vigencia { get; set; }
            public int CantidadElevadores { get; set; }
            public int id_estado_aceptacion { get; set; }
            public string estado_aceptacion { get; set; }
            public string patente { get; set; }
            public string tipo { get; set; }
            public string estado { get; set; }
            public int dias_pendiente { get; set; }
            public DateTime? fecha_pago { get; set; }

        }

        public class itemGrillaConsultaPorElevador
        {
            public int id_elevador { get; set; }
            public string usuario { get; set; }
            public int? patente { get; set; }
            public int id_dgubicacion { get; set; }
            public string estado_oblea { get; set; }
            public string cod_estado_aceptacion { get; set; }
            public string empasc { get; set; }
            public string direccion { get; set; }
            public string estado_elevador { get; set; }
        }



        public class itemGrillaEmpresaICI
        {
            public int id_empici { get; set; }
            public int NroRegistro_empici { get; set; }
            public string RazonSocial_empici { get; set; }
            public string Cuit_empici { get; set; }
            public string DomicilioComercialCompuesto { get; set; }
            public Nullable<DateTime> fechavto_empicifec { get; set; }
            public bool DadoBaja_empici { get; set; }
            public Guid? userid { get; set; }
            public string usuario { get; set; }
        }

        public class itemGrillaEmpresaECA
        {
            public int id_emp { get; set; }
            public int nr_registro { get; set; }
            public string cuit { get; set; }
            public string ape_razon { get; set; }
            public string calle { get; set; }
            public int nr_puerta { get; set; }
            public bool dadoBaja { get; set; }
            public string usuarioNom { get; set; }
            public bool generar_visible { get; set; }
            public bool editar_visible { get; set; }
            public Nullable<DateTime> vencimiento { get; set; }
        }

        public class itemGrillaEmpresaIFCI
        {
            public int id_empresa { get; set; }
            public string Cuit { get; set; }
            public string Razon_Apenom { get; set; }
            public string Calle { get; set; }
            public int Nro_Puerta { get; set; }
            public bool Dado_Baja { get; set; }
            public string UserName { get; set; }
            public bool generar_visible { get; set; }
            public bool editar_visible { get; set; }

        }

        public static string getRedireccionURL(int id_solicitud, int id_tramitetarea)
        {
            DGHP_Entities db = new DGHP_Entities();

            int qHAB = (from hab in db.SGI_Tramites_Tareas_HAB
                        where hab.id_tramitetarea == id_tramitetarea
                        select hab).Count();
            int qCP = (from cp in db.SGI_Tramites_Tareas_CPADRON
                       where cp.id_tramitetarea == id_tramitetarea
                       select cp).Count();
            int qTR = (from tr in db.SGI_Tramites_Tareas_TRANSF
                       where tr.id_tramitetarea == id_tramitetarea
                       select tr).Count();

            string url = "";
            if (qHAB > 0)
            {
                url = string.Format("~/GestionTramite/VisorTramite.aspx?id={0}", id_solicitud);
            }
            else if (qCP > 0)
            {
                url = string.Format("~/VisorTramiteCP/{0}", id_solicitud);
            }
            else if (qTR > 0)
            {
                url = string.Format("~/VisorTramiteTR/{0}", id_solicitud);
            }

            return url;
        }

        public static List<ItemDireccion> getDirecciones(string[] arrUbicaciones)
        {
            // Obtiene las direcciones en una sola linea de una matriz de solicitudes

            DGHP_Entities db = new DGHP_Entities();

            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT ubic.id_ubicacion, ");
            sql.AppendLine("'direccion' = CASE ");
            sql.AppendLine("WHEN ubic.id_subtipoubicacion  in (SELECT id_subtipoubicacion FROM SubTiposDeUbicacion sub WHERE sub.id_tipoubicacion = ( SELECT TiposDeUbicacion.id_tipoubicacion FROM TiposDeUbicacion WHERE dbo.TiposDeUbicacion.descripcion_tipoubicacion = 'Objeto territorial')) ");
            sql.AppendLine("THEN IsNull(dbo.Bus_NombreCalle(puer.codigo_calle,puer.NroPuerta_ubic),'') +  ' ' +  IsNull(convert(nvarchar, puer.NroPuerta_ubic)+'t','') ");
            sql.AppendLine("ELSE ");
            sql.AppendLine("IsNull(dbo.Bus_NombreCalle(puer.codigo_calle,puer.NroPuerta_ubic),'') +  ' ' +  IsNull(convert(nvarchar, puer.NroPuerta_ubic),'') ");
            sql.AppendLine("END ");
            sql.AppendLine("FROM Ubicaciones ubic ");
            sql.AppendLine("INNER JOIN Ubicaciones_Puertas puer ON ubic.id_ubicacion = puer.id_ubicacion ");
            sql.AppendLine(string.Format("	WHERE ubic.id_ubicacion IN({0})", string.Join(",", arrUbicaciones)));
            sql.AppendLine(" ORDER BY ubic.id_ubicacion");
            List<ItemDireccion> lstDirecciones = null;
            if (arrUbicaciones.Count() > 0)
                lstDirecciones = db.Database.SqlQuery<ItemDireccion>(sql.ToString()).ToList();

            List<ItemDireccion> lstDireccionesClass = new List<ItemDireccion>();
            int count = 0;
            foreach (string ubicacion in arrUbicaciones)
            {

                ItemDireccion[] temp = lstDirecciones.FindAll(s => s.id_ubicacion.Equals(int.Parse(arrUbicaciones.ElementAt(count)))).ToArray();
                string dir = "";
                foreach (ItemDireccion s in temp)
                {
                    if (dir == "")
                        dir = s.direccion;
                    else
                    {
                        if (temp.Length > 1)
                            dir += " / " + s.direccion;
                        else
                            dir += s.direccion;
                    }
                }

                ItemDireccion item = new ItemDireccion();
                item.direccion = dir;
                item.id_ubicacion = int.Parse(ubicacion);
                count++;
                lstDireccionesClass.Add(item);
            }

            db.Dispose();

            return lstDireccionesClass;
        }

        public static List<ItemDireccion> getDireccionesTemp(string[] arrUbicaciones)
        {
            // Obtiene las direcciones en una sola linea de una matriz de solicitudes

            DGHP_Entities db = new DGHP_Entities();

            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("ubic.id_ubicacion_temp AS id_ubicacion, ");
            sql.AppendLine("IsNull(dbo.Bus_NombreCalle(puer.codigo_calle,puer.NroPuerta_ubic),'') +  ' ' +  ");
            sql.AppendLine("IsNull(convert(nvarchar, puer.NroPuerta_ubic),'')  as direccion ");
            sql.AppendLine("FROM ");
            sql.AppendLine("Ubicaciones_temp ubic INNER ");
            sql.AppendLine("JOIN Ubicaciones_Puertas_temp puer ON ubic.id_ubicacion_temp = puer.id_ubicacion_temp");
            sql.AppendLine(string.Format("	WHERE ubic.id_ubicacion_temp IN({0})", string.Join(",", arrUbicaciones)));
            sql.AppendLine(" ORDER BY ");
            sql.AppendLine("ubic.id_ubicacion_temp");
            List<ItemDireccion> lstDirecciones = null;
            if (arrUbicaciones.Count() > 0)
                lstDirecciones = db.Database.SqlQuery<ItemDireccion>(sql.ToString()).ToList();

            List<ItemDireccion> lstDireccionesClass = new List<ItemDireccion>();
            int count = 0;
            foreach (string ubicacion in arrUbicaciones)
            {

                ItemDireccion[] temp = lstDirecciones.FindAll(s => s.id_ubicacion.Equals(int.Parse(arrUbicaciones.ElementAt(count)))).ToArray();
                string dir = "";
                foreach (ItemDireccion s in temp)
                {
                    if (dir == "")
                        dir = s.direccion;
                    else
                    {
                        if (temp.Length > 1)
                            dir += " / " + s.direccion;
                        else
                            dir += s.direccion;
                    }
                }

                ItemDireccion item = new ItemDireccion();
                item.direccion = dir;
                item.id_ubicacion = int.Parse(ubicacion);
                count++;
                lstDireccionesClass.Add(item);
            }

            db.Dispose();

            return lstDireccionesClass;
        }

        public static List<ItemDireccionDGFYCO> getDireccionesDGFYCO(string[] arrUbicaciones)
        {
            // Obtiene las direcciones en una sola linea de una matriz de solicitudes

            DGHP_Entities db = new DGHP_Entities();

            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("ubic.id_dgubicacion, ");
            sql.AppendLine("IsNull(dbo.Bus_NombreCalle(puer.codigo_calle,puer.NroPuerta),'') +  ' ' +  ");
            sql.AppendLine("IsNull(convert(nvarchar, puer.NroPuerta),'')  as direccion ");
            sql.AppendLine("FROM ");
            sql.AppendLine("DGFYCO_Ubicaciones ubic INNER ");
            sql.AppendLine("JOIN DGFYCO_Ubicaciones_Puertas puer ON ubic.id_dgubicacion = puer.id_dgubicacion");
            sql.AppendLine(string.Format("	WHERE ubic.id_dgubicacion IN({0})", string.Join(",", arrUbicaciones)));
            sql.AppendLine(" ORDER BY ");
            sql.AppendLine("ubic.id_dgubicacion");

            List<ItemDireccionDGFYCO> lstDirecciones = db.Database.SqlQuery<ItemDireccionDGFYCO>(sql.ToString()).ToList();

            List<ItemDireccionDGFYCO> lstDireccionesClass = new List<ItemDireccionDGFYCO>();
            int count = 0;
            foreach (string ubicacion in arrUbicaciones)
            {

                ItemDireccionDGFYCO[] temp = lstDirecciones.FindAll(s => s.id_dgubicacion.Equals(int.Parse(arrUbicaciones.ElementAt(count)))).ToArray();
                string dir = "";
                foreach (ItemDireccionDGFYCO s in temp)
                {
                    if (temp.Length > 1)
                        dir += " / " + s.direccion;
                    else
                        dir += s.direccion;
                }

                ItemDireccionDGFYCO item = new ItemDireccionDGFYCO();
                item.direccion = dir;
                item.id_dgubicacion = int.Parse(ubicacion);
                count++;
                lstDireccionesClass.Add(item);
            }

            db.Dispose();

            return lstDireccionesClass;
        }

        //public static List<clsItemGrillaUbicacionesDireccionDGFYCO> GetDireccionesDG_Ubicacion(string[] arrUbicaciones)
        //{
        //    // Obtiene las direcciones en una sola linea de una matriz de solicitudes

        //    DGHP_Entities db = new DGHP_Entities();

        //    List<clsItemGrillaUbicacionesPuertaDGFYCO> lstPuertas = (from ub in db.Ubicaciones_
        //                                                             where arrUbicaciones.Contains(ub.id_direccion.ToString())
        //                                                             select new clsItemGrillaUbicacionesPuertaDGFYCO
        //                                                             {
        //                                                                 id_dgubicacion = ub.id_ubicacion,
        //                                                                 calle = ub.direccion,
        //                                                                 puerta = ub.direccion,
        //                                                             }).ToList();

        //    List<clsItemGrillaUbicacionesDireccionDGFYCO> lstDirecciones = new List<clsItemGrillaUbicacionesDireccionDGFYCO>();

        //    int id_solicitud_ant = 0;
        //    string calle_ant = "";
        //    string Direccion_armada = "";

        //    if (lstPuertas.Count > 0)
        //    {
        //        id_solicitud_ant = lstPuertas[0].id_dgubicacion;
        //        calle_ant = lstPuertas[0].calle;
        //    }

        //    foreach (var puerta in lstPuertas)
        //    {
        //        if (id_solicitud_ant != puerta.id_dgubicacion)
        //        {
        //            clsItemGrillaUbicacionesDireccionDGFYCO itemDireccion = new clsItemGrillaUbicacionesDireccionDGFYCO();
        //            itemDireccion.id_dgubicacion = id_solicitud_ant;
        //            itemDireccion.direccion = Direccion_armada;
        //            lstDirecciones.Add(itemDireccion);
        //            Direccion_armada = "";
        //            id_solicitud_ant = puerta.id_dgubicacion;
        //        }

        //        if (Direccion_armada.Length == 0 || puerta.calle != calle_ant)
        //        {
        //            if (Direccion_armada.Length > 0)
        //                Direccion_armada += " - ";

        //            Direccion_armada += puerta.calle + " " + puerta.puerta;
        //        }
        //        else
        //        {
        //            Direccion_armada += " / " + puerta.puerta;
        //        }

        //        calle_ant = puerta.calle;


        //    }

        //    if (Direccion_armada.Length > 0)
        //    {
        //        clsItemGrillaUbicacionesDireccionDGFYCO itemDireccion = new clsItemGrillaUbicacionesDireccionDGFYCO();
        //        itemDireccion.id_dgubicacion = id_solicitud_ant;
        //        itemDireccion.direccion = Direccion_armada;
        //        lstDirecciones.Add(itemDireccion);
        //        Direccion_armada = "";
        //    }

        //    db.Dispose();

        //    return lstDirecciones;
        //}

        public static List<clsItemDireccion> GetDireccionesENC(string[] arrSolicitudes)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("DECLARE @tbl TABLE(id int)");
            sql.AppendLine("");

            foreach (string id in arrSolicitudes)
            {
                sql.AppendLine(string.Format("INSERT INTO @tbl VALUES({0})", id));
            }
            sql.AppendLine("");

            sql.AppendLine("SELECT * FROM ( ");
            sql.AppendLine("	SELECT DISTINCT ");
            sql.AppendLine("		sol.id_solicitud, ");
            sql.AppendLine("		CASE  ");
            sql.AppendLine("			WHEN tubic.id_tipoubicacion in (SELECT TiposDeUbicacion.id_tipoubicacion FROM TiposDeUbicacion WHERE dbo.TiposDeUbicacion.descripcion_tipoubicacion in ('Objeto territorial','Parcela común'))");
            sql.AppendLine("			THEN ");
            sql.AppendLine("				IsNull(solpuer.nombre_calle,'')  ");
            sql.AppendLine("			ELSE ");
            sql.AppendLine("				tubic.descripcion_tipoubicacion + ' ' +	stubic.descripcion_subtipoubicacion ");
            sql.AppendLine("		END as calle, ");
            sql.AppendLine("		'puerta'  = CASE   ");
            sql.AppendLine("			WHEN tubic.id_tipoubicacion IN (SELECT id_tipoubicacion FROM TiposDeUbicacion ub WHERE ub.descripcion_tipoubicacion IN ('Objeto territorial'))  ");
            sql.AppendLine("			THEN CONVERT(nvarchar(20),solpuer.nropuerta)+'t' ");
            sql.AppendLine("			WHEN tubic.id_tipoubicacion = 0");
            sql.AppendLine("        THEN IsNull(convert(nvarchar, solpuer.nropuerta),'') ");
            sql.AppendLine("			ELSE ");
            sql.AppendLine("				IsNull('Local ' + solubic.local_subtipoubicacion,'') ");
            sql.AppendLine("		END ");
            sql.AppendLine("	FROM ");
            sql.AppendLine("		SSIT_Solicitudes sol ");
            sql.AppendLine("		INNER JOIN SSIT_Solicitudes_Ubicaciones solubic ON sol.id_solicitud = solubic.id_solicitud ");
            sql.AppendLine("		INNER JOIN SubTiposDeUbicacion stubic ON solubic.id_subtipoubicacion = stubic.id_subtipoubicacion ");
            sql.AppendLine("		INNER JOIN TiposDeUbicacion tubic ON stubic.id_tipoubicacion = tubic.id_tipoubicacion ");
            sql.AppendLine("		LEFT JOIN SSIT_Solicitudes_Ubicaciones_Puertas solpuer ON solubic.id_solicitudubicacion = solpuer.id_solicitudubicacion  ");
            sql.AppendLine("		LEFT JOIN SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal solphor ON solubic.id_solicitudubicacion = solphor.id_solicitudubicacion  ");
            sql.AppendLine("		LEFT JOIN Ubicaciones_PropiedadHorizontal phor ON solphor.id_propiedadhorizontal = phor.id_propiedadhorizontal ");
            sql.AppendLine("		INNER JOIN @tbl tmp ON sol.id_solicitud = tmp.id");
            sql.AppendLine("	) as con ");
            sql.AppendLine("	ORDER BY ");
            sql.AppendLine("		id_solicitud ");
            sql.AppendLine("		,calle ");

            return GetDirecciones(sql);
        }

        public static List<clsItemDireccion> GetDireccionesCP(string[] arrSolicitudes)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("DECLARE @tbl TABLE(id int)");
            sql.AppendLine("");

            foreach (string id in arrSolicitudes)
            {
                sql.AppendLine(string.Format("INSERT INTO @tbl VALUES({0})", id));
            }
            sql.AppendLine("");

            sql.AppendLine("SELECT * FROM ( ");
            sql.AppendLine("	SELECT DISTINCT ");
            sql.AppendLine("		sol.id_cpadron as id_solicitud, ");
            sql.AppendLine("		CASE  ");
            sql.AppendLine("			WHEN tubic.id_tipoubicacion in (SELECT TiposDeUbicacion.id_tipoubicacion FROM TiposDeUbicacion WHERE dbo.TiposDeUbicacion.descripcion_tipoubicacion in ('Objeto territorial','Parcela común'))");
            sql.AppendLine("			THEN ");
            sql.AppendLine("				IsNull(encpuer.nombre_calle,'')  ");
            sql.AppendLine("			ELSE ");
            sql.AppendLine("				tubic.descripcion_tipoubicacion + ' ' +	stubic.descripcion_subtipoubicacion ");
            sql.AppendLine("		END as calle, ");
            sql.AppendLine("		'puerta' = CASE  ");
            sql.AppendLine("			WHEN tubic.id_tipoubicacion = (SELECT TiposDeUbicacion.id_tipoubicacion FROM TiposDeUbicacion WHERE dbo.TiposDeUbicacion.descripcion_tipoubicacion = 'Objeto territorial') ");
            sql.AppendLine("			THEN CONVERT(nvarchar(20),encpuer.nropuerta)+'t' ");
            sql.AppendLine("			WHEN tubic.id_tipoubicacion = 0  THEN IsNull(convert(nvarchar, encpuer.nropuerta),'') ");
            sql.AppendLine("			ELSE ");
            sql.AppendLine("				IsNull('Local ' + encubic.local_subtipoubicacion,'') ");
            sql.AppendLine("		END ");
            sql.AppendLine("	FROM ");
            sql.AppendLine("		CPadron_Solicitudes sol ");
            sql.AppendLine("		INNER JOIN CPadron_Ubicaciones encubic ON sol.id_cpadron = encubic.id_cpadron ");
            sql.AppendLine("		INNER JOIN SubTiposDeUbicacion stubic ON encubic.id_subtipoubicacion = stubic.id_subtipoubicacion ");
            sql.AppendLine("		INNER JOIN TiposDeUbicacion tubic ON stubic.id_tipoubicacion = tubic.id_tipoubicacion ");
            sql.AppendLine("		LEFT JOIN CPadron_Ubicaciones_Puertas encpuer ON encubic.id_cpadronubicacion = encpuer.id_cpadronubicacion  ");
            sql.AppendLine("		LEFT JOIN CPadron_Ubicaciones_PropiedadHorizontal encphor ON encubic.id_cpadronubicacion = encphor.id_cpadronubicacion  ");
            sql.AppendLine("		LEFT JOIN Ubicaciones_PropiedadHorizontal phor ON encphor.id_propiedadhorizontal = phor.id_propiedadhorizontal ");
            sql.AppendLine("		INNER JOIN @tbl tmp ON sol.id_cpadron = tmp.id");
            sql.AppendLine("	) as con ");
            sql.AppendLine("	ORDER BY ");
            sql.AppendLine("		id_solicitud ");
            sql.AppendLine("		,calle ");

            return GetDirecciones(sql);
        }

        public static List<clsItemDireccion> GetDireccionesTR(string[] arrSolicitudes)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("DECLARE @tbl TABLE(id int)");
            sql.AppendLine("");

            foreach (string id in arrSolicitudes)
            {
                sql.AppendLine(string.Format("INSERT INTO @tbl VALUES({0})", id));
            }
            sql.AppendLine("");

            sql.AppendLine("SELECT * FROM ( ");
            sql.AppendLine("	SELECT DISTINCT ");
            sql.AppendLine("		sol.id_solicitud, ");
            sql.AppendLine("		CASE  ");
            sql.AppendLine("			WHEN tubic.id_tipoubicacion in (SELECT TiposDeUbicacion.id_tipoubicacion FROM TiposDeUbicacion WHERE dbo.TiposDeUbicacion.descripcion_tipoubicacion in ('Objeto territorial','Parcela común'))");
            sql.AppendLine("			THEN ");
            sql.AppendLine("				IsNull(encpuer.nombre_calle,'')  ");
            sql.AppendLine("			ELSE ");
            sql.AppendLine("				tubic.descripcion_tipoubicacion + ' ' +	stubic.descripcion_subtipoubicacion ");
            sql.AppendLine("		END as calle, ");
            sql.AppendLine("		'puerta'  = CASE   ");
            sql.AppendLine("			WHEN tubic.id_tipoubicacion = (SELECT TiposDeUbicacion.id_tipoubicacion FROM TiposDeUbicacion WHERE dbo.TiposDeUbicacion.descripcion_tipoubicacion = 'Objeto territorial') ");
            sql.AppendLine("			THEN CONVERT(nvarchar(20),encpuer.nropuerta)+'t' ");
            sql.AppendLine("			WHEN tubic.id_tipoubicacion = 0  THEN IsNull(convert(nvarchar, encpuer.nropuerta),'') ");
            sql.AppendLine("			ELSE ");
            sql.AppendLine("				IsNull('Local ' + encubic.local_subtipoubicacion,'') ");
            sql.AppendLine("		END ");
            sql.AppendLine("	FROM ");
            sql.AppendLine("		Transf_Solicitudes sol ");
            sql.AppendLine("		INNER JOIN CPadron_Ubicaciones encubic ON sol.id_cpadron = encubic.id_cpadron ");
            sql.AppendLine("		INNER JOIN SubTiposDeUbicacion stubic ON encubic.id_subtipoubicacion = stubic.id_subtipoubicacion ");
            sql.AppendLine("		INNER JOIN TiposDeUbicacion tubic ON stubic.id_tipoubicacion = tubic.id_tipoubicacion ");
            sql.AppendLine("		LEFT JOIN CPadron_Ubicaciones_Puertas encpuer ON encubic.id_cpadronubicacion = encpuer.id_cpadronubicacion  ");
            sql.AppendLine("		LEFT JOIN CPadron_Ubicaciones_PropiedadHorizontal encphor ON encubic.id_cpadronubicacion = encphor.id_cpadronubicacion  ");
            sql.AppendLine("		LEFT JOIN Ubicaciones_PropiedadHorizontal phor ON encphor.id_propiedadhorizontal = phor.id_propiedadhorizontal ");
            sql.AppendLine("		INNER JOIN @tbl tmp ON sol.id_solicitud = tmp.id");
            sql.AppendLine("	) as con ");
            sql.AppendLine("	ORDER BY ");
            sql.AppendLine("		id_solicitud ");
            sql.AppendLine("		,calle ");

            return GetDirecciones(sql);
        }

        public static List<clsItemDireccion> GetDireccionesTRNuevas(string[] arrSolicitudes)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("DECLARE @tbl TABLE(id int)");
            sql.AppendLine("");

            foreach (string id in arrSolicitudes)
            {
                sql.AppendLine(string.Format("INSERT INTO @tbl VALUES({0})", id));
            }
            sql.AppendLine("");

            sql.AppendLine("SELECT * FROM ( ");
            sql.AppendLine("	SELECT DISTINCT ");
            sql.AppendLine("		sol.id_solicitud, ");
            sql.AppendLine("		CASE  ");
            sql.AppendLine("			WHEN tubic.id_tipoubicacion in (SELECT TiposDeUbicacion.id_tipoubicacion FROM TiposDeUbicacion WHERE dbo.TiposDeUbicacion.descripcion_tipoubicacion in ('Objeto territorial','Parcela común'))");
            sql.AppendLine("			THEN ");
            sql.AppendLine("				IsNull(encpuer.nombre_calle,'')  ");
            sql.AppendLine("			ELSE ");
            sql.AppendLine("				tubic.descripcion_tipoubicacion + ' ' +	stubic.descripcion_subtipoubicacion ");
            sql.AppendLine("		END as calle, ");
            sql.AppendLine("		'puerta'  = CASE   ");
            sql.AppendLine("			WHEN tubic.id_tipoubicacion = (SELECT TiposDeUbicacion.id_tipoubicacion FROM TiposDeUbicacion WHERE dbo.TiposDeUbicacion.descripcion_tipoubicacion = 'Objeto territorial') ");
            sql.AppendLine("			THEN CONVERT(nvarchar(20),encpuer.nropuerta)+'t' ");
            sql.AppendLine("			WHEN tubic.id_tipoubicacion = 0  THEN IsNull(convert(nvarchar, encpuer.nropuerta),'') ");
            sql.AppendLine("			ELSE ");
            sql.AppendLine("				IsNull('Local ' + encubic.local_subtipoubicacion,'') ");
            sql.AppendLine("		END ");
            sql.AppendLine("	FROM ");
            sql.AppendLine("		Transf_Solicitudes sol ");
            sql.AppendLine("		INNER JOIN Transf_Ubicaciones encubic ON sol.id_solicitud = encubic.id_solicitud ");
            sql.AppendLine("		INNER JOIN SubTiposDeUbicacion stubic ON encubic.id_subtipoubicacion = stubic.id_subtipoubicacion ");
            sql.AppendLine("		INNER JOIN TiposDeUbicacion tubic ON stubic.id_tipoubicacion = tubic.id_tipoubicacion ");
            sql.AppendLine("		LEFT JOIN Transf_Ubicaciones_Puertas encpuer ON encubic.id_transfubicacion = encpuer.id_transfubicacion  ");
            sql.AppendLine("		LEFT JOIN Transf_Ubicaciones_PropiedadHorizontal encphor ON encubic.id_transfubicacion = encphor.id_transfubicacion  ");
            sql.AppendLine("		LEFT JOIN Ubicaciones_PropiedadHorizontal phor ON encphor.id_propiedadhorizontal = phor.id_propiedadhorizontal ");
            sql.AppendLine("		INNER JOIN @tbl tmp ON sol.id_solicitud = tmp.id");
            sql.AppendLine("	) as con ");
            sql.AppendLine("	ORDER BY ");
            sql.AppendLine("		id_solicitud ");
            sql.AppendLine("		,calle ");

            return GetDirecciones(sql);
        }

        public static List<clsItemDireccion> GetDirecciones(StringBuilder sql)
        {
            DGHP_Entities db = new DGHP_Entities();

            List<clsItemPuerta> lstPuertas = db.Database.SqlQuery<clsItemPuerta>(sql.ToString()).ToList();

            List<clsItemDireccion> lstDirecciones = new List<clsItemDireccion>();

            int id_solicitud_ant = 0;
            string calle_ant = "";
            string Direccion_armada = "";

            if (lstPuertas.Count > 0)
            {
                id_solicitud_ant = lstPuertas[0].id_solicitud;
                calle_ant = lstPuertas[0].calle;
            }

            foreach (var puerta in lstPuertas)
            {
                if (id_solicitud_ant != puerta.id_solicitud)
                {
                    clsItemDireccion itemDireccion = new clsItemDireccion();
                    itemDireccion.id_solicitud = id_solicitud_ant;
                    itemDireccion.direccion = Direccion_armada;
                    lstDirecciones.Add(itemDireccion);
                    Direccion_armada = "";
                    id_solicitud_ant = puerta.id_solicitud;
                }

                if (Direccion_armada.Length == 0 || puerta.calle != calle_ant)
                {
                    if (Direccion_armada.Length > 0)
                        Direccion_armada += " -";

                    Direccion_armada += puerta.calle + " " + puerta.puerta;
                }
                else
                {
                    Direccion_armada += " / " + puerta.puerta;
                }

                calle_ant = puerta.calle;


            }

            if (Direccion_armada.Length > 0)
            {
                clsItemDireccion itemDireccion = new clsItemDireccion();
                itemDireccion.id_solicitud = id_solicitud_ant;
                itemDireccion.direccion = Direccion_armada;
                lstDirecciones.Add(itemDireccion);
                Direccion_armada = "";
            }

            db.Dispose();

            return lstDirecciones;
        }

        public static bool esUbicacionEspecialConObjetoTerritorial(int? id_subtipoubicacion)
        {
            using (var db = new DGHP_Entities())
            {
                var ot = db.SubTiposDeUbicacion
                     .Where(x => x.id_tipoubicacion == (int)Constants.TiposDeUbicacion.ObjetoTerritorial)
                     .Select(x => x.id_subtipoubicacion).ToList();

                return ot.Contains(id_subtipoubicacion ?? default(int));
            }
        }


        public static bool esUbicacionEspecialConObjetoTerritorial(int id_ubicacion)
        {
            using (var db = new DGHP_Entities())
            {
                var sbUbicacionesOT = db.SubTiposDeUbicacion
                         .Where(x => x.id_tipoubicacion == (int)Constants.TiposDeUbicacion.ObjetoTerritorial)
                         .Select(x => x.id_subtipoubicacion).ToList();

                var result = from ubi in db.Ubicaciones
                             join sub in db.SubTiposDeUbicacion on ubi.id_subtipoubicacion equals sub.id_subtipoubicacion
                             where (sbUbicacionesOT.Contains(sub.id_subtipoubicacion) && (ubi.id_ubicacion == id_ubicacion))
                             select new
                             {
                                 ubi.id_ubicacion
                             };

                return result.Count() > 0;
            }
        }



        public partial class clsMenu : SGI_Menues
        {
            public List<clsMenu> Submenues;
        }

        public class ItemUbicacion
        {
            public int id_ubicacion { get; set; }
            public Nullable<int> partida { get; set; }
            public Nullable<int> seccion { get; set; }
            public string manzana { get; set; }
            public string parcela { get; set; }
            public Nullable<int> id_subtipoubicacion { get; set; }
            public Nullable<int> id_tipoubicacion { get; set; }
            public string inhibida { get; set; }
            public Nullable<int> id_estado_modif_ubi { get; set; }
            public string baja_logica { get; set; }
            public int codigo_calle { get; set; }
            public int nro_puerta { get; set; }
            public string nombre_calle { get; set; }
            public string direccion { get; set; }
            public int id_zona { get; set; }
        }

        public class ItemPartidas
        {
            public int id_ubicacion { get; set; }
            public Nullable<int> partida { get; set; }
            public Nullable<int> seccion { get; set; }
            public string manzana { get; set; }
            public string parcela { get; set; }
            public Nullable<int> id_subtipoubicacion { get; set; }
            public Nullable<int> id_tipoubicacion { get; set; }
            public string inhibida { get; set; }
            public Nullable<int> id_estado_modif_ubi { get; set; }
            public string baja_logica { get; set; }
            public int codigo_calle { get; set; }
            public int nro_puerta { get; set; }
        }
        public class ItemPartidasAux
        {
            public int id_ubicacion { get; set; }
            public Nullable<int> partida { get; set; }
            public Nullable<int> seccion { get; set; }
            public string manzana { get; set; }
            public string parcela { get; set; }
            public string direccion { get; set; }
            public Nullable<int> id_subtipoubicacion { get; set; }
            public Nullable<int> id_tipoubicacion { get; set; }
            public string inhibida { get; set; }
            public Nullable<int> id_estado_modif_ubi { get; set; }
            public string baja_logica { get; set; }
            public int codigo_calle { get; set; }
            public int nro_puerta { get; set; }

        }
        public class ItemSolicitudesAux
        {
            public int id_solAct { get; set; }
            public Nullable<int> partidaAct { get; set; }
            public string tipoPartidaAct { get; set; }
            public int id_tipoSolActI { get; set; }
            public string id_tipoSolAct { get; set; }
            public string direccionAct { get; set; }
            public string obsAct { get; set; }
            public string id_estado_sol { get; set; }
            public int id_estado_sol_id { get; set; }
            public int idUbis { get; set; }
            public int? Seccion { get; set; }
            public string Manzana { get; set; }
            public string Parcela { get; set; }


        }

        public class ZonasPlaneamientoAux
        {
            public int id_zonaplaneamiento { get; set; }
            public string CodZonaPla { get; set; }
            public string descripcion { get; set; }


        }
        public class ItemZonasComp
        {
            public int id_zonaplaneamiento { get; set; }
            public string tipoUbi { get; set; }



        }


        public class ItemPartidasHorizontales
        {
            public Nullable<int> partidaHor { get; set; }
            public string pisoHor { get; set; }


            public string deptoHor { get; set; }
            public int id_propiedadhorizontal { get; set; }
            public int id_estado_modif_ubi_h { get; set; }
            public int id_ubicacion { get; set; }
            public string baja_logica { get; set; }




        }
        public partial class clsGrillaUsuarios
        {
            public Guid UserId { get; set; }
            public string UserName { get; set; }
            public string Apellido { get; set; }
            public string Nombres { get; set; }
            public string UserName_SADE { get; set; }
            public string Reparticion_SADE { get; set; }
            public string Sector_SADE { get; set; }
            public string Email { get; set; }
            public bool Bloqueado { get; set; }
            public ICollection<SGI_Perfiles> Perfiles { get; set; }
            public ICollection<aspnet_Roles> Roles { get; set; }
            public string Perfiles_1Linea { get; set; }
        }


        public partial class UsuariosExportacion
        {
            public string Usuario { get; set; }
            public string Apellido { get; set; }
            public string Nombres { get; set; }
            public string Usuario_SADE { get; set; }
            public string Reparticion_SADE { get; set; }
            public string Sector_SADE { get; set; }
            public string Email { get; set; }
            public string Bloqueado { get; set; }
            public string ListaPerfiles { get; set; }
            public DateTime UltimaConexion { get; set; }
            public ICollection<SGI_Perfiles> Perfiles { get; set; }
        }


        public static List<ItemZona> getZonas(string[] arrUbicaciones, String tipozona)
        {
            // Obtiene las direcciones en una sola linea de una matriz de solicitudes

            DGHP_Entities db = new DGHP_Entities();

            var qz = (from ubicacioneszonascomplementarias in db.Ubicaciones_ZonasComplementarias
                      join zonasplaneamiento in db.Zonas_Planeamiento on ubicacioneszonascomplementarias.id_zonaplaneamiento equals zonasplaneamiento.id_zonaplaneamiento
                      select new ItemZona
                      {
                          CodZonaPla = zonasplaneamiento.CodZonaPla,
                          tipo_ubicacion = ubicacioneszonascomplementarias.tipo_ubicacion,
                          id_ubicacion = ubicacioneszonascomplementarias.id_ubicacion
                      }).ToList();
            var itemz = qz.Where(x => x.tipo_ubicacion == tipozona && arrUbicaciones.Contains(Convert.ToString(x.id_ubicacion))).ToList();

            db.Dispose();

            return itemz;

        }

        public static string GetNroExpediente(int id_tramitetarea)
        {
            string ret = "";
            DGHP_Entities db = new DGHP_Entities();

            var tt = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            if (tt != null)
            {

                if (tt.SGI_Tramites_Tareas_HAB.Count > 0)
                {
                    ret = tt.SGI_Tramites_Tareas_HAB.FirstOrDefault().SSIT_Solicitudes.NroExpedienteSade;
                }
                else if (tt.SGI_Tramites_Tareas_CPADRON.Count > 0)
                {
                    ret = tt.SGI_Tramites_Tareas_CPADRON.FirstOrDefault().CPadron_Solicitudes.NroExpedienteSade;
                }
                else if (tt.SGI_Tramites_Tareas_TRANSF.Count > 0)
                {
                    ret = tt.SGI_Tramites_Tareas_TRANSF.FirstOrDefault().Transf_Solicitudes.NroExpedienteSade;
                }

            }

            db.Dispose();

            return ret;

        }

        public static int GetGruposDeTramite(int id_tramitetarea)
        {
            int ret = 0;
            DGHP_Entities db = new DGHP_Entities();

            var tt = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            if (tt != null)
            {

                if (tt.SGI_Tramites_Tareas_HAB.Count > 0)
                    ret = (int)Constants.GruposDeTramite.HAB;
                else if (tt.SGI_Tramites_Tareas_CPADRON.Count > 0)
                    ret = (int)Constants.GruposDeTramite.CP;
                else if (tt.SGI_Tramites_Tareas_TRANSF.Count > 0)
                    ret = (int)Constants.GruposDeTramite.TR;
            }
            db.Dispose();
            return ret;
        }

        public static int GetGrupoCircuito(int id_solicitud)
        {
            int ret = 0;
            DGHP_Entities db = new DGHP_Entities();

            int id_encomienda = GetUltimaEncomiendaAprobada(Constants.GruposDeTramite.HAB, id_solicitud);

            ret = (from enc in db.Encomienda
                   join encrub in db.Encomienda_RubrosCN on enc.id_encomienda equals encrub.id_encomienda
                   join rub in db.RubrosCN on encrub.IdRubro equals rub.IdRubro
                   join grucir in db.ENG_Grupos_Circuitos on rub.IdGrupoCircuito equals grucir.id_grupo_circuito
                   where enc.id_encomienda == id_encomienda
                   orderby grucir.prioridad descending
                   select grucir.id_grupo_circuito).FirstOrDefault();

            db.Dispose();
            return ret;
        }

        public static int GetUltimaEncomiendaAprobada(Constants.GruposDeTramite GrupoTramite, int id_solicitud)
        {

            DGHP_Entities db = new DGHP_Entities();

            int id_encomienda = 0;

            if (GrupoTramite == Constants.GruposDeTramite.TR)
            {
                id_encomienda = (from enc in db.Encomienda
                                 join ets in db.Encomienda_Transf_Solicitudes on enc.id_encomienda equals ets.id_encomienda
                                 where ets.id_solicitud == id_solicitud && enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                                 orderby enc.id_encomienda descending
                                 select ets.id_encomienda).FirstOrDefault();
            }
            else if (GrupoTramite == Constants.GruposDeTramite.HAB)
            {
                id_encomienda = (from enc in db.Encomienda
                                 join ets in db.Encomienda_SSIT_Solicitudes on enc.id_encomienda equals ets.id_encomienda
                                 where ets.id_solicitud == id_solicitud && enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                                 orderby enc.id_encomienda descending
                                 select ets.id_encomienda).FirstOrDefault();
            }

            db.Dispose();
            return id_encomienda;
        }



    }

    public class ItemZonificacionParcelasAux
    {
        public int id_ubicacion { get; set; }
        public Nullable<int> partidaMatriz { get; set; }
        public Nullable<int> seccion { get; set; }
        public string manzana { get; set; }
        public string parcela { get; set; }
        public Nullable<DateTime> vigenciaHasta { get; set; }
        public int codigoCalle { get; set; }
        public int nroPuerta { get; set; }
        public string direccion { get; set; }
        public string zona1 { get; set; }
        public string zona2 { get; set; }
        public string zona3 { get; set; }
    }

    public class ItemZonificacionParcelas
    {
        public int id_ubicacion { get; set; }
        public Nullable<int> partidaMatriz { get; set; }
        public Nullable<int> seccion { get; set; }
        public string manzana { get; set; }
        public string parcela { get; set; }
        public string codZonaPla { get; set; }
        public string zona1 { get; set; }
        public string zona2 { get; set; }
        public string zona3 { get; set; }
        public string direccion { get; set; }
    }

    public partial class ItemDireccionDGFYCO
    {
        public int id_dgubicacion { get; set; }
        public string direccion { get; set; }

    }

    public partial class ItemDireccion
    {
        public int id_ubicacion { get; set; }
        public string direccion { get; set; }

    }
    public partial class ItemEstadosSolicitud
    {
        public int id_estado { get; set; }
        public string nombre_estado { get; set; }
        /*EnProceso = 0,
         Anulada = 1,
         Confirmada = 2,
         Aprobada = 3,
         Rechazada = 4*/

    }

    public partial class ItemZona
    {
        public string CodZonaPla { get; set; }
        public string tipo_ubicacion { get; set; }
        public int id_ubicacion { get; set; }

    }

    public class clsProfesionalGrupoConsejo
    {
        public int id_profesional { get; set; }
        public string consejoProfesional { get; set; }
    }

}