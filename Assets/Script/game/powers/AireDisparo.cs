using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AireDisparo : CAnimatedSprite
{
    public const float SPEED = 1000;
    private CSprite mRect;
    public AireDisparo()
    {
        setFrames(Resources.LoadAll<Sprite> ("Sprites/powers/air"));
        setScale(2f);
        initAnimation(1, 3, 12, true);
        setName("AirShot");
        setSortingLayerName("Bullets");

        setBounds(0 - getWidth(), 0 - getHeight(), CGameConstants.SCREEN_WIDTH + getWidth(), CGameConstants.SCREEN_HEIGHT + getHeight());
        setBoundAction(CGameObject.DIE);
        setLeftOffsetBoundingBox(105);
        setBottomOffsetBoundingBox(12);
        setRightOffsetBoundingBox(7);
        setRightOffsetBoundingBox(12);

        mRect = new CSprite ();
		mRect.setImage (Resources.Load<Sprite> ("Sprites/ui/pixel"));
		mRect.setSortingLayerName ("Player");
		mRect.setSortingOrder (20);
		mRect.setAlpha (0.5f);
		mRect.setName ("Aire Disparo");
        mRect.setParent(this.getGameObject());
        mRect.setX(this.getLeftOffsetBoundingBox());
        mRect.setY(-getHeight() / 2);

        render();
    }

    override public void update()
    {
        base.update();
        mRect.update();

        // TODO: HACER COLISIONES CON ENEMY SHIP.
        //Si la lista fuera de CEnemy(ies), las funciones particulares no tendrian que estar en  CGameObject.
        List<CGameObject> enemies = CEnemyManager.inst().collidesList(this);
        if (enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.getMovable())
                {
                    

                    if (enemy.getState() != CEnemy.STATE_FALLING)
                    {
                        enemy.setState(CEnemy.STATE_FALLING);
                        //TODO y si me aumenta la velocidad?
                        
                    }

                    enemy.setY(enemy.getY() - 1);
                    enemy.setVelX(this.getVelX() / 2);
                    enemy.setVelY(this.getVelY() / 2);
                }
                
            }
        }

        List<CTile> tiles = CTileMap.Instance.getTilesCollidingRect(getX() + getLeftOffsetBoundingBox(), getY() + getTopOffsetBoundingBox(), getWidth() - getRightOffsetBoundingBox(), getHeight() - getBottomOffsetBoundingBox());

        foreach (var tile in tiles)
        {
            if(tile.getTileType() != CTile.Type.AIR) 
            {
                this.setDead(true);
                break;
            }
        }
    }

    override public void render()
    {
        base.render();

        mRect.setXY (getX(), getY() - getHeight());
		mRect.setScaleX(getWidth());
		mRect.setScaleY(getHeight());
        mRect.setRotation(getRotation());
		

		mRect.render ();
    }

    override public void destroy()
    {
        base.destroy();
        mRect.destroy();

    }
}
