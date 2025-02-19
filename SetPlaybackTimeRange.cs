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
            var newTime = TimeInBars.GetValue(context);
            var mode = TriggerMode.GetEnumValue<Modes>(context);

            var enabled = Enabled.GetValue(context);
            var wasTriggered = MathUtils.WasTriggered(enabled, ref _wasEnabled);
            
            if (float.IsNaN(newTime) || float.IsInfinity(newTime))
            {
                newTime = 0;
            }

            if (wasTriggered || enabled && mode == Modes.Continuously)
            {
                if (Playback.Current == null)
                {
                    Log.Warning("Can't set playback time without active Playback", this);
                    return;
                }

                // if (Math.Abs(Playback.Current.PlaybackSpeed) < 0.001f)
                // {
                //     Log.Warning("Setting playback speed requires playback to be running.", this);
                // }

                if (ShowLogMessages.GetValue(context))
                {
                    Log.Debug($"Setting playback time to {newTime}", this);
                }                
                Playback.Current.TimeInBars = newTime;
            }
            
            SubGraph.GetValue(context);
        }

        private enum Modes
        {
            OnceEnabledGetsTrue,
            Continuously,
        }

        private bool _wasEnabled;
        
        [Input(Guid = "e6af3b56-039e-4673-ba04-b2c1e4f21adc")]
        public readonly InputSlot<Command> SubGraph = new();
        
        [Input(Guid = "b5ac524c-d994-4782-949e-2f793e5a4ff2")]
        public readonly InputSlot<float> TimeInBars = new();
        
        [Input(Guid = "9bd1ff84-805c-48e9-b9fd-c933b1935798", MappedType = typeof(Modes))]
        public readonly InputSlot<int> TriggerMode = new();
        
        [Input(Guid = "aa4ce82f-3ca8-44b6-ad5a-daf4d9ebc742")]
        public readonly InputSlot<bool> Enabled = new();
        
        [Input(Guid = "04755914-9481-491e-8a90-d6b562e2299c")]
        public readonly InputSlot<bool> ShowLogMessages = new();
    }
}