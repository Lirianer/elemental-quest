using UnityEngine;
using System.Collections;

public class CGolemTierra : CEnemy
{
    private const int WIDTH = 462;
    private const int HEIGHT = 295;

    // coordenada y que tenia en el frame anterior. Usada para chequear en la horizontal antes que en la vertical...
    private float mOldY;

    public const int TYPE_DONT_FALL = 0;  // No cae de las plataformas
    public const int TYPE_FALL = 1;       // Cae cuando llega al borde de una plataforma.

    public CGolemTierra(int aType)
    {
        setType(aType);
        setFrames(Resources.LoadAll<Sprite>("Sprites/enemies/earth-golem"));
        setName("GolemTierra");
        setSortingLayerName("Enemies");
        setScale(1.0f);
        setRegistration(CSprite.REG_TOP_LEFT);
        setWidth(WIDTH);
        setHeight(HEIGHT);
        setState(STATE_STAND);
        velocityBeforeFalling = 400f;
        setTopOffsetBoundingBox(50);

        verticalDetectRange = 3;
    }

    private void setOldYPosition()
    {
        mOldY = getY();
    }

    override public void update()
    {
        //Debug.Log (getState ());

        // Guardar la posicion anterior del objeto.
        setOldYPosition();

        base.update();

        if (getState() == STATE_STAND)
        {
            // En stand no deberia pasar nunca que quede metido en una pared. 
            // Si estamos en una pared, corregirnos. 
            if (isWallLeft(getX(), getY()))
            {
                // Reposicionar el personaje contra la pared.
                setX((mLeftX + 1) * CTileMap.Instance.getTileWidth());
            }
            if (isWallRight(getX(), getY()))
            {
                // Reposicionar el personaje contra la pared.
                setX(((mRightX) * CTileMap.Instance.getTileWidth()) - getWidth());
            }

            // Si en el pixel de abajo del jugador no hay piso, caemos.
            if (!isFloor(getX(), getY() + 1))
            {
                setState(STATE_FALLING);
                return;
            }

            setVelX(velocityBeforeFalling);
            setState(STATE_WALKING);
        }
        else if (getState() == STATE_WALKING)
        {
            if (isWallLeft(getX(), getY()))
            {
                // Reposicionar el personaje contra la pared.
                setX((mLeftX + 1) * CTileMap.Instance.getTileWidth());

                setVelX(getVelX() * -1);
            }
            if (isWallRight(getX(), getY()))
            {
                // Reposicionar el personaje contra la pared.
                setX(((mRightX) * CTileMap.Instance.getTileWidth()) - getWidth());
                setVelX(getVelX() * -1);
            }

            // Si en el pixel de abajo del jugador no hay piso, caemos.
            if (!isFloor(getX(), getY() + 1))
            {
                setState(STATE_FALLING);
                return;
            }

            if (getVelX() < 0)
            {
                // Chequear pared a la izquierda.
                // Si hay pared a la izquierda vamos a stand.
                if (isWallLeft(getX(), getY()))
                {
                    // Reposicionar el personaje contra la pared.
                    //setX((((int) getX ()/CTileMap.Instance.getTileWidth())+1)*CTileMap.Instance.getTileWidth());
                    setX((mLeftX + 1) * CTileMap.Instance.getTileWidth());

                    setVelX(getVelX() * -1);

                    return;
                }
                else
                {
                    // No hay pared, se puede mover.
                    setVelX(-400);
                    setFlip(false);

                    if (getType() == TYPE_DONT_FALL)
                    {
                        checkPoints(getX(), getY() + 1);
                        if (mTileDownLeft)
                        {
                            setVelX(getVelX() * -1);
                        }
                    }
                }
            }
            else if (getVelX() > 0)
            {
                // Chequear pared a la derecha.
                // Si hay pared a la derecha vamos a stand.
                if (isWallRight(getX(), getY()))
                {
                    // Reposicionar el personaje contra la pared.
                    setX(((mRightX) * CTileMap.Instance.getTileWidth()) - getWidth());

                    setVelX(getVelX() * -1);
                    return;
                }
                else
                {
                    // No hay pared, se puede mover.
                    setVelX(400);
                    setFlip(true);

                    if (getType() == TYPE_DONT_FALL)
                    {
                        checkPoints(getX(), getY() + 1);
                        if (mTileDownRight)
                        {
                            setVelX(getVelX() * -1);
                        }
                    }
                }
            }
        }
        else if (getState() == STATE_FALLING)
        {
            controlMoveHorizontal();

            if (isFloor(getX(), getY() + 1))
            {
                setY(mDownY * CTileMap.Instance.getTileHeight() - getHeight());
                setState(STATE_STAND);
                return;
            }
        }
        else if(getState() == STATE_ATTACKING)
        {

            switch (this.getCurrentFrame())
            {
                case 16:
                    setLeftOffsetBoundingBox(148);
                    setTopOffsetBoundingBox(100);
                    setRightOffsetBoundingBox(110);
                break;
                case 17:
                    setLeftOffsetBoundingBox(168);
                    setTopOffsetBoundingBox(135);
                    setRightOffsetBoundingBox(15);
                break;
                case 18:
                    setLeftOffsetBoundingBox(168);
                    setTopOffsetBoundingBox(44);
                    setRightOffsetBoundingBox(52);
                break;
                case 19:
                    setLeftOffsetBoundingBox(132);
                    setTopOffsetBoundingBox(15);
                    setRightOffsetBoundingBox(151);
                break;
                case 20:
                    setLeftOffsetBoundingBox(30);
                    setTopOffsetBoundingBox(10);
                    setRightOffsetBoundingBox(164);
                break;
                case 21:
                    setLeftOffsetBoundingBox(17);
                    setTopOffsetBoundingBox(100);
                    setRightOffsetBoundingBox(165);
                break;
            }

            if(isEnded()) 
            {
                setState(STATE_STAND);
                return;
            }
        }
    }



