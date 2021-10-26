/// Basic cache of microsoft
var serviceCollections = new ServiceCollection();
serviceCollections.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:55870";
    options.InstanceName = "SampleInstance";
});

var provider = serviceCollections.BuildServiceProvider();
var cache = provider.GetService<IDistributedCache>();
var data = Encoding.UTF8.GetBytes("This is second message");
cache.Set("SecondKey", data);
var cachedMessage = cache.Get("SecondKey");
System.Console.WriteLine($"The Message = {Encoding.UTF8.GetString(cachedMessage)}");


/// Create a redis Multiplixer connection
/// We Can use more instances and Multiplixe them in one redis connection 
/// configuration "localhost:6379,localhost:55870"
/// db[0] = dbInstance => localhost:6379
/// db[1] = dbInstance => localhost:55870
/// Get Databases instances using GetDatabase()
ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379,localhost:55870");
var db0 = redis.GetDatabase(0);
var db1 = redis.GetDatabase(1);


/// Create a new HashSet with Key = hash Set 1 and Add new Hash Entry 
db0.HashSet(new RedisKey("hash Set 1"), new HashEntry[] {
                new HashEntry("Key 1", "Value 5"),
            });
/// Update a  HashSet with Key = hash Set 1 and Update "Key 1" Entery Add new 2 Hash Entries
db0.HashSet(new RedisKey("hash Set 1"), new HashEntry[] {
                new HashEntry("Key 1", "Value 1"),
                new HashEntry("Key 2", "Value 2"),
                new HashEntry("Key 3", "Value 3"),
            });

System.Console.WriteLine("======================== Get One of Hash Set =====================");

var setOfKey1 = (string)db0.HashGet(new RedisKey("hash Set 1"), new RedisValue("Key 1"));
System.Console.WriteLine(setOfKey1);
System.Console.WriteLine("======================== Get All Set =====================");
var allSet = db0.HashGetAll(new RedisKey("hash Set 1"));
foreach (var hash in allSet)
{
    System.Console.WriteLine(hash.Name + " : " + hash.Value);
}

System.Console.WriteLine("======================== Get lease of Set =====================");
var leaseSet = db0.HashGetLease(new RedisKey("hash Set 1"), new RedisValue("Key 1"));
System.Console.WriteLine(Encoding.UTF8.GetString(leaseSet.Span));

System.Console.WriteLine("======================== Get all keys of Set =====================");
var keys = db0.HashKeys(new RedisKey("hash Set 1"));
foreach (var key in keys)
{
    System.Console.WriteLine(key);
}





// db0.StringSet("Hello First from db 0 ", "Hello World");
//db1.StringSet("Hello First from db 0 ", "Hello World 2");
//var redisKey =
//db0.SetAdd(new RedisKey("firstAdd"), new RedisValue("first Value"));
//db0.SetScan(new RedisKey("firstAdd"), new RedisValue("first Value"));
db0.GeoAdd(new RedisKey("Geo Add"), 50, 50, new RedisValue("Geo Value"));
db0.SortedSetAdd(new RedisKey("Sorted Add"), new RedisValue("Sorted Value"), 50);
//db1.SetAdd(new RedisKey("firstAdd"), new RedisValue("first Value"));
db0.SortedSetAdd(new RedisKey("Sorted Add"), new RedisValue("Sorted Value 2"), 30);
db0.SortedSetAdd(new RedisKey("Sorted Add"), new RedisValue("Sorted Value 1"), 40);
//db1.SetAdd(new RedisKey("firstAdd"), new RedisValue("first Value"));
db0.ListRightPush(new RedisKey("First List"), new RedisValue("First Value"));

