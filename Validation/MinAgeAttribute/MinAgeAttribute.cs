using System;
using System.ComponentModel.DataAnnotations;

namespace Attributes.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class MinAgeAttribute : ValidationAttribute, IClientValidatable
	{
		private readonly int _minAge;

		public MinAgeAttribute(int minAge)
		{
			_minAge = minAge;
		}


		public override bool IsValid(object value)
		{
			bool isValid = true;

			if (value is DateTime)
			{
				DateTime birthDate = (DateTime)value;
				DateTime maxDate = DateTime.Today.AddYears(-_minAge);

				if (maxDate < birthDate)
				{
					isValid = false;
				}
			}

			return isValid;
		}


		public override string FormatErrorMessage(string name)
		{
			return string.Format(ErrorMessageString, name, _minAge);
		}


		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			var rule = new ModelClientValidationRule
			{
				ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
				ValidationType = "minage"
			};

			rule.ValidationParameters["value"] = _minAge;
			yield return rule;
		}
	}
}