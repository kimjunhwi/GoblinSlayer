using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//최상위 계층
public class Controller : MonoBehaviour
{
    public enum E_TAG
    {
        E_NONE,
        E_PLAYER,
        E_ENEMY,
        E_NPC
    }

    public E_TAG ObjectTag = E_TAG.E_NONE;
}
