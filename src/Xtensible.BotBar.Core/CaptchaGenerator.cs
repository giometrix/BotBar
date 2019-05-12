using System;
using System.Drawing;

namespace Xtensible.BotBar.Core
{
	public class CaptchaGenerator : ICaptchaGenerator
	{
		private readonly IClock _clock;
		private static readonly Random _rng = new Random();
		public const string DefaultSalt = "jU#X^9B6C4XzTbxL!XY0ieygqEbnM0";

		public int CaptchaWidth { get; set; } = 100;
		public int CaptchaHeight { get; set; } = 30;
		public int CaptchaFontSize { get; set; } = 16;
		public Color CaptchaBackgroundColor { get; set; } = Color.White;
		public Color CaptchaForegroundColor { get; set; } = Color.Black;
		public string CaptchaFontFamily { get; set; } = "Arial";
		public Func<string> PhraseGenerator { get; set; } = () => DefaultPhraseList.Phrases[_rng.Next(0, DefaultPhraseList.Phrases.Length)];

		public CaptchaGenerator(IClock clock = null)
		{
			_clock = clock ?? Clock.Default;
		}

		public Captcha GenerateCaptcha()
		{
			return GenerateCaptcha(PhraseGenerator());
		}

		public Captcha GenerateCaptcha(string phrase)
		{
			return GenerateCaptcha(phrase, _clock.UtcNow, DefaultSalt);
		}

		public Captcha GenerateCaptcha(string phrase, DateTimeOffset timestamp, string salt)
		{
			if (String.IsNullOrWhiteSpace(phrase))
				throw new ArgumentException(nameof(phrase));

			if (salt == null)
				throw new ArgumentException(nameof(salt));

			return new Captcha(phrase.ToLowerInvariant(), timestamp, salt, CaptchaWidth, CaptchaHeight, CaptchaBackgroundColor, CaptchaForegroundColor, CaptchaFontFamily, CaptchaFontSize);
		}
	}
}
