using Humanizer;

namespace HomeBankingMinHub.Utilities
{
    public static class Number
    {
        public static String GenerateAccountNumber()
        {
            Random random = new Random();
            int randomnumber = random.Next(0, 99999999);
            return "VIN-" + randomnumber.ToString("D8");
        }
        public static int GenerateCvv()
        {
            Random random = new Random();
            return random.Next(100, 999);
        }
        public static String GenerateCreditNumber()
        {
            Random random = new Random();
            int randomNumber = 0;
            string numberCard="";
            for (int i = 0; i <= 3; i++)
            {
                randomNumber = random.Next(0, 9999);
                if (i > 2)
                {
                    numberCard = numberCard + randomNumber.ToString("D4");
                }
                else
                {
                    numberCard = numberCard + randomNumber.ToString("D4") + "-";
                }
            }
            return numberCard;
        }
    }
}
