using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.DataLayer
{
    public class DataLayerBase
    {
        protected static DGHP_Entities db = null;
        protected static AGC_FilesEntities dbFiles = null;

        #region Entidades
        protected static void IniciarEntity()
        {
            if (db == null)
            {
                db = new DGHP_Entities();
            }
        }

        protected static void FinalizarEntity()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }

        protected static void IniciarEntityFiles()
        {
            if (dbFiles == null)
            {
                dbFiles = new AGC_FilesEntities();
            }
        }

        protected static void FinalizarEntityFiles()
        {
            if (dbFiles != null)
            {
                dbFiles.Dispose();
            }
        }
        #endregion
    }
}