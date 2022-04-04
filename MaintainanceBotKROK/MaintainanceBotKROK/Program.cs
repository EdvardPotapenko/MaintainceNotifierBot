using MaintainanceBotKROK;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

var builder = WebApplication.CreateBuilder();

string env = builder.Environment.EnvironmentName;
builder.Configuration
	.AddJsonFile("botsettings.json", false, false)
	.AddJsonFile($"botsettings.{env}.json", true, false)
	.AddEnvironmentVariables();

if (builder.Configuration["BOT_TOKEN"]?.Contains("PLACEHOLDER", StringComparison.OrdinalIgnoreCase) ?? true)
	throw new InvalidOperationException("Bot token is null");

if (builder.Configuration["BOT_MESSAGE"]?.Contains("PLACEHOLDER", StringComparison.OrdinalIgnoreCase) ?? true)
	throw new InvalidOperationException("Message token is null or empty");

var botclient = new TelegramBotClient(builder.Configuration["BOT_TOKEN"]);
var message = builder.Configuration["BOT_MESSAGE"];

builder.Services.AddSingleton<ITelegramBotClient>(botclient);
builder.Services.AddSingleton<IUpdateHandler>(new BotHandler(botclient, message));
builder.Services.AddHostedService<BotHandlerService>();

var app = builder.Build();

await app.RunAsync();
