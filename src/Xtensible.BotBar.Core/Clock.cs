using System;
using System.Collections.Generic;
using System.Text;

namespace Xtensible.BotBar.Core
{
	public class Clock : IClock
	{
		public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

		public static IClock Default { get; } = new Clock();
	}
}
