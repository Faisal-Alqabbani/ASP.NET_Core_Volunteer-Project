using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CSharpProject.Models
{
    public class PastDateAttribute : ValidationAttribute
    {
        
    protected override ValidationResult IsValid(object v, ValidationContext validationContext)
    {
        DateTime date =Convert.ToDateTime(v);
        // CultureInfo enUS = new CultureInfo("en-US");
        // DateTime date = DateTime.ParseExact((string)v,"dd/MM/yyyy", enUS.DateTimeFormat );
        // Console.WriteLine(date);
        if(date <= DateTime.Now){
            return new ValidationResult("The date must be in the future!");
        }
        else{
            return ValidationResult.Success;
        }
    }

    }
}