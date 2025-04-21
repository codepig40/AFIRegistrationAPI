using System;
using System.Collections.Generic;

namespace AFIRegistrationAPI.Models;

public partial class Policy
{
    public int PolicyId { get; set; }

    public string PolicyReference { get; set; } = null!;

    public int IsActive { get; set; }

    public int? CustomerId { get; set; }
}
