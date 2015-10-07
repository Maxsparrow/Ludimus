using Microsoft.Xna.Framework;

public struct TileType
{
    public Color TileColor;
    public string Type;

    public TileType(Color tileColor, string type)
    {
        TileColor = tileColor;
        Type = type;
    }
}