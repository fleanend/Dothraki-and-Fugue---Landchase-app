namespace Dothraki_and_Fugue
{
    class Phenomenon : Card
    {
        public Phenomenon(string path, string name) : base(path, name)
        {
        }
        public Phenomenon(Card card) : base(card)
        {
        }
        public override bool IsTransforming()
        {
            return false;
        }
    }
}