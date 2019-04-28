using System;

namespace Xtensible.BotBar.Core
{
	public interface ICaptchaGenerator
	{
		Captcha GenerateCaptcha();
		Captcha GenerateCaptcha(string phrase);
		Captcha GenerateCaptcha(string phrase, DateTimeOffset timestamp, string salt);
	}
}