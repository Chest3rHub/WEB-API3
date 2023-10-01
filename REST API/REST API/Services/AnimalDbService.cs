using System.Data.SqlClient;
using REST_API.Models;

namespace REST_API.Services;

public class AnimalDbService : IAnimalDbService
{
    
    private const string _connString = "SECRET";

    public async Task<IList<Animal>> GetAnimalsListAsync(string orderBy)
    {
        List<Animal> animals = new();
        string sql;
        await using SqlConnection sqlConnection = new(_connString);
        await using SqlCommand sqlCommand = new();
        if (string.IsNullOrEmpty(orderBy))
        {
            sql = "SELECT * FROM Animal ORDER BY Name";
        }
        else
        {
            sql = $"SELECT * FROM Animal ORDER BY {orderBy}";
            sqlCommand.Parameters.AddWithValue("@orderBy", orderBy);
        }

        sqlCommand.CommandText = sql;
        sqlCommand.Connection = sqlConnection;
        // otwieramy polaczenie
        await sqlConnection.OpenAsync();
        // wszystko asynchronicznie bo inaczej zablokujemy watek głowny aplikacji i bedzie czekac na kazdy response
        // zanim przejdzie dalej

        // zwraca liczbe rekordow objetych operacja, ale bez tych rekordow
        // sqlCommand.ExecuteNonQuery();

        await using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

        while (await sqlDataReader.ReadAsync())
        {
            Animal animal = new()
            {
                IdAnimal = int.Parse(sqlDataReader["IdAnimal"].ToString()),
                Name = sqlDataReader["Name"].ToString(),
                Description = sqlDataReader["Description"].ToString(),
                Category = sqlDataReader["Category"].ToString(),
                Area = sqlDataReader["Area"].ToString()

            };
            animals.Add(animal);
        }
        await sqlConnection.CloseAsync();

        return animals;
    }

    public async Task<int> AddAnimal(Animal animal)
        {
            string sql = "INSERT INTO Animal (Name, Description, Category, Area) " + 
                         "VALUES (@name, @description, @category, @area)";
            await using SqlConnection sqlConnection = new(_connString);
            await using SqlCommand sqlCommand = new(sql, sqlConnection);
            await sqlConnection.OpenAsync();

            sqlCommand.Parameters.AddWithValue("@name", animal.Name);
            sqlCommand.Parameters.AddWithValue("@description", animal.Description);
            sqlCommand.Parameters.AddWithValue("@category", animal.Category);
            sqlCommand.Parameters.AddWithValue("@area", animal.Area);

            int result = await sqlCommand.ExecuteNonQueryAsync();
            await sqlConnection.CloseAsync();

            return result;
        }

        public async Task<int> UpdateAnimal(Animal animal, int idAnimal)
        {
            string sql = "UPDATE Animal SET Name = @name, Description = @description, " +
                         "Category = @category, Area = @area WHERE IdAnimal = @idAnimal";

            await using SqlConnection sqlConnection = new(_connString);
            await using SqlCommand sqlCommand = new(sql, sqlConnection);

            await sqlConnection.OpenAsync();

            sqlCommand.Parameters.AddWithValue("@name", animal.Name);
            sqlCommand.Parameters.AddWithValue("@description", animal.Description);
            sqlCommand.Parameters.AddWithValue("@category", animal.Category);
            sqlCommand.Parameters.AddWithValue("@area", animal.Area);
            sqlCommand.Parameters.AddWithValue("@idAnimal", idAnimal);

            int result = await sqlCommand.ExecuteNonQueryAsync();
            await sqlConnection.CloseAsync();

            return result;
        }

        public async Task<int> DeleteAnimal(int idAnimal)
        {
            string sql = "DELETE FROM Animal WHERE IdAnimal = @idAnimal";

            await using SqlConnection sqlConnection = new(_connString);
            await using SqlCommand sqlCommand = new(sql, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@idAnimal", idAnimal);
            await sqlConnection.OpenAsync();

            int result = await sqlCommand.ExecuteNonQueryAsync();
            await sqlConnection.CloseAsync();

            return result;
        }
    }
