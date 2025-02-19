using System;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Operator.Interfaces;
using T3.Core.DataTypes;
using T3.Core.Animation;
//using T3.Editor.Gui.UiHelpers;


namespace T3.Operators.Types.Id_1bf89533_2025_42b2_96a2_879f91b418ea
{
    public class ControlPlayback : Instance<ControlPlayback>, IStatusProvider
    {
        // no result is true if something has been done, false otherwise
        [Output(Guid = "21fa987e-a134-454a-a7b9-5df4d825c0cd")]
        public readonly Slot<bool> Result = new();       
        
        public ControlPlayback()
        {
            Result.UpdateAction = Update;
        }
            
        private void Update(EvaluationContext context)
        {
            var play = PlaybackPlayFromStartTime.GetValue(context);
			var stop = PlaybackStop.GetValue(context);
            var playback = Playback.Current; // TODO, this should be non-static eventually

            // play is triggered: create stream, play file
            if (play)
            {
                playback.TimeInBars = playback.IsLooping ? playback.LoopRange.Start : 0;
                playback.PlaybackSpeed = 1;
                _lastPlaybackStartTime = playback.TimeInBars;

                // set result output value
                Result.Value = true;
                return;
            }

            if (stop)
            {
                playback.PlaybackSpeed = 0;
//                if (UserSettings.Config.ResetTimeAfterPlayback)
                    playback.TimeInBars = _lastPlaybackStartTime;

                // set result output value
                Result.Value = true;
                return;
            }


            // set result output value
            Result.Value = false;
        }


        IStatusProvider.StatusLevel IStatusProvider.GetStatusLevel()
        {
            return string.IsNullOrEmpty(_errorMessageForStatus) ? IStatusProvider.StatusLevel.Success : IStatusProvider.StatusLevel.Error;
        }

        string IStatusProvider.GetStatusMessage()
        {
            return _errorMessageForStatus;
        }
        
        private string _errorMessageForStatus;
        public static double _lastPlaybackStartTime;


        [Input(Guid = "7427f1cd-45de-49dd-b6ac-7794ccb28671")]
        public readonly InputSlot<bool> PlaybackPlayFromStartTime = new();

        [Input(Guid = "e8c1cc08-14ab-4b48-a545-0cb658239fef")]
        public readonly InputSlot<bool> PlaybackStop = new();
    }
}