using TripETL.Domain.Interfaces;

namespace TripETL.Domain.Entities;

public class Trip : IEntity
{
    public int Id { get; set; }
    public DateTime TpepPickupDatetime { get; set; }
    public DateTime TpepDropoffDatetime { get; set; }
    public int PassengerCount { get; set; }
    public decimal TripDistance { get; set; }
    public string StoreAndFwdFlag { get; set; }
    public int PULocationId { get; set; }
    public int DOLocationId { get; set; }
    public decimal FareAmount { get; set; }
    public decimal TipAmount { get; set; }
}