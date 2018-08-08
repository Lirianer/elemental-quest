using UnityEngine;
using System.Collections;

public class CAsteroid : CSprite
{
	// Tipos de asteroides.
	public const int TYPE_BIG = 1;
	public const int TYPE_MEDIUM = 2;
	public const int TYPE_SMALL = 3;

	public const int COLOR_BLUE = 1;
	public const int COLOR_WHITE = 2;
	public const int COLOR_BROWN = 3;

	// Color (1, 2 o 3).
	private int mColor;

	// Parámetros: Tipo de Asteroide (1 = Grande, 2 = Medio, 3 = Chico),
	// Color (1, 2 o 3).
	public CAsteroid(int aType, int aColor)
	{
		// Tipo del asteroide.
		setType(aType);
		mColor = aColor;

		string sufix = "";

		if (getType () == TYPE_BIG) 
		{
			sufix = "_big";
			setScore (10);
			setRadius (32);
		} 
		else if (aType == TYPE_MEDIUM) 
		{
			sufix = "_medium";
			setScore (50);
			setRadius (16);
		}
		else if (aType == TYPE_SMALL)
		{
			sufix = "_small";
			setScore (100);
			setRadius(8);
		}

		setImage(Resources.Load<Sprite>("Sprites/asteroid/asteroid" + aColor + sufix));

		setName ("asteroid");
		setSortingLayerName ("Enemies");

		setBounds (0 - getRadius(), 0 - getRadius(), CGameConstants.SCREEN_WIDTH + getRadius(), CGameConstants.SCREEN_HEIGHT + getRadius());
		setBoundAction (CGameObject.WRAP);
	}
	
	public override void update()
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

	// Se llama desde CPlayerBullet cuando una bala nos toca.
	public void hit()
	{
		setDead (true);

		int t = 0;

		// Si es un asteroide grande, se crean asteroides medianos.
		if (getType () == TYPE_BIG) 
		{
			t = TYPE_MEDIUM;
		}
		// Si el asteroide es mediano, se crean asteroides chicos.
		else if (getType () == TYPE_MEDIUM) 
		{
			t = TYPE_SMALL;
		}

		// Si el asteroide es chico, se saltea esta parte.
		// No se crean asteroides.
		if (getType() != TYPE_SMALL)
		{
			for (int i = 0; i < 2; i++) 
			{
				// Crear los asteroides: se le pasa como parámetro el tipo y color (el color es el mismo).
				CAsteroid asteroid = new CAsteroid (t, mColor);
				asteroid.setXY (getX (), getY ());
				asteroid.setVelX (CMath.randomFloatBetween (-500, 500));
				asteroid.setVelY (CMath.randomFloatBetween (-500, 500));
				CEnemyManager.inst ().add (asteroid);
			}
		}

		CExplosion explosion = new CExplosion ();
		explosion.setXY (getX (), getY ());
		CParticleManager.inst ().add (explosion);
	}
}