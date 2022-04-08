using MaintainanceBotKROK;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;


var host = new HostBuilder()
           .ConfigureHostConfiguration(builder =>
           {
               builder.AddEnvironmentVariables("ASPNETCORE_");
           })
           .ConfigureAppConfiguration((context, builder) =>
           {
               var env = context.HostingEnvironment.EnvironmentName;
               builder.AddJsonFile("botsettings.json", false, false)
                      .AddJsonFile($"botsettings.{env}.json", true, false);             
           })
           .UseWindowsService()
           .ConfigureServices((hostContext, services) =>
           {
               if (hostContext.Configuration["BOT_TOKEN"]?.Contains("PLACEHOLDER", StringComparison.OrdinalIgnoreCase) ?? true)
                   throw new InvalidOperationException("Bot token is null");

               if (hostContext.Configuration["BOT_MESSAGE"]?.Contains("PLACEHOLDER", StringComparison.OrdinalIgnoreCase) ?? true)
                   throw new InvalidOperationException("Message is null or empty");

               var botclient = new TelegramBotClient(hostContext.Configuration["BOT_TOKEN"]);
               var message = hostContext.Configuration["BOT_MESSAGE"];

               services.AddSingleton<ITelegramBotClient>(botclient);
               services.AddSingleton<IUpdateHandler>(new BotHandler(botclient, message));
               services.AddHostedService<BotHandlerService>();
           });

var app = host.Build();

await app.RunAsync();
