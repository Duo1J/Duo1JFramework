using System.Threading;

namespace Duo1JFramework.Ext.Atomic
{
    /// <summary>
    /// Ô­×ÓÐÔInt
    /// </summary>
    public class AtomicInt
    {
        private int _value;

        public int Read()
        {
            return Thread.VolatileRead(ref _value);
        }

        public void Write(int value)
        {
            Thread.VolatileWrite(ref _value, value);
        }

        public int Increment()
        {
            return Interlocked.Increment(ref _value);

            //int initialValue, newValue;
            //do
            //{
            //    initialValue = Read();
            //    newValue = initialValue + 1;
            //} while (Interlocked.CompareExchange(ref _value, newValue, initialValue) != initialValue);

            //return newValue;
        }

        public int Decrement()
        {
            return Interlocked.Decrement(ref _value);

            //int initialValue, newValue;
            //do
            //{
            //    initialValue = Read();
            //    newValue = initialValue - 1;
            //} while (Interlocked.CompareExchange(ref _value, newValue, initialValue) != initialValue);

            //return newValue;
        }
    }
}
