using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TricksManager : MonoBehaviour {

    public static int respectBasic = 16;
    public static int respectAdvanced = 24;
    public static int respectPro = 40;

    public static int respectIncrease = 2;

    public static int SecondLevelMultiplier = 4;

    //////////////////////////////////////////////////////////////////////////////////////

    // public static int[] FlipTricksLevel = {0, 0, 0, 0, 0};

    public static List<FlipTricks> FlipTricksLeftArr = new List<FlipTricks>();
    public static List<FlipTricks> FlipTricksUpArr = new List<FlipTricks>();
    public static List<FlipTricks> FlipTricksRightArr = new List<FlipTricks>();
    public static List<FlipTricks> FlipTricksDownArr = new List<FlipTricks>();
    public static List<FlipTricks> FlipTricksNoDirArr = new List<FlipTricks>();

    public static List<List<FlipTricks>> FlipTricksArr = new List<List<FlipTricks>>();

    //////////////////////////////////////////////////////////////////////////////////////

    // public static int[] GrindTricksLevel = {0, 0, 0, 0, 0};

    public static List<GrindTricks> GrindTricksLeftArr = new List<GrindTricks>();
    public static List<GrindTricks> GrindTricksUpArr = new List<GrindTricks>();
    public static List<GrindTricks> GrindTricksRightArr = new List<GrindTricks>();
    public static List<GrindTricks> GrindTricksDownArr = new List<GrindTricks>();
    public static List<GrindTricks> GrindTricksNoDirArr = new List<GrindTricks>();

    public static List<List<GrindTricks>> GrindTricksArr = new List<List<GrindTricks>>();

    //////////////////////////////////////////////////////////////////////////////////////

    // public static int[] ManualTricksLevel = {0, 0, 0, 0};

    public static List<ManualTricks> ManualTricksLeftArr = new List<ManualTricks>();
    public static List<ManualTricks> ManualTricksUpArr = new List<ManualTricks>();
    public static List<ManualTricks> ManualTricksRightArr = new List<ManualTricks>();
    public static List<ManualTricks> ManualTricksDownArr = new List<ManualTricks>();

    public static List<List<ManualTricks>> ManualTricksArr = new List<List<ManualTricks>>();



    private void Awake() {
        //////////////////////////////////////////////////////////////////////////////////////

        FlipTricksLeftArr.Add(gameObject.AddComponent<Kickflip>());
        FlipTricksLeftArr.Add(gameObject.AddComponent<DoubleKickflip>());

        FlipTricksUpArr.Add(gameObject.AddComponent<ThreeSixtyFlip>());
        FlipTricksUpArr.Add(gameObject.AddComponent<SevenTwentyFlip>());

        FlipTricksRightArr.Add(gameObject.AddComponent<Impossible>());
        FlipTricksRightArr.Add(gameObject.AddComponent<ImpossibleThreeSixty>());

        FlipTricksDownArr.Add(gameObject.AddComponent<Hardflip>());
        FlipTricksDownArr.Add(gameObject.AddComponent<ThreeSixtyHardflip>());

        FlipTricksNoDirArr.Add(gameObject.AddComponent<PopShoveIt>());
        FlipTricksNoDirArr.Add(gameObject.AddComponent<ThreeSixtyShoveIt>());

        FlipTricksArr.Add(FlipTricksLeftArr);
        FlipTricksArr.Add(FlipTricksUpArr);
        FlipTricksArr.Add(FlipTricksRightArr);
        FlipTricksArr.Add(FlipTricksDownArr);
        FlipTricksArr.Add(FlipTricksNoDirArr);

        //////////////////////////////////////////////////////////////////////////////////////

        GrindTricksLeftArr.Add(gameObject.AddComponent<FiveOGrind>());
        GrindTricksLeftArr.Add(gameObject.AddComponent<Bluntslide>());
        
        GrindTricksUpArr.Add(gameObject.AddComponent<Boardslide>());
        GrindTricksUpArr.Add(gameObject.AddComponent<Darkslide>());

        GrindTricksRightArr.Add(gameObject.AddComponent<Nosegrind>());
        GrindTricksRightArr.Add(gameObject.AddComponent<Nosebluntslide>());

        GrindTricksDownArr.Add(gameObject.AddComponent<Tailslide>());
        GrindTricksDownArr.Add(gameObject.AddComponent<Feeble>());
        
        GrindTricksNoDirArr.Add(gameObject.AddComponent<FiftyFifty>());
        GrindTricksNoDirArr.Add(gameObject.AddComponent<Overcrook>());

        GrindTricksArr.Add(GrindTricksLeftArr);
        GrindTricksArr.Add(GrindTricksUpArr);
        GrindTricksArr.Add(GrindTricksRightArr);
        GrindTricksArr.Add(GrindTricksDownArr);
        GrindTricksArr.Add(GrindTricksNoDirArr);

        //////////////////////////////////////////////////////////////////////////////////////

        ManualTricksLeftArr.Add(gameObject.AddComponent<Casper>());

        ManualTricksUpArr.Add(gameObject.AddComponent<Manual>());

        ManualTricksRightArr.Add(gameObject.AddComponent<Primo>());

        ManualTricksDownArr.Add(gameObject.AddComponent<NoseManual>());

        ManualTricksArr.Add(ManualTricksLeftArr);
        ManualTricksArr.Add(ManualTricksUpArr);
        ManualTricksArr.Add(ManualTricksRightArr);
        ManualTricksArr.Add(ManualTricksDownArr);
    }

}
