namespace Dothraki_and_Fugue
{
   public class TransformingPlane : Plane
    {
        public int Id;
        public bool DorN;
        public TransformingPlane D;
        public TransformingPlane N;
        public void Link(TransformingPlane other)
        {
            if (DorN)
            {
                N = other;
                other.D = this;
            }
            else
            {
                D = other;
                other.N = this;
            }
        }
        public bool SameId(TransformingPlane other)
        {
            return this.Id == other.Id && this.Name != other.Name;
        }


        public TransformingPlane(string path, string name, int id, bool dOrN) : base(path, name)
        {
            this.Id = id;
            this.DorN = dOrN;
            if (dOrN)
            {
                D = this;
                N = null;
            }
            else
            {
                D = null;
                N = this;
            }
        }
        public override bool IsTransforming()
        {
            return true;
        }

        public TransformingPlane ManageToggle()
        {
            return DorN ? N : D;
        }
    }
}