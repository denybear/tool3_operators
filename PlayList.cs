using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

using System;
using System.Collections.Generic;


namespace T3.Operators.Types.Id_a9110543_78d9_4ede_8d9b_a7a557950721
{
	private class PlayListEntry
	{
		private int animNumber = 0;
		public string SongName { get; set; } = "";
		public string AnimName { get; set; } = "Standard";
		private int animNumber;
		public int AnimNumber {
			get {
				return animNumber;
			}
			set {
				if (this.AnimName == "We Are") animNumber = 0;
				if (this.AnimName == "Echoes") animNumber = 1;
				if (this.AnimName == "Procedural Moon") animNumber = 2;
				if (this.AnimName == "Astra Domine") animNumber = 3;
				if (this.AnimName == "Landing Strip") animNumber = 4;
				if (this.AnimName == "Axis Triangle") animNumber = 5;
				if (this.AnimName == "Moving Dots") animNumber = 6;
				if (this.AnimName == "Wool Strings") animNumber = 7;
				if (this.AnimName == "Electric Strings") animNumber = 8;			
			}
		}
		public string Sample1 { get; set; } = "";
		public string Sample2 { get; set; } = "";
		public string Sample3 { get; set; } = "";
		public string Sample4 { get; set; } = "";
		public string Sample5 { get; set; } = "";
		public string Sample6 { get; set; } = "";
	}

	// Playlist definition HERE
	List<PlayListEntry> pList = new List<PlayListEntry>
	{
		new PlayListEntry { SongName = "Intro", AminName = "We Are" },
		new PlayListEntry { SongName = "Speak to me", AminName = "Dark Side", Sample1 = "SpeakToMe_intro.mp3" },
		new PlayListEntry { SongName = "Breathe", AminName = "Dark Side" },
		new PlayListEntry { SongName = "Time", AminName = "Landing Strip", AminNumber = 4, Sample1 = "Time_intro.mp3" },
		new PlayListEntry { SongName = "The Great Gig In The Sky", AminName = "Dark Side", Sample1 = "TheGreatGigInTheSky_voice.mp3" },
		new PlayListEntry { SongName = "Money", AminName = "Axis Triangle", Sample1 = "Money_intro.mp3", Sample2 = "Money_outro.mp3" },
		new PlayListEntry { SongName = "Us & Them", AminName = "Wool Strings" },
		new PlayListEntry { SongName = "Any Color You Like", AminName = "Dark Side" },
		new PlayListEntry { SongName = "Brain Damage", AminName = "Procedural Moon", Sample1 = "BrainDamage_risata.mp3" },
		new PlayListEntry { SongName = "Eclipse", AminName = "Procedural Moon", Sample1 = "Eclipse_outro.mp3" },
		new PlayListEntry { SongName = "Intro", AminName = "We Are" },

		new PlayListEntry { SongName = "Cymbaline", AminName = "Standard" },
		new PlayListEntry { SongName = "Shine On You Crazy Diamond", AminName = "Astra Domine" },
		new PlayListEntry { SongName = "Dogs", AminName = "Animals", Sample1 = "Dogs_1.mp3", Sample2 = "Dogs_2.mp3" },
		new PlayListEntry { SongName = "Wish You Were Here", AminName = "Electric Strings", Sample1 = "WishYouWereHere_intro.mp3", Sample2 = "WishYouWereHere_outro.mp3" },
		new PlayListEntry { SongName = "Comfortably Numb", AminName = "Moving Dots", Sample1 = "ComfortablyNumb_intro.mp3", Sample2 = "ComfortablyNumb_aaaaa.mp3" },
		new PlayListEntry { SongName = "Another Brick In The Wall", AminName = "The Wall" },
		new PlayListEntry { SongName = "Intro", AminName = "We Are" }
	};

	
    public class PlayList : Instance<PlayList>
    {
        [Output(Guid = "5e679e49-b778-488c-b27c-84bd8fdb6c9a")]
        public readonly Slot<bool> StandardAnim = new();
        [Output(Guid = "5e679e49-b778-488c-b27c-84bd8fdb6c9a")]
        public readonly Slot<bool> DarkSideAnim = new();
        [Output(Guid = "5e679e49-b778-488c-b27c-84bd8fdb6c9a")]
        public readonly Slot<bool> AnimalsAnim = new();
        [Output(Guid = "5e679e49-b778-488c-b27c-84bd8fdb6c9a")]
        public readonly Slot<bool> TheWallAnim = new();
        [Output(Guid = "5e679e49-b778-488c-b27c-84bd8fdb6c9a")]
        public readonly Slot<bool> DedicatedAnim = new();
        [Output(Guid = "5e679e49-b778-488c-b27c-84bd8fdb6c9a")]
        public readonly Slot<int> DedicatedAnimNumber = new();
        [Output(Guid = "5e679e49-b778-488c-b27c-84bd8fdb6c9a")]
        public readonly Slot<int> DedicatedAnimNumber = new();
        [Output(Guid = "5e679e49-b778-488c-b27c-84bd8fdb6c9a")]
        public readonly Slot<string> Sample = new();
        [Output(Guid = "5e679e49-b778-488c-b27c-84bd8fdb6c9a")]
        public readonly Slot<bool> PlayStop = new();
        [Output(Guid = "5e679e49-b778-488c-b27c-84bd8fdb6c9a")]
        public readonly Slot<string> Song = new();

