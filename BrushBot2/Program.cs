using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            new Program().Run().Wait();
        }

        private async Task MessageRecieved(SocketMessage message)
        {
            Console.WriteLine(message.Channel.Name + " " + message.ToString());
            if (message.Channel.Name == "brushchat")
            {
                //embeds
                /*
                foreach(var embed in message.Embeds)
                {
                    Trace.TraceInformation("   +++ embed: " + embed.Type + " " + embed.ToString());
                    if(embed.Type == EmbedType.Image)
                    {
                        if (message.Channel is SocketGuildChannel)
                        {
                            var channels = ((SocketGuildChannel)message.Channel).Guild.TextChannels;
                            foreach(var channel in channels)
                            {
                                if(channel.Name == "gallery")
                                {
                                    channel.SendMessageAsync(null, false, embed, null);
                                }
                            }
                        }
                    }
                }
                */
                //Attachments
                foreach (var attachment in message.Attachments)
                {
                    Console.WriteLine("   +++ attachment: " + attachment.Url);
                    if (attachment.Height != null)
                    {
                        if (message.Channel is SocketGuildChannel)
                        {
                            var channels = ((SocketGuildChannel)message.Channel).Guild.TextChannels;
                            foreach (var channel in channels)
                            {
                                if (channel.Name == "gallery")
                                {
                                    channel.SendMessageAsync(attachment.Url + " \nFrom " + message.Author.Mention);
                                }
                            }
                        }
                    }
                }
            }

        }
        private async Task LogEvent(LogMessage message)
        {
            Console.WriteLine("discord: " + message.Severity + " | " + message.Message);
        }
        private async Task Run()
        {
            string token;
            using (StreamReader sr = File.OpenText("token.txt"))
            {
                token = sr.ReadToEnd();
            }
            DiscordSocketClient client = new DiscordSocketClient();

            client.MessageReceived += MessageRecieved;
            client.Log += LogEvent;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            while (true)
            {
                //Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }

}
