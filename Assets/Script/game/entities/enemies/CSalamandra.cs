using UnityEngine;
using System.Collections;

public class CSalamandra : CEnemy
{
    private const int WIDTH = 481;
    private const int HEIGHT = 283;

    // coordenada y que tenia en el frame anterior. Usada para chequear en la horizontal antes que en la vertical...
    private float mOldY;

    public const int TYPE_DONT_FALL = 0;  // No cae de las plataformas
    public const int TYPE_FALL = 1;       // Cae cuando llega al borde de una plataforma.

    private CAnimatedSprite tongue;

    public CSalamandra(int aType)
    {
        setType(aType);
        setFrames(Resources.LoadAll<Sprite>("Sprites/enemies/fire-salamander"));
        setName("Salamandra");
        setSortingLayerName("Enemies");
        setScale(0.5f);
        setRegistration(CSprite.REG_TOP_LEFT);
        setWidth((int)(WIDTH * 0.5f));
        setHeight((int)(HEIGHT * 0.5f));
        velocityBeforeFalling = 400f;
        setMovable(true);
        setSortingOrder(1);

        horizontalDetectRange = 2;

        setBottomOffsetBoundingBox(34);
        setTopOffsetBoundingBox(23);

        tongue = new CAnimatedSprite();
        tongue.setFrames(Resources.LoadAll<Sprite>("Sprites/enemies/fire-salamander/tongue"));
        tongue.setName("Lengua");
        tongue.setSortingLayerName("Enemies");
        tongue.setRegistration(CSprite.REG_TOP_LEFT);
        tongue.setParent(this.getGameObject());
        tongue.setWidth(0);
        tongue.setHeight(0);    
        tongue.setVisible(false);
        tongue.gotoAndStop(1);
        CEnemyManager.inst().add(tongue);

        
        setState(STATE_STAND);
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
                    setVelX(-200);
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
                    setVelX(200);
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
                setY(mDownY * CTileMap.Instance.getTileHeight() - getHeight() + getBottomOffsetBoundingBox());
                setState(STATE_STAND);
                return;
            }
        }
        else if(getState() == STATE_ATTACKING)
        {
            if(isEnded())
            {
                if(!tongue.isVisible())
                {
                    tongue.setVisible(true);
                    tongue.initAnimation(1,12, 24, false);
                    tongue.setLeftOffsetBoundingBox(480);
                    tongue.setWidth(481);
                    tongue.setHeight(35);
                }
                else
                {
                    switch (tongue.getCurrentFrame())
                    {
                        case 1:
                            tongue.setLeftOffsetBoundingBox(253);
                            break;
                        case 2:
                            tongue.setLeftOffsetBoundingBox(230);
                            break;
                        case 3:
                            tongue.setLeftOffsetBoundingBox(207);
                            break;
                        case 4:
                            tongue.setLeftOffsetBoundingBox(184);
                            break;
                        case 5:
                            tongue.setLeftOffsetBoundingBox(161);
                            break;
                        case 6:
                            tongue.setLeftOffsetBoundingBox(138);
                            break;
                        case 7:
                            tongue.setLeftOffsetBoundingBox(115);
                            break;
                        case 8:
                            tongue.setLeftOffsetBoundingBox(92);
                            break;
                        case 9:
                            tongue.setLeftOffsetBoundingBox(69);
                            break;
                        case 10:
                            tongue.setLeftOffsetBoundingBox(46);
                            break;
                        case 11:
                            tongue.setLeftOffsetBoundingBox(23);
                            break;
                        case 12:
                            tongue.setLeftOffsetBoundingBox(0);
                            break;
                    }

                    if(tongue.isEnded())
                    {
                        tongue.setVisible(false);
                        setState(STATE_STAND);
                        tongue.setWidth(0);
                        tongue.setHeight(0);
                        return;
                    }
                }
            }
        }

        if(this.getFlip())
        {
            tongue.setXY(
                this.getX(),
                this.getY() + 30
            );
        }
        else 
        {
            tongue.setXY(
                this.getX() - tongue.getWidth() / 2,
                this.getY() + 30
            );
        }

        tongue.setFlip(this.getFlip());
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

        tongue.render();
    }

    override public void destroy()
    {
        base.destroy();

        tongue.destroy();
    }

    public override void setState(int aState)
    {
        base.setState(aState);

        if (getState() == STATE_STAND)
        {
            stopMove();
            gotoAndStop(1);
            //initAnimation (1, 2, 12, true);
        }
        else if (getState() == STATE_FALLING)
        {
            velocityBeforeFalling = getVelX() != 0 ? getVelX() : 400;
            initAnimation(1, 2, 12, true);
            setAccelY(CGameConstants.GRAVITY);
        }
        else if (getState() == STATE_WALKING)
        {
            initAnimation(1, 9, 12, true);
        }
        else if(getState() == STATE_ATTACKING)
        {
            velocityBeforeFalling = getVelX() != 0 ? getVelX() : 400;
            stopMove();
            initAnimation(10, 11, 6, false);
        }

        if(getState() != STATE_ATTACKING)
        {
            tongue.setVisible(false);
            tongue.setWidth(0);
            tongue.setHeight(0);
        }
    }
}