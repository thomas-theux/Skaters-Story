using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TricksManager : MonoBehaviour {

    public static int respectBasic = 16;
    public static int respectAdvanced = 24;
    public static int respectPro = 40;

    public static int LevelUpMultiplier = 4;

    public static int[] TricksLevel = {0, 0, 0, 0};

    public static List<FlipTricks> FlipTricksLeftArr = new List<FlipTricks>();
    public static List<FlipTricks> FlipTricksUpArr = new List<FlipTricks>();
    public static List<FlipTricks> FlipTricksRightArr = new List<FlipTricks>();
    public static List<FlipTricks> FlipTricksDownArr = new List<FlipTricks>();

    public static List<List<FlipTricks>> FlipTricksArr = new List<List<FlipTricks>>();


    private void Awake() {
        FlipTricksLeftArr.Add(new Kickflip());
        FlipTricksLeftArr.Add(new DoubleKickflip());

        FlipTricksUpArr.Add(new ThreeSixtyFlip());
        FlipTricksUpArr.Add(new SevenTwentyFlip());

        FlipTricksRightArr.Add(new Impossible());
        FlipTricksRightArr.Add(new ImpossibleThreeSixty());

        FlipTricksDownArr.Add(new Hardflip());
        FlipTricksDownArr.Add(new ThreeSixtyHardflip());

        //////////////////////////////////////////////////////////////////////////////////////

        FlipTricksArr.Add(FlipTricksLeftArr);
        FlipTricksArr.Add(FlipTricksUpArr);
        FlipTricksArr.Add(FlipTricksRightArr);
        FlipTricksArr.Add(FlipTricksDownArr);
    }

}
