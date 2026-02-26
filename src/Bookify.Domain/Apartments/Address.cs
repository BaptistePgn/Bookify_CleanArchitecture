namespace Bookify.Domain.Apartements;

public record class Address(
    string Country,
    string State,
    string ZipCode,
    string City,
    string Street);
