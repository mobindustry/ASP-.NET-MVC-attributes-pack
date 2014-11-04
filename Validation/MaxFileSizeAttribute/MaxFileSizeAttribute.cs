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
		public enum Units
		{
			Byte = 1,
			Kilobyte = 1024,
			Megabyte = 1024*1024,
			Gigabyte = 1024*1024*1024
		}

		private readonly Dictionary<Units, string> _unitNames = new Dictionary<Units, string>
			{
				{Units.Byte, "b"},
				{Units.Kilobyte, "Kb"},
				{Units.Megabyte, "Mb"},
				{Units.Gigabyte, "Gb"}
			};


		private readonly long _maxFileSize;
		private Units? _formatUnit;

		/// <summary>
		/// FormatUnit is used for displaying user friendly file size value. If not specified detected automatically
		/// </summary>
		public Units FormatUnit
		{
			get
			{
				if (!_formatUnit.HasValue)
				{
					double value = _maxFileSize;

					if (value >= (int)Units.Gigabyte) _formatUnit = Units.Gigabyte;
					else if (value >= (int)Units.Megabyte) _formatUnit = Units.Megabyte;
					else if (value >= (int)Units.Kilobyte) _formatUnit = Units.Kilobyte;
					else _formatUnit = Units.Byte;
				}
				return _formatUnit.Value;
			}
			set { _formatUnit = value; }
		}



		/// <summary>
		/// MaxFileSizeAttribute constructor
		/// </summary>
		/// <param name="maxFileSize">Maximum file size in bytes</param>
		public MaxFileSizeAttribute(long maxFileSize)
		{
			_maxFileSize = maxFileSize;
		}

		/// <summary>
		/// MaxFileSizeAttribute constructor
		/// </summary>
		/// <param name="maxFileSizeSettingKey">Key of the Web.config appSetting, where the max file size value must be taken from</param>
		public MaxFileSizeAttribute(string maxFileSizeSettingKey)
		{
			var setting = ConfigurationManager.AppSettings[maxFileSizeSettingKey];
			int value;
			if (int.TryParse(setting, out value))
				_maxFileSize = value;
		}



		public override bool IsValid(object value)
		{
			var file = value as HttpPostedFileBase;
			if (file == null || _maxFileSize <= 0)
			{
				return true;
			}
			return file.ContentLength <= _maxFileSize;
		}


		public override string FormatErrorMessage(string name)
		{
			double value = Math.Round(_maxFileSize / (double)FormatUnit);

			string sizeString = string.Format("{0} {1}", value, _unitNames[FormatUnit]);
			return string.Format(ErrorMessageString, name, sizeString);
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
