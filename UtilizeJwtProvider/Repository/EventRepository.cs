using System;
using System.Linq;
using CQRSlite.Domain;
using UtilizeJwtProvider.Domain.Event;
using UtilizeJwtProvider.Services;

namespace UtilizeJwtProvider.Repository
{
    public interface IEventRepository
    {
        void Save(AggregateRoot aggregate);
        T GetById<T>(Guid id) where T : AggregateRoot;
    }

    public class EventRepository : IEventRepository
    {
        private readonly IAggregateFactory _factory;

        public EventRepository(IAggregateFactory factory)
        {
            _factory = factory;
        }

        public void Save(AggregateRoot aggregate)
        {
            var events = aggregate.FlushUncommitedChanges();
            if (events.Any() == false)
                return;
            var aggregateType = aggregate.GetType().Name;
            var originalVersion = aggregate.Version - events.Count() + 1;
            var eventsToSave = events
                .Select(e => e.ToEventData(aggregateType, aggregate.Id, originalVersion++))
                .ToArray();

            using (var context = new ApplicationDbContext())
            {
                context.Events.AddRange(eventsToSave);
                context.SaveChanges();
            }
       
        }

        public T GetById<T>(Guid id) where T : AggregateRoot
        {
            using (var context = new ApplicationDbContext())
            {

                var events = context.Events.Where(e => e.AggregateId.Equals(id)).ToList();
                var aggregate = _factory.Create<T>(events);
                return aggregate;
            }
        }
        
        
    }
}