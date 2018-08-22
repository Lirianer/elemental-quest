using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Air : Power
{
    private CAndy player;
    private bool recentlyDashed = false;

    private const float DASH_COOLDOWN = 3f;
    private float dashElapsedTime = 0f;

    public Air(CAndy player)
    {
        this.name = "Air";
        this.player = player;
    }

    override public void update()
    {
        base.update();

        if(!this.recentlyDashed && CMouse.firstPress(CMouse.BUTTONS.LEFT))
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

            player.setVelX(2500 * direction);

            this.recentlyDashed = true;
        }
        else if(this.recentlyDashed)
        {
            this.dashElapsedTime += Time.deltaTime;

            if(this.dashElapsedTime >= Air.DASH_COOLDOWN) 
            {
                this.recentlyDashed = false;
                this.dashElapsedTime = 0f;
            }
        }
    }

    override public void render()
    {
        base.render();
    }

    public void destroy()
    {
        base.destroy();
    }



   
}
