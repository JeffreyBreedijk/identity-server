using System;
using System.Collections.Generic;
using System.Text;
using CQRSlite.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UtilizeJwtProvider.Domain.Event.Data;

namespace UtilizeJwtProvider.Domain.Event
{
    public static class ExtendsEventData
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
	
        public static EventData ToEventData(this object @event, string aggregateType, Guid aggregateId, int version)
        {
            var data = JsonConvert.SerializeObject(@event, SerializerSettings);
            var eventHeaders = new Dictionary<string, object>
            {
                {
                    "EventClrType", @event.GetType().AssemblyQualifiedName
                }
            };
            var metadata = JsonConvert.SerializeObject(eventHeaders, SerializerSettings);
            var eventId = Guid.NewGuid();
	
            return new EventData
            {
                Id = eventId,
                Created = DateTime.UtcNow,
                AggregateType = aggregateType,
                AggregateId = aggregateId,
                Version = version,
                Event = data,
                Metadata = metadata,
            };
        }
        
        public static IEvent DeserializeEvent(this EventData x)
        {
            var eventClrTypeName = JObject.Parse(x.Metadata).Property("EventClrType").Value;
            return JsonConvert.DeserializeObject(x.Event, Type.GetType((string)eventClrTypeName)) as IEvent;
           
        }
    }
}