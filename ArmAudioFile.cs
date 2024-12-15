using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Operator.Interfaces;
using NAudio.Midi;
using System;
using Operators.Utils;
using T3.Core.Logging;


namespace T3.Operators.Types.Id_85a3bef9_6e33_44ec_864f_8046537c89ab
{
    public class ArmAudioFile : Instance<ArmAudioFile>, IStatusProvider, MidiConnectionManager.IMidiConsumer
    {
        // pad colors for Novation Launchpad mini (old version)
        public const int BLACK = 0x0C;
        public const int RED = 0x0F;
        public const int AMBER = 0x3F;
        public const int GREEN = 0x3C;
        public const int ORANGE = 0x2F;
        public const int YELLOW = 0x3E;

        // result = 
        [Output(Guid = "1bbeb6c9-d69a-48c8-b1c3-607d32c8ed52")]
        public readonly Slot<bool> Play = new();
        
        
        public ArmAudioFile()
        {
            Play.UpdateAction = Update;
        }


        protected override void Dispose(bool isDisposing)
        {
            if(!isDisposing) return;

            if (_initialized)
            {
                MidiConnectionManager.UnregisterConsumer(this);
            }
        }


        private void SendNoteOn(string devName, int channel, int note, int velocity)
        {
            var foundDevice = false;

            if(!_initialized)
            {
                MidiConnectionManager.RegisterConsumer(this);
                _initialized = true;
            }          
            
            foreach (var (m, device) in MidiConnectionManager.MidiOutsWithDevices)
            {
                if (device.ProductName != devName)
                    continue;
                
                try
                {
                    MidiEvent midiEvent = null;
                    midiEvent = new NoteEvent(0, channel, MidiCommandCode.NoteOn, note, velocity);	// note on

                    if(midiEvent != null)
                        m.Send(midiEvent.GetAsShortMessage());
                    
                    //Log.Debug("Sending MidiTo " + device.Manufacturer + " " + device.ProductName, this);
                    foundDevice = true;
                    break;
                }
                catch (Exception e)
                {
                    _lastErrorMessage = $"Failed to send midi to {devName}: " + e.Message;
                    Log.Warning(_lastErrorMessage, this);
                }
                
            }
            _lastErrorMessage = !foundDevice ? $"Can't find MidiDevice {devName}" : null;
        }


        static string ExtractSubstringAfterLastBackslash(string input)
        {
            // Checks if string contains '\'
            int lastBackslashIndex = input.LastIndexOf('\\');

            // If the char is not found, returns the entire string
            if (lastBackslashIndex == -1)
            {
                return input;
            }

            // Extract the sub-string after the last '\'
            return input.Substring(lastBackslashIndex + 1);
        }


    private void Update(EvaluationContext context)
        {
            var url = Path.GetValue(context);
			var isPadPressed = IsPadPressed.GetValue(context);
            var isPlayFinished = IsPlayFinished.GetValue(context);
			var padNumber = PadNumber.GetValue(context);
            var padVelocity = PadVelocity.GetValue(context);
            var deviceName = DeviceName.GetValue(context);
            var channelNumber = ChannelNumber.GetValue(context);
            string songName = ExtractSubstringAfterLastBackslash(url);      // last part of the path
            int i;

            // initial setup
            if (_isFirstTimeRun)
			{
                // midi send set neutral color to all pads
                for (i=0; i < 64; i++) SendNoteOn(deviceName, channelNumber, i, YELLOW);
                _isFirstTimeRun = false;
				Play.Value = false;
			}

            // in case pad is pressed, but velocity == 0, then this is a note-off: discard press
            if (isPadPressed && (padVelocity == 0)) isPadPressed = false;

			// in case song should be playing: pad is pressed, pad was armed previously, song was not playing before
			if (isPadPressed && (padNumber == _previousPadNumber) && !Play.Value)	// need to add: && !isPlayFinished ???
			{
                // display the name of the playing song in the console
                // SHOULD USE A MESSAGE OUTPUT AND logmessage operator
                Console.WriteLine($"Playing: {songName}");
                // midi send set play color to pressed pad
                SendNoteOn(deviceName, channelNumber, padNumber, GREEN);
                _previousPadNumber = padNumber;
				Play.Value = true;
			}
			// all other cases
			else
			{
				if (isPadPressed || isPlayFinished)
				{
                    // display the name of the armed song in the console
                    // SHOULD USE A MESSAGE OUTPUT AND logmessage operator
                    Console.WriteLine($"Arming: {songName}");
                    // midi send set neutral color to previously pressed pad
                    if ((_previousPadNumber != -1) && (_previousPadNumber != padNumber)) SendNoteOn(deviceName, channelNumber, _previousPadNumber, YELLOW);
                    // midi send set armed color to pressed pad
                    SendNoteOn(deviceName, channelNumber, padNumber, RED);
                    _previousPadNumber = padNumber;
					Play.Value = false;				
				}
			}
		}	
		
        IStatusProvider.StatusLevel IStatusProvider.GetStatusLevel()
        {
            return string.IsNullOrEmpty(_errorMessageForStatus) ? IStatusProvider.StatusLevel.Success : IStatusProvider.StatusLevel.Error;
        }

        string IStatusProvider.GetStatusMessage()
        {
            return _errorMessageForStatus;
        }

        
        private bool _initialized = false;
		private bool _isFirstTimeRun = true;
		private int _previousPadNumber = -1;
        private string _errorMessageForStatus;
        private string _lastErrorMessage;


        // We don't actually receive midi in this operator, those methods can remain empty, we just want the MIDI connection thread up
        public void MessageReceivedHandler(object sender, MidiInMessageEventArgs msg) {}
        public void ErrorReceivedHandler(object sender, MidiInMessageEventArgs msg) {}
        public void OnSettingsChanged() {}

        
        [Input(Guid = "5192e146-dee5-4278-9fe2-62c724fe3d5a")]
        public readonly InputSlot<string> Path = new();

        [Input(Guid = "a3d0b91d-6bf0-491c-a49f-fe6474049448")]
        public readonly InputSlot<string> DeviceName = new();

        [Input(Guid = "72d8c2c8-99fb-4e5a-a7b5-a4d664afb879")]
        public readonly InputSlot<int> ChannelNumber = new();

        [Input(Guid = "15585cf2-e5cb-43d8-869e-ec981caa8a76")]
        public readonly InputSlot<int> PadNumber = new();

        [Input(Guid = "477a5b32-1265-4b69-9afb-ff0b83e1f443")]
        public readonly InputSlot<int> PadVelocity = new();

        [Input(Guid = "8c326c6f-6f93-469f-b2c3-315873b81bc4")]
        public readonly InputSlot<bool> IsPadPressed = new();

        [Input(Guid = "4a2787ad-6bef-42c3-8156-a22551f61033")]
        public readonly InputSlot<bool> IsPlayFinished = new();

    }
}
