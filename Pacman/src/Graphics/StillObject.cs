using System.Collections.Specialized;
using BebraProject.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacman;

namespace BebraProject.Animation;

public class StillObject : IDrawing
{
    public Spritesheet Spritesheet { get; }
    public Rectangle Rectangle { get; }

    public StillObject(Spritesheet spritesheet, Vector2 position)
    {
        Spritesheet = spritesheet;
        Rectangle = spritesheet.GetRectangle(position);
    }

    public void Draw(Vector2 pos, bool flipX = false, bool flipY = false, float rotation = 0f, Vector2? origin = null)
    {
        var spriteEffects = SpriteEffects.None;
        if (flipX)
            spriteEffects |= SpriteEffects.FlipHorizontally;
        if (flipY)
            spriteEffects |= SpriteEffects.FlipVertically;

        // pos += camera.Position;
        
        Globals.SpriteBatch.Draw(
            Spritesheet.Texture, 
            pos,
            Rectangle, 
            Color.White, 
            rotation, 
            Vector2.Zero, 
            Vector2.One,
            spriteEffects, 
            1);
    }

    public void Stop()
    {
    }

    public void Start()
    {
    }

    public void Reset()
    {
    }

    public void Update()
    {
    }
}