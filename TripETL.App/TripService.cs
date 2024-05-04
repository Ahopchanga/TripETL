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
        var records = new List<Trip>();

        try
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, GetCsvConfiguration());

            await csv.ReadAsync();
            csv.ReadHeader();
            while (await csv.ReadAsync())
            {
                var record = CreateTripRecordFromCsv(csv);
                records.Add(record);
            }
        }
        catch(Exception ex)
        {
            throw new ApplicationException($"Error in {nameof(ReadCsvAsync)}: " + ex.Message);
        }

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
    
    private static DateTime ParseDate(string dateStr)
    {
        if(DateTime.TryParseExact(dateStr, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date.ToUniversalTime();
        }
        else
        {
            throw new ArgumentException($"Invalid date format: {dateStr}");
        }
    }

    private Trip CreateTripRecordFromCsv(CsvReader csv)
    {
        try
        {
            var pickupDate = ParseDate(csv.GetField<string>("tpep_pickup_datetime"));
            var dropoffDate = ParseDate(csv.GetField<string>("tpep_dropoff_datetime"));
            var storeAndFwdFlag = ConvertToReadableFlag(csv.GetField<string>("store_and_fwd_flag"));

            int.TryParse(csv.GetField<string>("passenger_count"), out var passengerCount);
            decimal.TryParse(csv.GetField<string>("trip_distance"), out var tripDistance);
            int.TryParse(csv.GetField<string>("PULocationID"), out var puLocationId);
            int.TryParse(csv.GetField<string>("DOLocationID"), out var doLocationId);
            decimal.TryParse(csv.GetField<string>("fare_amount"), out var fareAmount);
            decimal.TryParse(csv.GetField<string>("tip_amount"), out var tipAmount);

            return new Trip
            {
                TpepPickupDatetime = pickupDate,
                TpepDropoffDatetime = dropoffDate,
                TripDistance = tripDistance,
                StoreAndFwdFlag = storeAndFwdFlag,
                PULocationId = puLocationId,
                DOLocationId = doLocationId,
                FareAmount = fareAmount,
                TipAmount = tipAmount,
                PassengerCount = passengerCount
            };
        }
        catch(Exception ex)
        {
            throw new ApplicationException($"Error in {nameof(CreateTripRecordFromCsv)}: " + ex.Message);
        }
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