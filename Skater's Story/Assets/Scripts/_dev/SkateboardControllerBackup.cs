// private void Update() {
//         GetInput();

//         isGrounded = Physics2D.OverlapCircle(GroundChecker.position, GroundDistance, platformLayerMask);

//         ///////////////////////////////////////////////////////////////////////////////////////

//         // Reset player to the beginning of the level
//         if (DEVRESET) DEVReset();
//         BoardSpeed = rb.velocity.magnitude;

//         ///////////////////////////////////////////////////////////////////////////////////////

//         // 0 = Stop
//         if (SkateMode == 0) {
//             if (dPadRight) {
//                 // Start to ROLL
//                 SkateMode = 1;
//                 rb.velocity = transform.right * MaxRollForce;
//             }

//             if (XButtonUp) {
//                 // JUMP while skating
//                 if (isGrounded) {
//                     ApplyOllieForce();
//                 }
//             }

//             if (XButtonDown) {
//                 // JUMP while standing still
//                 EnterSkateMode();
//             }
//         }

//         // 1 = Roll
//         if (SkateMode == 1) {
//             ApplyPushForce(MaxRollForce);

//             if (dPadLeft) {
//                 // Decelerating until standing still
//                 ResetValuesWhenStopping();
//             }

//             if (XButtonUp) {
//                 // JUMP while rolling
//                 if (isGrounded) {
//                     ApplyOllieForce();
//                 }
//             }

//             if (XButtonDown) {
//                 // Go into SKATE mode
//                 EnterSkateMode();
//             }
//         }

//         // 2 = Skate
//         if (SkateMode == 2) {
//             ApplyPushForce(MaxSkateForce);

//             if (!XButtonDown) {
//                 if (dPadLeft) {
//                     // Decelerating until standing still
//                     ResetValuesWhenStopping();
//                 }
//             }

//             if (XButtonUp) {
//                 // JUMP while skating
//                 if (isGrounded) {
//                     ApplyOllieForce();
//                 }
//             }
//         }

//         // 3 = Air
//         if (SkateMode == 3) {
//             // Can do tricks
//             if (isGrounded) {
//                 SkateMode = savedSkateMode;

//                 // Check for bails
//             }
//         }

//         if (!isGrounded) {
//             if (SkateMode != 3) {
//                 savedSkateMode = SkateMode;
//                 SkateMode = 3;
//             }
//         }
//     }