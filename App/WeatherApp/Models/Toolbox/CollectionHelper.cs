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
}