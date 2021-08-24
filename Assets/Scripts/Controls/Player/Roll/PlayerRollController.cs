/// <remarks>
/// 
/// PlayerRollController is used for triggering roll mechinics using player input
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;

using Custom.Mechanics;

namespace Custom.Controlls
{
    [RequireComponent(typeof(Roll))]
    public class PlayerRollController : PlayerMechanicsController<EventArgs, Roll>
    {
    }
}
