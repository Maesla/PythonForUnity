namespace MicrokernelSystem.Addons.External
{
    public class Triangle
    {
        private readonly float @base;
        private readonly float length;

        public Triangle(float @base, float length)
        {
            this.@base = @base;
            this.length = length;
        }

        public float CalculateArea()
        {
            return @base * length * 0.5f;
        }
    } 
}
