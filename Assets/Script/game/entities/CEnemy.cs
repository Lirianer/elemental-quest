using UnityEngine;
using System.Collections.Generic;
public class CEnemy : CAnimatedSprite
{
    public const int STATE_STAND = 0;
    // ESTADO PARA LEVANTAR EN EL AIRE
    public const int STATE_ELEVATED = 1;
    public const int STATE_FALLING = 2;
    public const int STATE_WALKING = 3;
    public const int STATE_ATTACKING = 4;

    protected int horizontalDetectRange = 1;
    protected int verticalDetectRange = 1;

    public bool isMovable = false;
    
    protected float velocityBeforeFalling = 0;

    private CSprite mRect;
	private CSprite mRect2;
    private CSprite attackRange;

    public CEnemy() 
    {
        mRect = new CSprite ();
		mRect.setImage (Resources.Load<Sprite> ("Sprites/ui/pixel"));
		mRect.setSortingLayerName ("Enemies");
		mRect.setSortingOrder (20);
		mRect.setAlpha (0.5f);
		mRect.setName (this.getName() + "_debug_rect_1");

		mRect2 = new CSprite ();
		mRect2.setImage (Resources.Load<Sprite> ("Sprites/ui/pixel"));
		mRect2.setSortingLayerName ("Enemies");
		mRect2.setSortingOrder (20);
		mRect2.setColor (Color.red);
		mRect2.setAlpha (0.5f);
		mRect2.setName (this.getName() + "_debug_rect_2");

        attackRange = new CSprite();
        attackRange.setImage(Resources.Load<Sprite>("Sprites/ui/pixel"));
        attackRange.setSortingLayerName("Enemis");
        attackRange.setSortingOrder(20);
        attackRange.setColor(Color.blue);
        attackRange.setAlpha(0.5f);
        attackRange.setName(this.getName() + "_debug_attack_range");

        this.setParent(this.getGameObject());
    }

    override public void update()
    {
        base.update();

        if(getState() == STATE_STAND || getState() == STATE_WALKING)
        {
            //Obtener los dos tiles (en vertical) que están adelante mío
            List<CTile> frontTiles = new List<CTile>();
            CAndy player = CGame.inst().getPlayer();
            int yFrom = (int)((getY()));
            int xFrom = 0;

            if(getFlip())
            {
                xFrom = (int)((getX() - getRightOffsetBoundingBox() + getWidth()));
            }
            else 
            {
                xFrom = (int)(getX() + getLeftOffsetBoundingBox() - CTileMap.Instance.getTileWidth() * horizontalDetectRange);
            }

            attackRange.setXY(xFrom, yFrom);
            attackRange.setWidth(horizontalDetectRange * CTileMap.Instance.getTileWidth());
            attackRange.setHeight(verticalDetectRange * CTileMap.Instance.getTileHeight());
            attackRange.setScaleX(horizontalDetectRange * CTileMap.Instance.getTileWidth());
            attackRange.setScaleY(verticalDetectRange * CTileMap.Instance.getTileHeight());

            if(attackRange.collidesRect(player))
            {
                setState(STATE_ATTACKING);
                return;
            }

           
        }
    }

    override public void render()
    {
        base.render();

        // MOSTRAR TODA EL AREA DEL DIBUJO.
		mRect.setXY (getX(), getY());
		mRect.setScaleX(this.getWidth());
		mRect.setScaleY(this.getHeight());
		mRect.update ();

		mRect.render ();

		// Bounding box.
		mRect2.setXY (getX() + this.getLeftOffsetBoundingBox(), getY() + this.getTopOffsetBoundingBox());
		mRect2.setScaleX(this.getWidth() - this.getRightOffsetBoundingBox() - this.getLeftOffsetBoundingBox());
		mRect2.setScaleY(this.getHeight() - this.getBottomOffsetBoundingBox() - this.getTopOffsetBoundingBox());
		mRect2.update ();

		mRect2.render ();

        attackRange.render();
    }

    override public void destroy()
    {
        base.destroy();
    }

    override public void setState(int aState)
    {
        base.setState(aState);
    }

    override public void setName(string aName)
    {
        mRect.setName (aName + "_debug_rect_1");
        mRect2.setName (aName + "_debug_rect_2");

        base.setName(aName);
    }
    
}