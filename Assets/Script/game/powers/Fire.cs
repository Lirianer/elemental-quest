using UnityEngine;

public class Fire : Power
{



     private CAndy player;

    private const float FIRE_COOLDOWN = 0.9f;
    private const int DASH_VELOCITY = 2500;
  
    private CSprite shield;

    private const int WIDTH = 64 * 4;
    private const int HEIGHT = 74 * 4;

    private bool burning;

    private const int BURNING_TIME = 100;
    private int currentTime;
   

    public Fire(CAndy player)
    {
        this.name = "Fire";
        this.player = player;
        this.leftPowerBaseCooldown = FIRE_COOLDOWN;




        shield = new CSprite();
        shield.setImage(Resources.Load<Sprite>("Sprites/ui/pixel"));
        shield.setSortingLayerName("Player");
        shield.setSortingOrder(20);
        shield.setColor(Color.red);
        shield.setAlpha(0.5f);
        shield.setName("Fire - Shield");
        shield.setParent(this.player.getGameObject());
        //Seteo la escala para el persona

        shield.setScaleX(WIDTH);
        shield.setScaleY(HEIGHT);
        //Pongo el poder a false
        shield.setVisible(false);

        currentTime = 0;
    }

    override public void update()
    {
        base.update();

        
        //Si el perosnaje se esta quemando 
        if (burning)
        {
            //Seteo el poder en el medio del personaje 
            shield.setXY(player.getX() + player.getWidth() / 2 - WIDTH / 2, player.getY() - (HEIGHT - player.getHeight()));
            if (currentTime < BURNING_TIME)
            {
                currentTime++;
            }
            else
            {
                //Si el tiempo se pone en 0 se pone no visiblee el cuadradodo

                shield.setVisible(false);
                burning = false;
                currentTime = 0;
            }
        }

 
        shield.update();


    }
    
    override protected void rightClickPower ()
    {

        //Si Current time 
        if (currentTime == 0)
        {
            shield.setVisible(true);
            burning = true;
        }
        
    }

    override protected void leftClickPower()
    {
        // Cuento para que no sean mas de dos disparos 

        FuegoDisparo bullet = new FuegoDisparo();

        CVector playerCenter = new CVector(player.getX() + player.getWidth() / 2, player.getY() + player.getHeight() / 2);
        float radius = player.getHeight() > player.getWidth() ? player.getHeight() / 2 : player.getWidth() / 2;
        float xDiff = CMouse.getX() - playerCenter.x;
        float yDiff = CMouse.getY() - playerCenter.y;
        float angRad = Mathf.Atan2(yDiff, xDiff);

        bullet.setRotation(CMath.radToDeg(angRad));
        // X: Centro del personaje + ancho / 2 * angulo
        bullet.setX(playerCenter.x + radius * Mathf.Cos(angRad));
        // Y: Centro del personaje + alto / 2 * angulo
        bullet.setY(playerCenter.y + radius * Mathf.Sin(angRad));
        bullet.setVelXY(FuegoDisparo.SPEED * Mathf.Cos(angRad), FuegoDisparo.SPEED * Mathf.Sin(angRad));
        bullet.setBounds(0, 0, CTileMap.Instance.getMapWidth() * CTileMap.Instance.getTileWidth(), CTileMap.Instance.getMapHeight() * CTileMap.Instance.getTileHeight());
        CBulletManager.inst().add(bullet);

        Debug.Log("Ya hice todo y se supone estoy disparando wn maraca ql");
    }

    override public void render()
    {
        base.render();

         // MOSTRAR TODA EL AREA DEL DIBUJO.
     

        shield.render();

    }

    override public void destroy()
    {
        base.destroy();

        shield.destroy();
        shield = null;
    }
}
