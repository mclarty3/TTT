using Sandbox;

namespace TTT;

public static partial class TTTEvent
{
	public static class Game
	{
		public const string StateChanged = "ttt.game.state-changed";

		/// <summary>
		/// Called everytime the game state changes.
		/// <para>Event is passed the <strong><see cref="TTT.BaseState"/></strong> instance of the old state
		/// and the <strong><see cref="TTT.BaseState"/></strong> instance of the new state.</para>
		/// </summary>
		public class StateChangedAttribute : EventAttribute
		{
			public StateChangedAttribute() : base( StateChanged ) { }
		}
	}
}
