﻿using UnityEngine;
using System.Collections;

public class CItem : CAnimatedSprite
{
	private const int WIDTH = 48 * 2;
	private const int HEIGHT = 48 * 2;

	private const int STATE_STAND = 0;
	private const int STATE_FALLING = 1;
	private const int STATE_WALKING = 2;

	// coordenada y que tenia en el frame anterior. Usada para chequear en la horizontal antes que en la vertical...
	private float mOldY;

	public const int TYPE_DONT_FALL = 0;  // No cae de las plataformas
	public const int TYPE_FALL = 1;       // Cae cuando llega al borde de una plataforma.




	public CItem(int aType)
	{
		setType (aType);
		setFrames (Resources.LoadAll<Sprite> ("Sprites/items"));
		setName ("item");
		setSortingLayerName ("Enemies");
		setScale (2.0f);
		setRegistration (CSprite.REG_TOP_LEFT);
		setWidth (WIDTH);
		setHeight (HEIGHT);
		setState (STATE_STAND);
	}

	private void setOldYPosition()
	{
		mOldY = getY ();
	}

	override public void update()
	{
		int tileWidth = CGame.inst ().getMap ().getTileWidth ();
		int tileHeight = CGame.inst ().getMap ().getTileHeight ();
		//Debug.Log (getState ());

		// Guardar la posicion anterior del objeto.
		setOldYPosition ();

		base.update ();

		if (getState () == STATE_STAND) 
		{
			/*

			// En stand no deberia pasar nunca que quede metido en una pared. 
			// Si estamos en una pared, corregirnos. 
			if (isWallLeft (getX (), getY ())) {
				// Reposicionar el personaje contra la pared.
				setX ((mLeftX + 1) * CTileMap.Instance.getTileWidth());
			} 
			if (isWallRight (getX (), getY ())) {
				// Reposicionar el personaje contra la pared.
				setX (((mRightX) * CTileMap.Instance.getTileWidth()) - getWidth ());
			}

			// Si en el pixel de abajo del jugador no hay piso, caemos.
			if (!isFloor (getX (), getY () + 1)) {
				setState (STATE_FALLING);
				return;
			}

			if (!isWallRight (getX () + 1, getY ())) 
			{
				setVelX (400);
				setState (STATE_WALKING);
				return;
			}

			if (!isWallLeft (getX () - 1, getY ())) 
			{
				setVelX (-400);
				setState (STATE_WALKING);
				return;
			}

			*/
		} 
		else if (getState () == STATE_WALKING) 
		{
			if (isWallLeft (getX (), getY ())) 
			{
				// Reposicionar el personaje contra la pared.
				setX ((mLeftX + 1) * tileWidth);

				setVelX (getVelX () * -1);
			} 
			if (isWallRight (getX (), getY ())) 
			{
				// Reposicionar el personaje contra la pared.
				setX (((mRightX) * tileWidth) - getWidth ());
				setVelX (getVelX () * -1);
			}

			// Si en el pixel de abajo del jugador no hay piso, caemos.
			if (!isFloor (getX(), getY()+1)) 
			{
				setState (STATE_FALLING);
				return;
			}

			if (getVelX () < 0) 
			{
				// Chequear pared a la izquierda.
				// Si hay pared a la izquierda vamos a stand.
				if (isWallLeft (getX (), getY ())) {
					// Reposicionar el personaje contra la pared.
					//setX((((int) getX ()/tileWidth)+1)*tileWidth);
					setX ((mLeftX + 1) * tileWidth);

					setVelX (getVelX () * -1);

					return;
				} else {
					// No hay pared, se puede mover.
					setVelX (-400);
					setFlip (true);

					if (getType () == TYPE_DONT_FALL) 
					{
						checkPoints (getX (), getY () + 1);
						if (mTileDownLeft) 
						{
							setVelX (getVelX () * -1);
						}
					}
				}
			}
			else if (getVelX() > 0)
			{
				// Chequear pared a la derecha.
				// Si hay pared a la derecha vamos a stand.
				if (isWallRight (getX (), getY ())) 
				{
					// Reposicionar el personaje contra la pared.
					setX (((mRightX) * tileWidth) - getWidth ());

					setVelX (getVelX () * -1);
					return;
				} 
				else 
				{
					// No hay pared, se puede mover.
					setVelX (400);
					setFlip (false);

					if (getType () == TYPE_DONT_FALL) 
					{
						checkPoints (getX (), getY () + 1);
						if (mTileDownRight) 
						{
							setVelX (getVelX () * -1);
						}
					}
				}
			}
		}
		else if (getState () == STATE_FALLING) 
		{
			controlMoveHorizontal ();

			if (isFloor(getX(), getY()+1))
			{
				setY (mDownY * tileHeight - getHeight());
				setState (STATE_STAND);
				return;
			}
		}

		// Spark emmiter.
		if (CMath.randomIntBetween (1, 50) == 1) 
		{
			CSpark spark = new CSpark ();
			CVector pos  = new CVector(getX () + getWidth() / 2, getY () + getHeight() / 2);
			spark.setPos(pos + new CVector(CMath.randomIntBetween(-10, 10), CMath.randomIntBetween(-10, 10)));
			CParticleManager.inst ().add (spark);
		}
	}

	// Se llama desde los estados jumping y falling para movernos para los costados.
	private void controlMoveHorizontal()
	{
		int tileWidth = CGame.inst ().getMap ().getTileWidth ();
		// Si estamos en una pared, corregirnos.                // ESTE BLOQUE ES IGUAL A ANDY ---- ()?
		if (isWallLeft (getX (), mOldY)) 
		{
			// Reposicionar el personaje contra la pared.
			setX ((mLeftX + 1) * tileWidth);
		} 
		if (isWallRight (getX (), mOldY)) 
		{
			// Reposicionar el personaje contra la pared.
			setX (((mRightX) * tileWidth) - getWidth ());
		}                                                          // --------------------------
	}

	override public void render()
	{
		base.render ();
	}

	override public void destroy()
	{
		base.destroy ();	
	}

	public override void setState (int aState)
	{
		base.setState (aState);

		if (getState () == STATE_STAND) 
		{
			stopMove ();
			gotoAndStop (1);
			//initAnimation (1, 2, 12, true);
		} 
		else if (getState () == STATE_FALLING) 
		{
			initAnimation (1, 2, 12, true);
			setAccelY (CGameConstants.GRAVITY);
		}
		else if (getState () == STATE_WALKING) 
		{
			initAnimation (1, 2, 12, true);
		}
	}
}