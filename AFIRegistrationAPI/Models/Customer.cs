using System;
using System.Collections.Generic;

namespace AFIRegistrationAPI.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerFirstName { get; set; } = null!;

    public string CustomerLastName { get; set; } = null!;

    public int CustomerTitle { get; set; }

    public string? CustomerDateOfBirth { get; set; }

    public string? CustomerEmail { get; set; }
}
