using System;
using System.Threading.Tasks;
using Hazelcast;
using Hazelcast.DistributedObjects;

class HazelcastCRUD
{
    static async Task Main(string[] args)
    {
        var hazelcastOptions = new HazelcastOptionsBuilder().With(options =>
        {
            options.ClusterName = "hello-world";
            options.Networking.Addresses.Add("127.0.0.1:5701");
        }).Build();

        await using var hazelcastClient = await HazelcastClientFactory.StartNewClientAsync(hazelcastOptions);
        var hazelcastMap = await hazelcastClient.GetMapAsync<string, string>("default");

        while (true)
        {
            var choice = IndexScreen();

            switch (choice)
            {
                case "1":
                    await PerformCreate(hazelcastMap);
                    break;
                case "2":
                    await PerformRead(hazelcastMap);
                    break;
                case "3":
                    await PerformUpdate(hazelcastMap);
                    break;
                case "4":
                    await PerformDelete(hazelcastMap);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Gecersiz secim.\n");
                    break;
            }
        }
    }
    private static String IndexScreen()
    {
        Console.WriteLine("Hazelcast CRUD islemi seciniz.");
        Console.WriteLine("(1) Create");
        Console.WriteLine("(2) Read");
        Console.WriteLine("(3) Update");
        Console.WriteLine("(4) Delete");
        Console.WriteLine("(5) Exit");
        Console.Write("Secim: ");
        return Console.ReadLine();
    }

    private static async Task PerformCreate(IHMap<string, string> hazelcastMap)
    {
        Console.Write("Anahtar (key) giriniz: ");
        var key = Console.ReadLine();
        Console.Write("Deger (value) giriniz: ");
        var value = Console.ReadLine();
        await hazelcastMap.SetAsync(key, value);
        Console.WriteLine("Veri basariyla eklendi.\n");
    }

    private static async Task PerformRead(IHMap<string, string> hazelcastMap)
    {
        Console.Write("Anahtar (key) giriniz: ");
        var key = Console.ReadLine();
        var value = await hazelcastMap.GetAsync(key);
        Console.WriteLine(value != null ? $"Deger (value): {value}\n" : "Veri bulunamadi.\n");
    }

    private static async Task PerformUpdate(IHMap<string, string> hazelcastMap)
    {
        Console.Write("Anahtar (key) giriniz: ");
        var key = Console.ReadLine();
        Console.Write("Deger (value) giriniz: ");
        var newValue = Console.ReadLine();
        await hazelcastMap.SetAsync(key, newValue);
        Console.WriteLine("Veri basariyla guncellendi.\n");
    }

    private static async Task PerformDelete(IHMap<string, string> hazelcastMap)
    {
        Console.Write("Anahtar (key) giriniz: ");
        var key = Console.ReadLine();
        await hazelcastMap.DeleteAsync(key);
        Console.WriteLine("Veri basariyla silindi.\n");
    }
}
