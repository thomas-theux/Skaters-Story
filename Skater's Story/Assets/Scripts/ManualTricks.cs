using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualTricks : MonoBehaviour {
    public string trickName;
    public int respectGain;
    public int respectIncrease;

    public virtual void PlayAnimation(Animator SkateboardAnim) {}
}

//////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////

public class Casper: ManualTricks {
    public Casper() {
        this.trickName = "Casper";
        this.respectGain = TricksManager.respectBasic;
        this.respectIncrease = TricksManager.respectIncrease;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

//////////////////////////////////////////////////////////////////////////////////////

public class Manual: ManualTricks {
    public Manual() {
        this.trickName = "Manual";
        this.respectGain = TricksManager.respectBasic;
        this.respectIncrease = TricksManager.respectIncrease;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

//////////////////////////////////////////////////////////////////////////////////////

public class Primo: ManualTricks {
    public Primo() {
        this.trickName = "Primo";
        this.respectGain = TricksManager.respectBasic;
        this.respectIncrease = TricksManager.respectIncrease;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

//////////////////////////////////////////////////////////////////////////////////////

public class NoseManual: ManualTricks {
    public NoseManual() {
        this.trickName = "Nose Manual";
        this.respectGain = TricksManager.respectBasic;
        this.respectIncrease = TricksManager.respectIncrease;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}