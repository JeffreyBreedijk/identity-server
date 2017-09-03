using System;
using System.Linq;
using CQRSlite.Domain;
using UtilizeJwtProvider.DataSources;
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
        private readonly EventDbContext _dbContext;

        public EventRepository(EventDbContext eventDbContext, IAggregateFactory factory)
        {
            _dbContext = eventDbContext;
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


            _dbContext.Events.AddRange(eventsToSave);
            _dbContext.SaveChanges();
        }

        public T GetById<T>(Guid id) where T : AggregateRoot
        {
            var events = _dbContext.Events.Where(e => e.AggregateId.Equals(id)).ToList();
            var aggregate = _factory.Create<T>(events);
            return aggregate;
        }
    }
}