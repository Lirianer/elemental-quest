using UnityEngine;
using System.Collections;

public class CEnemyShip : CAnimatedSprite
{
	private float mTimeNextShot = 0;

	public const float SPEED = 200.0f;
	public const float BULLET_SPEED = 800.0f;

	public CEnemyShip()
	{
		setFrames (Resources.LoadAll<Sprite> ("Sprites/enemyShip"));
		initAnimation (1, 9, 24, true);
		setName ("enemy_ship");
		setSortingLayerName ("Enemies");
	}

	override public void update()
	{
		mTimeNextShot += Time.deltaTime;

		base.update ();

		CPlayer player = CGame.inst ().getPlayer ();

		CVector vel = player.getPos () - getPos ();
		vel.normalize ();
		vel.mul (SPEED);
		setVel (vel);

		if (isEnded ()) 
		{
			setDead (true);
		}

		if (mTimeNextShot >= 1.0f)
		{
			mTimeNextShot = 0;
			player = CGame.inst ().getPlayer ();

			CVector v = player.getPos () - getPos ();
			v.normalize ();
			v.mul (BULLET_SPEED);

			CEnemyBullet b = new CEnemyBullet ();
			b.setXY (getX (), getY ());
			b.setVel (v);
			CEnemyManager.inst ().add (b);
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