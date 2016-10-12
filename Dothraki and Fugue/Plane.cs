namespace Dothraki_and_Fugue
{
    public class Plane : Card
    {

        public Plane(string path, string name) : base(path, name)
        {

        }
        public override bool IsTransforming()
        {
            return false;
        }
    }
}