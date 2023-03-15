using System.Text.Json.Serialization;

public class SongData
{
    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; }

    [JsonPropertyName("trackId")]
    public string TrackId { get; set; }

    [JsonPropertyName("trackUrl")]
    public string TrackUrl { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("artists")]
    public string[] Artists { get; set; }

    [JsonPropertyName("artistsString")]
    public string ArtistsString { get; set; }
}

public class Status
{
    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("songData")]
    public SongData SongData { get; set; }
}