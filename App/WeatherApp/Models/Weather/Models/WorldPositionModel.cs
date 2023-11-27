namespace WeatherApp.Models.Weather.Models;

public class WorldPositionModel
{
	public float Lon { get; set; }
	public float Lat { get; set; }

	public WorldPositionModel(float lon, float lat)
	{
		Lon = lon;
		Lat = lat;
	}
	
	public ulong GetByteRepresentation()
	{
		// Store the position as a byte array for good and fast hashing in redis
		byte[] bytes = new byte[sizeof(float) * 2];
		Buffer.BlockCopy(BitConverter.GetBytes(Lon), 0, bytes, 0, sizeof(float));
		Buffer.BlockCopy(BitConverter.GetBytes(Lat), 0, bytes, sizeof(float), sizeof(float));
		
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(bytes);
		}

		return BitConverter.ToUInt64(bytes, 0);
	}
}