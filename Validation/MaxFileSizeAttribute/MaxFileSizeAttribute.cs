using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Attributes.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class MaxFileSizeAttribute : ValidationAttribute, IClientValidatable
	{
		public enum Units { Byte, Kilobyte, Megabyte, Gigabyte }

		private readonly Dictionary<Units, string> _unitNames = new Dictionary<Units, string>
			{
				{Units.Byte, "b"},
				{Units.Kilobyte, "Kb"},
				{Units.Megabyte, "Mb"},
				{Units.Gigabyte, "Gb"}
			};


		private readonly long _maxFileSize;

		/// <summary>
		/// FormatUnit is used for displaying user friendly file size value
		/// </summary>
		public Units FormatUnit { get; set; }

		/// <summary>
		/// MaxFileSizeAttribute constructor
		/// </summary>
		/// <param name="maxFileSize">Maximum file size in bytes</param>
		public MaxFileSizeAttribute(long maxFileSize)
		{
			FormatUnit = Units.Byte;
			_maxFileSize = maxFileSize;
		}

		public override bool IsValid(object value)
		{
			var file = value as HttpPostedFileBase;
			if (file == null)
			{
				return false;
			}
			return file.ContentLength <= _maxFileSize;
		}

		public override string FormatErrorMessage(string name)
		{
			const double _1024 = 1024d;
			double value = _maxFileSize;

			switch (FormatUnit)
			{
				case Units.Byte:
					break;
				case Units.Kilobyte:
					value = value / _1024;
					break;
				case Units.Megabyte:
					value = value / Math.Pow(_1024, 2);
					break;
				case Units.Gigabyte:
					value = value / Math.Pow(_1024, 3);
					break;
			}

			string size = string.Format("{0} {1}", Math.Round(value), _unitNames[FormatUnit]);
			return string.Format(ErrorMessageString, name, size);
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			var rule = new ModelClientValidationRule
			{
				ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
				ValidationType = "maxfilesize"
			};

			rule.ValidationParameters["value"] = _maxFileSize;
			yield return rule;
		}
	}
}