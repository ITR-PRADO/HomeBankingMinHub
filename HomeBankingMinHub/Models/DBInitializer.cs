﻿namespace HomeBankingMinHub.Models
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
            if (!context.Accounts.Any())
            {
                var accountVictor = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (accountVictor != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountVictor.Id, CreationDate = DateTime.Now, Number = "VIN001", Balance = 0 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();
                }
                var accountJuan = context.Clients.FirstOrDefault(c => c.Email == "jperez@gmail.com");
                if (accountJuan != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountJuan.Id, CreationDate = new DateTime(2023,11,20), Number = "VIN002", Balance = 2 },
                        new Account {ClientId = accountJuan.Id, CreationDate = DateTime.Now.AddDays(-10), Number = "VIN003", Balance = 10 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();

                }
                var accountPedro = context.Clients.FirstOrDefault(c => c.Email == "ppicapiedra@gmail.com");
                if (accountPedro != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountPedro.Id, CreationDate = DateTime.Now.AddMonths(-2), Number = "VIN004", Balance = 10 },
                        new Account {ClientId = accountPedro.Id, CreationDate = DateTime.Now.AddMonths(-1), Number = "VIN005", Balance = 20 },
                        new Account {ClientId = accountPedro.Id, CreationDate = DateTime.Now.AddDays(-15), Number = "VIN006", Balance = 30 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();

                }
                var accountPablo = context.Clients.FirstOrDefault(c => c.Email == "pcalvo@gmail.com");
                if (accountPablo != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountPablo.Id, CreationDate = DateTime.Now.AddYears(-3), Number = "VIN007", Balance = 0 },
                        new Account {ClientId = accountPablo.Id, CreationDate = DateTime.Now.AddDays(1), Number = "VIN008", Balance = 80 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();

                }
                var accountIgnacio = context.Clients.FirstOrDefault(c => c.Email == "iprado@gmail.com");
                if (accountIgnacio != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountIgnacio.Id, CreationDate = DateTime.Now.AddDays(-2), Number = "VIN009", Balance = 1000 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                    context.SaveChanges();

                }
            }
            if (!context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(acc => acc.Number == "VIN001");
                if (account1 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { AccountId= account1.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT },
                        new Transaction { AccountId= account1.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT },
                        new Transaction { AccountId= account1.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT },
                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
                var account2 = context.Accounts.FirstOrDefault(acc => acc.Number == "VIN004");
                if (account2 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { AccountId= account2.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT },
                        new Transaction { AccountId= account2.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT },
                        new Transaction { AccountId= account2.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT },
                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
                var account3 = context.Accounts.FirstOrDefault(acc => acc.Number == "VIN006");
                if (account3 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { AccountId= account3.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT },
                        new Transaction { AccountId= account3.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT },
                        new Transaction { AccountId= account3.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT },
                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
                var account4 = context.Accounts.FirstOrDefault(acc => acc.Number == "VIN008");
                if (account4 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { AccountId= account4.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT },
                        new Transaction { AccountId= account4.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT },
                        new Transaction { AccountId= account4.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT },
                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
                var account5 = context.Accounts.FirstOrDefault(acc => acc.Number == "VIN009");
                if (account5 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { AccountId= account5.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT },
                        new Transaction { AccountId= account5.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT },
                        new Transaction { AccountId= account5.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT },
                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }

            }
            if (!context.Loans.Any())
            {
                var loans = new Loan[]
                {
                    new Loan { Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60" },
                    new Loan { Name = "Personal", MaxAmount = 100000, Payments = "6,12,24" },
                    new Loan { Name = "Automotriz", MaxAmount = 300000, Payments = "6,12,24,36" },
                };
                foreach (Loan loan in loans)
                {
                    context.Loans.Add(loan);
                }
                context.SaveChanges();

                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (client1 != null)
                {
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 400000,
                            ClientId = client1.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }
                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client1.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }
                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 100000,
                            ClientId = client1.Id,
                            LoanId = loan3.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }
                    context.SaveChanges();

                }
                var client2 = context.Clients.FirstOrDefault(c => c.Email == "jperez@gmail.com");
                if (client2 != null)
                {
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 500000,
                            ClientId = client2.Id,
                            LoanId = loan1.Id,
                            Payments = "48"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }
                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 20000,
                            ClientId = client2.Id,
                            LoanId = loan2.Id,
                            Payments = "6"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }
                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 150000,
                            ClientId = client2.Id,
                            LoanId = loan3.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }
                    context.SaveChanges();

                }
                var client3 = context.Clients.FirstOrDefault(c => c.Email == "ppicapiedra@gmail.co");
                if (client3 != null)
                {
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 300000,
                            ClientId = client3.Id,
                            LoanId = loan1.Id,
                            Payments = "36"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }
                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 100000,
                            ClientId = client3.Id,
                            LoanId = loan2.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }
                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 30000,
                            ClientId = client3.Id,
                            LoanId = loan3.Id,
                            Payments = "6"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }
                    context.SaveChanges();

                }
                var client4 = context.Clients.FirstOrDefault(c => c.Email == "pcalvo@gmail.com");
                if (client4 != null)
                {
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 500000,
                            ClientId = client4.Id,
                            LoanId = loan1.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }
                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client4.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }
                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client4.Id,
                            LoanId = loan3.Id,
                            Payments = "6"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }
                    context.SaveChanges();

                }
                var client5 = context.Clients.FirstOrDefault(c => c.Email == "iprado@gmail.com");
                if (client5 != null)
                {
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 300000,
                            ClientId = client5.Id,
                            LoanId = loan1.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }
                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client5.Id,
                            LoanId = loan2.Id,
                            Payments = "6"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }
                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 300000,
                            ClientId = client5.Id,
                            LoanId = loan3.Id,
                            Payments = "36"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }
                    context.SaveChanges();

                }

            }

        }
    }
}
