using System.Collections.Generic;
using BebraProject.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacman;

namespace BebraProject.Animation;

public class Animation : IDrawing
{
    public struct FrameSet
    {
        public int Row;
        public int StartColumn;
        public int Frames;
    }
    
    private readonly List<Rectangle> _sourceRectangles = new();
    public int TotalFrames { get; protected set; }
    public int CurrentFrame{ get; protected set; }
    private readonly float _frameTime;
    private float _frameTimeLeft;
    private bool _active = true;
    private readonly Spritesheet _spritesheet;

    public Animation(Spritesheet spritesheet, float frameTime, 
        int row = -1, int startColumn = -1, int frames = -1)
    {
        _frameTime = frameTime;
        _frameTimeLeft = _frameTime;
        TotalFrames = 0;
        CurrentFrame = 0;
        _spritesheet = spritesheet;

        if (row >= 0 && startColumn >= 0 && frames > 0)
            AddFrameSet(row, startColumn, frames);
    }

    public void AddFrameSet(int row, int startColumn, int frames)
    {
        TotalFrames += frames;
        for (int i = startColumn; i < startColumn + frames; i++)
            _sourceRectangles.Add(_spritesheet.GetRectangle(new Vector2(i, row)));
    }
    

    public void Stop()
    {
        _active = false;
    }

    public void Start()
    {
        _active = true;
    }

    public void Reset()
    {
        CurrentFrame = 0;
        _frameTimeLeft = _frameTime;
    }

    public void Update()
    {
        if (!_active) return;

        _frameTimeLeft -= Globals.TotalSeconds;

        if (_frameTimeLeft <= 0)
        {
            _frameTimeLeft += _frameTime;
            CurrentFrame = (CurrentFrame + 1) % TotalFrames;
        }
    }

    public void Draw(Vector2 pos, bool flipX = false, bool flipY = false, float rotation = 0f)
    {
        var spriteEffects = SpriteEffects.None;
        if (flipX)
        {
            spriteEffects |= SpriteEffects.FlipHorizontally;
        }

        if (flipY)
        {
            spriteEffects |= SpriteEffects.FlipVertically;
        }
        
        // pos += camera.Position;
        
        Globals.SpriteBatch.Draw(
            _spritesheet.Texture, 
            pos, 
            _sourceRectangles[CurrentFrame], 
            Color.White, 
            0, 
            Vector2.Zero, 
            Vector2.One, 
            spriteEffects, 
            1);
    }
}