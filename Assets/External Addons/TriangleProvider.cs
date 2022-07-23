namespace MicrokernelSystem.Addons.External
{
    public class TriangleProvider : BasePythonAPIProvider
    {
        private IBufferWriter buffer;
        public TriangleProvider(IBufferWriter buffer)
        {
            this.buffer = buffer;
        }

        public override string Code
        {
            get
            {
                return
@"
from MicrokernelSystem.Addons.External import Triangle
";
            }
        }

        public Triangle Create(float @base, float height)
        {
            var triangle = new Triangle(@base, height);
            buffer.Write(triangle);
            return triangle;
        }
    } 
}
