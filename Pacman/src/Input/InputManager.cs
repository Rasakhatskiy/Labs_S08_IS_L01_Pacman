using Microsoft.Xna.Framework.Input;

namespace Pacman.Input;

public static class InputManager
{
    public static void Update()
    {
        Keyboard.Update();

        if (Keyboard.CurrentKeyState.GetPressedKeyCount() <= 0) return;
    }
}