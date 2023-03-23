using DiscordRPC;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text.Json;
using System.Xml.Linq;
using WebSocketSharp;

public class Program
{
    private static readonly string server = "tidal.viper.tools"; // No protocol so it can use https and wss
    private static HttpClient httpClient = new();
    private static DiscordRpcClient client = new("923066535449354240");

    private static void updateStatus(Status? status)
    {
        if (status == null)
        {
            return;
        }

        RichPresence presence = new()
        {
            Details = status.SongData.Title,
            State = status.SongData.ArtistsString,
            Timestamps = new Timestamps
            {
                Start = DateTime.UtcNow
            },
            Assets = new Assets
            {
                LargeImageKey = status.SongData.ImageUrl.Replace("80x80", "1280x1280"),
                SmallImageKey = "tidalrpc",
                SmallImageText = "Tidal"
            },
            Buttons = new Button[]
            {
                new Button
                {
                    Label = "Listen",
                    Url = status.SongData.TrackUrl
                }
            }
        };

        if (status.State == "pause")
        {
            presence.Assets.SmallImageKey = "paused";
            presence.Assets.SmallImageText = "Paused";
        }

        client.SetPresence(presence);
    }

    private static async Task<Status?> getCurrentStatus()
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://{server}/status");
        HttpResponseMessage response = httpClient.Send(request);

        return await response.Content.ReadFromJsonAsync<Status>();
    }

    public static void Main(string[] args)
    {
        // Setup Discord
        client.Initialize();

        updateStatus(getCurrentStatus().Result);

        // Setup Socket

        WebSocket socket = new WebSocket($"wss://{server}/ws");
        socket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
        socket.OnMessage += StatusChanged;
        socket.Connect();

        Console.ReadLine();
    }

    private static void StatusChanged(object? sender, MessageEventArgs msg)
    {
        updateStatus(JsonSerializer.Deserialize<Status>(msg.Data));
    }
}


