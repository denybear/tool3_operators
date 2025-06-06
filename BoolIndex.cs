using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_a9110543_78d9_4ede_8d9b_a7a557950721
{
    public class BoolIndex : Instance<BoolIndex>
    {
        [Output(Guid = "5e679e49-b778-488c-b27c-84bd8fdb6c9a")]
        public readonly Slot<int> Result = new();

        public BoolIndex()
        {
            Result.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            var anyConnected = false;

            // set default value if not set yet
            if (_defaultvalue == -1)
            {
                _defaultvalue = DefValue.GetValue(context);
                Result.Value = _defaultvalue;
            }

            int i = 0;
            foreach (var input in Input.GetCollectedTypedInputs())
            {
                anyConnected = true;
                if (input.GetValue(context) == true) Result.Value = i; // update result value if input is triggered
                i += 1;                              // if no input is triggered, keep previous value
            }

            if (anyConnected == false) Result.Value = _defaultvalue;    // if no input is connected, set result to default value
        }

        private int _defaultvalue = -1;

        [Input(Guid = "4f4c7495-d7dc-410b-9a1c-2f06c5da6f78")]
        public readonly MultiInputSlot<bool> Input = new();

        [Input(Guid = "cf0e3d52-4d97-4dd1-b4bb-c262af36db59")]
        public readonly InputSlot<int> DefValue = new();
    }
}