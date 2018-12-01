using MapMaker.DocumentModel;

namespace MapMaker.Tests.DocumentModel
{
    public class SampleMedium
    {
        public Revision<byte> ByteValue { get; }
        public Revision<short> ShortValue { get; }
        public Revision<int> IntValue { get; }
        public Revision<long> LongValue { get; }
        public Revision<sbyte> SByteValue { get; }
        public Revision<ushort> UShortValue { get; }
        public Revision<uint> UIntValue { get; }
        public Revision<ulong> ULongValue { get; }
        public Revision<float> FloatValue { get; }
        public Revision<double> DoubleValue { get; }
        public Revision<decimal> DecimalValue { get; }
        public Revision<bool> BoolValue { get; }

        public SampleMedium(IVersionController controller)
        {
            ByteValue = new Revision<byte>(controller);
            ShortValue = new Revision<short>(controller);
            IntValue = new Revision<int>(controller);
            LongValue = new Revision<long>(controller);
            SByteValue = new Revision<sbyte>(controller);
            UShortValue = new Revision<ushort>(controller);
            UIntValue = new Revision<uint>(controller);
            ULongValue = new Revision<ulong>(controller);
            FloatValue = new Revision<float>(controller);
            DoubleValue = new Revision<double>(controller);
            DecimalValue = new Revision<decimal>(controller);
            BoolValue = new Revision<bool>(controller);
        }
    }
}