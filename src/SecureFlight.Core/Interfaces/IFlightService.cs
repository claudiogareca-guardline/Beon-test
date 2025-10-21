using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureFlight.Core.Interfaces
{
    public interface IFlightService : IService<Entities.Flight>
    {
        Task<OperationResult<Entities.Flight>> AddPassengerToFlight(long flightId, string passengerId);
        Task<OperationResult<Entities.Flight>> RemovePassengerToFlight(long flightId, string passengerId);
    }
}
