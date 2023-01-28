
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacman;

namespace BebraProject.Animation;

/// <summary>
/// Spritesheet for animations or sprites with many rows and columns
/// </summary>
public class Spritesheet
{
    public Texture2D Texture { get; private set; }
    public Point NumFrames { get; private set; }
    public Point TileSize { get; private set; }

    public Spritesheet(string resourceName, Point numFrames)
    : this(Globals.Content.Load<Texture2D>(resourceName), numFrames) { }
    
    public Spritesheet(Texture2D texture, Point numFrames)
    {
        Texture = texture;
        NumFrames = numFrames;
        TileSize = new Point(
            (int)(Texture.Width / NumFrames.X),
            (int)(Texture.Height / NumFrames.Y));
    }

    public Rectangle GetRectangle(Vector2 coords)
    {
        return new Rectangle(
            (int)coords.X * TileSize.X, 
            (int)coords.Y * TileSize.Y, 
            TileSize.X, 
            TileSize.Y);
    }
}