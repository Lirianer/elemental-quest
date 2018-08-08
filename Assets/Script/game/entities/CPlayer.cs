using UnityEngine;
using System.Collections;

public class CPlayer : CAnimatedSprite
{
	private const float ROTATION_SPEED = 5.0f;

	private float mAngle = 0;
	private float mIncAngle = 5;

	private const float BULLET_SPEED = 800.0f;

	private const int RADIUS = 30;

	private CFire mFire;

	public CPlayer()
	{
		setFrames (Resources.LoadAll<Sprite> ("Sprites/player"));
		setName ("player");
		setSortingLayerName ("Player");

		setRadius (RADIUS);

		setBounds (0, 0, CGameConstants.SCREEN_WIDTH, CGameConstants.SCREEN_HEIGHT);
		setBoundAction (CGameObject.WRAP);

		mFire = new CFire ();
	}

	override public void update()
	{
		mAngle += mIncAngle;

		base.update ();

		if (CKeyboard.pressed (CKeyboard.LEFT)) 
		{
			turnLeft (ROTATION_SPEED);
		}
		if (CKeyboard.pressed (CKeyboard.RIGHT)) 
		{
			turnRight (ROTATION_SPEED);
		}

		if (CKeyboard.pressed(CKeyboard.UP) /*|| CMouse.pressed()*/)
		{
			float angRad = CMath.degToRad(getRotation());
			setAccelX(500.0f * Mathf.Cos (angRad));
			setAccelY(500.0f * Mathf.Sin(angRad));

			mFire.setVisible (true);
		}
		else 
		{
			setAccelX (0.0f);
			setAccelY (0.0f);

			mFire.setVisible (false);
		}

		if (CKeyboard.firstPress (CKeyboard.SPACE) /*|| CMouse.firstPress()*/) 
		{
			CPlayerBullet bullet = new CPlayerBullet ();

			float angRad = CMath.degToRad (getRotation ());
			bullet.setXY (getX () + Mathf.Cos (angRad) * RADIUS, getY () + Mathf.Sin (angRad) * RADIUS);
			bullet.setVelXY (BULLET_SPEED * Mathf.Cos (angRad), BULLET_SPEED * Mathf.Sin (angRad));
			CBulletManager.inst ().add (bullet);
		} 

		// Friccion.
		setFriction(0.99f);

		float xMouse = CMouse.getPos ().x;
		float yMouse = CMouse.getPos ().y;
		//Debug.Log ("Mouse: " + xMouse + "," + yMouse);

		// La nave mira al mouse.
		//lookAt(xMouse, yMouse);

		// Bullet Hell.
		if (CKeyboard.pressed (CKeyboard.KEY_G)) 
		{
			float angRad2 = CMath.degToRad (mAngle);
			CPlayerBullet bullet2 = new CPlayerBullet ();
			bullet2.setXY (getX () + Mathf.Cos (angRad2) * 30.0f, getY () + Mathf.Sin (angRad2) * 30.0f);
			bullet2.setVelXY (400.0f * Mathf.Cos (angRad2), 400.0f * Mathf.Sin (angRad2));
			CBulletManager.inst ().add (bullet2);
		}

		// Posicionar el fuego atras de la nave.
		float angFire = CMath.degToRad (getRotation()+180);
		mFire.setXY (getX () + Mathf.Cos (angFire) * 26.0f, getY () + Mathf.Sin (angFire) * 26.0f);
		mFire.setRotation (getRotation ());
		mFire.update ();

		CVector a = new CVector (10, 20);
		CVector b = new CVector (-10, -20);



	}
	
	override public void render()
	{
		base.render ();
		mFire.render ();
	}

	override public void destroy()
	{
		base.destroy ();	
		mFire.destroy ();
		mFire = null;
	}

	// Angulo en grados.
	private void turnLeft(float aAngle)
	{
		setRotation (getRotation () - aAngle);
	}

	private void turnRight(float aAngle)
	{
		setRotation (getRotation () + aAngle);
	}
}
