using TripETL.Domain.Entities;

namespace TripETL.Domain.Interfaces;

public interface ITripService
{
    Task<IEnumerable<Trip>> ReadCsvAsync(string filePath);
    Task LoadDataAsync(IEnumerable<Trip> trips);
    Task<string> GetLocationWithHighestTipAmountAsync();
    Task<IEnumerable<Trip>> GetTopLongestTripsByDistanceAsync(int top);
    Task<IEnumerable<Trip>> GetTopLongestTripsByTimeSpentAsync(int top);
    Task<IEnumerable<Trip>> SearchTripsByPickupLocationIdAsync(int pickupId);
    Task RemoveWhitespaceInStringFieldsAsync();
}