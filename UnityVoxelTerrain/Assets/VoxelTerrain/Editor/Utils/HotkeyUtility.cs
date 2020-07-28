using System;
using UnityEngine;

public enum HotkeyMode
{
    None,
    BrushMode = KeyCode.B
}

public class HotkeyUtility
{
    public HotkeyMode hotkeyMode;

    private int m_controlId;

    public HotkeyUtility()
    {
        m_controlId = "LOLITerrain".GetHashCode();
    }

    public void KeyDown(KeyCode keyCode)
    {
        int code = (int)keyCode;

        if (code != 0 && Enum.IsDefined(typeof(HotkeyMode), code))
        {
            HotkeyMode mode = (HotkeyMode)code;

            if (mode != HotkeyMode.None)
            {
                LockInput();
            }
        }
    }

    public void KeyUp(KeyCode keyCode)
    {
        if (hotkeyMode != HotkeyMode.BrushMode && (int)keyCode == (int)hotkeyMode)
        {
            UnlockInput();
        }
    }

    public void LockInput()
    {
        if (GUIUtility.hotControl != m_controlId)
            GUIUtility.hotControl = m_controlId;

        Event.current.Use();
    }

    public void UnlockInput()
    {
        if (GUIUtility.hotControl != 0)
            GUIUtility.hotControl = 0;

        Event.current.Use();
    }

    public bool IsInvertedMode()
    {
        return Event.current.shift;
    }
}
