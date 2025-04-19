using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AFIRegistrationAPI.Models;

[RequireEmailOrDobAttribute]
public class RegisterCustomer
{
    public int CustomerId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 50 characters.")]
    public string CustomerFirstName { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Surname must be between 3 and 50 characters.")]
    public string CustomerLastName { get; set; } 

    public int CustomerTitle { get; set; }

    [CustomValidation(typeof(RegisterCustomer), nameof(ValidateDateOfBirth))]
    public string? CustomerDateOfBirth { get; set; }
    
    [CustomValidation(typeof(RegisterCustomer), nameof(ValidateEmail))]
    public string? CustomerEmail { get; set; }

    [Required]
    [RegularExpression(@"^[A-Z]{2}-\d{6}$", ErrorMessage = "Policy Reference must be in format XX-999999.")]
    public string PolicyReference { get; set; }


    // Age check: must be 18+ if DOB is provided
    public static ValidationResult? ValidateDateOfBirth(string? value, ValidationContext context)
    {
        if (string.IsNullOrEmpty(value))
        {
            return ValidationResult.Success;
        }

        if (!DateTime.TryParseExact(
        value,
        new[] { "yyyy-MM-dd", "dd/MM/yyyy" },
        CultureInfo.InvariantCulture,
        DateTimeStyles.None,
        out DateTime dob))
        {
            return new ValidationResult("Date of birth must be a valid date in yyyy-MM-dd or dd/MM/yyyy format.");
        }

        var age = DateTime.Today.Year - dob.Year;
        if (dob.Date > DateTime.Today.AddYears(-age)) age--;

        if (age < 18)
        {
            return new ValidationResult("Policy holder must be at least 18 years old.");
        }

        return ValidationResult.Success;
    }

    // Custom Email Pattern
    public static ValidationResult? ValidateEmail(string? email, ValidationContext context)
    {
        if (string.IsNullOrWhiteSpace(email))
            return ValidationResult.Success; // Optional

        var pattern = @"^[a-zA-Z0-9]{4,}@[a-zA-Z0-9]{2,}\.(com|co\.uk)$";
        if (!Regex.IsMatch(email, pattern))
        {
            return new ValidationResult("Invalid email format. It must be at least 4 characters before '@', 2 after, and end in .com or .co.uk.");
        }
        return ValidationResult.Success;
    }


    public class RequireEmailOrDobAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var model = (RegisterCustomer)validationContext.ObjectInstance;

            if (string.IsNullOrWhiteSpace(model.CustomerEmail) && model.CustomerDateOfBirth == null)
            {
                return new ValidationResult("Either Email or Date of Birth must be provided.");
            }

            return ValidationResult.Success;
        }
    }
}
