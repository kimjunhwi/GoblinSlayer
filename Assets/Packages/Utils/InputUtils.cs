using System;
using UnityEngine;

/// <summary>
/// Input utils. Control the touch inputs.
/// Autodetect editor or mobile inputs.
/// </summary>
public static class InputUtils {

    /// <summary>
    /// Is the screen pressed?.
    /// </summary>
    /// <param name="limitTouchCount">Limit touch count. Default = Any touch.</param>
    /// <returns><c>true</c>, if screen pressed, <c>false</c> otherwise.</returns>
    public static bool IsScreenPressed(int limitTouchCount = -1) {
        // If we are in the editor, use the mouseinput
        if (Application.isEditor){
            return Input.GetMouseButton(0);
        } else {
            if (limitTouchCount != -1) {
                return (Input.touchCount == limitTouchCount);
            } else {
                return (Input.touchCount > 0);
            }
        }
    }

    /// <summary>
    /// Gets the touch position.
    /// </summary>
    /// <returns>The touch position.</returns>
    public static Vector2 GetOneTouchPosition() {
        // If we are in the editor, use the mouseinput
        if (Application.isEditor) {
            return new Vector2(Input.mousePosition.x ,Input.mousePosition.y);
        } else {
            if (Input.touchCount == 1) {
                return Input.touches[0].position;
            } else {
                return Vector2.zero;
            }
        }
    }
}
