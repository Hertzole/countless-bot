using CountlessBot.Classes;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CountlessBot.Modules.Fun
{
    [Name("Fun"), Summary("Some fun commands to liven up the mood."), ModuleColor(Colors.GAMES_R, Colors.GAMES_G, Colors.GAMES_B)]
    public class FunModule : DiscordModule
    {
        [Command("8ball"), Summary("Ask the magic 8-ball for guidence.")]
        public async Task EightBallAsync([Remainder] string question = "")
        {
            try
            {
                // If the question is blank, stop.
                if (string.IsNullOrWhiteSpace(question))
                {
                    await ReplyAsync(":8ball: Ask me a question before I can predict.");
                    return;
                }

                // Make the question lower case.
                question = question.ToLower();

                // Make sure the question is valid. If so get a random response.
                if (question.Contains("will") || question.Contains("are") || question.Contains("am") || question.Contains("is") || question.Contains("was") || question.Contains("did") || question.Contains("should") || question.Contains("do"))
                    await ReplyAsync(":8ball: " + CountlessBot.Instance.Configuration.EightBallMessages[Random.Next(CountlessBot.Instance.Configuration.EightBallMessages.Count)]);
                else
                    await ReplyAsync(":8ball: That is not a valid question.");

            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("choose"), Summary("Makes the bot choose between multiple things. Choices are separated by commas.")]
        public async Task ChooseAsync([Remainder] string choices)
        {
            try
            {
                // Get all the options. Split them using the ',' character.
                string[] options = choices.Split(',');
                // Reply back with a random choice.
                await ReplyAsync(options[Random.Next(options.Length)]);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("coinflip"), Summary("Flips a coin and tells you if it's heads or tails."), Alias("coin")]
        public async Task CoinflipAsync()
        {
            try
            {
                // Get a random number that is either 0 or 1.
                int number = Random.Next(0, 2);
                // If it's 0, say Heads! else Tails!
                string result = number == 0 ? "Heads!" : "Tails!";
                // Reply with the answer.
                await ReplyAsync(result);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("define"), Summary("Defines something using Urban Dictionary.")]
        public async Task DefineAsync([Remainder] string search)
        {
            try
            {
                // Create a new http client.
                using (var http = new HttpClient())
                {
                    // Do some magic.
                    http.DefaultRequestHeaders.Clear();
                    http.DefaultRequestHeaders.Add("Accept", "application/json");
                    // Get the search result.
                    var res = await http.GetStringAsync($"http://api.urbandictionary.com/v0/define?term={Uri.EscapeUriString(search)}").ConfigureAwait(false);
                    try
                    {
                        // Get the items.
                        var items = JObject.Parse(res);
                        // Get the first item.
                        var item = items["list"][0];
                        // Get the word.
                        var word = item["word"].ToString();
                        // Get the definition.
                        var def = item["definition"].ToString();
                        // Get the link.
                        var link = item["permalink"].ToString();

                        // Create the embed builder.
                        EmbedBuilder embedBuilder = new EmbedBuilder()
                        {
                            Author = new EmbedAuthorBuilder() { Name = "Definition for " + word.ToLower(), Url = link },
                            Description = def,
                            Color = new Color(29, 36, 57),
                        };

                        // Build the embed.
                        Embed embed = embedBuilder.Build();
                        //Send the embed.
                        await ReplyAsync("", false, embed);
                    }
                    catch
                    {
                        // There was no results.
                        await ReplyAsync("I can't find a definition for that.");
                    }
                }

            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("joke"), Summary("Tells you a random joke, with the chance of having something \"extra spicy\"")]
        public async Task JokeAsync()
        {
            try
            {
                // Create the joke chance.
                int jokeChance = Random.Next(0, 101);
                // Create an empty joke chance.
                string joke = "";
                // If the joke chance is below 86, normal joke.
                // If the chance is above 85 and below 93, sex joke.
                // If the chance is above 92 and below 98, dark joke.
                // if the chance is above 97, racist joke.
                if (jokeChance <= 85)
                    joke = CountlessBot.Instance.Configuration.Jokes[Random.Next(0, CountlessBot.Instance.Configuration.Jokes.Count)];
                else if (jokeChance > 85 && jokeChance <= 92)
                    joke = CountlessBot.Instance.Configuration.SexJokes[Random.Next(0, CountlessBot.Instance.Configuration.SexJokes.Count)];
                else if (jokeChance > 92 && jokeChance <= 97)
                    joke = CountlessBot.Instance.Configuration.DarkJokes[Random.Next(0, CountlessBot.Instance.Configuration.DarkJokes.Count)];
                else if (jokeChance > 97)
                    joke = CountlessBot.Instance.Configuration.RacistJokes[Random.Next(0, CountlessBot.Instance.Configuration.RacistJokes.Count)];

                // Send the joke.
                await ReplyAsync(joke);
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }

        [Command("roast"), Summary("Roasts a user. If you don't specify a user, it will roast you.")]
        public async Task RoastAsync(IUser user = null)
        {
            try
            {
                // If no user is specified, roast the user who did the command.
                if (user == null)
                    user = Context.User;

                // If the user is Fish Jesus, include Fish Jesus related roasts.
                if (Context.User.Id == 112769364129792000)
                {
                    // Create a new list of raosts.
                    List<string> roasts = new List<string>();
                    // Add the default roasts to the list.
                    roasts.AddRange(CountlessBot.Instance.Configuration.Roasts);
                    // Add the fish roasts.
                    roasts.AddRange(CountlessBot.Instance.Configuration.FishRoasts);
                    // Select a random roast.
                    await ReplyAsync(user.Mention + " " + roasts[Random.Next(0, roasts.Count)]);
                }
                else
                {
                    // Select a random roast.
                    await ReplyAsync(user.Mention + " " + CountlessBot.Instance.Configuration.Roasts[Random.Next(0, CountlessBot.Instance.Configuration.Roasts.Count)]);
                }
            }
            catch (Exception ex)
            {
                await ReportError(ex);
            }
        }
    }
}
