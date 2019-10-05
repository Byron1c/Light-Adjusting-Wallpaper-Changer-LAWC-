

namespace LAWC.Common
{
    internal class ComboboxItemCustom
    {
        public string Text { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
