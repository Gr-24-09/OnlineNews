using Newtonsoft.Json;

namespace OnlineNews.Models.API
{
    public class ElectricitySpotPrice
    {
        [JsonProperty("date")]
        public string Date { get; set; }
        public SE1[] SE1 { get; set; }
        public SE2[] SE2 { get; set; }
        public SE3[] SE3 { get; set; }
        public SE4[] SE4 { get; set; }
    }
}
public class SE1
{
    public int hour { get; set; }
    public float price_eur { get; set; }
    public float price_sek { get; set; }
    public int kmeans { get; set; }
}

public class SE2
{
    public int hour { get; set; }
    public float price_eur { get; set; }
    public float price_sek { get; set; }
    public int kmeans { get; set; }
}

public class SE3
{
    public int hour { get; set; }
    public float price_eur { get; set; }
    public float price_sek { get; set; }
    public int kmeans { get; set; }
}

public class SE4
{
    public int hour { get; set; }
    public float price_eur { get; set; }
    public float price_sek { get; set; }
    public int kmeans { get; set; }
}

