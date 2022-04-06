using Sandbox;

namespace TTT;

[Hammer.EditorModel( "models/c4/c4.vmdl" )]
[Library( "ttt_entity_c4", Title = "C4" )]
public partial class C4Entity : Prop, IEntityHint, IUse
{
	private static readonly Model WorldModel = Model.Load( "models/c4/c4.vmdl" );

	[Net, Local]
	private bool IsArmed { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		Model = WorldModel;
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		Health = 100f;
	}

	void IEntityHint.Tick( Player player )
	{
		if ( !player.IsLocalPawn || !Input.Down( InputButton.Use ) )
		{
			UI.FullScreenHintMenu.Instance?.Close();
			return;
		}

		if ( UI.FullScreenHintMenu.Instance.IsOpen )
			return;

		if ( IsArmed )
			UI.FullScreenHintMenu.Instance?.Open( new UI.C4DefuseMenu( this ) );
		else
			UI.FullScreenHintMenu.Instance?.Open( new UI.C4ArmMenu( this ) );
	}

	// Maybe we setup a keybinding to pickup c4?
	bool IUse.OnUse( Entity user )
	{
		return false;
	}

	bool IUse.IsUsable( Entity user )
	{
		return user is Player;
	}
}
