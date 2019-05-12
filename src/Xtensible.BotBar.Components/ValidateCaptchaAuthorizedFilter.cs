using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xtensible.BotBar.Core;

namespace Xtensible.BotBar.Components
{
	public class ValidateCaptchaAuthorizationFilter : IAsyncAuthorizationFilter
	{
		public readonly ILogger _logger;

		public ValidateCaptchaAuthorizationFilter(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<ValidateCaptchaAuthorizationFilter>();
		}

		public Task OnAuthorizationAsync(AuthorizationFilterContext context)
		{
			ValidateCaptchaAuthorizationFilter authorizedFilter = this;

			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			if (authorizedFilter.ShouldValidate(context))
			{
				try
				{
					var hashedPhrase = context.HttpContext.Request.Form[nameof(Captcha.HashedPhrase)];
					var timestamp = long.Parse(context.HttpContext.Request.Form[nameof(Captcha.Timestamp)]);
					var userValue = context.HttpContext.Request.Form["captcha"];

					if (!Captcha.IsMatch(hashedPhrase, userValue, timestamp, CaptchaGenerator.DefaultSalt, out string errorReason))
					{
						context.ModelState.AddModelError("captcha", errorReason);
					}
				}
				catch (Exception ex)
				{
					authorizedFilter._logger.LogError(ex, ex.Message);
					throw;
				}
			}

			return Task.CompletedTask;
		}

		public virtual bool ShouldValidate(AuthorizationFilterContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			return true;
		}
	}
}
