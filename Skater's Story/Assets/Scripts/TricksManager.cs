﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TricksManager : MonoBehaviour {

    public static int respectBasic = 16;
    public static int respectAdvanced = 24;
    public static int respectPro = 40;

    public static int LevelUpMultiplier = 4;

    //////////////////////////////////////////////////////////////////////////////////////

    public static int[] FlipTricksLevel = {0, 0, 0, 0, 0};

    public static List<FlipTricks> FlipTricksLeftArr = new List<FlipTricks>();
    public static List<FlipTricks> FlipTricksUpArr = new List<FlipTricks>();
    public static List<FlipTricks> FlipTricksRightArr = new List<FlipTricks>();
    public static List<FlipTricks> FlipTricksDownArr = new List<FlipTricks>();
    public static List<FlipTricks> FlipTricksNoDirArr = new List<FlipTricks>();

    public static List<List<FlipTricks>> FlipTricksArr = new List<List<FlipTricks>>();

    //////////////////////////////////////////////////////////////////////////////////////

    public static int[] GrindTricksLevel = {0, 0, 0, 0, 0};

    public static List<GrindTricks> GrindTricksLeftArr = new List<GrindTricks>();
    public static List<GrindTricks> GrindTricksUpArr = new List<GrindTricks>();
    public static List<GrindTricks> GrindTricksRightArr = new List<GrindTricks>();
    public static List<GrindTricks> GrindTricksDownArr = new List<GrindTricks>();
    public static List<GrindTricks> GrindTricksNoDirArr = new List<GrindTricks>();

    public static List<List<GrindTricks>> GrindTricksArr = new List<List<GrindTricks>>();



    private void Awake() {
        //////////////////////////////////////////////////////////////////////////////////////

        FlipTricksLeftArr.Add(new Kickflip());
        FlipTricksLeftArr.Add(new DoubleKickflip());

        FlipTricksUpArr.Add(new ThreeSixtyFlip());
        FlipTricksUpArr.Add(new SevenTwentyFlip());

        FlipTricksRightArr.Add(new Impossible());
        FlipTricksRightArr.Add(new ImpossibleThreeSixty());

        FlipTricksDownArr.Add(new Hardflip());
        FlipTricksDownArr.Add(new ThreeSixtyHardflip());

        FlipTricksNoDirArr.Add(new PopShoveIt());
        FlipTricksNoDirArr.Add(new ThreeSixtyShoveIt());

        FlipTricksArr.Add(FlipTricksLeftArr);
        FlipTricksArr.Add(FlipTricksUpArr);
        FlipTricksArr.Add(FlipTricksRightArr);
        FlipTricksArr.Add(FlipTricksDownArr);
        FlipTricksArr.Add(FlipTricksNoDirArr);

        //////////////////////////////////////////////////////////////////////////////////////

        GrindTricksLeftArr.Add(new FiveOGrind());
        GrindTricksLeftArr.Add(new Bluntslide());
        
        GrindTricksUpArr.Add(new Boardslide());
        GrindTricksUpArr.Add(new Darkslide());

        GrindTricksRightArr.Add(new Nosegrind());
        GrindTricksRightArr.Add(new Nosebluntslide());

        GrindTricksDownArr.Add(new Tailslide());
        GrindTricksDownArr.Add(new Feeble());
        
        GrindTricksNoDirArr.Add(new FiftyFifty());
        GrindTricksNoDirArr.Add(new Overcrook());

        GrindTricksArr.Add(GrindTricksLeftArr);
        GrindTricksArr.Add(GrindTricksUpArr);
        GrindTricksArr.Add(GrindTricksRightArr);
        GrindTricksArr.Add(GrindTricksDownArr);
        GrindTricksArr.Add(GrindTricksNoDirArr);
    }

}
