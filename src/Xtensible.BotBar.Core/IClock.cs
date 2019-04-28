using System;

namespace Xtensible.BotBar.Core
{
	public interface IClock
	{
		DateTimeOffset UtcNow { get; }
	}
}