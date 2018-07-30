using UnityEngine;
using System.Collections;

public class CExplosion : CAnimatedSprite
{
	public CExplosion()
	{
		setFrames (Resources.LoadAll<Sprite> ("Sprites/explosion"));
		initAnimation (1, 10, 24, false);
		setName ("explosion");
		setSortingLayerName ("Particles");
	}

	override public void update()
	{
		base.update ();

		if (isEnded ()) 
		{
			setDead (true);
		}
	}

	override public void render()
	{
		base.render ();
	}

	override public void destroy()
	{
		base.destroy ();	
	}
}
