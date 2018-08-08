using UnityEngine;
using System.Collections;

public class CSpark : CAnimatedSprite
{
	public CSpark()
	{
		setFrames (Resources.LoadAll<Sprite> ("Sprites/spark"));
		setName ("spark");
		setSortingLayerName ("Particles");
		setScale (2.0f);
		initAnimation (1, 5, 10, false);
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