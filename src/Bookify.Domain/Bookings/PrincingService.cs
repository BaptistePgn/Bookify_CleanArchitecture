using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

public class PrincingService
{
    public PrincingDetails CalculatePrice(Apartment apartment, DateRange period)
    {
        var currency = apartment.Price.Currency;

        var priceForPeriod = new Money(
            apartment.Price.Amount * period.LenghtInDays,
            currency);

        decimal percantageUpCharge = 0;
        foreach (var amenity in apartment.Amenities)
        {
            percantageUpCharge += amenity switch
            {
                Amenity.GardenView or Amenity.MountainView => 0.05m,
                Amenity.AirConditioning => 0.01m,
                Amenity.Parking => 0.01m,
                _ => 0,
            };
        }

        var amenitiesUpCharge = Money.Zero(currency);
        if (percantageUpCharge > 0)
        {
            amenitiesUpCharge = new Money(
                priceForPeriod.Amount * percantageUpCharge,
                currency);
        }

        var totalPrice = Money.Zero();

        totalPrice += priceForPeriod;

        if (!apartment.CleaningFee.IsZero())
        {
            totalPrice += apartment.CleaningFee;
        }

        totalPrice += amenitiesUpCharge;

        return new PrincingDetails(
            priceForPeriod,
            apartment.CleaningFee,
            amenitiesUpCharge,
            totalPrice);
    }
}
