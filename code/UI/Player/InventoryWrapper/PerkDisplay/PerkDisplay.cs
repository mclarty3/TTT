using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;

namespace TTT.UI;

[UseTemplate]
public class PerkDisplay : Panel
{
	private readonly Dictionary<Perk, PerkSlot> _entries = new();

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as Player;

		foreach ( var perk in player.Perks )
		{
			if ( !_entries.ContainsKey( perk ) )
			{
				_entries[perk] = AddPerkSlot( perk );
			}
		}

		foreach ( var keyValue in _entries )
		{
			var perk = keyValue.Key;
			var slot = keyValue.Value;

			if ( !player.Perks.Contains( perk ) )
			{
				_entries.Remove( perk );
				slot?.Delete();
			}
		}
	}

	public PerkSlot AddPerkSlot( Perk perk )
	{
		var perkSlot = new PerkSlot( perk );
		AddChild( perkSlot );
		return perkSlot;
	}
}
