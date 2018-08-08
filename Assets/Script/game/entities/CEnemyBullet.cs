using UnityEngine;
using System.Collections;

public class CEnemyBullet : CSprite
{
	public const float ACCEL = 100.0f;

	public CEnemyBullet()
	{
		setImage (Resources.Load<Sprite>("Sprites/enemyBullet/red_bullet"));

		setName ("enemy_bullet");
		setSortingLayerName ("Bullets");

		setRadius (5);

		//setBounds (0 - getRadius(), 0 - getRadius(), CGameConstants.SCREEN_WIDTH + getRadius(), CGameConstants.SCREEN_HEIGHT + getRadius());
		//setBoundAction (CGameObject.DIE);

		render ();
	}

	override public void update()
	{
		base.update ();

		CPlayer player = CGame.inst ().getPlayer ();
		CVector a = player.getPos () - getPos ();
		a.normalize ();
		a.mul (ACCEL);
		setAccel (a);

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
