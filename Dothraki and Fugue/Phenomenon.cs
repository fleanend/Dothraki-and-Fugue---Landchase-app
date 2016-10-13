namespace Dothraki_and_Fugue
{
    class Phenomenon : Card
    {
        public bool IsChangeOfSeason;
        public Phenomenon(string path, string name) : base(path, name)
        {
            IsChangeOfSeason = false;
        }

        public Phenomenon( string path, string name, bool isCoS ) : base(path, name)
        {
            IsChangeOfSeason = isCoS;
        }

        public Phenomenon(Card card, bool iCoS) : base(card)
        {
            IsChangeOfSeason = iCoS;
        }

        public override bool IsTransforming()
        {
            return false;
        }
    }
}