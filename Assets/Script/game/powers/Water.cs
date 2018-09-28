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

    override protected void rightClickPower()
    {
        int col = (int)(CMouse.getX() / CTileMap.Instance.getTileWidth());
        int row = (int)(CMouse.getY() / CTileMap.Instance.getTileHeight());
        CTile tile = CTileMap.Instance.getTile(col, row);

        if (tile == null)
        {
            return;
        }

        CTile topTile = tile.getNeighbourTile(CTile.Direction.TOP);

        if(tile.getTileType() == CTile.Type.WATER && topTile.getTileType() == CTile.Type.AIR && iceTileCounter < MAX_ICE_TILES)
        {
            CTileMap.Instance.changeTile(tile, CTile.Type.ICE, 15);
            iceTileCounter++;
        }
        else if(tile.getTileType() == CTile.Type.ICE && topTile.getTileType() == CTile.Type.AIR && iceTileCounter > 0)
        {
            CTileMap.Instance.changeTile(tile, CTile.Type.WATER, 17);
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
