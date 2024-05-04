using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TripETL.Domain.Entities;
using TripETL.Domain.Interfaces;

namespace TripETL.App;

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
        using var csv = new CsvReader(reader, GetCsvConfiguration());

        var records = await ParseRecords(csv);

        return records;
    }

    private static CsvConfiguration GetCsvConfiguration()
    {
        return new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            IgnoreBlankLines = true,
            HeaderValidated = null,
            MissingFieldFound = null
        };
    }

    private static async Task<List<Trip>> ParseRecords(CsvReader csv)
    {
        var records = new List<Trip>();

        await csv.ReadAsync();
        csv.ReadHeader();

        while (await csv.ReadAsync())
        {
            records.Add(CreateTripRecordFromCsv(csv));
        }

        return records;
    }

    private static Trip CreateTripRecordFromCsv(CsvReader csv)
    {
        var pickupDate = ConvertToUtc(csv.GetField<string>("tpep_pickup_datetime"));
        var dropoffDate = ConvertToUtc(csv.GetField<string>("tpep_dropoff_datetime"));
        var storeAndFwdFlag = ConvertToReadableFlag(csv.GetField<string>("store_and_fwd_flag"));
    
        var record = new Trip
        {
            TpepPickupDatetime = pickupDate,
            TpepDropoffDatetime = dropoffDate,
            TripDistance = csv.GetField<decimal>("trip_distance"),
            StoreAndFwdFlag = storeAndFwdFlag,
            PULocationId = csv.GetField<int>("PULocationID"),
            DOLocationId = csv.GetField<int>("DOLocationID"),
            FareAmount = csv.GetField<decimal>("fare_amount"),
            TipAmount = csv.GetField<decimal>("tip_amount"),
        };

        if (csv.TryGetField<int>("passenger_count", out var passengerCount))
        {
            record.PassengerCount = passengerCount;
        }

        return record;
    }

    private static DateTime ConvertToUtc(string dateStr)
    {
        var date = DateTime.ParseExact(dateStr, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
        return date.ToUniversalTime();
    }

    private static string ConvertToReadableFlag(string flag)
    {
        return flag == "N" ? "No" : "Yes";
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
        return await _repository.GetLocationWithHighestTipAmountAsync();
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