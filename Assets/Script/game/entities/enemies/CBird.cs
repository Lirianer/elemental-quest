﻿using UnityEngine;
using System.Collections;

public class CBird : CEnemy
{
    private const int WIDTH = 72 * 2;
    private const int HEIGHT = 58 * 2;

    // coordenada y que tenia en el frame anterior. Usada para chequear en la horizontal antes que en la vertical...
    private float mOldY;

    public const int TYPE_DONT_FALL = 0;  // No cae de las plataformas
    public const int TYPE_FALL = 1;       // Cae cuando llega al borde de una plataforma.

    public CBird(int aType)
    {
        setType(aType);
        setFrames(Resources.LoadAll<Sprite>("Sprites/enemies/air-bird"));
        setName("Pajaro");
        setSortingLayerName("Enemies");
        setScale(2.0f);
        setRegistration(CSprite.REG_TOP_LEFT);
        setWidth(WIDTH);
        setHeight(HEIGHT);
        setState(STATE_STAND);
        velocityBeforeFalling = 400f;
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



            //setVelX(velocityBeforeFalling);
            setState(STATE_WALKING);
        }
        else if (getState() == STATE_WALKING)

        {

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
                    setFlip(true);
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
                    setFlip(false);


                }


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
            stopMove();
            gotoAndStop(1);
            //Para poner la animacion del personaje idle 
            //initAnimation (1, 2, 12, true);
        }
        
        else if (getState() == STATE_WALKING)
        {
            initAnimation(1, 2, 12, true);
            controlMoveHorizontal();
            setVelX(-1);



        }
    }
}