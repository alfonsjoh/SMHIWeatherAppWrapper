namespace WeatherApp.Models.Toolbox;

public static class CollectionHelper
{
    public static T MostCommon<T>(this IEnumerable<T> collection)
    {
        return collection.GroupBy(item => item)
            .OrderByDescending(group => group.Count())
            .First()
            .Key;
    }
    
    public static T RandomElement<T>(this IEnumerable<T> collection, Random rng)
    {
        var enumerable = collection as T[] ?? collection.ToArray();
        var index = rng.Next(enumerable.Length);
        return enumerable.ElementAt(index);
    }
}