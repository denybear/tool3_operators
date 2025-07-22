using T3.Core.Operator;
using T3.Core.Utils;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

using System;
using System.Collections.Generic;


namespace T3.Operators.Types.Id_813d7cdd_cf59_4c2d_a4ac_307a4c5c3b20
{
    public class PlayList : Instance<PlayList>
    {		
        [Output(Guid = "15287d50-43e0-4bef-ad49-446ab3f54e8a")]
        public readonly Slot<bool> StandardAnim = new();
        [Output(Guid = "4a2e5f6e-5f8c-4c11-a183-1cea01c8a41b")]
        public readonly Slot<bool> DarkSideAnim = new();
        [Output(Guid = "955e85b1-4264-4581-b964-595f9a7ea6ec")]
        public readonly Slot<bool> AnimalsAnim = new();
        [Output(Guid = "31069392-078d-4262-b18d-0fdeb96175c0")]
        public readonly Slot<bool> TheWallAnim = new();
        [Output(Guid = "5d23d4bb-aa12-493b-8895-5c65b700a2b0")]
        public readonly Slot<bool> DedicatedAnim = new();
        [Output(Guid = "666f7460-d674-4d0c-b90b-3dcddff16d95")]
        public readonly Slot<int> DedicatedAnimNumber = new();
        [Output(Guid = "7a485b5f-5ef8-4129-90d6-34e4e61b66fd")]
        public readonly Slot<string> Sample = new();
        [Output(Guid = "308b4693-f13c-4bdd-9880-eca86309ceaa")]
        public readonly Slot<bool> PlayStop = new();
        [Output(Guid = "e046662a-6b6b-4663-9e39-439563b698ac")]
        public readonly Slot<string> Song = new();

        public class PlayListEntry
        {
            private int animNumber = 0;
			private string animName = "Standard";
            public string SongName { get; set; } = "";
            public string AnimName
            {
                get => animName;
                set
                {
					animName = value;
                    if (animName == "We Are") animNumber = 0;
                    if (animName == "Echoes") animNumber = 1;
                    if (animName == "Procedural Moon") animNumber = 2;
                    if (animName == "Astra Domine") animNumber = 3;
                    if (animName == "Landing Strip") animNumber = 4;
                    if (animName == "Axis Triangle") animNumber = 5;
                    if (animName == "Moving Dots") animNumber = 6;
                    if (animName == "Wool Strings") animNumber = 7;
                    if (animName == "Electric Strings") animNumber = 8;
                }
            }
            public int AnimNumber
			{
				get => animNumber;
			}
 
			public string Sample1 { get; set; } = "";
            public string Sample2 { get; set; } = "";
            public string Sample3 { get; set; } = "";
            public string Sample4 { get; set; } = "";
            public string Sample5 { get; set; } = "";
            public string Sample6 { get; set; } = "";
        }

        // Playlist definition HERE
        public List<PlayListEntry> pList = new List<PlayListEntry>
        {
            new PlayListEntry { SongName = "Intro", AnimName = "We Are" },
            new PlayListEntry { SongName = "Speak to me", AnimName = "Dark Side", Sample1 = "SpeakToMe_intro.mp3" },
            new PlayListEntry { SongName = "Breathe", AnimName = "Dark Side" },
            new PlayListEntry { SongName = "Time", AnimName = "Landing Strip", Sample1 = "Time_intro.mp3" },
            new PlayListEntry { SongName = "The Great Gig In The Sky", AnimName = "Dark Side", Sample1 = "TheGreatGigInTheSky_voice.mp3" },
            new PlayListEntry { SongName = "Money", AnimName = "Axis Triangle", Sample1 = "Money_intro.mp3", Sample2 = "Money_outro.mp3" },
            new PlayListEntry { SongName = "Us & Them", AnimName = "Wool Strings" },
            new PlayListEntry { SongName = "Any Color You Like", AnimName = "Dark Side" },
            new PlayListEntry { SongName = "Brain Damage", AnimName = "Procedural Moon", Sample1 = "BrainDamage_risata.mp3" },
            new PlayListEntry { SongName = "Eclipse", AnimName = "Procedural Moon", Sample1 = "Eclipse_outro.mp3" },
            new PlayListEntry { SongName = "Intro", AnimName = "We Are" },

            new PlayListEntry { SongName = "Cymbaline", AnimName = "Standard" },
            new PlayListEntry { SongName = "Shine On You Crazy Diamond", AnimName = "Astra Domine" },
            new PlayListEntry { SongName = "Dogs", AnimName = "Animals", Sample1 = "Dogs_1.mp3", Sample2 = "Dogs_2.mp3" },
            new PlayListEntry { SongName = "Wish You Were Here", AnimName = "Electric Strings", Sample1 = "WishYouWereHere_intro.mp3", Sample2 = "WishYouWereHere_outro.mp3" },
            new PlayListEntry { SongName = "Comfortably Numb", AnimName = "Moving Dots", Sample1 = "ComfortablyNumb_intro.mp3", Sample2 = "ComfortablyNumb_aaaaa.mp3" },
            new PlayListEntry { SongName = "Another Brick In The Wall", AnimName = "The Wall" },
            new PlayListEntry { SongName = "Intro", AnimName = "We Are" }
        };


