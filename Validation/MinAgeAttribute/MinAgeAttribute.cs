using System;
using System.ComponentModel.DataAnnotations;

namespace Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class MinAgeAttribute : ValidationAttribute
	{
		private readonly int _minAge;

		public MinAgeAttribute(int minAge)
		{
			_minAge = minAge;
		}


		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is DateTime)
			{
				DateTime birthDate = (DateTime) value;
				DateTime maxDate = DateTime.Today.AddYears(-_minAge);

				if (maxDate < birthDate)
					return new ValidationResult(String.Format("You must be at least {0} years old", _minAge));
			}

			return ValidationResult.Success;
		}
	}
}