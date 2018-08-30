using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AireDisparo : CSprite
{
    public const float SPEED = 1000;
    private CSprite mRect;
    public AireDisparo()
    {
        setImage (Resources.Load<Sprite> ("Sprites/powers/air"));
        setScale(2f);

        setName("PlayerBullet");
        setSortingLayerName("Bullets");

        setBounds(0 - getWidth(), 0 - getHeight(), CGameConstants.SCREEN_WIDTH + getWidth(), CGameConstants.SCREEN_HEIGHT + getHeight());
        setBoundAction(CGameObject.DIE);

        mRect = new CSprite ();
		mRect.setImage (Resources.Load<Sprite> ("Sprites/ui/pixel"));
		mRect.setSortingLayerName ("Player");
		mRect.setSortingOrder (20);
		mRect.setAlpha (0.5f);
		mRect.setName ("player_debug_rect");

        render();
    }

    override public void update()
    {
        base.update();

        // TODO: HACER COLISIONES CON ENEMY SHIP.
        List<CGameObject> enemies = CEnemyManager.inst().collidesList(this);
        if (enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                if(enemy.getState() != CEnemy.STATE_FALLING)
                {
                    enemy.setState(CEnemy.STATE_FALLING);
                }
                enemy.setY(enemy.getY() - 1);
                enemy.setVelX(this.getVelX() / 2);
                enemy.setVelY(this.getVelY() / 2);
            }
        }
    }

    override public void render()
    {
        base.render();

        mRect.setXY (getX(), getY());
		mRect.setScaleX(getWidth());
		mRect.setScaleY(getHeight());
        mRect.setRotation(getRotation());
		mRect.update ();

		mRect.render ();
    }

    override public void destroy()
    {
        base.destroy();
    }
}
