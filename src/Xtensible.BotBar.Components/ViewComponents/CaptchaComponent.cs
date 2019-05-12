using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xtensible.BotBar.Core;

namespace Xtensible.BotBar.Components
{
	[ViewComponent(Name = "Captcha")]
	public class CaptchaComponent : ViewComponent
	{
		public Task<IViewComponentResult> InvokeAsync()
		{
			var generator = new CaptchaGenerator();
			IViewComponentResult view = View(generator.GenerateCaptcha());
			return Task.FromResult(view);
		}
	}
}
