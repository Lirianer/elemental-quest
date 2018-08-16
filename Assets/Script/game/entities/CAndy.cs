using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CAndy : CAnimatedSprite
{
	private const int WIDTH = 64 * 2;
	private const int HEIGHT = 74 * 2;

	private const int STATE_STAND = 0;
	private const int STATE_WALKING = 1;
	private const int STATE_JUMPING = 2;
	private const int STATE_FALLING = 3;
	private const int STATE_HIT_ROOF = 4;

	private CSprite mRect;
	private CSprite mRect2;

    private CText textoPoderes;
    private List<Power> powers;
    private int selectedPower;

    //Pruebo
    public CTileMap muestro;
    public bool estatierra = false;

    // coordenada y que tenia en el frame anterior. Usada para chequear en la horizontal antes que en la vertical...
    private float mOldY;

	private const int X_OFFSET_BOUNDING_BOX = 8 * 2;
	private const int Y_OFFSET_BOUNDING_BOX = 13 * 2;

	public CAndy()
	{
		setFrames (Resources.LoadAll<Sprite> ("Sprites/andy"));
		setName ("andy");
		setSortingLayerName ("Player");

		setScale (2.0f);

		setRegistration (CSprite.REG_TOP_LEFT);

		setWidth (WIDTH);
		setHeight (HEIGHT);

		// TODO: PASAR A LA CAMARA CUANDO SE IMPLEMENTE.
		CAudioManager.Inst.setAudioListener(this);

		// Agregate al audio manager como audio source.
		// Este objeto emite sonido.
		CAudioManager.Inst.addAudioSource(this);

		setState (STATE_STAND);

		setXOffsetBoundingBox (X_OFFSET_BOUNDING_BOX);
		setYOffsetBoundingBox (Y_OFFSET_BOUNDING_BOX);

		mRect = new CSprite ();
		mRect.setImage (Resources.Load<Sprite> ("Sprites/ui/pixel"));
		mRect.setSortingLayerName ("Player");
		mRect.setSortingOrder (20);
		mRect.setAlpha (0.5f);
		mRect.setName ("player_debug_rect");

		mRect2 = new CSprite ();
		mRect2.setImage (Resources.Load<Sprite> ("Sprites/ui/pixel"));
		mRect2.setSortingLayerName ("Player");
		mRect2.setSortingOrder (20);
		mRect2.setColor (Color.red);
		mRect2.setAlpha (0.5f);
		mRect2.setName ("player_debug_rect2");

        this.powers = new List<Power>();
        this.powers.Add(new Earth());
        this.powers.Add(new Air());
        this.powers.Add(new Water());
        this.powers.Add(new Fire());
     
        this.selectedPower = 0;

        textoPoderes = new CText(this.powers[this.selectedPower].getName());
        textoPoderes.setWidth(this.getWidth());
        textoPoderes.setWrapping(false);
        textoPoderes.setFontSize(400f);
        textoPoderes.setXY(this.getX(), this.getY() - textoPoderes.getHeight());
        textoPoderes.setAlignment(TMPro.TextAlignmentOptions.Center);
    }

	private void setOldYPosition()
	{
		mOldY = getY ();
	}

	override public void update()
	{
		//Debug.Log ("test left : " + CKeyboard.pressed (CKeyboard.LEFT) + CKeyboard.pressed (CKeyboard.UP) + CKeyboard.pressed (CKeyboard.SPACE));
		//Debug.Log ("test right: " + CKeyboard.pressed (CKeyboard.RIGHT) + CKeyboard.pressed (CKeyboard.UP) + CKeyboard.pressed (CKeyboard.SPACE));

		// Guardar la posicion anterior del objeto.
		setOldYPosition ();

        base.update();

        if (CKeyboard.firstPress(KeyCode.E))
        {
            this.selectNextPower();
        }
        else if (CKeyboard.firstPress(KeyCode.Q))
        {
            this.selectPreviousPower();
        }

        if (getState () == STATE_STAND) {
			// En stand no deberia pasar nunca que quede metido en una pared.
			// Si estamos en una pared, corregirnos. 
			if (isWallLeft (getX (), getY ())) {
				// Reposicionar el personaje contra la pared.
				setX (((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);
			} 
			if (isWallRight (getX (), getY ())) {
				// Reposicionar el personaje contra la pared.
				setX ((((mRightX) * CTileMap.TILE_WIDTH) - getWidth ()) + X_OFFSET_BOUNDING_BOX);
			}


			// Si en el pixel de abajo del jugador no hay piso, caemos.
			if (!isFloor (getX (), getY () + 1)) {
				setState (STATE_FALLING);
				return;
			}

			if (CKeyboard.firstPress (CKeyboard.SPACE)) {
				setState (STATE_JUMPING);
				return;
			}

			if (CKeyboard.pressed (CKeyboard.KEY_A) && !isWallLeft (getX () - 1, getY ())) {
				setState (STATE_WALKING);
				return;
			}

			if (CKeyboard.pressed (CKeyboard.KEY_D) && !isWallRight (getX () + 1, getY ())) {
				setState (STATE_WALKING);
				return;
			}
		} else if (getState () == STATE_WALKING) {
			if (isWallLeft (getX (), getY ())) {
				// Reposicionar el personaje contra la pared.
				setX (((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);
			} 
			if (isWallRight (getX (), getY ())) {
				// Reposicionar el personaje contra la pared.
				setX ((((mRightX) * CTileMap.TILE_WIDTH) - getWidth ()) + X_OFFSET_BOUNDING_BOX);
			}

			if (CKeyboard.firstPress (CKeyboard.SPACE)) {
				setState (STATE_JUMPING);
				return;
			}

			// Si en el pixel de abajo del jugador no hay piso, caemos.
			if (!isFloor (getX (), getY () + 1)) {
				setState (STATE_FALLING);
				return;
			}

			if (!(CKeyboard.pressed (CKeyboard.KEY_A) || CKeyboard.pressed (CKeyboard.KEY_D))) {
				setState (STATE_STAND);
				return;
			} else {
				if (CKeyboard.pressed (CKeyboard.KEY_A)) {
					// Chequear pared a la izquierda.
					// Si hay pared a la izquierda vamos a stand.
					if (isWallLeft (getX (), getY ())) {
						// Reposicionar el personaje contra la pared.
						//setX((((int) getX ()/CTileMap.TILE_WIDTH)+1)*CTileMap.TILE_WIDTH);
						setX (((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);

						// Carlos version.
						//setX (getX()+CTileMap.TILE_WIDTH/(getWidth()-1));

						setState (STATE_STAND);
						return;
					} else {
						// No hay pared, se puede mover.
						setVelX (-400);
						setFlip (true);
					}
				} else {
					// Chequear pared a la derecha.
					// Si hay pared a la derecha vamos a stand.
					if (isWallRight (getX (), getY ())) {
						// Reposicionar el personaje contra la pared.
						setX ((((mRightX) * CTileMap.TILE_WIDTH) - getWidth ()) + X_OFFSET_BOUNDING_BOX);

						setState (STATE_STAND);
						return;
					} else {
						// No hay pared, se puede mover.
						setVelX (400);
						setFlip (false);
					}
				}
			}
		} else if (getState () == STATE_JUMPING) {
			controlMoveHorizontal ();

			if (isFloor (getX (), getY () + 1)) {
				setY (mDownY * CTileMap.TILE_HEIGHT - getHeight ());
				setState (STATE_STAND);
				return;
			}

			if (isRoof (getX (), getY () - 1)) 
			{
				setY (((mUpY + 1) * CTileMap.TILE_HEIGHT) - Y_OFFSET_BOUNDING_BOX);
				setVelY (0);
				setState (STATE_HIT_ROOF);
				return;
			}
		} else if (getState () == STATE_FALLING) {
			controlMoveHorizontal ();

			if (isFloor (getX (), getY () + 1)) {
				setY (mDownY * CTileMap.TILE_HEIGHT - getHeight ());
				setState (STATE_STAND);
				return;
			}
		} 
		else if (getState () == STATE_HIT_ROOF) 
		{
			if (getTimeState () > 0.02f * 5.0f) 
			{
				setState (STATE_FALLING);
				return;
			}
		}

        // Chequear el paso entre pantallas.
        controlRooms ();

        textoPoderes.setXY(this.getX(), this.getY() - textoPoderes.getHeight());
        textoPoderes.setText(this.powers[this.selectedPower].getName());
        textoPoderes.update();


		if( CMouse.firstPress() )
		{
			this.powers[this.selectedPower].update();
		}
    }

	private void controlRooms()
	{
		CTileMap map = CGame.inst ().getMap ();

		if (getX () + getWidth () / 2 > CTileMap.WORLD_WIDTH) 
		{
			// Se fue por la derecha.
			map.changeRoom(CGameConstants.D);

			// Aparece por la izquierda.
			setX(-getWidth () / 2);
		} 
		else if (getX () + getWidth () / 2 < 0) 
		{
			// Se fue por la izquierda.
			map.changeRoom(CGameConstants.A);

			setX (CTileMap.WORLD_WIDTH - getWidth () / 2);
		} 
		else if (getY () + getHeight () / 2 > CTileMap.WORLD_HEIGHT) 
		{
			// Se fue por abajo.
			map.changeRoom(CGameConstants.S);

			setY (-getHeight () / 2);
		} 
		else if (getY () + getHeight () / 2 < 0) 
		{
			// Se fue por arriba.
			map.changeRoom(CGameConstants.W);

			setY(CTileMap.WORLD_HEIGHT - getHeight() / 2);
		}
	}

	// Se llama desde los estados jumping y falling para movernos para los costados.
	private void controlMoveHorizontal()
	{
		// Si estamos en una pared, corregirnos.
		if (isWallLeft (getX (), mOldY)) 
		{
			// Reposicionar el personaje contra la pared.
			setX (((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);
		} 
		if (isWallRight (getX (), mOldY)) 
		{
			// Reposicionar el personaje contra la pared.
			setX ((((mRightX) * CTileMap.TILE_WIDTH) - getWidth ()) + X_OFFSET_BOUNDING_BOX);
		} 

		// Chequeamos si podemos movernos.
		if (!(CKeyboard.pressed (CKeyboard.KEY_A) || CKeyboard.pressed (CKeyboard.KEY_D))) 
		{
			setVelX (0);
		} 
		else 
		{
			if (CKeyboard.pressed (CKeyboard.KEY_A)) 
			{
				// Chequear pared a la izquierda.
				// Si hay pared a la izquierda vamos a stand.
				if (isWallLeft (getX ()-1, mOldY)) 
				{
					// Reposicionar el personaje contra la pared.
					setX (((mLeftX + 1) * CTileMap.TILE_WIDTH) - X_OFFSET_BOUNDING_BOX);
				} 
				else 
				{
					// No hay pared, se puede mover.
					setVelX (-400);
					setFlip (true);
				}
			} 
			else 
			{
				// Chequear pared a la derecha.
				// Si hay pared a la derecha vamos a stand.
				if (isWallRight (getX ()+1, mOldY)) 
				{
					// Reposicionar el personaje contra la pared.
					setX ((((mRightX) * CTileMap.TILE_WIDTH) - getWidth ()) + X_OFFSET_BOUNDING_BOX);
				} 
				else 
				{
					// No hay pared, se puede mover.
					setVelX (400);
					setFlip (false);
				}
			}
		}
	}
		
	override public void render()
	{
		base.render ();

		// MOSTRAR TODA EL AREA DEL DIBUJO.
		mRect.setXY (getX(), getY());
		mRect.setScaleX(WIDTH);
		mRect.setScaleY(HEIGHT);
		mRect.update ();

		mRect.render ();

		// Bounding box.
		mRect2.setXY (getX() + X_OFFSET_BOUNDING_BOX, getY() + Y_OFFSET_BOUNDING_BOX);
		mRect2.setScaleX(WIDTH - (X_OFFSET_BOUNDING_BOX * 2));
		mRect2.setScaleY(HEIGHT - Y_OFFSET_BOUNDING_BOX);
		mRect2.update ();

		mRect2.render ();

        textoPoderes.render();
	}

	override public void destroy()
	{
		base.destroy ();	
		mRect.destroy ();
		mRect = null;
		mRect2.destroy ();
		mRect2 = null;
	}

	public override void setState (int aState)
	{
		base.setState (aState);

		if (getState () == STATE_STAND) 
		{
			stopMove ();
			gotoAndStop (1);
		} 
		else if (getState () == STATE_WALKING) 
		{
			initAnimation (2, 9, 12, true);
		}
		else if (getState () == STATE_JUMPING) 
		{
			initAnimation (10, 17, 12, false);
			setVelY (CGameConstants.JUMP_SPEED);
			setAccelY (CGameConstants.GRAVITY);

			CAudioManager.Inst.playFX("jump");
		}
		else if (getState () == STATE_FALLING) 
		{
			initAnimation (15, 17, 12, false);
			setAccelY (CGameConstants.GRAVITY);
		}
		else if (getState () == STATE_HIT_ROOF) 
		{
			stopMove();
		}
	}

    private void selectPower(int power)
    {
        this.selectedPower = power;
        if(this.selectedPower > this.powers.Count - 1)
        {
            this.selectedPower = 0;
        }
        else if(this.selectedPower < 0)
        {
            this.selectedPower = this.powers.Count - 1;
        }

        Debug.Log(this.selectedPower);
    }

    private void selectNextPower()
    {
        this.selectPower(this.selectedPower + 1);
    }

    private void selectPreviousPower()
    {
        this.selectPower(this.selectedPower - 1);
    }
}
