# TigerVerse26

**Telekenetic Tennis**
Telekenetic Tennis is a virtual reality tennis game built in Unity. This project  features custom physics-based ball interactions, standard tennis scoring, dynamic VR controller inputs, and a predictive AI opponent.

**Core Features**
- Custom Ball Physics: Calculates custom gravity, velocity, and bounce vectors, determining out-of-bounds hits, net hits, and double bounces.
- Dynamic Racket Tracking: Calculates racket swing speed to apply force to the ball and dynamically scales the racket's hit-box based on movement speed to improve VR playability.
- Predictive Enemy AI: The opponent calculates the exact landing position of the ball and moves to intercept it, featuring a configurable "flub chance" for realistic errors.
- Authentic Scoring: Full implementation of standard tennis scoring (Love, 15, 30, 40, Deuce, Advantage) requiring a win-by-two scenario.
- Immersive Feedback: Includes spatial audio for ball bounces, racket hits, net collisions, crowd reactions, and controller haptic rumble on impact.

**How To Play**
- Right Hand Joysitck: Move player around court
- Rigth Hand Rotation: Moves ball left, right, up, and down

**Dependencies & Requirements**
To run this project, your Unity environment must have the following packages installed:

- Unity Input System: Used for modern VR controller mapping (UnityEngine.InputSystem).
- XR Interaction Toolkit: Used for the VR rig and spatial tracking (UnityEngine.InputSystem.XR).
- TextMeshPro: Used for rendering the floating 3D score text in the VR space (TMPro).

**Hardware Integration: Spectator Score Board**
- Sends score updates to an esp32 driving an lcd display over usb serial so others can follow allong in real time.
- Requires setting "portName" in ScoreTracker.cs to the name of the USB port the esp32 is connected to on your machine
