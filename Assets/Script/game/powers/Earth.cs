using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Earth : Power
{
    private const int MAX_EARTH_TILES = 3;
    private int earthTileCounter = 0;

    public Earth()
    {
        this.name = "Tierra";
    }

    override public void update()
    {
        base.update();
    }

    override protected void leftClickPower()
    {
        CTile tile = this.getMouseTile();

        if(tile == null)
        {
            return;
        }

        CTile bottomTile = tile.getNeighbourTile(CTile.Direction.BOTTOM);

        if(tile.getTileType() == CTile.Type.AIR && (bottomTile.getTileType() == CTile.Type.MUD || bottomTile.getTileType() == CTile.Type.ARTIFICIAL_EARTH) && earthTileCounter < MAX_EARTH_TILES)
        {
            CTileMap.Instance.changeTile(tile, CTile.Type.ARTIFICIAL_EARTH);
            earthTileCounter++;
        }
    }

    override protected void rightClickPower()
    {
        CTile tile = this.getMouseTile();

        if(tile == null)
        {
            return;
        }

        CTile topTile = tile.getNeighbourTile(CTile.Direction.TOP);

        if(tile.getTileType() == CTile.Type.ARTIFICIAL_EARTH && topTile.getTileType() == CTile.Type.AIR)
        {
            CTileMap.Instance.changeTile(tile, CTile.Type.AIR);
            earthTileCounter--;
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

    private CTile getMouseTile()
    {
        int col = (int)(CMouse.getX() / CTileMap.Instance.getTileWidth());
        int row = (int)(CMouse.getY() / CTileMap.Instance.getTileHeight());
        return CTileMap.Instance.getTile(col, row);
    }
   
}
