using UnityEngine;

public class Air : Power
{
    private CAndy player;

    private const float DASH_COOLDOWN = 3f;
    private const int DASH_VELOCITY = 2500;

    public Air(CAndy player)
    {
        this.name = "Air";
        this.player = player;
        this.leftPowerBaseCooldown = DASH_COOLDOWN;
    }

    override public void update()
    {
        base.update();
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
        else {
            player.setFlip(false);
        }

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
