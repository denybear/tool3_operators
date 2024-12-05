using System.IO;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Operator.Interfaces;
using ManagedBass;


namespace T3.Operators.Types.Id_67d695fe_bdfd_48ba_b091_3a427d687a41
{
    public class PlayAudioFile : Instance<PlayAudioFile>, IStatusProvider
    {
        // endoffile = true in case "is playing" is true, and audio file is finished: ie. "play is finished"
        [Output(Guid = "4782802f-39bf-47bf-9598-6a7be88fefab")]
        public readonly Slot<bool> EndOfFile = new();
        
        
        public PlayAudioFile()
        {
            EndOfFile.UpdateAction = Update;
        }
            
        private void Update(EvaluationContext context)
        {
            var url = Path.GetValue(context);
			var isPlaying = IsPlaying.GetValue(context);
			var volume = Volume.GetValue(context);

            // set result output value
            EndOfFile.Value = false;

			// make sure volume is in boundaries (although this is checked by UI already)
			if (volume < 0.0f) volume = 0.0f;
			if (volume > 1.0f) volume = 1.0f;
			
			// play is triggered: create stream, play file
			if (isPlaying && !_wasPlayingPreviously)
			{
                if(!File.Exists(url))
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
			if (isPlaying)
			{
                Bass.ChannelSetAttribute(_stream, ChannelAttribute.Volume, volume);

                // check if play is finished
				Bass.ChannelSetSync(_stream, SyncFlags.End, 0, (handle, channel, data, user) =>
				{
                    if (IsLooping.GetValue(context)) Bass.ChannelPlay(_stream);
                    else EndOfFile.Value = true;
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