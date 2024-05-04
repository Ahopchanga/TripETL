using TripETL.Domain.Entities;

namespace TripETL.Domain.Interfaces;

public interface ITripRepository : IRepository<Trip>
{
    Task<Trip> GetByDetailsAsync(DateTime pickup, DateTime dropoff, int passengerCount);
    Task<IEnumerable<Trip>> GetLongestDistancesAsync(int count);
    Task<IEnumerable<Trip>> GetLongestTimesAsync(int count);
    Task<IEnumerable<Trip>> SearchByPickupIdAsync(int pickupId);
    Task BulkInsertAsync(IEnumerable<Trip> trips);
    Task RemoveWhitespaceInStringFieldsAsync();
    Task<string> GetLocationWithHighestTipAmountAsync();
}