﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Este código fuente fue generado automáticamente por wsdl, Versión=4.6.81.0.
// 
namespace ws_ssit
{
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "WSssitSoap", Namespace = "http://tempuri.org/")]
    public partial class WSssit : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback generarDocInicioTramiteOperationCompleted;

        private System.Threading.SendOrPostCallback enviarEncuestaOperationCompleted;

        /// <remarks/>
        public WSssit()
        {
            this.Url = "http://www.dghpsh.agcontrol.gob.ar/test/ssit.agcontrol.gob.ar/WSssit.asmx";
        }

        /// <remarks/>
        public event generarDocInicioTramiteCompletedEventHandler generarDocInicioTramiteCompleted;

        /// <remarks/>
        public event enviarEncuestaCompletedEventHandler enviarEncuestaCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/generarDocInicioTramite", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool generarDocInicioTramite(string user, string pass, int id_solicitud)
        {
            object[] results = this.Invoke("generarDocInicioTramite", new object[] {
                        user,
                        pass,
                        id_solicitud});
            return ((bool)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BegingenerarDocInicioTramite(string user, string pass, int id_solicitud, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("generarDocInicioTramite", new object[] {
                        user,
                        pass,
                        id_solicitud}, callback, asyncState);
        }

        /// <remarks/>
        public bool EndgenerarDocInicioTramite(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }

        /// <remarks/>
        public void generarDocInicioTramiteAsync(string user, string pass, int id_solicitud)
        {
            this.generarDocInicioTramiteAsync(user, pass, id_solicitud, null);
        }

        /// <remarks/>
        public void generarDocInicioTramiteAsync(string user, string pass, int id_solicitud, object userState)
        {
            if ((this.generarDocInicioTramiteOperationCompleted == null))
            {
                this.generarDocInicioTramiteOperationCompleted = new System.Threading.SendOrPostCallback(this.OngenerarDocInicioTramiteOperationCompleted);
            }
            this.InvokeAsync("generarDocInicioTramite", new object[] {
                        user,
                        pass,
                        id_solicitud}, this.generarDocInicioTramiteOperationCompleted, userState);
        }

        private void OngenerarDocInicioTramiteOperationCompleted(object arg)
        {
            if ((this.generarDocInicioTramiteCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.generarDocInicioTramiteCompleted(this, new generarDocInicioTramiteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/enviarEncuesta", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool enviarEncuesta(string user, string pass, int id_solicitud)
        {
            object[] results = this.Invoke("enviarEncuesta", new object[] {
                        user,
                        pass,
                        id_solicitud});
            return ((bool)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginenviarEncuesta(string user, string pass, int id_solicitud, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("enviarEncuesta", new object[] {
                        user,
                        pass,
                        id_solicitud}, callback, asyncState);
        }

        /// <remarks/>
        public bool EndenviarEncuesta(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }

        /// <remarks/>
        public void enviarEncuestaAsync(string user, string pass, int id_solicitud)
        {
            this.enviarEncuestaAsync(user, pass, id_solicitud, null);
        }

        /// <remarks/>
        public void enviarEncuestaAsync(string user, string pass, int id_solicitud, object userState)
        {
            if ((this.enviarEncuestaOperationCompleted == null))
            {
                this.enviarEncuestaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnenviarEncuestaOperationCompleted);
            }
            this.InvokeAsync("enviarEncuesta", new object[] {
                        user,
                        pass,
                        id_solicitud}, this.enviarEncuestaOperationCompleted, userState);
        }

        private void OnenviarEncuestaOperationCompleted(object arg)
        {
            if ((this.enviarEncuestaCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.enviarEncuestaCompleted(this, new enviarEncuestaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.81.0")]
    public delegate void generarDocInicioTramiteCompletedEventHandler(object sender, generarDocInicioTramiteCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class generarDocInicioTramiteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal generarDocInicioTramiteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public bool Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.81.0")]
    public delegate void enviarEncuestaCompletedEventHandler(object sender, enviarEncuestaCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class enviarEncuestaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal enviarEncuestaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public bool Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
}