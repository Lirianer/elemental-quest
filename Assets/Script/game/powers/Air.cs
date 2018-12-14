using UnityEngine;

public class Air : Power
{
    private CAndy player;

    private const float DASH_COOLDOWN = 1.5f;
    private const float FIRE_COOLDOWN = 0.9f;
    private const int DASH_VELOCITY = 833;
    private const int WIDTH = 64 * 2;
    private const int HEIGHT = 74 * 2;

    public Air(CAndy player)
    {
        this.name = "Air";
        this.player = player;
        this.rightPowerBaseCooldown = DASH_COOLDOWN;
        this.leftPowerBaseCooldown = FIRE_COOLDOWN;
    }

    override public void update()
    {
        base.update();
    }

    
     override protected void leftClickPower()
    { 
        AireDisparo bullet = new AireDisparo();
        //La direcion de la bala
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
        bullet.setBounds(0, 0, CTileMap.Instance.getMapWidth() * CTileMap.Instance.getTileWidth(), CTileMap.Instance.getMapHeight() * CTileMap.Instance.getTileHeight());
        CBulletManager.inst().add(bullet); 

    }
    
    override protected void rightClickPower()
    {
        int direction = 1;

        

        if(CMouse.getX() <= player.getX() + player.getWidth() / 2)
        {
            direction = -1;
            player.setFlip(true);
        }
        else
        {
            player.setFlip(false);
        }

        player.setState(CAndy.STATE_DASHING);
        player.setVelX(DASH_VELOCITY * direction);
    }

   

    override public void render()
    {
        base.render();
    }

    override public void destroy()
    {
        base.destroy();
    }



   
}
