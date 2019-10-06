using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using EnsoulSharp;
using EnsoulSharp.Common;
using Color = System.Drawing.Color;

namespace HeavenStrikeTalon
{
    using static Program;
    public static class Gettarget
    {
        public static bool Selected()
        {
            if (!GetTarget)
            {
                return false;
            }
            else
            {
                var target = TargetSelector.SelectedTarget;
                if (!target.IsValidTarget() || target.IsZombie || Player.Position.To2D().Distance(target.Position.To2D()) > GetRange)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static AIBaseClient TargetSelect(float range, Vector3? rangecheckfrom = null)
        {
            if (Selected())
            {
                return TargetSelector.SelectedTarget.IsValidTarget(range,true,rangecheckfrom == null ? default(Vector3) : (Vector3) rangecheckfrom )? 
                    TargetSelector.SelectedTarget : null;
            }
            else
            {
                return TargetSelector.GetTarget(range, TargetSelector.DamageType.Physical,true,null,rangecheckfrom);
            }
        }

    }
}

