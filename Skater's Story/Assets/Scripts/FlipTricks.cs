using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTricks: MonoBehaviour {
    public string trickName;
    public int respectGain;

    public virtual void PlayAnimation(Animator SkateboardAnim) {}
}

//////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////

public class PopShoveIt: FlipTricks {
    public PopShoveIt() {
        this.trickName = "Pop Shove-It";
        this.respectGain = TricksManager.respectBasic;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

public class ThreeSixtyShoveIt: FlipTricks {
    public ThreeSixtyShoveIt() {
        this.trickName = "360 Shove-It";
        this.respectGain = TricksManager.respectBasic * TricksManager.LevelUpMultiplier;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

//////////////////////////////////////////////////////////////////////////////////////

public class Kickflip: FlipTricks {
    public Kickflip() {
        this.trickName = "Kickflip";
        this.respectGain = TricksManager.respectBasic;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

public class DoubleKickflip: FlipTricks {
    public DoubleKickflip() {
        this.trickName = "Double Kickflip";
        this.respectGain = TricksManager.respectBasic * TricksManager.LevelUpMultiplier;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

//////////////////////////////////////////////////////////////////////////////////////

public class ThreeSixtyFlip: FlipTricks {
    public ThreeSixtyFlip() {
        this.trickName = "360 Flip";
        this.respectGain = TricksManager.respectAdvanced;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

public class SevenTwentyFlip: FlipTricks {
    public SevenTwentyFlip() {
        this.trickName = "720 Flip";
        this.respectGain = TricksManager.respectAdvanced * TricksManager.LevelUpMultiplier;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

//////////////////////////////////////////////////////////////////////////////////////

public class Impossible: FlipTricks {
    public Impossible() {
        this.trickName = "Impossible";
        this.respectGain = TricksManager.respectBasic;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

public class ImpossibleThreeSixty: FlipTricks {
    public ImpossibleThreeSixty() {
        this.trickName = "Impossible 360";
        this.respectGain = TricksManager.respectBasic * TricksManager.LevelUpMultiplier;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

//////////////////////////////////////////////////////////////////////////////////////

public class Hardflip: FlipTricks {
    public Hardflip() {
        this.trickName = "Hardflip";
        this.respectGain = TricksManager.respectAdvanced;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}

public class ThreeSixtyHardflip: FlipTricks {
    public ThreeSixtyHardflip() {
        this.trickName = "360 Hardflip";
        this.respectGain = TricksManager.respectAdvanced * TricksManager.LevelUpMultiplier;
    }

    public override void PlayAnimation(Animator SkateboardAnim) {
        SkateboardAnim.SetTrigger(this.trickName);
    }
}