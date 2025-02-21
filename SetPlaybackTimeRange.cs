using System;
using T3.Core.Animation;
using T3.Core.DataTypes;
using T3.Core.Logging;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Utils;

namespace T3.Operators.Types.Id_fb2d2134_f5b2_4ecc_8b83_a3c89494b488
{
    public class SetPlaybackTimeRange : Instance<SetPlaybackTimeRange>
    {
        [Output(Guid = "c3f04d4d-01df-4c3c-b5aa-110909ab82bf")]
        public readonly Slot<Command> Commands = new();
        
        public SetPlaybackTimeRange()
        {
            Commands.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            var startb = StartBar.GetValue(context);
            var endb = EndBar.GetValue(context);
            
            if (float.IsNaN(startb) || float.IsInfinity(startb))
            {
                startb = 0.0f;
            }

            if (float.IsNaN(endb) || float.IsInfinity(endb))
            {
                endb = float.PositiveInfinity;
            }

            if (Playback.Current == null)
            {
                Log.Warning("Can't set playback time without active Playback", this);
                return;
			}

            if (ShowLogMessages.GetValue(context))
            {
                Log.Debug($"Setting playback start time to {startb}", this);
                Log.Debug($"Setting playback end time to {endb}", this);
            }

            if (Playback.Current.TimeInBars < startb) Playback.Current.TimeInBars = startb;
            if (Playback.Current.TimeInBars >= endb) Playback.Current.TimeInBars = startb;

            
            SubGraph.GetValue(context);
        }
        
        [Input(Guid = "e6af3b56-039e-4673-ba04-b2c1e4f21adc")]
        public readonly InputSlot<Command> SubGraph = new();
        
        [Input(Guid = "b5ac524c-d994-4782-949e-2f793e5a4ff2")]
        public readonly InputSlot<float> StartBar = new();
        
        [Input(Guid = "aa4ce82f-3ca8-44b6-ad5a-daf4d9ebc742")]
        public readonly InputSlot<float> EndBar = new();
        
        [Input(Guid = "04755914-9481-491e-8a90-d6b562e2299c")]
        public readonly InputSlot<bool> ShowLogMessages = new();
    }
}