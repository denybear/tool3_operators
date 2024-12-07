# tool3_operators
Set of custom operators for tool3
- PlayAudioFile: plays an audio file without need for timebase; returns true in case End Of File (file that is playing is finished)
- ArmAudioFile: ultra-custom operator to be used with Novation Launchpad; wait for press of a pad, and highlight the corresponding pad. Highlight is red at 1st press (armed), then green at 2nd press (play); non-active pads are yellow.
