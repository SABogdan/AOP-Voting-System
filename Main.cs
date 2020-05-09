using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOPVotingSystem
{
    public class Main : BaseScript
    {
        int bcVoting = 0;
        int lsVoting = 0;
        List<int> playersWhoVoted = new List<int>();
        Boolean voting = false;
        public Main()
        {
            API.RegisterCommand("startvote", new Action<int, List<object>, string>((src, args, raw) =>
                {
                    voting = true;
                }), false);

            API.RegisterCommand("aop", new Action<int, List<object>, string>((src, args, raw) =>
            {
                if(lsVoting > bcVoting)
                {
                    Screen.ShowNotification("The AOP is LS");
                }
                else
                {
                    Screen.ShowNotification("The AOP is BC");
                }
            }), false);

            API.RegisterCommand("stopvote", new Action<int, List<object>, string>((src, args, raw) =>
            {
                voting = false;
                if(lsVoting > bcVoting)
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        multiline = true,
                        args = new[] { "System", "The AOP is now LS" }
                    });
                } else
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        multiline = true,
                        args = new[] { "System", "The AOP is now BC" }
                    });
                }
            }), false);

            API.RegisterCommand("vote", new Action<int, List<object>, string>((src, args, raw) =>
            {
                int playerId = Game.Player.ServerId;
                //Not sure witch one is mroe correct, server or netwrok id.
                //int playerId = Game.PlayerPed.NetworkId;
                //Debug.WriteLine($" test id {playerId}");
                if (voting == true)
                {
                    var arguments = args.Select(o => o.ToString()).ToList();
                    if(arguments.Contains("LS") || arguments.Contains("BC"))
                    {
                        Vote(arguments[0], playerId);
                    }
                    else
                    {
                        Screen.ShowNotification("You used the wrong command! !vote [LS or BC]");
                    }
                }
                else
                {
                    Screen.ShowNotification("You cannot vote!");
                }
            }), false);
        }
        
        private void Vote(string vote, int playerId)
        {
            if (playersWhoVoted.Contains(playerId) == false)
            {
                Screen.ShowNotification($"Thank you for the vote!You voted { vote}");
                if (vote == "LS")
                {
                    lsVoting++;
                }
                else
                {
                    bcVoting++;
                }
                playersWhoVoted.Add(playerId);
            }
            else
            {
                Screen.ShowNotification($"You already voted! {Game.Player.Name}");
            }

        }

    }
}
