﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.17929
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Este código fuente fue generado automáticamente por wsdl, Versión=4.0.30319.17929.
// 
namespace SGI.Webservices.SADE.documentos_oficiales {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="IAdministracionDeDocumentosOficialesServiceSoapBinding", Namespace="http://ar.gob.gcaba.ee.services.external/")]
    public partial class IAdministracionDeDocumentosOficialesService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback desvincularDocumentosOficialesNumeroEspecialOperationCompleted;
        
        private System.Threading.SendOrPostCallback hacerDefinitivosDocsDeEEOperationCompleted;
        
        private System.Threading.SendOrPostCallback vincularDocumentosOficialesAEEGuardaTemporalOperationCompleted;
        
        private System.Threading.SendOrPostCallback vincularDocumentosOficialesConTransaccionFCOperationCompleted;
        
        private System.Threading.SendOrPostCallback vincularDocumentosOficialesOperationCompleted;
        
        private System.Threading.SendOrPostCallback vincularDocumentosOficialesNumeroEspecialOperationCompleted;
        
        private System.Threading.SendOrPostCallback eliminarDocumentosNoDefinitivosOperationCompleted;
        
        private System.Threading.SendOrPostCallback desvincularDocumentosOficialesOperationCompleted;
        
        /// <remarks/>
        public IAdministracionDeDocumentosOficialesService() {
            this.Url = "http://sade-mule.hml.gcba.gob.ar/EEServices/documentos-oficiales";
        }
        
        /// <remarks/>
        public event desvincularDocumentosOficialesNumeroEspecialCompletedEventHandler desvincularDocumentosOficialesNumeroEspecialCompleted;
        
        /// <remarks/>
        public event hacerDefinitivosDocsDeEECompletedEventHandler hacerDefinitivosDocsDeEECompleted;
        
        /// <remarks/>
        public event vincularDocumentosOficialesAEEGuardaTemporalCompletedEventHandler vincularDocumentosOficialesAEEGuardaTemporalCompleted;
        
        /// <remarks/>
        public event vincularDocumentosOficialesConTransaccionFCCompletedEventHandler vincularDocumentosOficialesConTransaccionFCCompleted;
        
        /// <remarks/>
        public event vincularDocumentosOficialesCompletedEventHandler vincularDocumentosOficialesCompleted;
        
        /// <remarks/>
        public event vincularDocumentosOficialesNumeroEspecialCompletedEventHandler vincularDocumentosOficialesNumeroEspecialCompleted;
        
        /// <remarks/>
        public event eliminarDocumentosNoDefinitivosCompletedEventHandler eliminarDocumentosNoDefinitivosCompleted;
        
        /// <remarks/>
        public event desvincularDocumentosOficialesCompletedEventHandler desvincularDocumentosOficialesCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://ar.gob.gcaba.ee.services.external/", ResponseNamespace="http://ar.gob.gcaba.ee.services.external/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void desvincularDocumentosOficialesNumeroEspecial([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] requestRelacionDocumentoOficialEE request) {
            this.Invoke("desvincularDocumentosOficialesNumeroEspecial", new object[] {
                        request});
        }
        
        /// <remarks/>
        public System.IAsyncResult BegindesvincularDocumentosOficialesNumeroEspecial(requestRelacionDocumentoOficialEE request, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("desvincularDocumentosOficialesNumeroEspecial", new object[] {
                        request}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EnddesvincularDocumentosOficialesNumeroEspecial(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        public void desvincularDocumentosOficialesNumeroEspecialAsync(requestRelacionDocumentoOficialEE request) {
            this.desvincularDocumentosOficialesNumeroEspecialAsync(request, null);
        }
        
        /// <remarks/>
        public void desvincularDocumentosOficialesNumeroEspecialAsync(requestRelacionDocumentoOficialEE request, object userState) {
            if ((this.desvincularDocumentosOficialesNumeroEspecialOperationCompleted == null)) {
                this.desvincularDocumentosOficialesNumeroEspecialOperationCompleted = new System.Threading.SendOrPostCallback(this.OndesvincularDocumentosOficialesNumeroEspecialOperationCompleted);
            }
            this.InvokeAsync("desvincularDocumentosOficialesNumeroEspecial", new object[] {
                        request}, this.desvincularDocumentosOficialesNumeroEspecialOperationCompleted, userState);
        }
        
        private void OndesvincularDocumentosOficialesNumeroEspecialOperationCompleted(object arg) {
            if ((this.desvincularDocumentosOficialesNumeroEspecialCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.desvincularDocumentosOficialesNumeroEspecialCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://ar.gob.gcaba.ee.services.external/", ResponseNamespace="http://ar.gob.gcaba.ee.services.external/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void hacerDefinitivosDocsDeEE([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] vinculacionDefinitivaDeDocsRequest request) {
            this.Invoke("hacerDefinitivosDocsDeEE", new object[] {
                        request});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginhacerDefinitivosDocsDeEE(vinculacionDefinitivaDeDocsRequest request, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("hacerDefinitivosDocsDeEE", new object[] {
                        request}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndhacerDefinitivosDocsDeEE(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        public void hacerDefinitivosDocsDeEEAsync(vinculacionDefinitivaDeDocsRequest request) {
            this.hacerDefinitivosDocsDeEEAsync(request, null);
        }
        
        /// <remarks/>
        public void hacerDefinitivosDocsDeEEAsync(vinculacionDefinitivaDeDocsRequest request, object userState) {
            if ((this.hacerDefinitivosDocsDeEEOperationCompleted == null)) {
                this.hacerDefinitivosDocsDeEEOperationCompleted = new System.Threading.SendOrPostCallback(this.OnhacerDefinitivosDocsDeEEOperationCompleted);
            }
            this.InvokeAsync("hacerDefinitivosDocsDeEE", new object[] {
                        request}, this.hacerDefinitivosDocsDeEEOperationCompleted, userState);
        }
        
        private void OnhacerDefinitivosDocsDeEEOperationCompleted(object arg) {
            if ((this.hacerDefinitivosDocsDeEECompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.hacerDefinitivosDocsDeEECompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://ar.gob.gcaba.ee.services.external/", ResponseNamespace="http://ar.gob.gcaba.ee.services.external/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void vincularDocumentosOficialesAEEGuardaTemporal([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string sistemaUsuario, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string usuario, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string codigoEE, [System.Xml.Serialization.XmlElementAttribute("listaDocumentos", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string[] listaDocumentos) {
            this.Invoke("vincularDocumentosOficialesAEEGuardaTemporal", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE,
                        listaDocumentos});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginvincularDocumentosOficialesAEEGuardaTemporal(string sistemaUsuario, string usuario, string codigoEE, string[] listaDocumentos, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("vincularDocumentosOficialesAEEGuardaTemporal", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE,
                        listaDocumentos}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndvincularDocumentosOficialesAEEGuardaTemporal(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        public void vincularDocumentosOficialesAEEGuardaTemporalAsync(string sistemaUsuario, string usuario, string codigoEE, string[] listaDocumentos) {
            this.vincularDocumentosOficialesAEEGuardaTemporalAsync(sistemaUsuario, usuario, codigoEE, listaDocumentos, null);
        }
        
        /// <remarks/>
        public void vincularDocumentosOficialesAEEGuardaTemporalAsync(string sistemaUsuario, string usuario, string codigoEE, string[] listaDocumentos, object userState) {
            if ((this.vincularDocumentosOficialesAEEGuardaTemporalOperationCompleted == null)) {
                this.vincularDocumentosOficialesAEEGuardaTemporalOperationCompleted = new System.Threading.SendOrPostCallback(this.OnvincularDocumentosOficialesAEEGuardaTemporalOperationCompleted);
            }
            this.InvokeAsync("vincularDocumentosOficialesAEEGuardaTemporal", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE,
                        listaDocumentos}, this.vincularDocumentosOficialesAEEGuardaTemporalOperationCompleted, userState);
        }
        
        private void OnvincularDocumentosOficialesAEEGuardaTemporalOperationCompleted(object arg) {
            if ((this.vincularDocumentosOficialesAEEGuardaTemporalCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.vincularDocumentosOficialesAEEGuardaTemporalCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://ar.gob.gcaba.ee.services.external/", ResponseNamespace="http://ar.gob.gcaba.ee.services.external/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void vincularDocumentosOficialesConTransaccionFC([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string sistemaUsuario, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string usuario, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string codigoEE, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string documento, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] int idTransaccionFC, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] [System.Xml.Serialization.XmlIgnoreAttribute()] bool idTransaccionFCSpecified) {
            this.Invoke("vincularDocumentosOficialesConTransaccionFC", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE,
                        documento,
                        idTransaccionFC,
                        idTransaccionFCSpecified});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginvincularDocumentosOficialesConTransaccionFC(string sistemaUsuario, string usuario, string codigoEE, string documento, int idTransaccionFC, bool idTransaccionFCSpecified, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("vincularDocumentosOficialesConTransaccionFC", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE,
                        documento,
                        idTransaccionFC,
                        idTransaccionFCSpecified}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndvincularDocumentosOficialesConTransaccionFC(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        public void vincularDocumentosOficialesConTransaccionFCAsync(string sistemaUsuario, string usuario, string codigoEE, string documento, int idTransaccionFC, bool idTransaccionFCSpecified) {
            this.vincularDocumentosOficialesConTransaccionFCAsync(sistemaUsuario, usuario, codigoEE, documento, idTransaccionFC, idTransaccionFCSpecified, null);
        }
        
        /// <remarks/>
        public void vincularDocumentosOficialesConTransaccionFCAsync(string sistemaUsuario, string usuario, string codigoEE, string documento, int idTransaccionFC, bool idTransaccionFCSpecified, object userState) {
            if ((this.vincularDocumentosOficialesConTransaccionFCOperationCompleted == null)) {
                this.vincularDocumentosOficialesConTransaccionFCOperationCompleted = new System.Threading.SendOrPostCallback(this.OnvincularDocumentosOficialesConTransaccionFCOperationCompleted);
            }
            this.InvokeAsync("vincularDocumentosOficialesConTransaccionFC", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE,
                        documento,
                        idTransaccionFC,
                        idTransaccionFCSpecified}, this.vincularDocumentosOficialesConTransaccionFCOperationCompleted, userState);
        }
        
        private void OnvincularDocumentosOficialesConTransaccionFCOperationCompleted(object arg) {
            if ((this.vincularDocumentosOficialesConTransaccionFCCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.vincularDocumentosOficialesConTransaccionFCCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://ar.gob.gcaba.ee.services.external/", ResponseNamespace="http://ar.gob.gcaba.ee.services.external/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void vincularDocumentosOficiales([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string sistemaUsuario, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string usuario, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string codigoEE, [System.Xml.Serialization.XmlElementAttribute("listaDocumentos", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string[] listaDocumentos) {
            this.Invoke("vincularDocumentosOficiales", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE,
                        listaDocumentos});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginvincularDocumentosOficiales(string sistemaUsuario, string usuario, string codigoEE, string[] listaDocumentos, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("vincularDocumentosOficiales", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE,
                        listaDocumentos}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndvincularDocumentosOficiales(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        public void vincularDocumentosOficialesAsync(string sistemaUsuario, string usuario, string codigoEE, string[] listaDocumentos) {
            this.vincularDocumentosOficialesAsync(sistemaUsuario, usuario, codigoEE, listaDocumentos, null);
        }
        
        /// <remarks/>
        public void vincularDocumentosOficialesAsync(string sistemaUsuario, string usuario, string codigoEE, string[] listaDocumentos, object userState) {
            if ((this.vincularDocumentosOficialesOperationCompleted == null)) {
                this.vincularDocumentosOficialesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnvincularDocumentosOficialesOperationCompleted);
            }
            this.InvokeAsync("vincularDocumentosOficiales", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE,
                        listaDocumentos}, this.vincularDocumentosOficialesOperationCompleted, userState);
        }
        
        private void OnvincularDocumentosOficialesOperationCompleted(object arg) {
            if ((this.vincularDocumentosOficialesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.vincularDocumentosOficialesCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://ar.gob.gcaba.ee.services.external/", ResponseNamespace="http://ar.gob.gcaba.ee.services.external/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void vincularDocumentosOficialesNumeroEspecial([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] requestRelacionDocumentoOficialEE request) {
            this.Invoke("vincularDocumentosOficialesNumeroEspecial", new object[] {
                        request});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginvincularDocumentosOficialesNumeroEspecial(requestRelacionDocumentoOficialEE request, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("vincularDocumentosOficialesNumeroEspecial", new object[] {
                        request}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndvincularDocumentosOficialesNumeroEspecial(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        public void vincularDocumentosOficialesNumeroEspecialAsync(requestRelacionDocumentoOficialEE request) {
            this.vincularDocumentosOficialesNumeroEspecialAsync(request, null);
        }
        
        /// <remarks/>
        public void vincularDocumentosOficialesNumeroEspecialAsync(requestRelacionDocumentoOficialEE request, object userState) {
            if ((this.vincularDocumentosOficialesNumeroEspecialOperationCompleted == null)) {
                this.vincularDocumentosOficialesNumeroEspecialOperationCompleted = new System.Threading.SendOrPostCallback(this.OnvincularDocumentosOficialesNumeroEspecialOperationCompleted);
            }
            this.InvokeAsync("vincularDocumentosOficialesNumeroEspecial", new object[] {
                        request}, this.vincularDocumentosOficialesNumeroEspecialOperationCompleted, userState);
        }
        
        private void OnvincularDocumentosOficialesNumeroEspecialOperationCompleted(object arg) {
            if ((this.vincularDocumentosOficialesNumeroEspecialCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.vincularDocumentosOficialesNumeroEspecialCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://ar.gob.gcaba.ee.services.external/", ResponseNamespace="http://ar.gob.gcaba.ee.services.external/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void eliminarDocumentosNoDefinitivos([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string sistemaUsuario, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string usuario, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string codigoEE) {
            this.Invoke("eliminarDocumentosNoDefinitivos", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE});
        }
        
        /// <remarks/>
        public System.IAsyncResult BegineliminarDocumentosNoDefinitivos(string sistemaUsuario, string usuario, string codigoEE, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("eliminarDocumentosNoDefinitivos", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndeliminarDocumentosNoDefinitivos(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        public void eliminarDocumentosNoDefinitivosAsync(string sistemaUsuario, string usuario, string codigoEE) {
            this.eliminarDocumentosNoDefinitivosAsync(sistemaUsuario, usuario, codigoEE, null);
        }
        
        /// <remarks/>
        public void eliminarDocumentosNoDefinitivosAsync(string sistemaUsuario, string usuario, string codigoEE, object userState) {
            if ((this.eliminarDocumentosNoDefinitivosOperationCompleted == null)) {
                this.eliminarDocumentosNoDefinitivosOperationCompleted = new System.Threading.SendOrPostCallback(this.OneliminarDocumentosNoDefinitivosOperationCompleted);
            }
            this.InvokeAsync("eliminarDocumentosNoDefinitivos", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE}, this.eliminarDocumentosNoDefinitivosOperationCompleted, userState);
        }
        
        private void OneliminarDocumentosNoDefinitivosOperationCompleted(object arg) {
            if ((this.eliminarDocumentosNoDefinitivosCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.eliminarDocumentosNoDefinitivosCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://ar.gob.gcaba.ee.services.external/", ResponseNamespace="http://ar.gob.gcaba.ee.services.external/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void desvincularDocumentosOficiales([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string sistemaUsuario, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string usuario, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string codigoEE, [System.Xml.Serialization.XmlElementAttribute("listaDocumentos", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string[] listaDocumentos) {
            this.Invoke("desvincularDocumentosOficiales", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE,
                        listaDocumentos});
        }
        
        /// <remarks/>
        public System.IAsyncResult BegindesvincularDocumentosOficiales(string sistemaUsuario, string usuario, string codigoEE, string[] listaDocumentos, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("desvincularDocumentosOficiales", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE,
                        listaDocumentos}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EnddesvincularDocumentosOficiales(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        public void desvincularDocumentosOficialesAsync(string sistemaUsuario, string usuario, string codigoEE, string[] listaDocumentos) {
            this.desvincularDocumentosOficialesAsync(sistemaUsuario, usuario, codigoEE, listaDocumentos, null);
        }
        
        /// <remarks/>
        public void desvincularDocumentosOficialesAsync(string sistemaUsuario, string usuario, string codigoEE, string[] listaDocumentos, object userState) {
            if ((this.desvincularDocumentosOficialesOperationCompleted == null)) {
                this.desvincularDocumentosOficialesOperationCompleted = new System.Threading.SendOrPostCallback(this.OndesvincularDocumentosOficialesOperationCompleted);
            }
            this.InvokeAsync("desvincularDocumentosOficiales", new object[] {
                        sistemaUsuario,
                        usuario,
                        codigoEE,
                        listaDocumentos}, this.desvincularDocumentosOficialesOperationCompleted, userState);
        }
        
        private void OndesvincularDocumentosOficialesOperationCompleted(object arg) {
            if ((this.desvincularDocumentosOficialesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.desvincularDocumentosOficialesCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ar.gob.gcaba.ee.services.external/")]
    public partial class requestRelacionDocumentoOficialEE {
        
        private string[] documentosOficialesField;
        
        private string numeroExpedienteElectronicoField;
        
        private string sistemaUsuarioField;
        
        private string usuarioField;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("documentosOficiales", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] documentosOficiales {
            get {
                return this.documentosOficialesField;
            }
            set {
                this.documentosOficialesField = value;
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string numeroExpedienteElectronico {
            get {
                return this.numeroExpedienteElectronicoField;
            }
            set {
                this.numeroExpedienteElectronicoField = value;
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sistemaUsuario {
            get {
                return this.sistemaUsuarioField;
            }
            set {
                this.sistemaUsuarioField = value;
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string usuario {
            get {
                return this.usuarioField;
            }
            set {
                this.usuarioField = value;
            }
        }
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ar.gob.gcaba.ee.services.external/")]
    public partial class vinculacionDefinitivaDeDocsRequest {
        
        private string codigoEEField;
        
        private string sistemaUsuarioField;
        
        private string usuarioField;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string codigoEE {
            get {
                return this.codigoEEField;
            }
            set {
                this.codigoEEField = value;
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sistemaUsuario {
            get {
                return this.sistemaUsuarioField;
            }
            set {
                this.sistemaUsuarioField = value;
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string usuario {
            get {
                return this.usuarioField;
            }
            set {
                this.usuarioField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void desvincularDocumentosOficialesNumeroEspecialCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void hacerDefinitivosDocsDeEECompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void vincularDocumentosOficialesAEEGuardaTemporalCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void vincularDocumentosOficialesConTransaccionFCCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void vincularDocumentosOficialesCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void vincularDocumentosOficialesNumeroEspecialCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void eliminarDocumentosNoDefinitivosCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
    public delegate void desvincularDocumentosOficialesCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}
