using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindTricks : MonoBehaviour {
    public string trickName;
    public int respectGain;
    public int respectIncrease;

    public virtual void PlayAnimation(Animator SkateboardAnim) {}
}

//////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////

public class FiveOGrind: GrindTricks {
    public FiveOGrind() {
        this.trickName = "5-0 Grind";
        this.respectGain = TricksManager.respectAdvanced;
        this.respectIncrease = TricksManager.respectIncrease;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

public class Bluntslide: GrindTricks {
    public Bluntslide() {
        this.trickName = "Bluntslide";
        this.respectGain = TricksManager.respectAdvanced * TricksManager.SecondLevelMultiplier;
        this.respectIncrease = TricksManager.respectIncrease * TricksManager.SecondLevelMultiplier;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

//////////////////////////////////////////////////////////////////////////////////////

public class Boardslide: GrindTricks {
    public Boardslide() {
        this.trickName = "Boardslide";
        this.respectGain = TricksManager.respectBasic;
        this.respectIncrease = TricksManager.respectIncrease;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

public class Darkslide: GrindTricks {
    public Darkslide() {
        this.trickName = "Darkslide";
        this.respectGain = TricksManager.respectBasic * TricksManager.SecondLevelMultiplier;
        this.respectIncrease = TricksManager.respectIncrease * TricksManager.SecondLevelMultiplier;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

//////////////////////////////////////////////////////////////////////////////////////

public class Nosegrind: GrindTricks {
    public Nosegrind() {
        this.trickName = "Nosegrind";
        this.respectGain = TricksManager.respectAdvanced;
        this.respectIncrease = TricksManager.respectIncrease;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

public class Nosebluntslide: GrindTricks {
    public Nosebluntslide() {
        this.trickName = "Nosebluntslide";
        this.respectGain = TricksManager.respectAdvanced * TricksManager.SecondLevelMultiplier;
        this.respectIncrease = TricksManager.respectIncrease * TricksManager.SecondLevelMultiplier;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

//////////////////////////////////////////////////////////////////////////////////////

public class Tailslide: GrindTricks {
    public Tailslide() {
        this.trickName = "Tailslide";
        this.respectGain = TricksManager.respectBasic;
        this.respectIncrease = TricksManager.respectIncrease;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

public class Feeble: GrindTricks {
    public Feeble() {
        this.trickName = "Feeble";
        this.respectGain = TricksManager.respectBasic * TricksManager.SecondLevelMultiplier;
        this.respectIncrease = TricksManager.respectIncrease * TricksManager.SecondLevelMultiplier;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

//////////////////////////////////////////////////////////////////////////////////////

public class FiftyFifty: GrindTricks {
    public FiftyFifty() {
        this.trickName = "50-50";
        this.respectGain = TricksManager.respectBasic;
        this.respectIncrease = TricksManager.respectIncrease;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

public class Overcrook: GrindTricks {
    public Overcrook() {
        this.trickName = "Overcrook";
        this.respectGain = TricksManager.respectBasic * TricksManager.SecondLevelMultiplier;
        this.respectIncrease = TricksManager.respectIncrease * TricksManager.SecondLevelMultiplier;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}