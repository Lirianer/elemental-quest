using UnityEngine;

public class Air : Power
{
    private CAndy player;

    private const float DASH_COOLDOWN = 3f;
    private const int DASH_VELOCITY = 2500;
    private CSprite mRect;
    private const int WIDTH = 64 * 2;
    private const int HEIGHT = 74 * 2;

    public Air(CAndy player)
    {
        this.name = "Air";
        this.player = player;
        this.leftPowerBaseCooldown = DASH_COOLDOWN;

        mRect = new CSprite();
        mRect.setImage(Resources.Load<Sprite>("Sprites/ui/pixel"));
        mRect.setSortingLayerName("Player");
        mRect.setSortingOrder(20);
        mRect.setAlpha(0.5f);
        mRect.setName("Segundo poder");
    }

    override public void update()
    {
        base.update();
        mRect.update();
    }

    override protected void leftClickPower()
    {
         int direction = 1;

        player.setState(CAndy.STATE_DASHING);

        if(CMouse.getX() <= player.getX() + player.getWidth() / 2)
        {
            direction = -1;
            player.setFlip(true);
        }
        else
        {
            player.setFlip(false);
        }

        player.setVelX(DASH_VELOCITY * direction);
    }

    override protected void rightClickPower()
    { 
        AireDisparo bullet = new AireDisparo();

        CVector playerCenter = new CVector(player.getX() + player.getWidth() / 2, player.getY() + player.getHeight() / 2);
        float radius = player.getHeight() > player.getWidth() ? player.getHeight() / 2 : player.getWidth() / 2;
        float xDiff = CMouse.getX() - playerCenter.x;
		float yDiff = CMouse.getY() - playerCenter.y;
		float angRad = Mathf.Atan2 (yDiff, xDiff);

        bullet.setRotation(CMath.radToDeg(angRad));
        // X: Centro del personaje + ancho / 2 * angulo
        bullet.setX(playerCenter.x + radius * Mathf.Cos(angRad));
        // Y: Centro del personaje + alto / 2 * angulo
        bullet.setY(playerCenter.y + radius * Mathf.Sin(angRad));
        bullet.setVelXY (AireDisparo.SPEED * Mathf.Cos (angRad), AireDisparo.SPEED * Mathf.Sin (angRad));
        CBulletManager.inst().add(bullet);
    }

    override public void render()
    {
        base.render();

        // MOSTRAR TODA EL AREA DEL DIBUJO.
        mRect.setXY(player.getX(), player.getY());
        mRect.setScaleX(WIDTH);
        mRect.setScaleY(HEIGHT);
        
        mRect.render();
    }

    override public void destroy()
    {
        base.destroy();
    }



   
}
