using SecureFlight.Core.Entities;
using SecureFlight.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureFlight.Core.Services
{
    public class FlightService : BaseService<Flight> , IFlightService
    {
        private readonly IRepository<Flight> _repository;
        private readonly IRepository<Passenger> _passengerRepository;

        public FlightService(IRepository<Flight> repository, IRepository<Passenger> passengerRepository) : base(repository)
        {
            _repository = repository;
            _passengerRepository = passengerRepository;
        }

        public async Task<OperationResult<Flight>> AddPassengerToFlight(long flightId, string passengerId)
        {
            var flight = await _repository.GetByIdAsync(flightId);
            if(flight is null)
            {
                return OperationResult<Flight>.NotFound($"Flight with ID {flightId} was not found");
            }
            var flightContainsPassenger = flight.Passengers.Any(p => p.Id == passengerId);
            if(flightContainsPassenger)
            {
               return OperationResult<Flight>.Error("Passenger is already booked on this flight");
            }
            flight.PassengerFlights.Add(new PassengerFlight
            {
                FlightId = flightId,
                PassengerId = passengerId
            });
            var passenger = await _passengerRepository.GetByIdAsync(passengerId);
            flight.Passengers.Add(passenger);

            _repository.Update(flight);
            return  OperationResult<Flight>.Success(flight);
        }

        public async Task<OperationResult<Flight>> RemovePassengerToFlight(long flightId, string passengerId)
        {
            var flight = await _repository.GetByIdAsync(flightId);
            if (flight is null)
            {
                return OperationResult<Flight>.NotFound($"Flight with ID {flightId} was not found");
            }
            var flightContainsPassenger = flight.Passengers.Any(p => p.Id == passengerId);
            if (flightContainsPassenger)
            {
                return OperationResult<Flight>.Error("Passenger is already booked on this flight");
            }
            flight.PassengerFlights.Remove(new PassengerFlight
            {
                FlightId = flightId,
                PassengerId = passengerId
            });
            var passenger = await _passengerRepository.GetByIdAsync(passengerId);
            flight.Passengers.Remove(passenger);

            _repository.Update(flight);
            return OperationResult<Flight>.Success(flight);
        }
    }
}
