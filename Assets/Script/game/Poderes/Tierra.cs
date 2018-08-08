using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tierra : CAnimatedSprite
{
    public Tierra()
    {
        setFrames(Resources.LoadAll<Sprite>("Sprites/tierra"));
       
        setName("PoderTierra");
        setSortingLayerName("Player");
        setScale(2);
 
    }



    override public void update()
    {
        base.update();
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
