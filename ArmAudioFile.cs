using System.IO;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Operator.Interfaces;
using ManagedBass;


namespace T3.Operators.Types.Id_85a3bef9_6e33_44ec_864f_8046537c89ab
{
    public class ArmAudioFile : Instance<ArmAudioFile>, IStatusProvider
    {
        // result = true in case "is playing" is true, and audio file is finished: ie. "play is finished"
        [Output(Guid = "1bbeb6c9-d69a-48c8-b1c3-607d32c8ed52", DirtyFlagTrigger = DirtyFlagTrigger.None)]
        public readonly Slot<bool> Result = new();
        
        
        public ArmAudioFile()
        {
            Result.UpdateAction = Update;
        }
            
        private void Update(EvaluationContext context)
        {
            var url = Path.GetValue(context);
			var isPlaying = IsPlaying.GetValue(context);
			var volume = Volume.GetValue(context);

            // set result output value
            Result.Value = false;

			// make sure volume is in boundaries
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
                    else Result.Value = true;
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

        
        [Input(Guid = "5192e146-dee5-4278-9fe2-62c724fe3d5a")]
        public readonly InputSlot<string> Path = new();

        [Input(Guid = "9629e7e9-00d4-43a9-8078-96f91e0f9536")]
        public readonly InputSlot<float> Volume = new();
        
        [Input(Guid = "15585cf2-e5cb-43d8-869e-ec981caa8a76")]
        public readonly InputSlot<bool> IsPlaying = new();
        
        [Input(Guid = "8c326c6f-6f93-469f-b2c3-315873b81bc4")]
        public readonly InputSlot<bool> IsLooping = new();
    }
}