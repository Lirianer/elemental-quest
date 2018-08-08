using UnityEngine;
using System.Collections;

public class CBackground : CSprite
{
	public CBackground()
	{
		setImage (Resources.Load<Sprite>("Sprites/background/game_background"));
		setName ("background");
		setSortingLayerName ("Background");
		render ();
	}

	override public void update()
	{
		base.update();
	}

	override public void render()
	{
		base.render();
	}

	override public void destroy()
	{
		base.destroy();
	}
}