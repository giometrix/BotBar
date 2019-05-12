using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Xtensible.BotBar.Components
{
	public class ValidateCaptchaAttribute : Attribute, IFilterFactory, IOrderedFilter
	{
		public int Order { get; } = 1000;

		public bool IsReusable => true;

		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetRequiredService<ValidateCaptchaAuthorizationFilter>();
		}
	}
}
