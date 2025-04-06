using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace TicketHubFunction
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Function1))]
        public async Task RunAsync([QueueTrigger("tickethub", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

            string messageJson = message.MessageText;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var customer = JsonSerializer.Deserialize<Customer>(messageJson, options);

            if(customer == null)
            {
                _logger.LogError("Failed to desearalize message");
                return;
            }

            _logger.LogInformation($"Customer: {customer.Name}, {customer.Email}");

            //Add contact to Azure SQL Database

            // get connection string from app settings
            string? connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("SQL connection string is not set in the environment variables.");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync(); // Note the ASYNC

                var query = "INSERT INTO Customer (ConcertID, Email, Name, Phone, Quantity, CreditCard, Expiration, SecurityCode, Address, City, Province, PostalCode, Country) VALUES (@ConcertID, @Email, @Name, @Phone, @Quantity, @CreditCard, @Expiration, @SecurityCode, @Address, @City, @Province, @PostalCode, @Country);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ConcertID", customer.ConcertID);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                    cmd.Parameters.AddWithValue("@Quantity", customer.Quantity);
                    cmd.Parameters.AddWithValue("@CreditCard", customer.CreditCard);
                    cmd.Parameters.AddWithValue("@Expiration", customer.Expiration);
                    cmd.Parameters.AddWithValue("@SecurityCode", customer.SecurityCode);
                    cmd.Parameters.AddWithValue("@Address", customer.Address);
                    cmd.Parameters.AddWithValue("@City", customer.City);
                    cmd.Parameters.AddWithValue("@Province", customer.Province);
                    cmd.Parameters.AddWithValue("@PostalCode", customer.PostalCode);
                    cmd.Parameters.AddWithValue("@Country", customer.Country);

                    await cmd.ExecuteNonQueryAsync();
                }
            }

        }
    }
}