    // Se llama desde los estados jumping y falling para movernos para los costados.
    private void controlMoveHorizontal()
    {
        // Si estamos en una pared, corregirnos.                // ESTE BLOQUE ES IGUAL A ANDY ---- ()?
        if (isWallLeft(getX(), mOldY))
        {
            // Reposicionar el personaje contra la pared.
            setX((mLeftX + 1) * CTileMap.Instance.getTileWidth());
        }
        if (isWallRight(getX(), mOldY))
        {
            // Reposicionar el personaje contra la pared.
            setX(((mRightX) * CTileMap.Instance.getTileWidth()) - getWidth());
        }                                                          // --------------------------
    }

    override public void render()
    {
        base.render();
    }

    override public void destroy()
    {
        base.destroy();
    }

    public override void setState(int aState)
    {
        base.setState(aState);

        if (getState() == STATE_STAND)
        {
            setLeftOffsetBoundingBox(126);
            setRightOffsetBoundingBox(126);
            setTopOffsetBoundingBox(128);
            stopMove();
            gotoAndStop(1);
            //initAnimation (1, 2, 12, true);
        }
        else if (getState() == STATE_FALLING)
        {
            setLeftOffsetBoundingBox(126);
            setRightOffsetBoundingBox(126);
            setTopOffsetBoundingBox(128);
            velocityBeforeFalling = getVelX() != 0 ? getVelX() : 400;
            initAnimation(1, 2, 6, true);
            setAccelY(CGameConstants.GRAVITY);
        }
        else if (getState() == STATE_WALKING)
        {
            setLeftOffsetBoundingBox(126);
            setRightOffsetBoundingBox(126);
            setTopOffsetBoundingBox(128);
            initAnimation(1, 9, 13, true);
          //  setAccelY(CGameConstants.GRAVITY);
        }
        else if(getState() == STATE_ATTACKING)
        {
            stopMove();
            initAnimation(10, 21, 9, false);
        }
    }
}