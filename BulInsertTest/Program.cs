using BulInsertTest.Context;
using BulInsertTest.Entities;
using EFCore.BulkExtensions;
using System.Diagnostics;

var produtos = new List<Produto>();

for (int i = 0; i < 1000000; i++)
    produtos.Add(new Produto
    {

        Nome = $"Produto {i}",
        Descricao = $"Descricao do produto {i}",
        Preco = i * 10.99M
    });

using (var context = new AppDbContext())
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    Console.WriteLine("\nIniciando a inclusao usando a abordagem padrao");

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    context.AddRange(produtos);
    context.SaveChanges();

    stopwatch.Stop();
    Console.WriteLine($"Insert Padrao : {stopwatch.ElapsedMilliseconds} ms");
};

using (var context = new AppDbContext())
{

    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    Console.WriteLine("\nIniciando a inclusao usando a biblioteca EFCore. BulkExtensions");

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    await context.BulkInsertAsync(produtos);

    stopwatch.Stop();
    Console.WriteLine($"Bulk Insert : {stopwatch.ElapsedMilliseconds} ms");

}

using (var context = new AppDbContext())
{

    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    // Criar uma lista de produtos
    var newProdutos = Enumerable.Range(1, 1000000)
    .Select(i => new Produto()
    {


        Nome = $"Produto {i}",
        Descricao = $"Descriçao do produto {i}",
        Preco = i * 10.99M

    }).ToList();

    Console.WriteLine("\nIniciando a inclusao usando uma abordagem otimizada");

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    // Desativa o changetracker (desabilita a validacao automatica das entidades)
    context.ChangeTracker.AutoDetectChangesEnabled = false;

    // Dividir a operacao em lotes menores
    // (ajuste o tamanho do batch conforme necessário)
    // Chama o SaveChanges periodicamente
    const int batchSize = 500;
    for (int i = 0; i < newProdutos.Count; i += batchSize)
    {
        var batch = newProdutos.Skip(i).Take(batchSize).ToList();
        context.Produtos.AddRange(batch);
        await context.SaveChangesAsync();
    }

    context.ChangeTracker.AutoDetectChangesEnabled = true;

    stopwatch.Stop();
    Console.WriteLine($"Insert Otimizado : {stopwatch.ElapsedMilliseconds} ms");

}