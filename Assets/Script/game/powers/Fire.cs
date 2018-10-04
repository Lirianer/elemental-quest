using UnityEngine;

public class Fire : Power
{



     private CAndy player;

    private const float FIRE_COOLDOWN = 0.9f;
    private const int DASH_VELOCITY = 2500;
  
    private CSprite mRect2;

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




        mRect2 = new CSprite();
        mRect2.setImage(Resources.Load<Sprite>("Sprites/ui/pixel"));
        mRect2.setSortingLayerName("Player");
        mRect2.setSortingOrder(20);
        mRect2.setColor(Color.red);
        mRect2.setAlpha(0.5f);
        mRect2.setName("Segundo Poder Escudo");
        //Seteo la escala para el persona

        mRect2.setScaleX(WIDTH);
        mRect2.setScaleY(HEIGHT);
        //Pongo el poder a false
        mRect2.setVisible(false);

        currentTime = 0;
    }

    override public void update()
    {
        base.update();

        
        //Si el perosnaje se esta quemando 
        if (burning)
        {
            //Seteo el poder en el medio del personaje 
            mRect2.setXY(player.getX() + player.getWidth() / 2 - WIDTH / 2, player.getY() - (HEIGHT - player.getHeight()));
            if (currentTime < BURNING_TIME)
            {
                currentTime++;
            }
            else
            {
                //Si el tiempo se pone en 0 se pone no visiblee el cuadradodo

                mRect2.setVisible(false);
                burning = false;
                currentTime = 0;
            }
        }

 
        mRect2.update();


    }
    
    override protected void rightClickPower ()
    {

        //Si Current time 
        if (currentTime == 0)
        {
            mRect2.setVisible(true);
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
     

        mRect2.render();

    }

    override public void destroy()
    {
        base.destroy();

        mRect2.destroy();
        mRect2 = null;
    }
}
