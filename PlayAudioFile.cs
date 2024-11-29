using System;
using System.IO;
using T3.Core.Animation;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Audio;
using T3.Core.DataTypes;
using T3.Core.Logging;
using T3.Core.Operator.Interfaces;
using ManagedBass;


namespace T3.Operators.Types.Id_c2b2758a_5b3e_465a_87b7_c6a13d3fba48
{
    public class PlayAudioFile : Instance<PlayAudioFile>, IStatusProvider
    {
        [Output(Guid = "4782802f-39bf-47bf-9598-6a7be88fefab", DirtyFlagTrigger = DirtyFlagTrigger.Animated)]
        public readonly Slot<Command> Result = new();
        
        
        public PlayAudioFile()
        {
            Result.UpdateAction = Update;
        }
            
        private void Update(EvaluationContext context)
        {
            var url = Path.GetValue(context);
			var isPlaying = IsPlaying.GetValue(context);
			var volume = Volume.GetValue(context);
			// make sure volume is in boundaries
			if (volume < 0.0) volume = 0.0;
			if (volume > 1.0) volume = 1.0;
			
			// play is triggered: create stream, play file
			if (isPlaying && !_wasPlayingPreviously)
			{
                if(!File.Exists(url))
                {
                    _errorMessageForStatus = $"File not found: {url}";
                    return;
                }

				if (!Bass.Init())
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
			if (isPlaying)
			{
                Bass.ChannelSetAttribute(_stream, ChannelAttribute.Volume, volume);

				Bass.ChannelSetSync(_stream, SyncFlags.End, 0, (handle, channel, data, user) =>
				{
					if (IsLooping.GetValue(context)) Bass.ChannelPlay(_stream);
				});
			}
			
			// stop playing
			if (!isPlaying && _wasPlayingPreviously)
			{
				Bass.ChannelStop(_stream);
				Bass.Free();
			}
			
			// update playing state
			_wasPlayingPreviously = isPlaying;
		}
		
		
		private bool _wasPlayingPreviously = false;
		private int _stream = 0;

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
        
        [Input(Guid = "80182a35-2762-46f2-b683-36a0e696037c")]
        public readonly InputSlot<bool> IsPlaying = new();
        
        [Input(Guid = "23463284-526f-4e1b-a851-1e6893a58611")]
        public readonly InputSlot<bool> IsLooping = new();
    }
}