		public ResetOuputs ()
		{
            StandardAnim.Value = false;
            DarkSideAnim.Value = false;
            AnimalsAnim.Value = false;
            TheWallAnim.Value = false;
            DedicatedAnim.Value = false;
			PlayStop.Value = false;
		}

		// read playlist element and set the outputs accordingly
		public SetAnim (PlayListEntry p)
		{
			// reset "pad pressed" outputs
            ResetOuputs ();

			// display SongName on the console
			if (p.AnimName == "Standard") StandardAnim.Value = true;
			if (p.AnimName == "Dark Side") DarkSideAnim.Value = true;
			if (p.AnimName == "Animals") AnimalsAnim.Value = true;
			if (p.AnimName == "The Wall") TheWallAnim.Value = true;
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
			Song.UpdateAction = Update;        }

        private void Update(EvaluationContext context)
        {

        // Iterate through the list and print each object
//        foreach (var f in fList)
//        {
//            Console.WriteLine(f);
//        }

			// check previous / next pads: if present, set position in the playlist, and check the boundaries
			// anti-bouncing mechanism is implemented
			var preventry = PrevEntry.GetValue(context);
			var nextentry = nextEntry.GetValue(context);
			if (preventry) {				// previous pad pressed
				if (!_formerstateprev) {	// make sure it was not pressed before, and set the outputs
					_formerstateprev = true;
					if (_indexInPlayList > 0) _indexInPlayList -=1
					SetAnim (pList [_indexInPlayList]);
				}
			}
			else {
				_formerstateprev = false;
				// reset "pad pressed" outputs
				ResetOuputs ();
			}

			if (nextentry) {				// next pad pressed
				if (!_formerstatenext) {	// make sure it was not pressed before, and set the outputs
					_formerstatenext = true;
					if (_indexInPlayList + 1 < pList.Count) _indexInPlayList +=1
					SetAnim (pList [_indexInPlayList]);				
				}
			}
			else {
				_formerstatenext = false;
				// reset "pad pressed" outputs
				ResetOuputs ();
			}


			// check input from the "sample" pads
            int i = 1;
			bool sampleFound = false;

			foreach (var pad in SamplePads.GetCollectedTypedInputs())
            {
                if (pad.GetValue(context) == true) {
					sampleFound = true;
					break;		// go out the loop
				}
				i = i + 1;
            }
			
			// in case sample is found, set the right outputs
			if (sampleFound) {
				if (!_formerstateplaystop) {	// this is the first time we press the pad
					_formerstateplaystop = true;
					Sample.Value = SetSound (pList, i);
					PlayStop.Value = true;
				}
				else {							// pad is still pressed, but this has been taken into account already
					PlayStop.Value = false;
				}
			}
			else {		// no sample pad pressed, we 
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
		private List<PlayListEntry> pList = new List<PlayListEntry>;

		
        [Input(Guid = "4f4c7495-d7dc-410b-9a1c-2f06c5da6f78")]
        public readonly InputSlot<bool> SamplePads = new();

        [Input(Guid = "4f4c7495-d7dc-410b-9a1c-2f06c5da6f78")]
        public readonly InputSlot<bool> PrevEntry = new();

        [Input(Guid = "4f4c7495-d7dc-410b-9a1c-2f06c5da6f78")]
        public readonly InputSlot<bool> NextEntry = new();
    }
}