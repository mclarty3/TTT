using System;
using System.Collections.Generic;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace TTT.UI;

[UseTemplate]
public class ScoreboardEntry : Panel
{
	public static Dictionary<Client, ColorGroup> TagCollection = new();

	public static readonly ColorGroup[] TagGroups = new ColorGroup[]
	{
		new ColorGroup("Friend", Color.FromBytes(0, 255, 0)),
		new ColorGroup("Suspect", Color.FromBytes(179, 179, 20)),
		new ColorGroup("Missing", Color.FromBytes(130, 190, 130)),
		new ColorGroup("Kill", Color.FromBytes(255, 0, 0))
	};

	public PlayerStatus PlayerStatus;
	private readonly Client _client;

	private Image PlayerAvatar { get; init; }
	private Label PlayerName { get; init; }
	private Label Tag { get; set; }
	private Label Karma { get; init; }
	private Label Score { get; init; }
	private Label Ping { get; init; }
	private Panel DropdownPanel { get; set; }

	public ScoreboardEntry( Panel parent, Client client ) : base( parent )
	{
		_client = client;
	}

	public void Update()
	{
		if ( _client.Pawn is not Player player )
			return;

		PlayerName.Text = _client.Name;

		Karma.Enabled( Game.KarmaEnabled );

		if ( Karma.IsEnabled() )
			Karma.Text = MathF.Round( player.BaseKarma ).ToString();

		Ping.Text = _client.IsBot ? "BOT" : _client.Ping.ToString();
		Score.Text = player.Score.ToString();

		SetClass( "me", _client == Local.Client );

		if ( player.Role is not NoneRole and not Innocent )
			Style.BackgroundColor = player.Role.Color.WithAlpha( 0.15f );
		else
			Style.BackgroundColor = null;

		PlayerAvatar.SetTexture( $"avatar:{_client.PlayerId}" );
	}

	public void OnClick()
	{
		if ( DropdownPanel == null )
		{
			DropdownPanel = new Panel( this, "dropdown" );
			foreach ( var tagGroup in TagGroups )
			{
				var tagButton = DropdownPanel.Add.Button( tagGroup.Title, () => { SetTag( tagGroup ); } );
				tagButton.Style.FontColor = tagGroup.Color;
			}
		}
		else
		{
			DropdownPanel.Delete();
			DropdownPanel = null;
		}
	}

	private void SetTag( ColorGroup tagGroup )
	{
		if ( tagGroup.Title == Tag.Text )
		{
			ResetTag();
			return;
		}

		Tag.Text = tagGroup.Title;
		Tag.Style.FontColor = tagGroup.Color;
		TagCollection[_client] = tagGroup;
	}

	private void ResetTag()
	{
		Tag.Text = string.Empty;
		TagCollection.Remove( _client );
	}

	[Event.Entity.PostCleanup]
	private void OnRoundStart()
	{
		ResetTag();
	}
}
