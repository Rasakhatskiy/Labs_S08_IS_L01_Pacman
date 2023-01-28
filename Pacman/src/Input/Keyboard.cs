using System.Data;
using Microsoft.Xna.Framework.Input;

namespace Pacman.Input;

public static class Keyboard
{
    public static KeyboardState CurrentKeyState
    {
        get => _currentKeyState;
        private set => _currentKeyState = value;
    }

    private static KeyboardState _currentKeyState;
    private static KeyboardState _previousKeyState;

    public static void Update()
    {
        _previousKeyState = _currentKeyState;
        _currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
    }
    
    public static KeyboardState GetState()
    {
        return _currentKeyState;
    }

    public static bool IsKeyDown(Keys key)
    {
        return _currentKeyState.IsKeyDown(key);
    }

    public static bool IsKeyPressedOnce(Keys key)
    {
        return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
    }
}