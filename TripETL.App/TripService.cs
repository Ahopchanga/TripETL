using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TripETL.Domain.Entities;
using TripETL.Domain.Interfaces;

namespace TripETL.Data;

public class TripService : ITripService
{
    private readonly ITripRepository _repository;
    
    public TripService(ITripRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<Trip>> ReadCsvAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            IgnoreBlankLines = true,
            HeaderValidated = null,
            MissingFieldFound = null
        });

        var records = new List<Trip>();

        await csv.ReadAsync();
        csv.ReadHeader();

        while (await csv.ReadAsync())
        {
            var pickupDateStr = csv.GetField<string>("tpep_pickup_datetime");
            var pickupDate = DateTime.ParseExact(pickupDateStr, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            var dropoffDateStr = csv.GetField<string>("tpep_dropoff_datetime");
            var dropoffDate = DateTime.ParseExact(dropoffDateStr, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            
            var record = new Trip
            {
                TpepPickupDatetime = pickupDate,
                TpepDropoffDatetime = dropoffDate,
                TripDistance = csv.GetField<decimal>("trip_distance"),
                StoreAndFwdFlag = csv.GetField<string>("store_and_fwd_flag"),
                PULocationId = csv.GetField<int>("PULocationID"),
                DOLocationId = csv.GetField<int>("DOLocationID"),
                FareAmount = csv.GetField<decimal>("fare_amount"),
                TipAmount = csv.GetField<decimal>("tip_amount"),
            };

            if (csv.TryGetField<int>("passenger_count", out var passengerCount))
            {
                record.PassengerCount = passengerCount;
            }

            records.Add(record);
        }

        return records;
    }

    public async Task LoadDataAsync(IEnumerable<Trip> trips)
    {
        //Removing duplicate entries based on pickup/dropoff datetime and passenger count
        var distinctTrips = trips
            .GroupBy(trip => new 
            { 
                trip.TpepPickupDatetime, 
                trip.TpepDropoffDatetime, 
                trip.PassengerCount 
            })
            .Select(group => group.First());

        await _repository.BulkInsertAsync(distinctTrips);
    }

    public async Task<string> GetLocationWithHighestTipAmountAsync()
    {
        var trips = await _repository.GetAllAsync();

        return trips.GroupBy(trip => trip.PULocationId)
            .Select(group => new
            {
                PULocationId = group.Key,
                AverageTipAmount = group.Average(trip => trip.TipAmount)
            })
            .OrderByDescending(group => group.AverageTipAmount)
            .Select(group => group.PULocationId)
            .First()
            .ToString();
    }

    public async Task<IEnumerable<Trip>> GetTopLongestTripsByDistanceAsync(int top)
    {
        return await _repository.GetLongestDistancesAsync(top);
    }

    public async Task<IEnumerable<Trip>> GetTopLongestTripsByTimeSpentAsync(int top)
    {
        return await _repository.GetLongestTimesAsync(top);
    }

    public async Task<IEnumerable<Trip>> SearchTripsByPickupLocationIdAsync(int pickupId)
    {
        return await _repository.SearchByPickupIdAsync(pickupId);
    }

    public async Task RemoveWhitespaceInStringFieldsAsync()
    {
        await _repository.RemoveWhitespaceInStringFieldsAsync();
    }
}