using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Models
{
    public class TaxPayer
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+(?:\s[a-zA-Z]+)+$", ErrorMessage = "Full name must contain at least two words separated by space.")]
        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Gross income must be a valid number.")]
        public decimal GrossIncome { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Charity spent must be a valid number.")]
        public decimal? CharitySpent { get; set; }

        [Required]
        [RegularExpression(@"^\d{5,10}$", ErrorMessage = "SSN must be a valid 5 to 10 digits number.")]
        public string SSN { get; set; }
    }
}
