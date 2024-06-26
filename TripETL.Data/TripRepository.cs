﻿using Microsoft.EntityFrameworkCore;
using TripETL.Domain.Entities;
using TripETL.Domain.Interfaces;
using Z.EntityFramework.Plus;

namespace TripETL.Data
{
    public class TripRepository : ITripRepository
    {
    
        private readonly TripDbContext _dbContext;
        
        public TripRepository(TripDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Trip> GetByIdAsync(int id)
        {
            return await _dbContext.Trips.FindAsync(id);
        }

        public async Task<IEnumerable<Trip>> GetAllAsync()
        {
            return await _dbContext.Trips.ToListAsync();
        }

        public async Task AddAsync(Trip entity)
        {
            await _dbContext.Trips.AddAsync(entity);
            await _dbContext.SaveChangesAsync();    
        }

        public async Task UpdateAsync(Trip entity)
        {
            _dbContext.Trips.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trip = await GetByIdAsync(id);
            _dbContext.Trips.Remove(trip);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Trip> GetByDetailsAsync(DateTime pickup, DateTime dropoff, int passengerCount)
        {
            return await _dbContext.Trips.FirstOrDefaultAsync(
                trip => trip.TpepPickupDatetime == pickup && 
                        trip.TpepDropoffDatetime == dropoff && 
                        trip.PassengerCount == passengerCount);
        }

        public async Task<IEnumerable<Trip>> GetLongestDistancesAsync(int count)
        {
            return await _dbContext.Trips
                .OrderByDescending(trip => trip.TripDistance)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Trip>> GetLongestTimesAsync(int count)
        {
            return await _dbContext.Trips
                .OrderByDescending(trip => trip.TpepDropoffDatetime - trip.TpepPickupDatetime)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Trip>> SearchByPickupIdAsync(int pickupId)
        {
            return await _dbContext.Trips
                .Where(trip => trip.PULocationId == pickupId)
                .ToListAsync();
        }

        public async Task BulkInsertAsync(IEnumerable<Trip> trips)
        {
            await _dbContext.Trips.AddRangeAsync(trips);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task RemoveWhitespaceInStringFieldsAsync()
        {
            var tripsWithWhitespace = _dbContext.Trips
                .Where(t => t.StoreAndFwdFlag.Contains(" "));

            await tripsWithWhitespace
                .UpdateAsync(t => new Trip { StoreAndFwdFlag = t.StoreAndFwdFlag.Trim() });
        }
        
        public async Task<string> GetLocationWithHighestTipAmountAsync()
        {
            var locationWithHighestTipAmount = await _dbContext.Trips
                .GroupBy(trip => trip.PULocationId)
                .Select(group => new
                {
                    PULocationId = group.Key,
                    AverageTipAmount = group.Average(trip => trip.TipAmount)
                })
                .OrderByDescending(group => group.AverageTipAmount)
                .FirstAsync();

            return locationWithHighestTipAmount.PULocationId.ToString();
        }
    }
}