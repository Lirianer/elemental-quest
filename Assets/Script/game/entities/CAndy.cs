using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CAndy : CAnimatedSprite
{
	private const int WIDTH = 64 * 2;
	private const int HEIGHT = 74 * 2;

	public const int STATE_STAND = 0;
	public const int STATE_WALKING = 1;
	public const int STATE_PRE_JUMPING = 2;
	public const int STATE_JUMPING = 3;
	public const int STATE_FALLING = 4;
	public const int STATE_HIT_ROOF = 5;
	public const int STATE_DASHING = 6;

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

	private CVector lastCheckPoint;

	public CAndy()
	{
		setFrames (Resources.LoadAll<Sprite> ("Sprites/nina"));
		setName ("Nina");
		setSortingLayerName ("Player");

		setScale (0.5f);

		setRegistration (CSprite.REG_TOP_LEFT);

		setWidth (WIDTH);
		setHeight (HEIGHT);

		// TODO: PASAR A LA CAMARA CUANDO SE IMPLEMENTE.
		CAudioManager.Inst.setAudioListener(this);

		// Agregate al audio manager como audio source.
		// Este objeto emite sonido.
		CAudioManager.Inst.addAudioSource(this);

		setState (STATE_STAND);

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
        this.powers.Add(new Air(this));
        this.powers.Add(new Water(this));
        this.powers.Add(new Fire(this));
     
        this.selectedPower = 0;

		this.powers[this.selectedPower].setActive();

        textoPoderes = new CText(this.powers[this.selectedPower].getName());
        textoPoderes.setWidth(this.getWidth());
        textoPoderes.setWrapping(false);
        textoPoderes.setFontSize(40f);
        textoPoderes.setXY(this.getX(), this.getY() - textoPoderes.getHeight());
        textoPoderes.setAlignment(TMPro.TextAlignmentOptions.Center);
    }

	private void setOldYPosition()
	{
		mOldY = getY ();
	}

	override public void update()
	{
		int tileWidth = CGame.inst().getMap().getTileWidth();
		int tileHeight = CGame.inst().getMap().getTileHeight();

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
				setX (((mLeftX + 1) * tileWidth) - this.getLeftOffsetBoundingBox());
			} 
			if (isWallRight (getX (), getY ())) {
				// Reposicionar el personaje contra la pared.
				setX ((((mRightX) * tileWidth) - getWidth ()) + this.getRightOffsetBoundingBox());
			}


			// Si en el pixel de abajo del jugador no hay piso, caemos.
			if (!isFloor (getX (), getY () + 1)) {
				setState (STATE_FALLING);
				return;
			}

			if (CKeyboard.firstPress (CKeyboard.SPACE)) {
				setState (STATE_PRE_JUMPING);
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
				setX (((mLeftX + 1) * tileWidth) - this.getLeftOffsetBoundingBox());
			} 
			if (isWallRight (getX (), getY ())) {
				// Reposicionar el personaje contra la pared.
				setX ((((mRightX) * tileWidth) - getWidth ()) + this.getRightOffsetBoundingBox());
			}

			if (CKeyboard.firstPress (CKeyboard.SPACE)) {
				setState (STATE_PRE_JUMPING);
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
						//setX((((int) getX ()/tileWidth)+1)*tileWidth);
						setX (((mLeftX + 1) * tileWidth) - this.getLeftOffsetBoundingBox());

						// Carlos version.
						//setX (getX()+tileWidth/(getWidth()-1));

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
						setX ((((mRightX) * tileWidth) - getWidth ()) + this.getRightOffsetBoundingBox());

						setState (STATE_STAND);
						return;
					} else {
						// No hay pared, se puede mover.
						setVelX (400);
						setFlip (false);
					}
				}
			}
		} else if (getState () == STATE_PRE_JUMPING) {
			controlMoveHorizontal();
			
			if (!isFloor (getX (), getY () + 1) && getAccelY() == 0) {
				setAccelY(CGameConstants.GRAVITY);
			}

			if(this.isEnded())
			{
				this.setState(STATE_JUMPING);
			}
		} else if (getState () == STATE_JUMPING) {
			controlMoveHorizontal ();

			if(this.getVelY() > 0)
			{
				setState(STATE_FALLING);
				return;
			}

			if (isFloor (getX (), getY () + 1)) {
				setY (mDownY * tileHeight - getHeight ());
				setState (STATE_STAND);
				return;
			}

			if (isRoof (getX (), getY () - 1)) 
			{
				setY (((mUpY + 1) * tileHeight) - this.getTopOffsetBoundingBox());
				setVelY (0);
				setState (STATE_HIT_ROOF);
				return;
			}
		} else if (getState () == STATE_FALLING) {
			controlMoveHorizontal ();

			if (isFloor (getX (), getY () + 1)) {
				setY (mDownY * tileHeight - getHeight ());
				setState (STATE_STAND);
				return;
			}
		} 
		else if (getState () == STATE_HIT_ROOF) {
			if (getTimeState () > 0.02f * 5.0f) 
			{
				setState (STATE_FALLING);
				return;
			}
		}
		else if(getState() == STATE_DASHING) {
			setVelY (0);
			// Si estamos en una pared, corregirnos.
			if (isWallLeft (getX (), mOldY)) 
			{
				// Reposicionar el personaje contra la pared.
				setX (((mLeftX + 1) * CTileMap.Instance.getTileWidth()) - this.getLeftOffsetBoundingBox());
			} 
			if (isWallRight (getX (), mOldY)) 
			{
				// Reposicionar el personaje contra la pared.
				setX ((((mRightX) * CTileMap.Instance.getTileWidth()) - getWidth ()) + this.getRightOffsetBoundingBox());
			} 

			if (isFloor (getX (), getY () + 1)) {
				setY (mDownY * CTileMap.Instance.getTileHeight() - getHeight ());
			}

			if (isRoof (getX (), getY () - 1)) 
			{
				setY (((mUpY + 1) * CTileMap.Instance.getTileHeight()) - this.getTopOffsetBoundingBox());
				
			}

			if(this.getTimeState() >= 0.6f / 3 * 2)
			{
				this.initAnimation(47, 49, 12, false);
			}

			if(this.getTimeState() >= 0.6f) {
				this.setState(STATE_STAND);
			}
		}
		

        // Chequear el paso entre pantallas.
        controlRooms ();

        textoPoderes.setXY(this.getX(), this.getY() - textoPoderes.getHeight());
        textoPoderes.setText(this.powers[this.selectedPower].getName());
        textoPoderes.update();

		for (int i = 0; i < this.powers.Count; i++)
		{
			this.powers[i].update();
		}
    }

	private void controlRooms()
	{
		CTileMap map = CGame.inst ().getMap ();

		if (getX () + getWidth () / 2 > map.getWorldWidth()) 
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

			setX (map.getWorldWidth() - getWidth () / 2);
		} 
		else if (getY () + getHeight () / 2 > map.getWorldHeight()) 
		{
			// Se fue por abajo.
			map.changeRoom(CGameConstants.S);

			setY (-getHeight () / 2);
		} 
		else if (getY () + getHeight () / 2 < 0) 
		{
			// Se fue por arriba.
			map.changeRoom(CGameConstants.W);

			setY(map.getWorldHeight() - getHeight() / 2);
		}
	}

	// Se llama desde los estados jumping y falling para movernos para los costados.
	private void controlMoveHorizontal()
	{
		int tileWidth = CGame.inst().getMap().getTileWidth();
		
		// Si estamos en una pared, corregirnos.
		if (isWallLeft (getX (), mOldY)) 
		{
			// Reposicionar el personaje contra la pared.
			setX (((mLeftX + 1) * tileWidth) - this.getLeftOffsetBoundingBox());
		} 
		if (isWallRight (getX (), mOldY)) 
		{
			// Reposicionar el personaje contra la pared.
			setX ((((mRightX) * tileWidth) - getWidth ()) + this.getRightOffsetBoundingBox());
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
					setX (((mLeftX + 1) * tileWidth) - this.getLeftOffsetBoundingBox());
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
					setX ((((mRightX) * tileWidth) - getWidth ()) + this.getRightOffsetBoundingBox());
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
		mRect2.setXY (getX() + this.getLeftOffsetBoundingBox(), getY() + this.getTopOffsetBoundingBox());
		mRect2.setScaleX(WIDTH - this.getRightOffsetBoundingBox() - this.getLeftOffsetBoundingBox());
		mRect2.setScaleY(HEIGHT - this.getBottomOffsetBoundingBox() - this.getTopOffsetBoundingBox());
		mRect2.update ();

		mRect2.render ();

        textoPoderes.render();

        for (int i = 0; i < this.powers.Count; i++)
        {
            this.powers[i].render();
        }

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
			if(this.getFlip())
			{
				this.setLeftOffsetBoundingBox(32);
				this.setRightOffsetBoundingBox(19);
			}
			else {
				this.setLeftOffsetBoundingBox(19);
				this.setRightOffsetBoundingBox(28);
			}

			this.setTopOffsetBoundingBox(Y_OFFSET_BOUNDING_BOX);
            stopMove ();
			gotoAndStop (1);
            initAnimation(21, 31, 12, true);

        } 
		else if (getState () == STATE_WALKING) 
		{
			initAnimation (1, 12, 12, true);
		}
		else if (getState () == STATE_PRE_JUMPING) 
		{
			initAnimation (13, 15 , 24, false);

		}
		else if (getState () == STATE_JUMPING) 
		{
			initAnimation (16, 17 , 12, true);
			setVelY (CGameConstants.JUMP_SPEED);
			setAccelY (CGameConstants.GRAVITY);

			CAudioManager.Inst.playFX("jump");
		}
		else if (getState () == STATE_FALLING) 
		{
			initAnimation (18, 20, 12, false);
			setAccelY (CGameConstants.GRAVITY);
		}
		else if (getState () == STATE_HIT_ROOF) 
		{
			stopMove();
		}
		else if (getState () == STATE_HIT_ROOF)
		{
			stopMove();
			initAnimation(34, 40, 12, false);
			
		} 
		else if(getState() == STATE_DASHING)
		{
			initAnimation(41, 46, 12, false);
		}

		if(getState() != STATE_DASHING) {
			this.setFriction(1.0f);
		}
	}

    private void selectPower(int power)
    {
		this.powers[this.selectedPower].setInactive();
        this.selectedPower = power;
        if(this.selectedPower > this.powers.Count - 1)
        {
            this.selectedPower = 0;
        }
        else if(this.selectedPower < 0)
        {
            this.selectedPower = this.powers.Count - 1;
        }

		this.powers[this.selectedPower].setActive();
    }

    private void selectNextPower()
    {
        this.selectPower(this.selectedPower + 1);
    }

    private void selectPreviousPower()
    {
        this.selectPower(this.selectedPower - 1);
    }

	public void die()
	{
		this.setXY(this.lastCheckPoint.x, this.lastCheckPoint.y);

		this.setState(STATE_STAND);
	}

	public void setCheckpoint(float x, float y)
	{
		lastCheckPoint = new CVector(x, y);
	}
}
