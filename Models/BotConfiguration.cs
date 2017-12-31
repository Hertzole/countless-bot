using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CountlessBot.Models
{
    public class BotConfiguration
    {
        // The default prefix the bot uses for it's commands.
        public string DefaultCommandPrefix { get; set; } = ".";
        // All the games the bot will switch between.
        public List<string> MotdGames { get; set; }
        // Messages for the eight ball.
        public List<string> EightBallMessages { get; set; }
        // All default jokes.
        public List<string> Jokes { get; set; }
        // All racist jokes.
        public List<string> RacistJokes { get; set; }
        // All dark jokes.
        public List<string> DarkJokes { get; set; }
        // All sex jokes.
        public List<string> SexJokes { get; set; }
        // All roasts.
        public List<string> Roasts { get; set; }
        // All roasts targeted at Fish Jesus.
        public List<string> FishRoasts { get; set; }

        [JsonIgnore]
        private string[] m_MotdGames = new string[] { };

        [JsonIgnore]
        private string[] m_EightBallMessages = new string[]
        {
            "It is certain",
            "It is decidedly so",
            "Without a doubt",
            "Yes definitely",
            "You may rely on it",
            "As I see it, yes",
            "Most likely",
            "Outlook good",
            "Yes",
            "Signs point to yes",
            "Reply hazy try again",
            "Ask again later",
            "Better not tell you now",
            "Cannot predict now",
            "Concentrate and ask again",
            "Don't count on it",
            "My reply is no",
            "My sources say no",
            "Outlook not so good",
            "Very doubtful"
        };

        [JsonIgnore]
        private string[] m_Jokes = new string[]
        {
            "I don't get why women brag about multi-tasking. There's nothing cool about doing three things wrong at once.",
            "I asked my daughter if she’d seen my newspaper. She told me that newspapers are old school. She said that people use tablets nowadays and handed me her iPad. That fly didn’t stand a chance.",
            "I once made a belt out of $100 bills. Turns out it was just a waist of money.",
            "What do you call a fake noodle?  An impasta.",
            "How did the hipster burn the roof of his mouth? He ate the pizza before it was cool.",
            "Two fish in a tank. One looks to the other and says 'Can you even drive this thing?'",
            "I'm glad I know sign language, it's pretty handy.",
            "My friend's bakery burned down last night. Now his business is toast.",
            "Sauron is a great name. It has a nice ring to it.",
            "Why are politicians so stupid? Because they represent the people.",
            "Why did the console gamer cross the road? To render the building on the other side.",
            "Knock knock; Who's there?; Broken pencil; \"Broken pencil\" who?; ...Nevermind, it's pointless.",
            "Where do you find a cow with no legs? Right where you left it.",
            "\"You're telling me that I'm losing my job because Donald Trump won the election? WHY, BECAUSE I'M BLACK?!\" \"Mister President, we've been over this...\"",
            "Son: \"Mom, Dad, I'm gay.\" Mom: *stars at Dad* Dad: *clenches fist* Mom: \"Don't!\" Dad: \\*Sweats Profusely\\* Mom: \"...\" Dad: \"HI GAY, I'M DAD\"",
            "Steve Jobs would've been a better president than Donald Trump. But it's a silly comparison really, it's like comparing apples to oranges.",
            "My girlfriend told me to take the spider out instead of killing it. We went and had some drinks. Cool guy. Wants to be a web developer.",
            "How do you milk sheep? With iPhone accessories.",
            "Today a girl kissed me. I wish I could say that in relation to another subject.",
            "I dig, she dig, we dig, he dig, they dig, you dig... It's not a beautiful poem but it's really deep.",
            "I can cut down a tree just by looking at it. It's true, I saw it with my own eyes.",
            "Two blind guys were about to fight. I shouted: \"I bet the one with the knife wins!\" Both started running away.",
            "I tried to sue the airport for losing my luggage. I lost my case.",
            "If Minecraft taught me one thing... It's to never spend diamonds on a hoe.",
            "I've finally worked out why Spain is so good at soccer. Nobody expects the Spanish in position.",
            "When does a joke become a dad joke? When the punchline becomes apparent."
        };

        [JsonIgnore]
        private string[] m_RacistJokes = new string[]
        {
            "So I painted my laptop black, hoping it would run faster� Now it doesn't work.",
            "What's a word that white people can call white people, but black people can't call black people? Dad.",
            "What's the most confusing day in Detroit? Father's day.",
            "Why do all black people have nightmares? Because the one that had a dream got shot.",
            "What did the black kid get for christmas? Your bike.",
            "Why do black people have white palms? Everyone has a little good in them.",
        };

        [JsonIgnore]
        private string[] m_DarkJokes = new string[]
        {
            "My Grandpa said, \"Your generation relies too much on technology!\" I replied, \"No, your generation relies too much on technology!\" Then I unplugged his life support.",
            "What's the worst thing about breaking up with a Japanese girl? You have to drop the bomb twice before she gets the message.",
            "What did the boy with no hands get for Christmas? GLOVES! Nah, just kidding... He still hasn't unwrapped his present.",
            "How can you tell if your wife is dead? The sex is the same but the dishes start piling up.",
            "So I suggested to my wife that she'd look sexier with her hair back� Which is apparently an insensitive thing to say to a cancer patient.",
            "What is a pedophiles favorite part about Halloween? Free delivery.",
            "Girls are like blackjack... I'm trying to go for 21 but I always hit on 14.",
            "Why does Stephen Hawking do one-liners? Because he can't do stand up.",
            "A Jew, a black, and a Muslim are on a frozen lake, not talking to each other, so I thought I would go over there and break the ice.",
            "What's the hardest part of watching a school bus full of kindergarteners go off a cliff? The erection.",
            "How do you fuck a special person? You go down.",
            "Why can't you fool an aborted baby? Cause it wasn't born yesterday.",
            "What's pale, white, and bounces up and down in a baby's crib? A pedophile's ass.",
            "One time I fucked this chick so hard, she almost came back to life",
            "What's the difference between a baby and a watermelon? One is fun to shoot and the other is fun to eat.",
            "How many babies does it take to paint a wall? Depends on how hard you throw them.",
            "A guy was wondering what being a suicide bomber was like. So I told him \"C4 yourself\".",
            "As God created this human child, God asked him \"How about an extra chromosome?\". The child replied \"I'd be down for that\"."
        };

        [JsonIgnore]
        private string[] m_SexJokes = new string[]
        {
            "What do a penis and a Rubik's Cube have in common? The more you play with it, the harder it gets.",
            "What's the difference between your wife and your job? After five years, your job will still suck.",
            "My wife left me because I spent out life savings on a penis enlargement... She couldn't take it any longer...",
            "If a girl sleeps with a lot of guys, she's a slut. But if a man does he's gay."
        };

        [JsonIgnore]
        private string[] m_Roasts = new string[]
        {
            "Your suit sucks.",
            "is really weak, even for their generation!",
            "is really depressing to just look at.",
            "looks like a car crash in slow-motion.",
            "can't even handle being the human they were supposed to be.",
            "disappointed everyone around them, even me, a bot.",
            "knows nothing about the topic at hand.",
            "is absolutely about everything going on around them.",
            "looks dumb.",
            "can't bring anything new to this world.",
            "can't even comprehend a simple sentence. It would be a surprise if they even understood this one!",
            "is so average that even a flat surface has more going on.",
            "can't talk to the opposite gender.",
            "I've met feminists that could take a joke better than you.",
            "breaks all fashion laws.",
            "will never be close to a rational thought in their rambling incoherent response. Everyone around them is getting dumber as soon as they speak.",
            "lies and they know it.",
            "spent all their money on gambling and never once thought to blame themselves for losing the money.",
            "is just up to no good.",
            "only think about themselves.",
            "would lose to a knife in a machine gun battle.",
            "is desperate for attention.",
            "only annoys people around them because they hate everyone.",
            "is a degenerate.",
            "graduated last in their class.",
            "is weak in so many places.",
            "is a dummy.",
            "is just stupid.",
            "once got scared by a walnut.",
            "is a disgrace to their family and friends!",
            "needs to get a life.",
            "is spreading fake news.",
            "Your face looks like the surface of the moon. Full of craters and dust.",
            "could stop an alien invasion by just showing their face.",
            "has an alternate account to hide all their embarrassments. WE ALL KNOW WHAT YOU'RE HIDING!",
            "nevers shut their cakehole and just spews out muck.",
            "probably think ISIS is just some kind of ice.",
            "has never said anything that made sense.",
            "can't keep a promise.",
            "always messes up the simplest of tasks.",
            "always finishes last and it wasn't even worth the wait.",
            "even sucks at sucking.",
            "can't even language properly.",
            "is a disaster just waiting to happen.",
            "don't even know how to drink. Like, come on!",
            "thinks roasting is just taking a Trump insult and heating it up.",
            "relies on a magic 8-ball for their economic decisions.",
            "can't even get past the first Goomba in Super Mario Bros.",
            "just needs to stop and give up before they hurt someone.",
            "was voted to be the last one you pick for your team in school.",
            "has zero communication skills."
        };

        [JsonIgnore]
        private string[] m_FishRoasts = new string[]
        {
            "smells really bad.",
            "has something fishy about them.",
            "STILL can't keep a promise.",
            "don't understands what the letters N and O form.",
            "smells really, REALLY bad.",
            "is very intolerant.",
            "is a sad little troll.",
            "keeps taking ideas from HertzBot and putting them in me.",
        };

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            Serialize();
        }

        [OnSerializing]
        internal void OnSerializing(StreamingContext context)
        {
            Serialize();
        }

        private void Serialize()
        {
            MotdGames = AddMissingEntries(m_MotdGames, MotdGames);
            EightBallMessages = AddMissingEntries(m_EightBallMessages, EightBallMessages);
            Jokes = AddMissingEntries(m_Jokes, Jokes);
            RacistJokes = AddMissingEntries(m_RacistJokes, RacistJokes);
            DarkJokes = AddMissingEntries(m_DarkJokes, DarkJokes);
            SexJokes = AddMissingEntries(m_SexJokes, SexJokes);
            Roasts = AddMissingEntries(m_Roasts, Roasts);
            FishRoasts = AddMissingEntries(m_FishRoasts, FishRoasts);
        }

        private List<string> AddMissingEntries(string[] defaults, List<string> entries)
        {
            // If the entries is just null, create it and fill the list and then stop.
            if (entries == null)
            {
                entries = new List<string>();
                entries.AddRange(defaults);
                return entries;
            }

            // Loop through all default values.
            for (int i = 0; i < defaults.Length; i++)
            {
                // If the entries list doesn't contain a default value, add it.
                if (!entries.Contains(defaults[i]))
                    entries.Add(defaults[i]);
            }

            return entries;
        }
    }
}
