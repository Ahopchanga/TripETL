using System.ComponentModel.DataAnnotations.Schema;
using TripETL.Domain.Interfaces;

namespace TripETL.Domain.Entities;

public class Trip : IEntity
{
    [Column("Id")]
    public int Id { get; set; }

    [Column("tpep_pickup_datetime")]
    public DateTime TpepPickupDatetime { get; set; }
    
    [Column("tpep_dropoff_datetime")]
    public DateTime TpepDropoffDatetime { get; set; }
    
    [Column("passenger_count")]
    public int PassengerCount { get; set; }
    
    [Column("trip_distance")]
    public decimal TripDistance { get; set; }
    
    [Column("store_and_fwd_flag")]
    public string StoreAndFwdFlag { get; set; }
    
    [Column("PULocationID")]
    public int PULocationId { get; set; }

    [Column("DOLocationID")]
    public int DOLocationId { get; set; }
    
    [Column("fare_amount")]
    public decimal FareAmount { get; set; }
    
    [Column("tip_amount")]
    public decimal TipAmount { get; set; }
}