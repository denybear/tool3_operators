using System.IO;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Operator.Interfaces;
using T3.Core.DataTypes;
using ManagedBass;
using static T3.Core.DataTypes.Gradient;
using T3.Core.Utils;


namespace T3.Operators.Types.Id_67d695fe_bdfd_48ba_b091_3a427d687a41
{
    public class PlayAudioFile : Instance<PlayAudioFile>, IStatusProvider
    {
        [Output(Guid = "b8ab02cc-2856-428d-bf88-2b775c8ca1e3")]
        public readonly Slot<Command> Result = new();

        // true in case audio is playing, false if audio is not playing or audio file EOF (audio file playing is finished)
        [Output(Guid = "4782802f-39bf-47bf-9598-6a7be88fefab")]
        public readonly Slot<bool> IsPlaying = new();
        
        
        public PlayAudioFile()
        {
            Result.UpdateAction = Update;
        }
            
        private void Update(EvaluationContext context)
        {
            var url = Path.GetValue(context);
			var enabled = Enabled.GetValue(context);
			var volume = Volume.GetValue(context);
            var mode = TriggerMode.GetEnumValue<Modes>(context);
            var wasPlayStopTriggered = MathUtils.WasTriggered(enabled, ref _wasEnabled);

            // make sure volume is in boundaries (although this is checked by UI already)
            if (volume < 0.0f) volume = 0.0f;
			if (volume > 1.0f) volume = 1.0f;

            // play is triggered: create stream, play file
            if (enabled && mode == Modes.OnceEnabledGetsTrue) _isPlaying = !_isPlaying;
            if (mode == Modes.Continuously) _isPlaying = enabled;

            // start playing, while previous state was stopped
            if (_isPlaying && !_wasPlayingPreviously)
			{
                if (!File.Exists(url))
                {
                    _errorMessageForStatus = $"File not found: {url}";
                    return;
                }

				if ((!Bass.Init()) && (Bass.LastError != Errors.Already))
                {
                    _errorMessageForStatus = "Cannot create Bass instance";
                    return;
				}

				_stream = Bass.CreateStream(url);
				if (_stream == 0)
				{
                    _errorMessageForStatus = "Cannot create Bass audio stream";
					return;
				}

				Bass.ChannelPlay(_stream);
			}

			// play is in progress: adjust volume, loop in case required
			if (_isPlaying)
			{
                Bass.ChannelSetAttribute(_stream, ChannelAttribute.Volume, volume);

                // check if play is finished
				Bass.ChannelSetSync(_stream, SyncFlags.End, 0, (handle, channel, data, user) =>
				{
                    if (IsLooping.GetValue(context)) Bass.ChannelPlay(_stream);
                    else _isPlaying = false;     // end of file, we shall stop playing
                });
			}
			
			// stop playing, while previous state was playing
			if (!_isPlaying && _wasPlayingPreviously)
			{
				Bass.ChannelStop(_stream);
				Bass.Free();
			}

			
			// update playing state
			_wasPlayingPreviously = _isPlaying;

            // set result output value
            IsPlaying.Value = _isPlaying;
        }


        private bool _wasPlayingPreviously = false;
        private bool _isPlaying = false;
        private int _stream = 0;
        private bool _wasEnabled = false;

        private enum Modes
        {
            OnceEnabledGetsTrue,
            Continuously
        };

        IStatusProvider.StatusLevel IStatusProvider.GetStatusLevel()
        {
            return string.IsNullOrEmpty(_errorMessageForStatus) ? IStatusProvider.StatusLevel.Success : IStatusProvider.StatusLevel.Error;
        }

        string IStatusProvider.GetStatusMessage()
        {
            return _errorMessageForStatus;
        }
        
        private string _errorMessageForStatus;

        
        [Input(Guid = "52e826b9-9ff5-4858-9420-365358b5b028")]
        public readonly InputSlot<string> Path = new();

        [Input(Guid = "fb38a142-0341-4509-809a-5a6e7332071f")]
        public readonly InputSlot<float> Volume = new();

        [Input(Guid = "44d67c1c-b7ff-49bf-be04-d4ef8cb06677", MappedType = typeof(Modes))]
        public readonly InputSlot<int> TriggerMode = new();

        [Input(Guid = "80182a35-2762-46f2-b683-36a0e696037c")]
        public readonly InputSlot<bool> Enabled = new();
        
        [Input(Guid = "23463284-526f-4e1b-a851-1e6893a58611")]
        public readonly InputSlot<bool> IsLooping = new();
    }
}