namespace DevProdWebApp.Utilities
{
    public class Initializer
    {
        public static List<int> Discrete { get; set; }
        public static List<double> Continuous { get; set; }
        public static List<int> Binary { get; set; }
        public Initializer()
        {
                Binary = GenerateRandomBinary();
                Continuous = GenerateRandomContinuous();
                Discrete = GenerateRandomDiscrete();
        }
        public List<int> GenerateRandomDiscrete()
        {
            List<int> discrete = new List<int>();
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                discrete.Add(random.Next(0, 1000));
            }
            return discrete;
        }
        public List<double> GenerateRandomContinuous()
        {
            List<double> continuous = new List<double>();
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                continuous.Add(0 + (random.NextDouble() * (100 - 0)));
            }
            return continuous;
        }

        public List<int> GenerateRandomBinary()
        {
            List<int> binary = new List<int>();
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                binary.Add(random.Next(0, 2));
            }
            return binary;
        }
    }
}
