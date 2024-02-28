namespace HomeBankingMinHub.Utilities
{
    public static class Number
    {
        public static int RandomNumber(int min, int max)
        {
          Random random = new Random();
          return random.Next(min, max);
        }
    }
}
