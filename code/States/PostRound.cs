using System;
using Sandbox;

namespace TTT;

public enum WinType
{
	TimeUp,
	Elimination,
	Objective,
}

public partial class PostRound : BaseState
{
	[Net]
	public Team WinningTeam { get; private set; }

	[Net]
	public WinType WinType { get; private set; }

	public override string Name { get; } = "Post";
	public override int Duration => Game.PostRoundTime;

	public PostRound() { }

	public PostRound( Team winningTeam, WinType winType )
	{
		RevealEveryone();

		WinningTeam = winningTeam;
		WinType = winType;
	}

	public static void Load( Team winningTeam, WinType winType )
	{
		Game.Current.ForceStateChange( new PostRound( winningTeam, winType ) );
	}

	public override void OnPlayerKilled( Player player )
	{
		base.OnPlayerKilled( player );

		player.Reveal();
	}

	public override void OnPlayerJoin( Player player )
	{
		base.OnPlayerJoin( player );

		player.Status = PlayerStatus.Spectator;
		player.UpdateStatus( To.Everyone );
	}

	protected override void OnStart()
	{
		Game.Current.TotalRoundsPlayed++;
		Event.Run( GameEvent.Round.End, WinningTeam, WinType );
	}

	protected override void OnTimeUp()
	{
		bool shouldChangeMap;

		shouldChangeMap = Game.Current.TotalRoundsPlayed >= Game.RoundLimit;
		shouldChangeMap |= Game.Current.RTVCount >= MathF.Round( Client.All.Count * Game.RTVThreshold );

		Game.Current.ChangeState( shouldChangeMap ? new MapSelectionState() : new PreRound() );
	}
}
