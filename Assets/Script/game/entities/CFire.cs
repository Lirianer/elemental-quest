using UnityEngine;
using System.Collections;

public class CFire : CAnimatedSprite
{
	public CFire()
	{
		setFrames (Resources.LoadAll<Sprite> ("Sprites/fire"));
		initAnimation (1, 3, 24, true);
		setName ("fire");
		setSortingLayerName ("Player");
	}

	override public void update()
	{
		base.update ();
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
