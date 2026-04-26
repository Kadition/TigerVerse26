# TigerVerse26

**Telekenetic Tennis**
Telekenetic Tennis is a virtual reality tennis game built in Unity.

**How To Play**
- Move around court: Right hand joystick 
- Telekenesis: Moves ball left, right, forward, and backwards after serving by rotating the right controller, but only after you hit it last
- Follows standard tennis scoring (except serving, where all faults count as a lost point)

**Dependencies & Requirements**
To run this project, you need the correct unity version, a vr headset, and a right vr controller

**Hardware Integration: Spectator Score Board**
- Sends score updates to an esp32 driving an lcd display over usb serial so others can follow allong in real time.
- Requires setting "portName" in ScoreTracker.cs to the name of the USB port the esp32 is connected to on your machine
