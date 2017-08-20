using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Domain;
using CQRSlite.Events;
using UtilizeJwtProvider.Domain.Aggregates;
using UtilizeJwtProvider.Domain.Event;
using UtilizeJwtProvider.Domain.Event.Data;

namespace UtilizeJwtProvider.Services
{
    public interface IAggregateFactory
    {
        T Create<T>(IEnumerable<EventData> eventList) where T : AggregateRoot;
    }

    public class AggregateFactory : IAggregateFactory
    {
       
        
        public T Create<T>(IEnumerable<EventData> eventList) where T : AggregateRoot
        {
            var eventDatas = eventList as IList<EventData> ?? eventList.ToList();
             var events = eventDatas.Select(e => e.DeserializeEvent()).OrderBy(e => e.Version).ToList();
            var obj = (T) Activator.CreateInstance(typeof(T));
            obj.LoadFromHistory(events);
            return obj;
        }

       

    }
}