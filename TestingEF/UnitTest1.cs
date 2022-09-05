using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SGI.Model;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace TestingEF
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ObtenerFuncionesImportadasEnEF()
        {
            using (DGHP_Entities context = new DGHP_Entities())
            {
                var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;
                
                List<EdmFunction> functions = (from func in metadata.GetItems<EdmFunction>(DataSpace.SSpace)
                                               where func.ReturnParameter == null
                                               select func).ToList();

                //var metadata2 = metadata.GetItemCollection(DataSpace.SSpace);
                //foreach (var item in metadata2)
                //{
                //    foreach (var itemMeta in item.MetadataProperties)
                //    {
                //        if (itemMeta.Name == "PruebasEntityFWK")
                //        {
                //            int p = 0;
                //        }
                //    }
                //}
                               

                int Total = 0;
                int Totalerror = 0;
                foreach (var function in functions)
                {                    
                    if (function.Name == null || function.Name == "")
                    {
                        Totalerror++;
                    }
                    else
                    {
                        Total++;

                    }                    
                }
            }

        }

        [TestMethod]
        public void TestMethod1()
        {
            //		BuiltInTypeKind	EdmFunction	System.Data.Entity.Core.Metadata.Edm.BuiltInTypeKind
            using (DGHP_Entities context = new DGHP_Entities())
            {
               
                var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;
                var Functions = metadata.GetItemCollection(DataSpace.SSpace)
                    .GetItems<EdmFunction>()
                    //.Single()
                    //.BaseEntitySets.OfType<EntitySet>()
                    .Where(s => (!s.MetadataProperties.Contains("Type")
                        //|| s.MetadataProperties["Type"].ToString() == "EdmFunction"
                    )
                    //& s.NamespaceName == "DGHP_Model.Store"                    
                    )
                    ;

                //var tables = metadata.GetItemCollection(DataSpace.SSpace)
                //  .GetItems<EdmFunction>()
                //    //.Single()
                //    //.BaseEntitySets.OfType<EntitySet>()
                //  .Where(s => !s.MetadataProperties.Contains("Type")
                //  || s.MetadataProperties["Type"].ToString() == "Tables")
                //  ;


                int Total = 0;
                int Totalerror = 0;
                foreach (var function in Functions)
                {
                    //var tableName = function.MetadataProperties.Contains("Table") & function.MetadataProperties["Table"].Value != null
                    //    ? function.MetadataProperties["Table"].Value.ToString()
                    //    : function.Name;

                    if (function.Name == null || function.Name == "")
                    {
                        Totalerror++;
                    }
                    else
                    {
                        Total++;

                    }
                    //var tableSchema = function.MetadataProperties["Schema"].Value.ToString();
                    //Console.WriteLine(tableSchema + "." + function.Name);
                }
            }




        }
    }



}
