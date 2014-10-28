using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Drawing;
using System.Web;

namespace Attributes.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ImageDimensionsAttribute : ValidationAttribute
	{
		private readonly int _height;
		private readonly int _width;

		/// <param name="width">Image width. Pass 0 to ingore</param>
		/// <param name="height">Image height. Pass to 0 to ingore</param>
		public ImageDimensionsAttribute(int width, int height)
		{
			_width = width;
			_height = height;
		}

		/// <param name="widthSettingKey">Key of the Web.config appSetting, where the width value must be taken from. Pass null to ignore</param>
		/// <param name="heightSettingKey">Key of the Web.config appSetting, where the height value must be taken from. Pass null to ignore</param>
		public ImageDimensionsAttribute(string widthSettingKey, string heightSettingKey)
		{
			_width = GetDimensionFromSettings(widthSettingKey);
			_height = GetDimensionFromSettings(heightSettingKey);
		}


		public override bool IsValid(object value)
		{
			var file = value as HttpPostedFileBase;
			if (file == null || (_height <= 0 && _width <= 0))
			{
				return true;
			}

			try
			{
				using (var image = Image.FromStream(file.InputStream))
				{
					if ((_height > 0 && image.Height != _height)
						|| (_width > 0 && image.Width != _width))
						return false;
				}
			}
			catch (ArgumentException)
			{
				// this is not image, do nothing
			}

			return true;
		}

		public override string FormatErrorMessage(string name)
		{
			return string.Format(ErrorMessageString, name, _width, _height);
		}


		private int GetDimensionFromSettings(string settingKey)
		{
			int result = 0;

			if (!string.IsNullOrEmpty(settingKey))
			{
				int value;
				var setting = ConfigurationManager.AppSettings[settingKey];

				if (int.TryParse(setting, out value))
					result = value;
			}

			return result;
		}
	}
}