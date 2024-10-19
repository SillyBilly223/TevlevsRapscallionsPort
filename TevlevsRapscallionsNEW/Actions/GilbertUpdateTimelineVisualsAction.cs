using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TevlevsRapscallionsNEW.Actions
{
    public class GilbertUpdateTimelineVisualsAction : CombatAction
    {
        public TurnUIInfo[] turnUIInfos;

        public GilbertUpdateTimelineVisualsAction(TurnUIInfo[] UIInfos)
        {
            turnUIInfos = UIInfos;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            yield return ExtraUtils.GiblertAddTimelineSlots(turnUIInfos);
        }
    }
}