        public void ResetOuputs ()
		{
            StandardAnim.Value = false;
            DarkSideAnim.Value = false;
            AnimalsAnim.Value = false;
            TheWallAnim.Value = false;
            DedicatedAnim.Value = false;
			PlayStop.Value = false;
		}


		// read playlist element and set the outputs accordingly
		public void SetAnim (PlayListEntry p)
		{
			// reset "pad pressed" outputs
//            ResetOuputs ();

			// display SongName on the console
			if (p.AnimName == "Standard") StandardAnim.Value = true;
			else if (p.AnimName == "Dark Side") DarkSideAnim.Value = true;
			else if (p.AnimName == "Animals") AnimalsAnim.Value = true;
			else if (p.AnimName == "The Wall") TheWallAnim.Value = true;
			else	// dedicated anims
			{
				DedicatedAnim.Value = true;
				DedicatedAnimNumber.Value = p.AnimNumber;
			}
		}

		// read playlist element and set the outputs accordingly
		public string SetSound (PlayListEntry p, int i)
		{
			if (i == 1) return p.Sample1;
			if (i == 2) return p.Sample2;
			if (i == 3) return p.Sample3;
			if (i == 4) return p.Sample4;
			if (i == 5) return p.Sample5;
			if (i == 6) return p.Sample6;
			return "";
		}

        public PlayList()
        {
            StandardAnim.UpdateAction = Update;
            DarkSideAnim.UpdateAction = Update;
            AnimalsAnim.UpdateAction = Update;
            TheWallAnim.UpdateAction = Update;
            DedicatedAnim.UpdateAction = Update;
            DedicatedAnimNumber.UpdateAction = Update;
            Sample.UpdateAction = Update;
			PlayStop.UpdateAction = Update;
			Song.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {

			// check previous / next pads: if present, set position in the playlist, and check the boundaries
			// anti-bouncing mechanism is implemented
			var preventry = PrevEntry.GetValue(context);
			var nextentry = NextEntry.GetValue(context);
			if (preventry)
            {								// previous pad pressed
				if (!_formerstateprev)		// limit to a single press only
                {
					_formerstateprev = true;
					if (_indexInPlayList > 0) _indexInPlayList -= 1;
					SetAnim (pList [_indexInPlayList]);
				}
			}
			else if (nextentry)
            {								// next pad pressed
				if (!_formerstatenext)		// limit to a single press only
                {
					_formerstatenext = true;
					if (_indexInPlayList + 1 < pList.Count) _indexInPlayList += 1;
					SetAnim (pList [_indexInPlayList]);				
				}
			}
			else
			{
				_formerstateprev = false;
				_formerstatenext = false;
				ResetOuputs ();
			}

			// check input from the "sample" pads
            int i = 1;
			bool sampleFound = false;

			foreach (var pad in SamplePads.GetCollectedTypedInputs())
            {
                if (pad.GetValue(context) == true)
                {
					sampleFound = true;
					break;		// go out the loop
				}
				i++;
            }
			
			// in case sample pad is pressed, set the right outputs
			if ((sampleFound) && (i<=6))
            {		// we manage 6 sample pads maximum
//				if (!_formerstateplaystop)		// limit to a single press only
                {
					_formerstateplaystop = true;
					if (SetSound (pList[_indexInPlayList], i) == "")
					{
						PlayStop.Value = false;
						Sample.Value = "empty sample slot";		// relevant message instead of ""
					}
					else {
						PlayStop.Value = true;		// activate play only is there is a valid sample
						Sample.Value = SetSound (pList[_indexInPlayList], i);
					}
				}
			}
			else
			{
				_formerstateplaystop = false;
				PlayStop.Value = false;
			}
		
			
			// return song name
			Song.Value = pList [_indexInPlayList].SongName;
        }


        private int _indexInPlayList = 0;
        private bool _formerstateprev = false;
        private bool _formerstatenext = false;
        private bool _formerstateplaystop = false;
		
        [Input(Guid = "76cc3cfb-63c3-4b44-9568-b8b8a62aebbf")]
        public readonly MultiInputSlot<bool> SamplePads = new();

        [Input(Guid = "0e555710-9445-487e-9357-d1cb3b449429")]
        public readonly InputSlot<bool> PrevEntry = new();

        [Input(Guid = "f5cdfdac-0fe9-4eb8-a27e-9284f7ba7316")]
        public readonly InputSlot<bool> NextEntry = new();
    }
}