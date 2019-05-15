﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AlarmEventService.EventServiceRef {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="EventServiceRef.IEventServiceOperations")]
    public interface IEventServiceOperations {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEventServiceOperations/AddEvent", ReplyAction="http://tempuri.org/IEventServiceOperations/AddEventResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ScadaCommon.Database.Event))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ScadaCommon.Database.Event[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ScadaCommon.AlarmEventType))]
        bool AddEvent(object newEvent);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEventServiceOperations/AddEvent", ReplyAction="http://tempuri.org/IEventServiceOperations/AddEventResponse")]
        System.Threading.Tasks.Task<bool> AddEventAsync(object newEvent);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEventServiceOperations/GetAllEvents", ReplyAction="http://tempuri.org/IEventServiceOperations/GetAllEventsResponse")]
        ScadaCommon.Database.Event[] GetAllEvents();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEventServiceOperations/GetAllEvents", ReplyAction="http://tempuri.org/IEventServiceOperations/GetAllEventsResponse")]
        System.Threading.Tasks.Task<ScadaCommon.Database.Event[]> GetAllEventsAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEventServiceOperationsChannel : AlarmEventService.EventServiceRef.IEventServiceOperations, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EventServiceOperationsClient : System.ServiceModel.ClientBase<AlarmEventService.EventServiceRef.IEventServiceOperations>, AlarmEventService.EventServiceRef.IEventServiceOperations {
        
        public EventServiceOperationsClient() {
        }
        
        public EventServiceOperationsClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EventServiceOperationsClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EventServiceOperationsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EventServiceOperationsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool AddEvent(object newEvent) {
            return base.Channel.AddEvent(newEvent);
        }
        
        public System.Threading.Tasks.Task<bool> AddEventAsync(object newEvent) {
            return base.Channel.AddEventAsync(newEvent);
        }
        
        public ScadaCommon.Database.Event[] GetAllEvents() {
            return base.Channel.GetAllEvents();
        }
        
        public System.Threading.Tasks.Task<ScadaCommon.Database.Event[]> GetAllEventsAsync() {
            return base.Channel.GetAllEventsAsync();
        }
    }
}
