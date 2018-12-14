using UnityEngine;

public class Water : Power
{
    private const int MAX_ICE_TILES = 1;
    private int iceTileCounter = 0;
    private CAndy player;

    public Water(CAndy player)
    {
        this.name = "Water";
        this.player = player;
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
