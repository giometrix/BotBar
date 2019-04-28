using NETCore.Encrypt;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Xtensible.BotBar.Core
{
	public class Captcha
	{
		public string HashedPhrase { get; }
		public byte[] Image { get; private set; }
		public string Base64EncodedImageSrc => "data:image/png;base64," + Convert.ToBase64String(Image);
		public long Timestamp { get; }

		public static bool IsMatch(string expectedOutput, string userInput, long timestamp, string salt, out string errorReason, int leewayInMinutes = 120, IClock clock = null)
		{
			if (userInput == null)
				throw new ArgumentException(nameof(userInput));

			if (salt == null)
				throw new ArgumentException(nameof(salt));


			clock = clock ?? Clock.Default;

			if(Math.Abs(clock.UtcNow.Subtract(DateTimeOffset.FromUnixTimeSeconds(timestamp)).TotalMinutes) > leewayInMinutes)
			{
				errorReason = "Captcha is expired";
				return false;
			}

			var result = EncryptProvider.Sha256(userInput.ToLowerInvariant() + salt + timestamp) == expectedOutput;
			if (result)
				errorReason = String.Empty;
			else
				errorReason = "Captcha does not match";

			return result;
		}

		internal Captcha(string phrase, DateTimeOffset timestamp, string salt, int width, int height, Color backgroundColor, Color foregroundColor, string fontFamily, int fontSize)
		{
			if (phrase == null)
				throw new ArgumentException(nameof(phrase));
			if (salt == null)
				throw new ArgumentException(nameof(salt));
			if (width <= 0)
				throw new ArgumentException(nameof(width));
			if (height <= 0)
				throw new ArgumentException(nameof(height));
			if (String.IsNullOrWhiteSpace(fontFamily))
				throw new ArgumentException(nameof(fontFamily));
			if (fontSize <= 0)
				throw new ArgumentException(nameof(fontSize));

			Timestamp = timestamp.ToUnixTimeSeconds();
			HashedPhrase = EncryptProvider.Sha256(phrase.ToLowerInvariant() + salt + Timestamp.ToString());
			DrawPhrase(phrase, width, height, backgroundColor, foregroundColor, fontFamily, fontSize);
		}

		private void DrawPhrase(string phrase, int width, int height, Color backgroundColor, Color foregroundColor, string fontFamily, int fontSize)
		{
			using (var bitmap = new Bitmap(width, height))
			{
				using (Graphics g = Graphics.FromImage(bitmap))
				{
					g.Clear(backgroundColor);
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.PixelOffsetMode = PixelOffsetMode.HighQuality;
					var sf = new StringFormat
					{
						Alignment = StringAlignment.Center,
						LineAlignment = StringAlignment.Center
					};
					var rectf = new RectangleF((int)(width * 0.10), (int)(height * 0.2), (int)(width * 0.8), (int)(height * 0.6));
					g.DrawString(phrase, new Font(fontFamily, fontSize, FontStyle.Bold), new SolidBrush(foregroundColor), rectf, sf);
				}
				using (var stream = new MemoryStream())
				{
					bitmap.Save(stream, ImageFormat.Png);
					Image = stream.GetBuffer();
				}
			}
		}
	}
}