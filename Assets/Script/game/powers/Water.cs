using UnityEngine;

public class Water : Power
{
    private const int MAX_ICE_TILES = 1;
    private int iceTileCounter = 0;
    private CSprite mRect;
    private CAndy player;



    public Water(CAndy player)
    {
        this.name = "Water";
        this.player = player;



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
    }





    override protected void leftClickPower()
    {



        AguaDisparo bullet = new AguaDisparo();
        //La direcion de la bala
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
        bullet.setVelXY(AireDisparo.SPEED * Mathf.Cos(angRad), AireDisparo.SPEED * Mathf.Sin(angRad));
        CBulletManager.inst().add(bullet);




    }
    override protected void rightClickPower()
    {
        int col = (int)(CMouse.getX() / CTileMap.TILE_WIDTH);
        int row = (int)(CMouse.getY() / CTileMap.TILE_HEIGHT);
        CTile tile = CTileMap.Instance.getTile(col, row);

        if (tile == null)
        {
            return;
        }

        CTile topTile = tile.getNeighbourTile(CTile.Direction.TOP);

        if(tile.getTileType() == CTile.Type.WATER && topTile.getTileType() == CTile.Type.AIR && iceTileCounter < MAX_ICE_TILES)
        {
            CTileMap.Instance.changeTile(tile, CTile.Type.ICE);
            iceTileCounter++;
        }
        else if(tile.getTileType() == CTile.Type.ICE && topTile.getTileType() == CTile.Type.AIR && iceTileCounter > 0)
        {
            CTileMap.Instance.changeTile(tile, CTile.Type.WATER);
            iceTileCounter--;
        }
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
