# tool3_operators
Set of custom operators for tool3
- PlayAudioFile: plays an audio file without need for timebase; returns true in case End Of File (file that is playing is finished)
- ArmAudioFile: ultra-custom operator to be used with Novation Launchpad; wait for press of a pad, and highlight the corresponding pad. Highlight is red at 1st press (armed), then green at 2nd press (play); non-active pads are yellow.--> This operator is deprecated
- ControlPlayback: allows to play/stop the timeline in Tooll3. This is the equivalent of clicking on the "start playback" button at the bottom of the screen. Start Bar can be specified.
- SetPlaybackTimeRange: check if current playback (timeline in Tooll3) is between startbar and endbar parameters. If not, then playback is brought back to startbar again.
- BoolIndex: features N bool inputs. Returns the index (# of the input) of the input that is triggered to true. Default value of result can be specified. In case several inputs are set to true simultaneously, then returns the highest index of input set to true.
- PlayList: implements playlist mode, where 6 pads can be used top play samples of current song, and previous-next pads allow to navigate through songs of the playlist (defined in the c# program). 
