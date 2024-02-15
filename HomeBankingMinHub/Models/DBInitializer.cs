namespace HomeBankingMinHub.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client { Email = "vcoronado@gmail.com", FirstName="Victor", LastName="Coronado", Password="111111"},
                    new Client { Email = "jperez@gmail.com", FirstName="Juan", LastName="Perez", Password="222222"},
                    new Client { Email = "ppicapiedra@gmail.com", FirstName="Pedro", LastName="Picapiedra", Password="333333"},
                    new Client { Email = "pcalvo@gmail.com", FirstName="Pablo", LastName="Calvo", Password="444444"},
                    new Client { Email = "iprado@gmail.com", FirstName="Ignacio", LastName="Prado", Password="123456"}

                };

                context.Clients.AddRange(clients);

                //guardamos
                context.SaveChanges();
            }

        }
    }
}
