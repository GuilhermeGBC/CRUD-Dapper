using System;
using Microsoft.Data.SqlClient;
using Dapper;
using BaltaDataAccess.Models;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace BaltaDataAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=guisql;User ID=sa;Password=";

            using (var connection = new SqlConnection(connectionString))
            {
                //Devemos inserir aqui os métodos que criamos para "linkar" com a conexão ao banco.

                //CreateCategory(connection);
                //CreateManyCategory(connection);
                //UpdateCategory(connection);
                //DeleteCategory(connection);
                //ListCategories(connection);
                //GetCategory(connection);
                //ExecuteProcedure(connection);
                //ExecuteReadProcedures(connection);
                //ExecuteScalar(connection);
                //ReadView(connection);
                //OneToOne(connection);
                //OneToMany(connection);
                //QueryMultiple(connection);
                //SelectIn(connection);
                //Like(connection);

                //Forma utilizando ADO puro abaixo

                //var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]"); //Já utilizando Dapper. Caso os nomes das propriedades sejam outros, podemos utilizar ALIAS

                //foreach (var item in categories)
                //{
                //    Console.WriteLine($"{item.Id} {item.Title}");
                //}

                //connection.Open(); - ADO

                //using (var command = new SqlCommand())
                //{
                //    command.Connection = connection; Para ler informações do banco.
                //    command.CommandType = System.Data.CommandType.Text;
                //    command.CommandText = "SELECT [Id], [Title] FROM [Category]"; Os campos viram uma coluna, como se fosse um índice começando por 0.

                //    var reader = new SqlDataReader();  Forma mais rápida e pura de ler informações.
                //    var reader = command.ExecuteReader();

                //    while (reader.Read()) Percorrendo as linhas, enquanto conseguir ler, o cursor ir para frente
                //    {
                //        Console.WriteLine($"{reader.GetGuid(0)} - {reader.GetString(1)} "); Mapear. Cada campo do SELECT ele joga para dentro de uma coluna: Sql Data Column
                //    } 

                //}
            }

        }

        static void ListCategories(SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]"); //Já utilizando Dapper. Caso os nomes das propriedades sejam outros, podemos utilizar ALIAS. Ex: [Id] AS [Codigo] 

            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} {item.Title}");
            }
        }

        static void GetCategory(SqlConnection connection)
        {
            var category = connection.
                QueryFirstOrDefault<Category>("SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id] = @id", new
                {
                    id = "af3407aa-11ae-4621-a2ef-2028b85507c4"
                });

            Console.WriteLine($"{category.Id} {category.Title}");

        }

        static void CreateCategory(SqlConnection connection)
        {
            Category category = new Category(); //Instanciando a entidade.
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO 
                [Category] 
            VALUES(
                @Id, 
                @Title, 
                @Url, 
                @Summary, 
                @Order, 
                @Description, 
                @Featured)"; //Caso o nome dos parâmetros sejam os mesmos das propriedades, nãp será necessário atribuir o nome do parâmetro à propriedade, pois o nome sendo o mesmo ele identifica automático.

            var rows = connection.Execute(insertSql, new //Executa o insertSql
            {
                Id = category.Id,
                Title = category.Title,
                Url = category.Url,
                Summary = category.Summary,
                Order = category.Order,
                Description = category.Description,
                Featured = category.Featured
            });

            Console.WriteLine($"{rows} linhas inseridas!");
        }

        static void CreateManyCategory(SqlConnection connection)
        {
            Category category = new Category(); //Instanciando a entidade.
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            Category category1 = new Category();
            category1.Id = Guid.NewGuid();
            category1.Title = "Categoria Nova";
            category1.Url = "categoria-nova";
            category1.Description = "Categoria nova";
            category1.Order = 9;
            category1.Summary = "Categoria";
            category1.Featured = true;

            var insertSql = @"INSERT INTO 
                [Category] 
            VALUES(
                @Id, 
                @Title, 
                @Url, 
                @Summary, 
                @Order, 
                @Description, 
                @Featured)"; //Caso o nome dos parâmetros sejam os mesmos das propriedades, nãp será necessário atribuir o nome do parâmetro à propriedade, pois o nome sendo o mesmo ele identifica automático.

            var rows = connection.Execute(insertSql, new[] //utilizando um array para criarmos mais de 1 categoria ao mesmo tempo.
            {
               new
               {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
               },
                new
                {
                    category1.Id,
                    category1.Title,
                    category1.Url,
                    category1.Summary,
                    category1.Order,
                    category1.Description,
                    category1.Featured
                }
            });

            Console.WriteLine($"{rows} linhas inseridas!");
        }

        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title]=@title WHERE [Id]=@id";
            var rows = connection.Execute(updateQuery, new
            {

                id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"),
                title = "Frontend 2022"
            });

            Console.WriteLine($"{rows} registros atualizadao!");
        }

        static void DeleteCategory(SqlConnection connection)
        {
            var deleteQuery = "DELETE [Category] WHERE [Id] = @id";
            var rows = connection.Execute(deleteQuery, new
            {
                id = "ea8059a2-e679-4e74-99b5-e4f0b310fe6f"
            });

            Console.WriteLine($"{rows} registros excluídos!");
        }

        static void ExecuteProcedure(SqlConnection connection)
        {
            var proc = "[spDeletaEstudante]";
            var pars = new { StudentId = "6bd552ea-7187-4bae-abb6-54e8f8b9f530" };
            var affectedRows = connection.Execute(proc,
                pars,
                commandType: CommandType.StoredProcedure);

            Console.WriteLine($"{affectedRows} linhas afetadas!");
        }

        static void ExecuteReadProcedures(SqlConnection connection)
        {
            var proc = "[spGetCoursesByCategory]";
            var parametro = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };
            var courses = connection.Query(proc, //<Category>
                parametro,
                commandType: CommandType.StoredProcedure);

            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Title}"); //Caso na connection.Query<PROPRIEDADE> não esteja especificando a ENTIDADE, não conseguimos acessar as propriedades.
            }
        }

        static void ExecuteScalar(SqlConnection connection)
        {
            var category = new Category();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"
                INSERT INTO 
                    [Category] 
                OUTPUT inserted.[Id] //Select ScopedIdentity, funciona apenas para campo id INTEIRO, para GUID temos o OUTPUT Insers
                VALUES(
                    NEWID(), 
                    @Title, 
                    @Url, 
                    @Summary, 
                    @Order, 
                    @Description, 
                    @Featured) 
                ";

            var id = connection.ExecuteScalar<Guid>(insertSql, new
            {
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"A categoria inserida foi: {id}");
        }

        static void ReadView(SqlConnection connection)
        {
            var view = "SELECT * FROM vwCursos";
            var reads = connection.Query<Category>(view);

            foreach (var read in reads)
            {
                Console.WriteLine($"{read.Id} - {read.Title}");
            }

        }

        static void OneToOne(SqlConnection connection)
        {
            // 1 pra 1, INNER JOIN. - Criar as classes e abstrações necessárias. Uma carreira tem um curso.
            var sql = @"SELECT * FROM [CareerItem] 
                      INNER JOIN [Course] ON CareerItem.CourseId = Course.Id";

            var items = connection.Query<CareerItem, Course, CareerItem>(sql, //O último CareerItem quer dizer que o resultado final da junção dos dois está no mesmo.
                (careerItem, course) =>
            {
                careerItem.Course = course;
                return careerItem;
            }, splitOn: "Id"); //Definindo aonde acaba o CareerItem e quando começa o Course pelo Id do Curso.

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title} - Curso:{item.Course.Title}");
            }
        }

        static void OneToMany(SqlConnection connection)
        {
            var sql = @"SELECT 
                          [Career].[Id],   
                          [Career].[Title],    
                          [CareerItem].[CareerId],
                          [CareerItem].[Title] 
                        FROM    
                          [Career]
                        INNER JOIN 
                          [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
                        ORDER BY 
                          [Career].[Title]";

            var careers = new List<Career>();

            var items = connection.Query<Career, CareerItem, Career>(sql,
                (career, item) =>
            {
                var car = careers.Where(x => x.Id == career.Id).FirstOrDefault(); //verificando se o item já existe na CARREIRA

                if (car == null) //Validando a carreira
                {
                    car = career;
                    car.Items.Add(item); //Populando a carreira, adicionando um novo item na carreira (List)
                    careers.Add(car); // Criando uma carreira, adicionando um item nessa carreira e adicionando a carreira na list.
                }
                else //Caso o item já exista na lista.
                {
                    car.Items.Add(item);
                }
                return career;
            }, splitOn: "CareerId");

            foreach (var career in careers)
            {
                Console.WriteLine($"{career.Title}");
                foreach (var item in career.Items)
                {
                    Console.WriteLine($" - {item.Title}");
                }
            }
        }

        static void QueryMultiple(SqlConnection connection)
        {
            var sql = "SELECT * FROM [Category]; SELECT * FROM [Course]";

            using (var multiQuery = connection.QueryMultiple(sql))
            {
                var categories = multiQuery.Read<Category>();
                var courses = multiQuery.Read<Course>();

                foreach (var item in categories)
                {
                    Console.WriteLine($"{item.Title}");
                }

                foreach (var item in courses)
                {
                    Console.WriteLine($"{item.Title}");
                }
            }
        }

        static void SelectIn(SqlConnection connection)
        {
            var query = @"select * from Career where [Id] IN @Id";

            var items = connection.Query<Career>(query, new
            {
                Id = new[] //Parâmetro com um array dentro, @Id da QUERY
                 {
                    "4327ac7e-963b-4893-9f31-9a3b28a4e72b",
                    "e6730d1c-6870-4df3-ae68-438624e04c72"
                }
            });
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title}");
            }
        }

        static void Like(SqlConnection connection)//Podemos também, esperar como parâmetro o "termo", assim quando formos chamar o método, colocamos, exemplo: (sqlconnection connection, string term).
        {
            var query = @"SELECT * FROM Course WHERE Title LIKE @exp";
            var termo = "api";
            var items = connection.Query<Course>(query, new
            {
                exp = $"%{termo}%"
            });
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title}");
            }

        }

        static void Transaction(SqlConnection connection)
        {
            Category category = new Category(); //Instanciando a entidade.
            category.Id = Guid.NewGuid();
            category.Title = "Categoria não salvar";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO 
                [Category] 
            VALUES(
                @Id, 
                @Title, 
                @Url, 
                @Summary, 
                @Order, 
                @Description, 
                @Featured)"; //Caso o nome dos parâmetros sejam os mesmos das propriedades, nãp será necessário atribuir o nome do parâmetro à propriedade, pois o nome sendo o mesmo ele identifica automático.

            connection.Open();
            using (var transaction = connection.BeginTransaction()) //Tudo que for colocado dentro, será executado em uma transação. Rollback/Commit
            {
                var rows = connection.Execute(insertSql, new 
                {
                    Id = category.Id,
                    Title = category.Title,
                    Url = category.Url,
                    Summary = category.Summary,
                    Order = category.Order,
                    Description = category.Description,
                    Featured = category.Featured
                }, transaction);
                transaction.Rollback();

                Console.WriteLine($"{rows} linhas inseridas!");
            }            
        }
    }
}
