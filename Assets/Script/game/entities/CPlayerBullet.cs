using UnityEngine;
using System.Collections;

public class CPlayerBullet : CSprite
{
	public const float SPEED = -1800;
	
	public CPlayerBullet()
	{
		setImage (Resources.Load<Sprite>("Sprites/bullet"));

		setName ("PlayerBullet");
		setSortingLayerName ("Bullets");

		setRadius (5);

		setBounds (0 - getRadius(), 0 - getRadius(), CGameConstants.SCREEN_WIDTH + getRadius(), CGameConstants.SCREEN_HEIGHT + getRadius());
		setBoundAction (CGameObject.DIE);

		render ();
	}
	
	override public void update()
	{
		base.update ();

		// TODO: HACER COLISIONES CON ENEMY SHIP.
		/*CAsteroid enemy = (CAsteroid) CEnemyManager.inst ().collides (this);
		if (enemy != null) 
		{
			setDead (true);
			enemy.hit ();
		}*/
	}
	
	override public void render()
	{
		base.render ();
		
		setX (getX ());
		setY (getY ());
	}
	
	override public void destroy()
	{
		base.destroy ();	
	}
}
