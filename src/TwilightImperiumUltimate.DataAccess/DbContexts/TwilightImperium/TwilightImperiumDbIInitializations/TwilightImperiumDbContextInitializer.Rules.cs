using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Entities.Rules;
using TwilightImperiumUltimate.Core.Enums.Languages;
using TwilightImperiumUltimate.Core.Enums.Rules;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeRules()
    {
        using var context = _dbContextFactory.CreateDbContext();

        var rules = new List<Rule>()
        {
            new Rule()
            {
                RuleCategory = RuleCategory.Abilities,
                Content = "<p>1 ABILITIESCards and faction sheets each have abilities that players can resolve " +
                "to trigger various game effects.1.1 If information in this Rules Reference contradicts the Learn to " +
                "Play booklet, the Rules Reference takes precedence.1.2 If a card ability contradicts information in the Rules " +
                "Reference, the card takes precedence. If both the card and the rules can be followed at the same time, they " +
                "should be.1.3 Each ability describes when and how a player can resolve it.a If an ability with a specified duration " +
                "is resolved, the effect of the ability remains through that duration, even if the component that caused the ability " +
                "is removed.1.4 If a card has multiple abilities, each ability is presented as its own paragraph.1.5 If an ability " +
                "contains the word &ldquo;Action,&rdquo; a player must use a component action during the action phase to resolve that ability." +
                "1.6 If an ability uses the word &ldquo;cannot,&rdquo; that effect is absolute.a If two abilities use the word &ldquo;cannot," +
                "&rdquo; a persistent ability takes precedence over a one-time ability and an enabling ability takes precedence over a cancel " +
                "ability.1.7 When a player resolves an ability, they must resolve the ability in its entirety. Any parts of the ability preceded " +
                "by the word &ldquo;may&rdquo; are optional, and the player resolving the ability may choose not to resolve those parts." +
                "1.8 Abilities on components that remain in play are mandatory unless those abilities use the word &ldquo;may.&rdquo;1.9 If an " +
                "ability has multiple effects separated by the word &ldquo;and,&rdquo; a player must resolve as many of the ability&rsquo;s effects as " +
                "possible. However, if the player cannot resolve all of its effects, that player is allowed to resolve as many as they can." +
                "1.10 COSTS1.11 Some abilities have a cost that is followed by an effect. The cost of an ability is separated from the effect by " +
                "the word &ldquo;to&rdquo; or by a semicolon. A player cannot resolve the effect of such an ability if they cannot resolve that " +
                "ability&rsquo;s cost. 1.12 Some examples of an ability&rsquo;s cost include spending resources, spending trade goods, spending " +
                "command tokens, exhausting cards, purging cards, and activating specific systems.</p>\r\n<p>1.13 TIMING 1.14 If the timing of an ability " +
                "uses the word &ldquo;before&rdquo; or &ldquo;after,&rdquo; the ability&rsquo;s effect occurs immediately before or after the described " +
                "timing event, respectively.a For example, if an ability is resolved &ldquo;after a ship is destroyed,&rdquo; the ability" +
                " must be resolved as soon as the ship is destroyed and not later during that turn or round.1.15 If the timing of an ability uses " +
                "the word &ldquo;when,&rdquo; the ability&rsquo;s effect occurs at the moment of the described timing event.a Such an ability " +
                "typically modifies or replaces the timing event in some way.1.16 Effects that occur &ldquo;when&rdquo; an event happens take " +
                "priority over effects that occur &ldquo;after&rdquo; an event happens.1.17 If an ability uses the word &ldquo;then,&rdquo; a " +
                "player must resolve the effect that occurs before the word &ldquo;then&rdquo; or they cannot resolve the effect that occurs " +
                "after the word &ldquo;then.&rdquo;1.18 Each ability can be resolved once for each occurrence of that ability&rsquo;s timing " +
                "event. For example, if an ability is resolved &ldquo;At the start of combat,&rdquo; it can be resolved at the start of each " +
                "combat.1.19 If there are multiple abilities that players wish to resolve at the same time during the action phase, each player " +
                "takes a turn resolving an ability in initiative order, beginning with the active player. This process continues until each player " +
                "has resolved each ability that they wish to resolve during that window.1.20 If there are multiple abilities that players wish to " +
                "resolve at the same time during the strategy or agenda phases, players take turns resolving abilities starting with the speaker " +
                "and proceeding clockwise. This process continues until each player has resolved each ability that they wish to resolve during " +
                "that window.1.21 COMPONENT-SPECIFIC RULES 1.22 The opening paragraph of each ability found on an action card describes " +
                "when a player can resolve that card&rsquo;s ability.1.23 The opening paragraph of most abilities found on promissory notes describes " +
                "when a player can resolve that card&rsquo;s ability.a Some promissory notes have abilities that trigger as soon as a player receives " +
                "the card.1.24 Abilities on agenda cards correspond to an outcome. Players resolve these abilities during the agenda phase after " +
                "players vote for a particular outcome.1.25 Each faction has faction abilities presented on its faction sheet. Each " +
                "faction&rsquo;s flagship has one or more unique abilities. Some abilities provide players with perpetual effects.</p>\r\n<p>" +
                "1.26 Some units have unit abilities. These abilities are named and presented above a unit&rsquo;s attributes on a player&rsquo;s faction " +
                "sheet or on a unit upgrade card. Each unit ability has unique rules for when a player can resolve that ability. The following " +
                "abilities are unit abilities:&bull; Anti-Fighter Barrage&bull; Bombardment&bull; Deploy&bull; Planetary Shield" +
                "&bull; Production&bull; Space Cannon&bull; Sustain Damage1.27 If a unit&rsquo;s ability uses the phrase &ldquo;this system&rdquo; " +
                "or &ldquo;this planet,&rdquo; the ability is referring to the system or planet that contains that unit.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.ActionCards,
                Content = "<p>2 ACTION CARDSAction cards provide players with various abilities that they can resolve as described on the cards." +
                "2.1 Each player draws one action card during each status phase.2.2 Players can draw action cards by resolving the primary and secondary abilities " +
                "of the &ldquo;Politics&rdquo; strategy card.2.3 When a player draws an action card, they take the top card from the action card deck and add it " +
                "to their hand of action cards.2.4 Each player&rsquo;s hand can have a maximum of seven action cards. If a player ever has more than seven " +
                "action cards, that player must choose seven cards to keep and discard the rest.a A game effect can increase or decrease the number of cards " +
                "a player&rsquo;s hand can have.2.5 A player&rsquo;s action cards remain hidden from other players until those cards are played.2.6 The first " +
                "paragraph of each action card is presented in bold text and describes the timing of when that card&rsquo;s ability can be resolved.a If an action " +
                "card contains the word &ldquo;Action,&rdquo; a player must use a component action during the action phase to resolve the ability. A player cannot resolve " +
                "a component action if they cannot completely resolve its ability.b Multiple action cards with the same name cannot be played during a single timing " +
                "window to affect the same units or game effect. Canceled cards are not treated as being played.2.7 To play an action card, a player reads and resolves the " +
                "card&rsquo;s ability text, making any decisions as prompted by the card. Then, that player discards the card, placing it in the action discard pile." +
                "2.8 If an action card is canceled, that card has no effect and is discarded</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.ActionPhase,
                Content = "<p>3 ACTION PHASEDuring the action phase, each player takes a turn in initiative order. During a player&rsquo;s turn, they perform a single action. " +
                "After each player has taken a turn, player turns begin again in initiative order. This process continues until all players have passed.3.1 During " +
                "a player&rsquo;s turn, they may perform one of the following three types of actions: a strategic action, a tactical action, or a component action." +
                "3.2 If a player cannot perform an action, they must pass.3.3 After a player has passed, they have no further turns and cannot perform additional actions during " +
                "that action phase.a During a turn that a player passes, they can resolve transactions and &ldquo;at the start of your turn&rdquo; abilities.b A player that " +
                "has passed can still resolve the secondary ability of other players&rsquo; strategy cards.c It is possible for a player to perform multiple, consecutive actions " +
                "during an action phase if all other players have passed during that action phase.3.4 A player cannot pass until they have performed the strategic action of their " +
                "strategy card.a During a three-player or four-player game, a player cannot pass until they have exhausted both of their strategy cards.3.5 After all players have " +
                "passed, play proceeds to the status phase.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.ActivePlayer,
                Content = "<p>4 ACTIVE PLAYERThe active player is the player taking a turn during the action phase.4.1 During the action phase, the player who is first in initiative " +
                "order is the first active player.4.2 After the active player takes a turn, the next player in initiative order becomes the active player.4.3 After the last player " +
                "in initiative order takes a turn, the player who is first in initiative order becomes the active player again, and turns begin again in initiative order, ignoring any players " +
                "who have already passed.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.ActiveSystem,
                Content = "<p>5 ACTIVE SYSTEMThe active system is the system that is activated during a tactical action.5.1 When a player performs a tactical action, they activate " +
                "a system by placing a command token from their tactic pool in that system. That system is the active system.5.2 A player cannot activate a system that already contains " +
                "one of their command tokens.5.3 A player can activate a system that contains command tokens that match other players&rsquo; factions.5.4 A system remains the " +
                "active system for the duration of the tactical action during which it was activated.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Adjacency,
                Content = "<p>6 ADJACENCYTwo system tiles are adjacent to each other if any of the tiles&rsquo; sides are touching each other.6.1 A system that has a wormhole is treated " +
                "as being adjacent to a system that has a matching wormhole.6.2 A unit or planet is adjacent to all system tiles that are adjacent to the system tile that contains that unit " +
                "or planet.a A system is not adjacent to itself.6.3 A planet is treated as being adjacent to the system that contains that planet.6.4 Systems that are connected by lines " +
                "drawn across one or more hyperlane tiles are adjacent for all purposes.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.AgendaCard,
                Content = "<p>7 AGENDA CARDAgenda cards represent galactic laws and policies. During each agenda phase, players cast votes for specific outcomes on two agenda cards." +
                "7.1 There are two types of agenda cards: laws and directives.7.2 Laws can permanently change the rules of the game. 7.3 When resolving a law, if a &ldquo;For&rdquo; outcome " +
                "received the most votes, or if the law requires an election, the law&rsquo;s ability becomes a permanent part of the game. Players resolve the outcome and place the agenda " +
                "card either in the common play area or in a player&rsquo;s play area, as dictated by the card.7.4 If a law is in a player&rsquo;s play area as opposed to the common play area, " +
                "that player owns that law.7.5 If a law is discarded from play, that law&rsquo;s ability is no longer in effect. Place that card on the top of the agenda card discard pile." +
                "7.6 If an &ldquo;Against&rdquo; outcome of a law received the most votes, players resolve the outcome and discard the agenda.7.7 Directives provide one-time game effects. " +
                "7.8 When resolving a directive, players resolve the outcome that received the most votes and discard the agenda card.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.AgendaPhase,
                Content = "8 AGENDA PHASE\r\nDuring the agenda phase, players can cast votes on agendas that can \r\nchange the rules of the game.\r\n" +
                "8.1 Players skip the agenda phase during the early portion of each \r\ngame. After the custodians token is removed from Mecatol \r\nRex, " +
                "the agenda phase is added to each game round. To resolve \r\nthe agenda phase, players perform the following steps:\r\n" +
                "8.2 STEP 1—FIRST AGENDA: Players resolve the first agenda by \r\nfollowing these steps in order:\r\ni. REVEAL AGENDA: The speaker draws one agenda " +
                "card \r\nfrom the top of the agenda deck and reads it aloud to \r\nall players, including all of its possible outcomes.\r\nii. VOTE: Each player, " +
                "starting with the player to the left of \r\nthe speaker and continuing clockwise, can cast votes \r\nfor an outcome of the current agenda.\r\niii. " +
                "RESOLVE OUTCOME: Players tally each vote that was \r\ncast and resolve the outcome that received the most votes.\r\n8.3 STEP 2—SECOND AGENDA: Players repeat " +
                "the “First \r\nAgenda” step of this phase for a second agenda.\r\n8.4 STEP 3—READY PLANETS: Each player readies each of their \r\nexhausted planets. " +
                "Then, a new game round begins starting \r\nwith the strategy phase.\r\n8.5 VOTING\r\nWhen voting during the agenda phase, a player can cast votes for " +
                "a \r\nspecific outcome of an agenda.\r\n8.6 To cast votes, a player exhausts any number of their planets and \r\nchooses an outcome. The number of votes " +
                "cast for that outcome \r\nis equal to the combined influence values of the planets that the \r\nplayer exhausts.\r\na When a player exhausts a planet to " +
                "cast votes, that player \r\nmust cast the full amount of votes provided by that planet.\r\n8.7 A player cannot cast votes for multiple outcomes of the " +
                "same \r\nagenda. Each vote a player casts must be for the same outcome.\r\n8.8 Some agendas have “For” and “Against” outcomes. When a \r\nplayer casts " +
                "votes on such an agenda, that player must cast their \r\nvotes either “For” or “Against.”\r\n8.9 Some agendas instruct players to elect either a player " +
                "or a \r\nplanet. When a player casts votes for such an agenda, that \r\nplayer must cast their vote for an eligible player or planet as \r\ndescribed " +
                "on the agenda.\r\n8.10 When electing a player, a player can cast votes for themselves. \r\na When resolving these agendas, the “elected player” is the " +
                "\r\nplayer for whom the most votes are cast.\r\n8.11 When electing a planet, a player must cast votes for a planet \r\nthat is controlled by a player." +
                "<p>a When resolving these agendas, the &ldquo;elected planet&rdquo; is the planet that had the most votes cast for it.8.12 When casting votes, " +
                "a player must declare aloud the outcome for which their votes are being cast.8.13 Trade goods cannot be spent to cast votes.8.14 A player " +
                "may choose to abstain by not casting any votes.8.15 Some game effects allow a player to cast additional votes for an outcome. These votes cannot " +
                "be cast for a different outcome than other votes cast by that player.8.16 If a player cannot vote on an agenda because of a game effect, that " +
                "player cannot cast votes for that agenda by exhausting planets or through any other game effect.8.17 OUTCOMES8.18 To resolve an outcome, the " +
                "speaker follows the instructions on the agenda card.8.19 If there is a tie for the outcome that received the most votes, or if no outcome receives " +
                "any votes, the speaker decides which of the tied outcomes to resolve.a The speaker&rsquo;s decision is not a vote.8.20 If an &ldquo;Elect&rdquo; " +
                "or &ldquo;For&rdquo; outcome of a law was resolved, that card remains in play and permanently affects the game.8.21 If a directive or an &ldquo;Against&rdquo; " +
                "outcome of a law was resolved, that card is placed in the agenda discard pile.8.22 Some game effects instruct a player to predict an outcome. To predict " +
                "an outcome, a player declares aloud the outcome they think will receive the most votes. That player must make this prediction after the agenda is revealed but " +
                "before any votes have been cast.a A predicted outcome must be a possible outcome of the revealed agenda.b After resolving the outcome of the agenda, " +
                "any abilities that were dependent upon predicting the outcome are resolved.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Anomalies,
                Content = "<p>9 ANOMALIESAn anomaly is a system tile that has unique rules.9.1 An anomaly is identified by a red border located on the tile&rsquo;s corners." +
                "9.2 There are four types of anomalies: asteroid fields, nebulae, supernovas, and gravity rifts.a Some anomalies contain planets; those systems are still anomalies.</p>\r\n<p>" +
                "9.3 Each type of anomaly is identified by its art, as follows:9.4 Abilities can cause a system tile to become an anomaly; that system tile is an anomaly in addition to its other properties." +
                "9.5 Abilities can cause a system to be two different anomalies; that system has the properties of both anomalies</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.AntiFighterBarrage,
                Content = "<p>10 ANTI-FIGHTER BARRAGE (UNIT ABILITY)A unit with the &ldquo;Anti-Fighter Barrage&rdquo; ability may be able to destroy an opponent&rsquo;s fighters at the onset of a space battle." +
                " During the &ldquo;Anti-Fighter Barrage&rdquo; step of the first round of space combat, players perform the following steps:10.1 STEP 1&mdash;Each player rolls dice for each of their units " +
                "in the combat that has the &ldquo;Anti-Fighter Barrage&rdquo; ability; this is called an anti-fighter barrage roll. A hit is produced for each die roll that is equal to or greater than the " +
                "unit&rsquo;s anti-fighter barrage value.a A unit&rsquo;s &ldquo;Anti-Fighter Barrage&rdquo; ability is presented along with a unit&rsquo;s attributes on faction sheets and unit upgrade " +
                "technology cards.b The &ldquo;Anti-Fighter Barrage&rdquo; ability is displayed as &ldquo;Anti\u0002Fighter Barrage X (xY).&rdquo; The X is the minimum value needed for a die to produce a hit, and Y " +
                "is the number of dice rolled.c Game effects that reroll, modify, or otherwise affect combat rolls do not affect anti-fighter barrage rolls.d This ability can still be used if no fighters are " +
                "present; hits produced may be used to trigger specific abilities.10.2 STEP 2:&mdash;Each player must choose and destroy one of their fighters in the active system for each hit their " +
                "opponent&rsquo;s anti\u0002fighter barrage roll produced.a If a player has to assign more hits than they have fighters in the active system, the excess hits have no effect.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.AsteroidField,
                Content = "<p>11 ASTEROID FIELDAn asteroid field is an anomaly that affects movement." +
                            "11.1 A ship cannot move through or into an asteroid field.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Attach,
                Content = "<p>12 ATTACHSome game effects instruct a player to attach a card to a planet card. The attached card modifies that planet card in some way.12.1 To attach a card to a planet card, " +
                "a player places the card with the attach effect partially underneath the planet card.12.2 If a player gains or loses control of planet that contains a card with an attach effect, the attached " +
                "card stays with that planet.a The attached card maintains its exhausted or readied state.b If a planet card is purged, also purge all cards that are attached to that planet card and remove the " +
                "corresponding attachment tokens from the game board.12.3 When a card is attached to a planet card, place the corresponding attachment token on that planet on the game board.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Attacker,
                Content = "<p>13 ATTACKERDuring combat, the active player is the attacker.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Blockaded,
                Content = "<p>14 BLOCKADEDA player&rsquo;s unit with &ldquo;Production&rdquo; is blockaded if it is in a system that does not contain any of their ships and contains other players&rsquo;ships." +
                "14.1 A player cannot use a blockaded unit to produce ships; that player can still use a blockaded unit to produce ground forces.14.2 When a player blockades another player&rsquo;s space dock, " +
                "if the blockaded player has captured any of the blockading player&rsquo;s units, those units are returned to the blockading player&rsquo;s reinforcements.a While a player is blockading another player, " +
                "the blockaded player cannot capture any of the blockading player&rsquo;s units.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Bombardment,
                Content = "<p>15 BOMBARDMENT (UNIT ABILITY)A unit with the &ldquo;Bombardment&rdquo; ability may be able to destroy another player&rsquo;s ground forces during an invasion. During the &ldquo;" +
                "Bombardment&rdquo; step, players perform the following steps:15.1 STEP 1&mdash; The active player chooses which planet each of their units that has a &ldquo;Bombardment&rdquo; ability will bombard. " +
                "Then, that player rolls dice for each of those units; this is called a bombardment roll. A hit is produced for each die roll that is equal to or greater than the unit&rsquo;s &ldquo;Bombardment&rdquo; value." +
                "a A unit&rsquo;s &ldquo;Bombardment&rdquo; ability is presented along with a unit&rsquo;s attributes on faction sheets and unit upgrade technology cards.b The &ldquo;Bombardment&rdquo; ability is displayed as &ldquo;" +
                "Bombardment X (xY).&rdquo; The X is the minimum value needed for a die to produce a hit, and Y is the number of dice rolled. Not all &ldquo;Bombardment&rdquo; abilities have a (Y) value; a bombardment roll for such " +
                "a unit consists of one die.c Game effects that reroll, modify, or otherwise affect combat rolls do not affect bombardment rolls.d Multiple planets in a system may be bombarded, but a player must declare which planet a unit " +
                "is bombarding before making a bombardment roll.e The L1Z1X&rsquo;s &ldquo;Harrow&rdquo; ability does not affect the L1Z1X player&rsquo;s own ground forces.f Planets that contain a unit with the &ldquo;Planetary Shield&rdquo; " +
                "ability cannot be bombarded.15.2 STEP 2&mdash; The player who controls the planet that is being bombarded chooses and destroys one of their ground forces on that planet for each hit result the bombardment roll produced." +
                "a If a player has to assign more hits than that player has ground forces, the excess hits have no effect.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Capacity,
                Content = "<p>16 CAPACITY (ATTRIBUTE)Capacity is an attribute of some units that is presented on faction sheets and unit upgrade technology cards.16.1 A unit&rsquo;s capacity value indicates the maximum combined number of " +
                "fighters and ground forces that it can transport.16.2 The combined capacity values of a player&rsquo;s ships in a system determine the number of fighters and ground forces that player can have in that system&rsquo;s space area." +
                "16.3 If a player has more fighters and ground forces in the space area of a system than the total capacity of that player&rsquo;s ships in that system, that player must remove the excess units.</p>\r\n<p>a A player can choose which of " +
                "their excess units to remove.b Ground forces on planets do not count against capacity.c A player&rsquo;s fighters and ground forces do not count against capacity during combat. At the end of combat, any excess units are removed " +
                "and returned to that player&rsquo;s reinforcements.16.4 Fighters and ground forces are not assigned to specific ships, except while they are being transported.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Capture,
                Content = "<p>17 CAPTURESome abilities instruct a player to capture a unit, preventing the unit&rsquo;s original owner from using it.17.1 If a player captures a non-fighter ship or mech, they place it on their faction sheet. " +
                "When such a unit is returned, it is placed into the reinforcements of the original owner.17.2 A captured non-fighter ship or mech is returned under the following circumstances:a If the player who captured the unit agrees to return it " +
                "as part of a transaction.b If an ability instructs the capturing player to return the unit as part of an ability&rsquo;s cost.c If the player whose unit was captured blockades a space dock of the player who captured the unit." +
                "17.3 If a player captures a fighter or infantry, it is placed in its reinforcements instead of on the capturing player&rsquo;s faction sheet; the capturing player places a fighter or infantry token from the supply on their faction sheet instead. " +
                "17.4 Captured fighters and infantry do not belong to any player and are returned only when an ability instructs the capturing player to do so. a Captured fighters and infantry cannot be returned as part of a transaction.b Captured " +
                "fighters and infantry are not returned as the result of a blockade. c When a captured fighter or infantry is returned, it is placed in the supply.17.5 While a unit is captured, it cannot be produced or placed by its original owner " +
                "until it is returned.17.6 If one or more of a player&rsquo;s space docks is being blockaded, that player cannot capture units from the blockading players.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Combat,
                Content = "<p>18 COMBAT (ATTRIBUTE)Combat is an attribute of some units that is presented on faction sheets and unit upgrade technology cards. 18.1 During combat, if a unit&rsquo;s combat roll produces a result equal to or greater than its combat value, " +
                "it produces a hit.18.2 If a unit&rsquo;s combat value contains two or more burst icons, instead of rolling a single die, the player rolls one die for each burst icon when making that unit&rsquo;s combat rolls.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.CommandSheet,
                Content = "<p>19 COMMAND SHEETEach player has a command sheet that contains a strategy pool, a tactic pool, a fleet pool, a trade good area, and a quick reference.19.1 The pools on the command sheet are where players place their command tokens. " +
                "Command tokens in a player&rsquo;s pools are used by that player to perform strategic and tactical actions and to increase the number of ships that player can have in each system.19.2 The trade good area on the command sheet is where a player places their trade " +
                "good tokens; trade tokens in a player&rsquo;s trade good area can be spent by that player as resources, influence, or to resolve certain game effects that require trade goods.19.3 Players who are familiar with the game can hide the quick reference by placing that portion " +
                "of the command sheet under their faction sheets.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.CommandTokens,
                Content = "<p>20 COMMAND TOKENSCommand tokens are a currency that players use to perform actions and expand their fleets.20.1 Each player begins the game with eight tokens on their command sheet: three in their tactic pool, three in their fleet pool, and two in their " +
                "strategy pool.a Command tokens in the strategy and tactic pool are placed with the faction symbol faceup.b Command tokens in the fleet pool are placed with the ship silhouette faceup.</p>\r\n<p>20.2 When a player gains a command token, they choose which of their three " +
                "pools to place it in.20.3 A player is limited by the amount of command tokens in their reinforcements.a If a player would gain a command token but has none available in their reinforcements, that player cannot gain that command token.b If a game effect would place " +
                "a player&rsquo;s command token from their reinforcements and none are available, that player must take a token from a pool on their command sheet, unless the token would be placed into a system that already contains one of their command tokens.20.4 During the action phase, " +
                "a player can perform a tactical action by spending a command token from their tactic pool; they place the command token in a system.20.5 After a player performs a strategic action during the action phase, each other player can resolve the secondary ability of that strategy " +
                "card by spending a command token from their strategy pool.a A player does not spend a command token to resolve the secondary ability of the &ldquo;Leadership&rdquo; strategy card.20.6 If a game effect would place a player&rsquo;s command token in a system where they already " +
                "have one, they place the token in their reinforcements instead. Any effects that resolve by placing that token are resolved as normal.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Commodities,
                Content = "<p>21 COMMODITIESCommodities represent goods that are plentiful for their own faction and are desired by other factions. A commodity has no inherent game effects, but converts into a trade good if given to or received from another player.21.1 Commodities and trade " +
                "goods are represented by opposite sides of the same token.21.2 The commodity value on a player&rsquo;s faction sheet indicates the maximum number of commodities that player can have.21.3 When an effect instructs a player to replenish commodities, that player takes the number of " +
                "commodity tokens necessary so that the amount of commodities that player has equals the commodity value on their faction sheet. Then, those tokens are placed faceup in the commodity area of that player&rsquo;s faction sheet.21.4 When a player replenishes commodities, that player " +
                "takes the commodity tokens from the supply.</p>\r\n<p>21.5 Players can trade commodities following the rules for transactions. When a player receives a commodity from another player, the player who received that token converts it into a trade good by placing it in the trade good area of " +
                "their command sheet with the trade good side faceup.a That token is no longer a commodity token; it is a trade good token.b A player can trade commodity tokens before resolving a game effect that allows them to replenish commodities.c If a game effect instructs a player to " +
                "convert a number of their own commodities to trade goods, those trade goods are not treated as being gained for the purpose of triggering other abilities.21.6 Any game effect that instructs a player to give a commodity to another player causes that commodity to be converted into a " +
                "trade good.21.7 A player cannot spend commodities unless otherwise specified; a player can only trade them during a transaction.21.8 Commodity tokens come in values of one and three. A player can swap between these tokens as necessary.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.ComponentAction,
                Content = "<p>22 COMPONENT ACTIONA component action is a type of action that a player can perform during their turn of an action phase.22.1 Component actions can be found on various game components, including action cards, technology cards, leaders, exploration cards, relics, promissory notes, " +
                "and faction sheets. Each component action is indicated by an &ldquo;Action&rdquo; header.22.2 To perform a component action, a player reads the action&rsquo;s text and follows the instructions as described.22.3 A component action cannot be performed if its ability cannot be completely resolved." +
                "22.4 If a component action is canceled, it does not use that player&rsquo;s action.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.ComponentLimitations,
                Content = "<p>23 COMPONENT LIMITATIONSIf a component type is depleted during the game, players obey the following rules:23.1 DICE: Dice are limitless. If a player needs to roll more dice than the game provides, that player should roll as many as possible, record the results, and then reroll dice as " +
                "necessary.</p>\r\n<p>23.2 TOKENS: Tokens are limited to those included in the game, except for the following: &bull; Control Tokens&bull; Fighter Tokens&bull; Trade Good Tokens&bull; Infantry Tokens23.3 If any of the above tokens are depleted, players can use a suitable substitute, such " +
                "as a coin or bead.23.4 UNITS: Units are limited to those included in the game, except for fighters and ground forces.a When a player would place a unit, if there are none of that type left in their reinforcements, that player can remove a unit from any system that does not contain one of their " +
                "command tokens and place that unit in their reinforcements. A player can remove any number of their units in this way; however, any units that are removed must be placed immediately. Abilities cannot force a player to remove and place a unit in this manner.b When producing a fighter or infantry unit, " +
                "a player can use a fighter or infantry token, as appropriate, from the supply instead of a plastic piece. These tokens must be accompanied by at least one plastic piece of the same type; players can swap infantry and fighter tokens for plastic pieces at any time.23.5 CARDS: When a deck is depleted, " +
                "players shuffle the deck&rsquo;s discard pile and place it facedown to create a new deck.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Construction,
                Content = "<p>24 CONSTRUCTION (STRATEGY CARD)The &ldquo;Construction&rdquo; strategy card allows players to construct structures on planets they control. This card&rsquo;s initiative value is &ldquo;4.&rdquo;24.1 During the action phase, if the active player has the &ldquo;Construction&rdquo; strategy card, " +
                "they can perform a strategic action to resolve that card&rsquo;s primary ability.24.2 To resolve the primary ability on the &ldquo;Construction&rdquo; strategy card, the active player may place either one PDS or one space dock on a planet they control. Then, that player may place an additional PDS on a planet " +
                "they control.a The structures can be placed on the same planet or different planets.b The structures can be placed in any systems, regardless of whether the player has a command token in the system or not.</p>\r\n<p>24.3 After the active player resolves the primary ability of the &ldquo;Construction&rdquo; " +
                "strategy card, each other player, beginning with the player to the left of the active player and proceeding clockwise, may spend one command token from their strategy pool and place that command token in any system. If that player already has a command token in that system, the spent token is returned to their " +
                "reinforcements instead. Then, that player places either one PDS or one space dock on a planet they control in that system.24.4 When a player places either a PDS or space dock using the &ldquo;Construction&rdquo; strategy card, they take that PDS or space dock from their reinforcements.a If a player does " +
                "not have the unit in their reinforcements, that player can remove a unit from any system that does not contain one of their command tokens and place that unit in their reinforcements. Then, that player must place the unit on the game board as instructed by the effect that is placing the unit.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Control,
                Content = "<p>25 CONTROLEach player begins the game with control of each planet in their home system. During the game, players can gain control of additional planets.25.1 When a player gains control of a planet, they take the planet card that corresponds to that planet and place it in their play area; " +
                "that card is exhausted.a If a player is the first player to control a planet, they take the planet card from the planet card deck.b If another player controls the planet, they take that planet&rsquo;s card from the other player&rsquo;s play area.c When a player gains control of a planet that is not already " +
                "controlled by another player, they explore that planet.25.2 A player cannot gain control of a planet that they already control.25.3 While a player controls a planet, that planet&rsquo;s card remains in their play area until they lose control of that planet.25.4 A player can control a planet that they do not have any " +
                "units on; that player places a control token on that planet to mark that they control it.25.5 A player loses control of a planet if they no longer have units on it and another player has units on it.a The player that placed units on the planet gains control of that planet.b During the invasion step of a " +
                "tactical action, control is determined during the &ldquo;Establish Control&rdquo; step instead.</p>\r\n<p>25.6 A player can lose control of a planet through some game effects.25.7 If a player loses control of a planet that contains their control token, they remove their control token from the planet.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Cost,
                Content = "<p>26 COST (ATTRIBUTE)Cost is an attribute of some units that is presented on faction sheets and unit upgrade technology cards. A unit&rsquo;s cost determines the number of resources a player must spend to produce that unit.26.1 To produce a unit, a player must spend a number of resources equal to or greater " +
                "than the cost of the unit they are producing.26.2 If the cost is accompanied by two icons&mdash;typically for fighters and ground forces&mdash;a player produces two of that unit for that cost.26.3 If a unit does not have a cost, it cannot be produced.a Structures do not have costs and are usually placed by resolving " +
                "the &ldquo;Construction&rdquo; strategy card.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.CustodiansToken,
                Content = "<p>27 CUSTODIANS TOKENThe custodians token begins each game on Mecatol Rex. The token represents the caretakers that safeguard the seat of the empire until such time as the galactic council can be reconvened.27.1 Units can move into the system that contains Mecatol Rex following normal rules; however, " +
                "players cannot commit ground forces to land on Mecatol Rex until the custodians token is removed from the planet.27.2 Before the &ldquo;Commit Ground Forces&rdquo; step of an invasion, the active player can remove the custodians token from Mecatol Rex by spending six influence. Then, that player must commit " +
                "at least one ground force to land on the planet.a If a player cannot commit ground forces to land on Mecatol Rex, they cannot remove the custodians token.27.3 When a player removes the custodians token from Mecatol Rex, they take the token from the game board and place it in their play area. Then, they gain one victory point." +
                "27.4 After a player removes the custodians token from Mecatol Rex, the agenda phase is added to all subsequent game rounds, including the game round during which the custodians token was removed from Mecatol Rex.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Deals,
                Content = "<p>28 DEALSA deal is an agreement between two players that may or may not include a transaction that involves physical components.28.1 Players can make deals with each other at any time, even if they are not neighbors. However, deals that include a transaction must follow the rules for transactions, including that " +
                "the players be neighbors.28.2 Deals are binding or non-binding according to the conditions of the deal.28.3 If the terms of a deal can be resolved immediately, it is a binding deal. When a deal is binding, a player must adhere to the terms of the agreement and whatever transactions, if any, were agreed upon.a The results " +
                "of playing an action card, including the act of successfully resolving a card, are not instantaneous and cannot be guaranteed. They cannot be part of a binding deal.28.4 If the terms of a deal cannot be resolved immediately, it is a non-binding deal. When a deal is non-binding, a player does not have to adhere to any part of " +
                "the agreement.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Defender,
                Content = "<p>29 DEFENDERDuring either a space or ground combat, the player who is not the active player is the defender.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Deploy,
                Content = "<p>30 DEPLOYSome units have deploy abilities. Deploy abilities are indicated by the &ldquo;Deploy&rdquo; header and provide the means to place specific units on the game board without producing them as normal. 30.1 A player can use a unit&rsquo;s deploy ability when the ability&rsquo;s conditions are met to place that unit on " +
                "the game board.a A player does not have to spend resources to deploy a unit unless otherwise specified.30.2 A player can only resolve a deploy ability to place a unit that is in their reinforcements. a If there are no units that have a deploy ability in a player&rsquo;s reinforcements, the deploy ability cannot be used." +
                "30.3 A unit&rsquo;s deploy ability can be resolved only once per timing window.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Destroyed,
                Content = "<p>31 DESTROYEDVarious game effects can cause a unit to be destroyed. When a player&rsquo;s unit is destroyed, it is removed from the game board and returned to their reinforcements.31.1 When a player assigns hits that were produced against their units, that player chooses a number of their units to be destroyed equal " +
                "to the number of hits produced against those units.31.2 If a player&rsquo;s unit is removed from the board by a game effect, it is not treated as being destroyed; effects that trigger when a unit is destroyed are not triggered.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Diplomacy,
                Content = "<p>32 DIPLOMACY (STRATEGY CARD)The &ldquo;Diplomacy&rdquo; strategy card can be used to preemptively prevent other players from activating a specific system. It can also be used to ready planets. This card&rsquo;s initiative value is &ldquo;2.&rdquo;32.1 During the action phase, if the active player has the &ldquo;Diplomacy&rdquo; " +
                "strategy card, they can perform a strategic action to resolve that card&rsquo;s primary ability.32.2 To resolve the primary ability on the &ldquo;Diplomacy&rdquo; strategy card, the active player chooses a system that contains a planet they control other than the Mecatol Rex system; each other player places one command token from their reinforcements " +
                "in that system. Then, the active player readies any two of their exhausted planets.a If a player has no command tokens in their reinforcements, that player places one command token of their choice from their command sheet.b If a player already has a command token in the chosen system, they do not place a command token there.32.3 After the " +
                "active player resolves the primary ability of the &ldquo;Diplomacy&rdquo; strategy card, each other player, beginning with the player to the left of the active player and proceeding clockwise, may spend one command token from their strategy pool to ready up to two exhausted planets they control.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Elimination,
                Content = "<p>33 ELIMINATIONA player who is eliminated is no longer part of the game.33.1 A player is eliminated when they meet all of the following three conditions:a The player has no ground forces on the game board.b The player has no unit that has &ldquo;Production.&rdquo;c The player does not control any planets.33.2 When a player becomes eliminated, " +
                "all of the units, command tokens, control tokens, promissory notes, technologies, command sheets, and the faction sheet that matches that player&rsquo;s faction or color are returned to the game box, including those in their reinforcements.33.3 When a player becomes eliminated, all agenda cards they own are discarded.33.4 When a player becomes eliminated, " +
                "each promissory note they have that matches another player&rsquo;s faction or color is returned to that player.a Promissory notes that match the eliminated player are returned to the game box, even if another player has them.33.5 When a player becomes eliminated, each action card in their hand is discarded.33.6 When a player becomes eliminated, their " +
                "strategy cards are returned to the common play area whether those cards have been exhausted or not.33.7 When a player becomes eliminated, their secret objectives are shuffled back into the secret objective deck whether those secret objectives have been completed or not.33.8 If the speaker becomes eliminated, the speaker token passes to the player to the " +
                "speaker&rsquo;s left.33.9 If a game that started with five or more players becomes a game with four or fewer players due to elimination, the players continue to select only one strategy card during the strategy phase.33.10 When players are eliminated, faction-specific componentsinteract with the game as follows:a If a player becomes eliminated and the " +
                "Nekro Virus&rsquo; assimilator &ldquo;X&rdquo; or assimilator &ldquo;Y&rdquo; token is placed on one of their faction technologies, that technology remains in play.b If the Ghost of Creuss player becomes eliminated, their wormhole tokens remain on the game board for the remainder of the game.c If the Naalu player becomes eliminated while another " +
                "player has the Naalu player&rsquo;s &ldquo;0&rdquo; token, that token remains with its current player until the end of the status phase, and then it is removed from play.</p>\r\n<p>d If the Titans of Ul player becomes eliminated while their hero or promissory note is attached to a planet, those attachments and attachment tokens remain in play for the remainder " +
                "of the game.e If the Mahact Gene-Sorcerers become eliminated while they have another player&rsquo;s command tokens on their faction sheet, those command tokens are returned to their respective players&rsquo; reinforcements.f If the Mahact Gene-Sorcerers have an eliminated player&rsquo;s command token on their faction sheet, that command token remains " +
                "in play, as does the eliminated player&rsquo;s commander, if it is unlocked.33.11 If a player becomes eliminated, any units they have captured are returned to the reinforcements of their original owners.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Exhausted,
                Content = "<p>34 EXHAUSTEDSome cards can be exhausted. A player cannot resolve abilities or spend the resources or influence of an exhausted card.34.1 To exhaust a card, a player flips the card facedown.34.2 During the &ldquo;Ready Cards&rdquo; step of the status phase, each player readies all of their exhausted cards by flipping those cards faceup." +
                "34.3 A player exhausts their planet cards to spend either the resources or influence on that card.34.4 Abilities, including some found on technology cards, may instruct a player to exhaust a card to resolve those abilities. If a card is already exhausted, it cannot be exhausted again.a Passive abilities on an exhausted card are still in effect while that card " +
                "is exhausted.34.5 After a player performs a strategic action, they exhaust the strategy card that corresponds to that action.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Exploration,
                Content = "<p>35 EXPLORATIONPlanets and some space areas can be explored, yielding varying results determined by the cards drawn from the exploration decks.35.1 When a player takes control of a planet that is not already controlled by another player, they explore that planet.35.2 When a player explores a planet, they draw and resolve a card " +
                "from the exploration deck that corresponds to that planet&rsquo;s trait.</p>\r\n<p>a There are three planetary exploration decks, each of which corresponds to a planet trait: cultural, hazardous, and industrial.b Planets that do not have traits, such as Mecatol Rex and planets in home systems, cannot be explored.c If a planet has multiple traits, the player exploring the " +
                "planet chooses which of the corresponding exploration decks to draw from.d If a player gains control of multiple planets or resolves multiple explore effects at the same time, they choose the order in which they resolve those explorations, completely resolving each exploration card before resolving the next.35.3 Certain abilities may allow a planet to be explored multiple " +
                "times.35.4 Players can explore space areas that contain frontier tokens if they own the &ldquo;Dark Energy Tap&rdquo; technology or if another game effect allows them to.a Frontier tokens are placed in systems during setup and by specific abilities.35.5 When a player explores a frontier token, they draw and resolve a card from the frontier exploration deck." +
                "35.6 After a frontier token is explored, it is discarded and returned to the supply.35.7 To resolve an exploration card, a player reads the card, makes any necessary decisions, and resolves its ability. If the card was not a relic fragment or an attachment, it is discarded into its respective discard pile.a If there are no cards in an exploration deck, its discard pile " +
                "is shuffled to form a new exploration deck.35.8 If a player resolves an exploration card that has an &ldquo;attach&rdquo; header, they attach that card to the planet card of the planet being explored.35.9 If a player resolves an exploration card that has &ldquo;relic fragment&rdquo; in the title, they place that card faceup in their play area.a Players can resolve the " +
                "ability of relic fragments that are in their play area. Resolving these abilities allows players to draw cards from the relic deck.b Relic fragments can be exchanged as part of transactions.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.FighterTokens,
                Content = "<p>36 FIGHTER TOKENSA fighter token functions as a plastic fighter unit for all game purposes.36.1 When producing a fighter unit, a player can use a fighter token from the supply instead of a plastic piece.36.2 Players can replace their plastic fighters with tokens at any time.36.3 If a player ever has a fighter token in a system that does not " +
                "contain one of their plastic fighters, that player must replace it with a plastic fighter from their reinforcements.a If the player cannot replace the token, the unit is destroyed.36.4 Fighter tokens come in values of one and three. A player can swap between these tokens as necessary.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.FleetPool,
                Content = "<p>37 FLEET POOLThe fleet pool is an area of a player&rsquo;s command sheet.37.1 The number of command tokens in a player&rsquo;s fleet pool indicates the maximum number of non-fighter ships that a player can have in a system.a Units that are on planets or that count against a player&rsquo;s capacity do not count against that player&rsquo;s fleet pool." +
                "b Units that are being transported through systems do not count against a player&rsquo;s fleet pool in those systems.37.2 Players place command tokens in their fleet pools with the ship silhouette faceup.37.3 If at any time the number of a player&rsquo;s non-fighter ships in a system exceeds the number of tokens in that player&rsquo;s fleet pool, they choose and remove " +
                "excess ships in that system, returning those units to their reinforcements.37.4 Players do not spend command tokens from this pool unless a game effect specifically allows it.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.FrontierTokens,
                Content = "<p>38 FRONTIER TOKENSFrontier tokens can be explored for a variety of game effects.38.1 Frontier tokens are placed on the game board during setup. One frontier token is placed in each system that does not contain any planets.</p>\r\n<p>a Frontier tokens are not placed on hyperlane tiles.b A system cannot have more than one frontier token.c Frontier tokens " +
                "are placed in anomalies that do not have planets.d A frontier token is placed in the Creuss gate system.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.GameBoard,
                Content = "<p>39 GAME BOARDThe game board consists of all system tiles in play.39.1 The game board consists of all system tiles that were placed during setup, even if the sides of those tiles do not touch any other system tiles, such as the Ghosts of Creuss&rsquo; home system.39.2 A system tile is on the edge of the game board if any of its sides are not touching another system tile." +
                "a The Ghosts of Creuss home system and the wormhole nexus are on the edge of the game board.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.GameRound,
                Content = "<p>40 GAME ROUNDA game round consists of the following four phases:1. Strategy Phase2. Action Phase3. Status Phase4. Agenda Phase40.1 Players skip the agenda phase during the early portion of each game. After the custodians token is removed from Mecatol Rex, the agenda phase is added to each game round.40.2 Player turns occur during the action phase." +
                "40.3 Abilities that last until the end of a player&rsquo;s turn do not persist for the duration of a game round or into the other phases of that game round. Those effects end at the end of that player&rsquo;s turn, before the next player&rsquo;s turn begins.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.GravityRift,
                Content = "<p>41 GRAVITY RIFTA gravity rift is an anomaly that affects movement.41.1 A ship that will move out of or through a gravity rift at any time during its movement, applies +1 to its move value.a This can allow a ship to reach the active system from farther away than it normally could.41.2 For each ship that would move out of or through a gravity rift, one die is rolled " +
                "immediately before it exits the gravity rift system; on a result of 1&ndash;3, that ship is removed from the board.</p>\r\n<p>a Dice are not rolled for units that are being transported by ships that have capacity.b Units that are being transported are removed from the board if the ship transporting them is removed from the board.c Units that are removed are returned to the player&rsquo;s " +
                "reinforcements.41.3 A gravity rift can affect the same ship multiple times during a single movement.41.4 A system that contains multiple gravity rifts is treated as a single gravity rift.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.GroundCombat,
                Content = "<p>42 GROUND COMBATDuring the &ldquo;Ground Combat&rdquo; step of an invasion, if the active player has ground forces on a planet that contains another player&rsquo;s ground forces, those players resolve a ground combat on that planet. To resolve a ground combat, players perform the following steps:42.1 STEP 1&mdash;ROLL DICE: Each player rolls one die for each ground " +
                "force they have on the planet; this is a combat roll. If a unit&rsquo;s combat roll produces a result that is equal to or greater than that unit&rsquo;s combat value, that roll produces a hit.a If a unit&rsquo;s combat value contains two or more burst icons, the player rolls one die for each burst icon instead.42.2 STEP 2&mdash;ASSIGN HITS: Each player in the combat must choose one of " +
                "their own ground forces on the planet to be destroyed for each hit result their opponent produced.a When a unit is destroyed, the player who controls that unit removes it from the board and places it in their reinforcements.42.3 After assigning hits, if both players still have ground forces on the planet, players resolve a new combat round starting with the &ldquo;Roll Dice&rdquo; step." +
                "42.4 Ground combat ends when only one player (or neither player) has ground forces on the planet.a During the first round of a combat, &ldquo;start of combat&rdquo; and &ldquo;start of combat round&rdquo; effects occur during the same timing window.b During the last round of a combat, &ldquo;end of combat&rdquo; and &ldquo;end of combat round&rdquo; effects occur during the same timing " +
                "window.c After a combat ends, the player with one or more ground forces remaining on the planet is the winner of the combat; the other player is the loser of the combat.d If neither player has a ground force remaining, there is no winner; the combat ends in a draw.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.GroundForces,
                Content = "<p>43 GROUND FORCESA ground force is a type of unit. All infantry and mech units in the game are ground forces. Some races have unique infantry units.43.1 Ground forces are always on planets, in a space area with ships that have capacity values, or being transported by those ships.43.2 Ground forces being transported by a ship are placed in a system&rsquo;s space area along with the ship that is " +
                "transporting them.43.3 There is no limit to the number of ground forces a player can have on a planet</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Hyperlanes,
                Content = "<p>44 HYPERLANESHyperlanes are tiles that are used in some game board setups to create adjacency of system tiles that are not touching each other.44.1 Systems that are connected by lines drawn across one or more hyperlane tiles are adjacent for all purposes.44.2 Hyperlane tiles are not systems. They cannot have units on them and they cannot be targets for effects or abilities.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Imperial,
                Content = "<p>45 IMPERIAL (STRATEGY CARD)The &ldquo;Imperial&rdquo; strategy card allows players to score victory points and draw secret objectives. This card&rsquo;s initiative value is &ldquo;8.&rdquo;45.1 During the action phase, if the active player has the &ldquo;Imperial&rdquo; strategy card, they can perform a strategic action to resolve that card&rsquo;s primary ability." +
                "45.2 To resolve the primary ability on the &ldquo;Imperial&rdquo; strategy card, the active player can score one public objective of their choice if they meet that objective&rsquo;s requirements as described on its card. Then, if the active player controls Mecatol Rex, they gain one victory point; if they do not control Mecatol Rex, they can draw one secret objective card." +
                "45.3 After the active player resolves the primary ability of the &ldquo;Imperial&rdquo; strategy card, each other player, beginning with the player to the left of the active player and proceeding clockwise, may spend one command token from their strategy pool to draw one secret objective card.45.4 If a player has more than three secret objective cards after drawing a secret objective, " +
                "they must choose one of their unscored secret objectives and return it to the secret objective deck. This number includes the secret objective cards in the player&rsquo;s hand and the cards that player has already scored. Then, they shuffle the secret objective deck.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.InfantryTokens,
                Content = "<p>46 INFANTRY TOKENSAn infantry token functions as a plastic infantry unit for all game purposes.46.1 When producing an infantry unit, a player can use an infantry token from the supply instead of a plastic piece.46.2 Players can replace their plastic infantry with tokens at any time.46.3 If a player ever has an infantry token on a planet that does not " +
                "contain one of their plastic infantry or in the space area of a system that does not contain one of their plastic infantry, that player must replace it with one of their plastic infantry from their reinforcements.a If that player cannot replace the token, the unit is destroyed.46.4 Infantry tokens come in values of one and three. A player can swap between these tokens as necessary.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Influence,
                Content = "<p>47 INFLUENCEInfluence represents a planet&rsquo;s political power. Players spend influence to gain command tokens using the &ldquo;Leadership&rdquo; strategy card, and the influence values of planets are used to cast votes during the agenda phase.47.1 A planet&rsquo;s influence is the rightmost value (surrounded by a blue border) found on the planet&rsquo;s system tile and planet card." +
                "47.2 A player can spend a planet&rsquo;s influence by exhausting that planet&rsquo;s card.47.3 A player can spend a trade good as if it were one influence.a Players cannot spend trade goods to cast votes during the agenda phase.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.InitiativeOrder,
                Content = "<p>48 INITIATIVE ORDERInitiative order is the order in which players resolve steps of the action and status phases.48.1 Initiative order is determined by the initiative numbers on strategy cards.a A player who has the Naalu &ldquo;0&rdquo; token has the initiative number 0.48.2 Initiative order begins with the player who has the lowest\u0002numbered strategy card and proceeds " +
                "to the player who has the strategy card that is next in numerical order.a Only strategy cards that were chosen during the strategy phase are used when determining initiative order; strategy cards not chosen during the strategy phase are ignored.</p>\r\n<p>48.3 When playing with three or four players, a player&rsquo;s initiative is determined only by their lowest-numbered strategy card.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Invasion,
                Content = "<p>49 INVASIONInvasion is a step of the tactical action during which the active player can land ground forces on planets to gain control of those planets.To resolve an invasion, players perform the following steps:49.1 STEP 1&mdash;BOMBARDMENT: The active player may use the &ldquo;Bombardment&rdquo; ability of any of their units in the active system." +
                "49.2 STEP 2&mdash;COMMIT GROUND FORCES: If the active player has ground forces in the space area of the active system, that player may commit any number of those ground forces to land on any of the planets in that system.a To commit a ground force to a planet, the active player places that ground force unit on that planet.b The planet may contain another player&rsquo;s ground forces." +
                "c If the active player does not wish to commit ground forces, that player proceeds to the &ldquo;Production&rdquo; step of the tactical action.49.3 STEP 3&mdash;SPACE CANNON DEFENSE: If the active player commits any ground forces to a planet that contains units that have the &ldquo;Space Cannon&rdquo; ability, those &ldquo;Space Cannon&rdquo; abilities can be used against the committed ground forces." +
                "a If the active player committed ground forces to more than one planet that contained units with a &ldquo;Space Cannon&rdquo; ability, the active player chooses the order in which those &ldquo;Space Cannon&rdquo; abilities are resolved.49.4 STEP 4&mdash;GROUND COMBAT: If the active player has ground forces on a planet in the active system that contains another " +
                "player&rsquo;s ground forces, those players resolve a ground combat on that planet.a If players must resolve a combat on more than one planet, the active player chooses the order in which those combats are resolved.49.5 STEP 5&mdash;ESTABLISH CONTROL: The active player gains control of each planet they committed ground forces to if that planet still contains at least one of their ground forces." +
                "a When a player gains control of a planet, any structures on the planet that belong to other players are immediately destroyed.b When a player gains control of a planet, they gain the planet card that matches that planet and exhaust that card.</p>\r\n<p>c A player cannot gain control of a planet they already control.d If there was a combat, and all units belonging to both players were destroyed, " +
                "the player who was the defender retains control of the planet and places one of their control tokens on the planet.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.LeaderSheet,
                Content = "<p>50 LEADER SHEETEach player has a leader sheet that contains slots for their faction&rsquo;s three leader cards as well as their faction&rsquo;s mech unit card. 50.1 The leader slots of the leader sheet are where players place their three leader cards during setup. Each slot displays the name of the type of leader that is placed in that slot (agent, commander, and hero) as well " +
                "as a symbol in the upper-right that helps players quickly determine which side of those leaders begins the game faceup.a The sides of the leader cards that display one (agent), two (commander), or three (hero) hash marks, respectively, begin the game faceup.50.2 The mech slot of the leader sheet is where players place their mech unit cards during setup.50.3 Players who are familiar with the game can hide the quick " +
                "reference by placing that portion of the leader sheet under their faction sheets.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Leaders,
                Content = "<p>51 LEADERSEach player has several faction-specific leader cards that represent characters with unique abilities.51.1 Each faction has three leaders; one agent, one commander, and one hero.a The Nomad&rsquo;s &ldquo;The Company&rdquo; faction ability grants them two additional agents, for a total of five leaders.51.2 A player&rsquo;s leaders are placed on their leader sheet during setup.a Each leader card is " +
                "placed on the slot that matches its type. The leader cards are placed so that the hash mark symbols in the upper right are faceup and match the slot that the card is placed on.b The two additional Nomad agents are placed in the Nomad player&rsquo;s play area readied side up.</p>\r\n<p>51.3 AGENTS 51.4 An agent does not need to be unlocked and begins the game in a readied state. They can be exhausted by resolving their abilities, " +
                "and they ready during the &ldquo;Ready Cards&rdquo; step of the status phase.51.5 COMMANDERS51.6 A commander must be unlocked to use its abilities. A player unlocks their commander if they fulfill the conditions listed after the &ldquo;Unlock&rdquo; header.a Each faction&rsquo;s commander has a unique &ldquo;Unlock&rdquo; condition.b After a player fulfills the unlock condition of their commander, they flip it over " +
                "to its unlocked side.c A commander&rsquo;s unlock conditions cannot be met while an ability or game effect is being resolved. That is, pending abilities or partially resolved game effects must be completed before checking if conditions are met.d A commander cannot flip to its locked side after it is unlocked, even if its owner no longer meets the unlock conditions.51.7 A commander cannot be exhausted.51.8 The &ldquo;Alliance&rdquo; " +
                "promissory note allows a player to share their commander&rsquo;s ability with another player.a A commander&rsquo;s owner can still use their commander&rsquo;s ability, even if another player has their &ldquo;Alliance&rdquo; promissory note.51.9 HEROES51.10 A hero needs to be unlocked to use their abilities. A player unlocks their hero if they fulfill the conditions listed after the &ldquo;Unlock&rdquo; header.a The &ldquo;Unlock&rdquo; " +
                "condition for each hero is to have three scored objectives; these can be any combination of secret objectives and public objectives.b Victory points do not count toward unlocking heroes.c After a player fulfills the unlock condition of their hero, they flip it to its unlocked side.d A hero cannot flip to its locked side after it is unlocked.51.11 A hero cannot be exhausted.51.12 A hero is purged after its abilities are resolved." +
                "a The Titans of Ul&rsquo;s hero is not purged; it is attached to the planet Elysium instead.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Leadership,
                Content = "<p>52 LEADERSHIP (STRATEGY CARD)The &ldquo;Leadership&rdquo; strategy card allows players to gain command tokens. This card&rsquo;s initiative value is &ldquo;1.&rdquo;52.1 During the action phase, if the active player has the &ldquo;Leadership&rdquo; strategy card, they can perform a strategic action to resolve that card&rsquo;s primary ability.52.2 To resolve the primary ability on the &ldquo;Leadership&rdquo; strategy card, the active player " +
                "gains three command tokens. Then, that player can spend any amount of their influence to gain one command token for every three influence they spend.52.3 After the active player resolves the primary ability of the &ldquo;Leadership&rdquo; strategy card, each other player, beginning with the player to the left of the active player and proceeding clockwise, may spend any amount of influence to gain one command token for every three influence they spend." +
                "52.4 When a player gains command tokens, that player places each token on their command sheet in the pool of their choice.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.LegendaryPlanets,
                Content = "<p>53 LEGENDARY PLANETSLegendary planets grant the player that controls them unique, planet-specific abilities.53.1 A legendary planet is indicated by the legendary planet icon.53.2 When a player gains control of a legendary planet, they also place its legendary planet ability card in their play area.a If a player gains control of a legendary planet ability card from the deck, it is readied.b If a player gains control of an exhausted legendary planet " +
                "ability card, it remains exhausted.53.3 Players can use the abilities on the legendary planet ability cards in their play area.53.4 If a legendary planet&rsquo;s planet card is purged, its corresponding legendary planet ability card is also purged.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.MecatolRex,
                Content = "<p>54 MECATOL REXMecatol Rex is the planet placed in the center of the game board during setup.54.1 During setup, the custodians token is placed on Mecatol Rex. This token prevents a player from committing ground forces to land on the planet unless they spend six influence to remove the token.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Mechs,
                Content = "<p>55 MECHSMechs are unique, faction-specific heavy ground forces.55.1 Mechs are a type of ground force and can be transported and participate in ground combat.55.2 Each player begins with their mech unit card in play on their leader sheet and can produce mechs for the cost presented on the card.55.3 Some mechs have &ldquo;Deploy&rdquo; abilities which allow a player to place them on the game board without producing them normally." +
                "55.4 Mech unit cards are not technologies.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Modifiers,
                Content = "<p>56 MODIFIERSA modifier is a number that is applied by an ability to increase or decrease the attribute values of a unit or the results of a die roll.56.1 A modifier is always preceded by the word &ldquo;apply&rdquo; followed by a numerical value.56.2 A modifier value preceded by a &ldquo;+&rdquo; is added to the attribute or result being modified; a modifier value preceded by a &ldquo;-&rdquo; is subtracted from the attribute or result being modified.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Move,
                Content = "<p>57 MOVE (ATTRIBUTE)Move is an attribute of some units that is presented on faction sheets and unit upgrade technology cards.57.1 A unit&rsquo;s move value indicates the distance from its current system that it can move during the &ldquo;Movement&rdquo; step of a tactical action.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Movement,
                Content = "<p>58 MOVEMENTA player can move their ships by resolving a tactical action during the action phase. Additionally, some abilities can move a unit outside of the tactical action.58.2 TACTICAL ACTION MOVEMENT58.3 A ship&rsquo;s move value is presented along with its other attributes on faction sheets and unit upgrade technology cards. This value " +
                "indicates the distance from its current system that a ship can move.</p>\r\n<p>To resolve movement, players perform the following steps:58.4 STEP 1&mdash;MOVE SHIPS: A player can move any number of their eligible ships into the active system, obeying the following rules:a The ship must end its movement in the active system." +
                "b The ship cannot move through a system that contains ships that are controlled by another player.c The ship cannot move if it started its movement in another system that contains one of its faction&rsquo;s command tokens.d The ship can move through systems that contain its own faction&rsquo;s command tokens.e The ship can move out of the active system and back into it " +
                "if its move value is high enough.f The ship must move along a path of adjacent systems, and the number of systems the ship enters cannot exceed its move value.58.5 When a ship with a capacity value moves or is moved, it may transport ground forces and fighters.58.6 The active player declares which of their ships are moving before any ships move. " +
                "Those ships arrive in the active system simultaneously.58.7 STEP 2&mdash;SPACE CANNON OFFENSE: After the &ldquo;Move Ships&rdquo; step, players can use the &ldquo;Space Cannon&rdquo; abilities of their units in the active system.58.8 ABILITY MOVEMENT 58.9 If an ability moves a unit outside of the &ldquo;Movement&rdquo; step of a tactical action, players " +
                "follow the rules specified by that ability; neither a unit&rsquo;s move value nor the rules specified above apply.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Nebula,
                Content = "<p>59 NEBULAA nebula is an anomaly that affects movement and combat.59.1 A ship can only move into a nebula if it is the active system.a A ship cannot move through a nebula. That is, a ship cannot move into and out of a nebula during the same movement.59.2 A ship that begins the &ldquo;Movement&rdquo; step of a tactical action in a nebula treats its move value as &ldquo;1&rdquo; for the duration of that step.a Other abilities and effects can increase this number." +
                "59.3 If a space combat occurs in a nebula, the defender applies +1 to each combat roll of their ships during that combat.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Neighbors,
                Content = "<p>60 NEIGHBORSTwo players are neighbors if they both have a unit or control a planet in the same system. They are also neighbors if they both have a unit or control a planet in systems that are adjacent to each other.60.1 Players can resolve transactions with their neighbors.60.2 Players are neighbors if the adjacency of systems is granted by a wormhole.60.3 Players are neighbors with the Ghosts of Creuss if the Ghosts of Creuss&rsquo; &ldquo;Quantum Entanglement&rdquo; " +
                "faction ability is causing adjacency from the perspective of the Ghosts of Creuss player.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.ObjectiveCards,
                Content = "<p>61 OBJECTIVE CARDSPlayers can score objectives to gain victory points.61.1 There are two types of objective cards: public objectives and secret objectives.a Each public objective has a &ldquo;I&rdquo; or &ldquo;II&rdquo; on the back of its card; all other objectives are secret objectives.61.2 Each objective card indicates a number of victory points that a player gains by scoring that objective.61.3 Each objective card indicates the phase during which a player " +
                "can score that objective&mdash;the status, action, or agenda phases.61.4 Each objective card describes the requirement a player must fulfill to score that objective.61.5 If a player fulfills the requirement described on an objective card, they can score that objective following the timing indicated on the card, either during the action phase or the status phase.a When a player scores an objective during the status phase, they must fulfill the requirement on the card during the &ldquo;Score " +
                "Objectives&rdquo; step of the status phase to score that objective.b When a player scores an objective during the action phase, they can do so at any time during that phase.c When a player scores an objective during the agenda phase, they can do so at any time during that phase.61.6 A player can score a maximum of one public objective and one secret objective during each status phase.61.7 A player can score any number of objectives during the agenda phase or during a turn of the action phase; however," +
                " they can only score one objective during or after each combat.a A player can score an objective during both the space combat and the ground combat during the same tactical action.61.8 A player can score each objective only once during the game.</p>\r\n<p>61.9 If an objective requires a player to destroy one or more units, those units can be destroyed by producing hits against them, playing action cards, using technology, or any number of other abilities that use the &ldquo;destroy&rdquo; terminology." +
                "a Forcing a player to remove a unit from the board by reducing the number of command tokens in their fleet pool is not treated as destroying a unit.61.10 Players can score some objectives by spending resources, influence, or tokens, as described by the objective card. To score such an objective, a player must pay the specified cost at the time indicated on the card.61.11 PUBLIC OBJECTIVESA public objective is an objective that is revealed to all players.61.12 When scoring a public objective, the player " +
                "places one of their control tokens on that objective&rsquo;s card. Then, that player advances their control token on the victory point track a number of spaces equal to the number of victory points gained.61.13 Each game contains five stage I and five stage II public objective cards that the speaker places facedown near the victory point track during setup.a The speaker reveals two of the stage I objective cards during setup. All other objective cards remain facedown.61.14 During each status phase, the speaker reveals a facedown " +
                "public objective card.a The speaker does not reveal stage II objective cards until all stage I objective cards are revealed.61.15 If the speaker must reveal a facedown public objective card but all public objective cards are already revealed, the game ends immediately.a The player with the most victory points is the winner. If one or more players are tied for having the most victory points, the tied player who is first in initiative order is the winner.61.16 A player cannot score public objectives if that player does not " +
                "control each planet in their home system.61.17 SECRET OBJECTIVESA secret objective is an objective that is controlled by one player and is hidden from all other players until it is scored.61.18 When scoring a secret objective, a player reveals the objective by placing it faceup in their play area. Then, they place one of their control tokens on that objective&rsquo;s card and advances their control token on the victory point track a number of spaces equal to the number of victory points gained.</p>" +
                "\r\n<p>61.19 A player can only score their own secret objectives; a player cannot score secret objectives revealed by other players.61.20 Each player begins the game with one secret objective.61.21 Each player can have up to three total scored and unscored secret objectives.a If a player draws a secret objective and has more than three, that player must choose one of their unscored secret objectives and return it to the deck. Then, that player shuffles the secret objective deck.61.22 A player can gain secret objectives by resolving either the " +
                "primary or secondary ability of the &ldquo;Imperial&rdquo; strategy card.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Opponent,
                Content = "<p>62 OPPONENTDuring combat, a player&rsquo;s opponent is the other player that either has ships in the system at the start of the space combat or has ground forces on the planet at the start of a ground combat.62.1 Players who do not have units on either side of a combat are not opponents. Those players cannot use abilities or have abilities used against them that are used against an opponent.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Pds,
                Content = "<p>63 PDSA PDS (planetary defense system) is a structure that allows a player to defend their territory against invading forces.63.1 Each PDS has the &ldquo;Space Cannon&rdquo; ability.63.2 The primary way by which players acquire PDS units is by resolving either the primary or secondary ability of the &ldquo;Construction&rdquo; strategy card.63.3 A PDS unit is placed on a planet. Each planet can have a maximum of two PDS units.63.4 If a player&rsquo;s PDS is ever on a planet that does not contain any " +
                "of their own ground forces and contains a unit that belongs to another player, that PDS is destroyed.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Planets,
                Content = "<p>64 PLANETSPlanets provide players with resources and influence. Planets are on system tiles and each has a name, a resource value, and an influence value. Some planets also have traits.64.1 A planet&rsquo;s resources are indicated by the value on its planet card and system tile that is surrounded by a yellow triangular border.</p>\r\n<p>64.2 A planet&rsquo;s influence is indicated by the value on its planet card and system tile that is surrounded by a blue hexagonal border.64.3 A planet&rsquo;s trait has no inherent effects, but some game effects " +
                "refer to a planet&rsquo;s trait. There are three traits: cultural, hazardous, and industrial.64.4 Some planets have a technology specialty, which allows those planets to be exhausted to satisfy a prerequisite when researching technology.64.5 Some planets are legendary planets, as indicated by the legendary planet icon. When a player gains control of a legendary planet, they also gain control of its legendary planet ability card.64.6 PLANET CARDEach planet has a corresponding planet card that displays its name, resource value, influence value, and trait, " +
                "if it has one. If a player controls a planet, they keep that planet&rsquo;s card in their play area.64.7 A planet card has both a readied and exhausted state. When a planet is readied, it is placed faceup. When a planet is exhausted, it is placed facedown.64.8 A player can spend a readied planet&rsquo;s resources or influence.64.9 A player cannot spend an exhausted planet&rsquo;s resources or influence.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.PlanetaryShield,
                Content = "<p>65 PLANETARY SHIELD (UNIT ABILITY)Units cannot use the &ldquo;Bombardment&rdquo; ability against a planet that contains a unit that has the &ldquo;Planetary Shield&rdquo; ability.65.1 The &ldquo;Planetary Shield&rdquo; ability does not prevent a planet from being affected by the &ldquo;X-89 Bacterial Weapon&rdquo; technology.65.2 The &ldquo;Planetary Shield&rdquo; ability prevents an L1Z1X player from using their &ldquo;Harrow&rdquo; faction ability.65.3 If a war sun is in a system with any number of other players&rsquo; " +
                "units that have the &ldquo;Planetary Shield&rdquo; ability, those units are treated as if they do not have that ability.a Units treated as if they do not have a &ldquo;Planetary Shield&rdquo; ability cannot use the &ldquo;Magen Defense Grid&rdquo; technology.b A war sun can use its &ldquo;Bombardment&rdquo; ability against planets that contain units that have the &ldquo;Planetary Shield&rdquo; ability.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Politics,
                Content = "<p>66 POLITICS (STRATEGY CARD)The &ldquo;Politics&rdquo; strategy card allows players to draw action cards. Additionally, the active player chooses a new speaker and looks at cards in the agenda deck. This card&rsquo;s initiative value is &ldquo;3.&rdquo;66.1 During the action phase, if the active player has the &ldquo;Politics&rdquo; strategy card, they can perform a strategic action to resolve that card&rsquo;s primary ability.66.2 To resolve the primary ability on the &ldquo;Politics&rdquo; strategy card, " +
                "the active player resolves the following effects in order:i. The active player chooses any player that does not have the speaker token. The active player may choose themselves as long as they do not have the speaker token. The chosen player places the speaker token in their play area; they are now the speaker.ii. The active player draws two action cards.iii.The active player secretly looks at the top two cards of the agenda deck. Then, that player places each card on either the top or the bottom of the deck. " +
                "If they place both cards on either the top or bottom, they can place them in any order.66.3 After the active player resolves the primary ability of the &ldquo;Politics&rdquo; strategy card, each other player, beginning with the player to the left of the active player and proceeding clockwise, may spend one command token from their strategy pool to draw two action cards.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.ProducingUnits,
                Content = "<p>67 PRODUCING UNITSThe primary way that a player produces new units is by resolving the &ldquo;Production&rdquo; abilities of existing units during a tactical action. However, other game effects also allow players to produce units.67.1 Each unit that a player can produce has a cost value presented on its faction sheet or technology card. To produce a unit, a player must spend a number of resources equal to or greater than the cost value of the unit they are producing. " +
                "a Spent resources must come from planets or trade goods that are controlled by the player who is producing the units.b Any resources spent in excess of a unit&rsquo;s cost are lost.c If a player is producing multiple units at a time, that player can add the cost of each unit they are producing to create a total cost before they spend any resources.67.2 If the cost is accompanied by two icons&mdash;typically for fighters and infantry&mdash;a player produces two of that unit for that cost." +
                "a Each of the two units counts toward the total number of units a player can produce.</p>\r\n<p>b A player can choose to produce only one unit; however, they must still pay the entire cost.67.3 When a player produces a unit through the use of their units&rsquo; &ldquo;Production&rdquo; abilities during a tactical action, that player follows the rules of the &ldquo;Production&rdquo; ability to determine where in the active system the units can be placed.67.4 When a player produces a unit through an ability outside of the " +
                "tactical action, that ability will state how many units that player can produce and where that player can place those units.a A player cannot produce a unit on a planet they do not control.b If an ability allows a player to produce a unit in a system, they may produce that unit in the space area or on a planet they control in that system.67.5 A player is limited by the number of units in their reinforcements.a If a player cannot produce a unit because it is not in their reinforcements, that player can remove a unit from any " +
                "system that does not contain one of their command tokens and place that unit in their reinforcements. Then, that player can produce that unit. A player can remove any number of their units in this way; however, any units that are removed must be produced immediately.b When producing a fighter or infantry unit, a player can use a fighter or infantry token, as appropriate, from the supply instead of a plastic piece.67.6 A player cannot produce ships in a system that contains other players&rsquo; ships.a Ground forces can still be produced.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Production,
                Content = "<p>68 PRODUCTION (UNIT ABILITY)During the &ldquo;Production&rdquo; step of a tactical action, the active player can resolve the &ldquo;Production&rdquo; ability of each of their units that are in the active system to produce units.68.1 A unit&rsquo;s &ldquo;Production&rdquo; ability, which is presented on a faction sheet or unit upgrade technology card, is always followed by a value. This value is the maximum number of units that this unit can produce.a If the active player has multiple units in the active system " +
                "that have the &ldquo;Production&rdquo; ability, that player can produce a number of units up to the combined total of their units&rsquo; production values in that system.b When producing fighters or infantry, each individual unit counts toward the producing unit&rsquo;s production limit.c A player can choose to produce one fighter or infantry instead of two, but must still pay the entire cost.</p>\r\n<p>d &ldquo;Production&rdquo; value from Arborec space docks cannot be used to produce infantry, even if the Arborec player controls " +
                "other units that have &ldquo;Production&rdquo; in the same system.68.2 When a player produces ships by using &ldquo;Production,&rdquo; that player must place them in the active system.68.3 When a player produces ground forces, that player must place those unit on planets that contain a unit that used its &ldquo;Production&rdquo; ability.68.4 If a player uses the &ldquo;Production&rdquo; ability of a unit in a space area of a system to produce ground forces, those ground forces may either be placed on a planet the player controls " +
                "in that system or in the space area of that system.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.PromissaryNotes,
                Content = "<p>69 PROMISSORY NOTESEach player begins the game with one unique and five generic promissory note cards that can be given to other players.69.1 Each promissory note contains timing text and ability text. A player can resolve any of their promissory note cards by following the text on the card.a Promissory notes are not mandatory unless otherwise specified.69.2 A player cannot play their color&rsquo;s or faction&rsquo;s promissory notes. Since the cards are only valuable to other players, promissory " +
                "notes can be traded as powerful negotiation tools.69.3 Promissory notes that are returned to a player are returned after their abilities have been completely resolved.69.4 If a promissory note is returned to a player, that player may give it to other players again as part of a future transaction.a An unrevealed promissory note is not subject to effects in its ability text that return the card if certain conditions are met.69.5 When resolving a transaction, a player can trade a maximum of one promissory note from their hand to another player, even if " +
                "that card originally belonged to another player.a Promissory notes in the play area cannot be traded.69.6 Players should keep their hands of promissory notes hidden.69.7 If a player is eliminated, all of the promissory notes that match their color or faction are returned to the game box, including those that are in play or owned by other players.a Other players&rsquo; promissory notes are returned to those players.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Purge,
                Content = "<p>70 PURGEPurge is a cost that permanently removes a component from the game. If an ability requires that its component is purged, that component can only be used once per game.70.1 If an ability instructs a player to purge a component, that component is removed from the game and returned to the box.70.2 Purged components cannot be used or otherwise returned to the game by any means.70.3 When a player is intructed to purge a component, that component is purged even if its ability was only partially resolved.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Readied,
                Content = "<p>71 READIEDCards have a readied state, which indicates that a player can exhaust or resolve the abilities on those cards.71.1 A card that is readied is placed faceup in a player&rsquo;s play area; a card that is exhausted is placed facedown in a player&rsquo;s area.71.2 A player can exhaust a readied planet card to spend resources or influence from that card&rsquo;s planet.71.3 A player can exhaust certain readied technology cards to resolve those cards&rsquo; abilities.a Such a technology will specifically instruct a player to " +
                "exhaust the card as part of the ability&rsquo;s cost.71.4 If a card is exhausted, a player cannot resolve that card&rsquo;s abilities or spend resources or influence on that card until it is readied.71.5 During a &ldquo;Ready Cards&rdquo; step, each player readies all of their exhausted cards by flipping them faceup.71.6 When a player performs a strategic action, they exhaust their chosen strategy card.a That card is later readied during the status phase.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Reinforcements,
                Content = "<p>72 REINFORCEMENTSA player&rsquo;s reinforcements is that player&rsquo;s personal supply of units and command tokens that are not on the game board or otherwise in use.72.1 The components in a player&rsquo;s reinforcements are limited.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Relics,
                Content = "<p>73 RELICSRelics are powerful artifacts with unique abilities.73.1 Players can use the abilities of hazardous, cultural, and industrial relic fragments in their play area to draw cards from the relic deck.a Relic fragments can be found when exploring planets and frontier tokens, and can be exchanged with other players as part of transactions.73.2 When a player is instructed to gain a relic, they draw the top card of the relic deck and place it faceup in their play area.a If there are no cards in the relic deck, they do not gain a " +
                "relic.73.3 A player can use the abilities of relics that are in their play area.73.4 Relics cannot be traded.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Rerolls,
                Content = "<p>74 REROLLSSome game effects instruct a player to reroll dice.74.1 When a die is rerolled, its new result is used instead of its previous result.74.2 The same ability cannot be used to reroll the same die multiple times, but multiple abilities can be used to reroll a single die.74.3 Die rerolls must occur after rolling the dice, before other abilities are resolved.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Resources,
                Content = "<p>75 RESOURCESResources represent a planet&rsquo;s material value and industry. Many game effects, such as producing units, require players to spend resources.75.1 A planet&rsquo;s resources are the leftmost value that is surrounded by a yellow border on the planet&rsquo;s system tile and planet card.75.2 A player spends a planet&rsquo;s resources by exhausting its card." +
                "75.3 A player can spend a trade good as if it were one resource.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Ships,
                Content = "<p>76 SHIPSA ship is a unit type consisting of carriers, cruisers, dreadnoughts, destroyers, fighters, and war suns. Each race also has a unique flagship.76.1 Ships are always placed in space.76.2 A player can have a number of ships in a system equal to or less than the number of command tokens in that player&rsquo;s fleet pool.a Fighters do not count toward the fleet pool limit, and " +
                "instead count against a player&rsquo;s capacity.76.3 Ships can have any number of the following attributes: cost, combat, move, and capacity. These attributes are shown on faction sheets and unit upgrade technology cards.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.SpaceCannon,
                Content = "<p>77 SPACE CANNON (UNIT ABILITY)A unit that has the &ldquo;Space Cannon&rdquo; ability can use it during two different steps of a player&rsquo;s tactical action: after the &ldquo;Move Ships&rdquo; substep (Space Cannon Offense) and during an invasion (Space Cannon Defense).77.1 A player is not required to be the active player to use their &ldquo;Space Cannon&rdquo; ability of their units." +
                "77.2 SPACE CANNON OFFENSEDuring a tactical action, after the &ldquo;Move Ships&rdquo; substep of the &ldquo;Movement&rdquo; step, beginning with the active player and proceeding clockwise, each player may use the &ldquo;Space Cannon&rdquo; ability of each of their units in the active system by performing the following steps:77.3 STEP 1&mdash;ROLL DICE: The player rolls dice for each of their " +
                "units in the active system that has the &ldquo;Space Cannon&rdquo; ability; this is a space cannon roll. One hit is produced for each result that is equal to or greater than the unit&rsquo;s &ldquo;Space Cannon&rdquo; value.a A unit&rsquo;s &ldquo;Space Cannon&rdquo; ability is presented along with a unit&rsquo;s attributes on faction sheets and unit upgrade technology cards." +
                "b &ldquo;Space Cannon&rdquo; is displayed as &ldquo;Space Cannon X (xY).&rdquo; The X is the minimum value needed for a die to produce a hit, and Y is the number of dice rolled. Not all &ldquo;Space Cannon&rdquo; abilities are accompanied by a (Y) value; a space cannon roll for such a unit consists of one die.c If a player has a PDS unit upgrade technology, they can use the &ldquo;Space Cannon&rdquo; ability of their PDS units that are in " +
                "systems that are adjacent to the active system. The hits are still assigned to units in the active system.d Game effects that reroll, modify, or otherwise affect combat rolls do not affect space cannon rolls.</p>\r\n<p>77.4 This ability can be used even if no ships were moved during the &ldquo;Move Ships&rdquo; step.77.5 STEP 2&mdash;ASSIGN HITS: The player whose units have been targeted by &ldquo;Space Cannon&rdquo; must choose and destroy one of " +
                "their ships in the active system for each hit result produced against their units.a Players other than the active player must target the active player&rsquo;s units.b If the active player is using the &ldquo;Space Cannon&rdquo; ability of their units, they choose a player who has ships in the active system. That player must choose and destroy one of their ships in the active system for each hit the space cannon roll produced." +
                "77.6 SPACE CANNON DEFENSEDuring the invasion step of a tactical action, after ground forces have been committed to land on planets, players other than the active player can resolve the &ldquo;Space Cannon&rdquo; ability of their units on those planets by performing the following steps:77.7 STEP 1&mdash;ROLL DICE: Each player may use the &ldquo;Space Cannon&rdquo; ability of each of their units on the invaded planet by rolling " +
                "a specific number of dice for each of those units; this is called a space cannon roll. A hit is produced for each die roll that is equal to or greater than the unit&rsquo;s &ldquo;Space Cannon&rdquo; value.a If a unit has a &ldquo;Space Cannon&rdquo; ability, it is present on its faction sheet and technology cards.b &ldquo;Space Cannon&rdquo; is displayed as &ldquo;Space Cannon X (xY).&rdquo; The X is the minimum value needed for a die to produce " +
                "a hit, and Y is the number of dice rolled. Not all &ldquo;Space Cannon&rdquo; abilities are accompanied by a (Y) value; a space cannon roll for such a unit consists of one die.c Game effects that reroll, modify, or otherwise affect combat rolls do not affect space cannon rolls.d Game effects that allow the use of &ldquo;Space Cannon&rdquo; abilities against ships in adjacent systems have no effect during Space Cannon Defense." +
                "77.8 STEP 2&mdash;ASSIGN HITS: The active player must choose and destroy one of their ground forces on the planet for each hit the space cannon roll produced.a Hits can only be assigned to units that are on the same planet as the units using the &ldquo;Space Cannon&rdquo; ability.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.SpaceCombat,
                Content = "<p>78 SPACE COMBATAfter resolving the &ldquo;Space Cannon Offense&rdquo; step of a tactical action, if two players have ships in the active system, those players must resolve a space combat.78.1 If the active player is the only player with ships in the system, they skip the &ldquo;Space Combat&rdquo; step of the tactical action and proceeds to the &ldquo;Invasion&rdquo; step.78.2 If an ability occurs &ldquo;before combat,&rdquo; " +
                "it occurs immediately before the &ldquo;Anti-Fighter Barrage&rdquo; step.a During the first round of a combat, &ldquo;start of combat&rdquo; and &ldquo;start of combat round&rdquo; effects occur during the same timing window.b During the last round of a combat, &ldquo;end of combat&rdquo; and &ldquo;end of combat round&rdquo; effects occur during the same timing window.To resolve a space combat, players perform the following steps:" +
                "78.3 STEP 1&mdash;ANTI-FIGHTER BARRAGE: If this is the first round of a space combat, the players may simultaneously use the &ldquo;Anti-Fighter Barrage&rdquo; ability of any of their units in the active system.a If one or both players no longer have ships in the active system after resolving this step, the space combat ends immediately.b Players cannot resolve &ldquo;Anti-Fighter Barrage&rdquo; abilities during any rounds of space combat other than the first " +
                "round.c This step still occurs if no fighters are present.78.4 STEP 2&mdash;ANNOUNCE RETREATS: Each player may announce a retreat, beginning with the defender.a A retreat will not occur immediately; the units retreat during the &ldquo;Retreat&rdquo; step.b If the defender announces a retreat, the attacker cannot announce a retreat during that combat round.c A player cannot announce a retreat if there is not at least one eligible system to retreat to." +
                "78.5 STEP 3&mdash;ROLL DICE: Each player rolls one die for each ship they have in the active system; this is called a combat roll. If a unit&rsquo;s combat roll produces a result that is equal to or greater than that unit&rsquo;s combat value, that result produces a hit.a If a unit&rsquo;s combat value contains two or more burst icons, the player rolls one die for each burst icon instead.b If a player has ships that have different combat values in the active system, " +
                "that player rolls these dice separately.</p>\r\n<p>c First, that player should roll all dice for units with a combat value of &ldquo;1.&rdquo; Then, that player should roll all dice for units with combat value of &ldquo;2,&rdquo; and then &ldquo;3,&rdquo; continuing in numerical order until that player has rolled dice for each of their ships.d The player counts each hit their combat rolls produce. The total number of hits produced will destroy units during the &ldquo;Assign Hits&rdquo; step." +
                "e If a player has an ability that rerolls a die or affects a die after it is rolled, that player must resolve such an ability immediately after rolling all of their dice.f The attacker makes all of their combat rolls during this step before the defender. This procedure is important for abilities that allow a player to reroll an opponent&rsquo;s die.78.6 STEP 4&mdash;ASSIGN HITS: Each player in the combat must choose and destroy one of their own ships in the active system " +
                "for each hit their opponent produced.a Before assigning hits, players may use their units&rsquo; &ldquo;Sustain Damage&rdquo; abilities to cancel hits.b When a unit is destroyed, the player who controls that unit removes it from the board and places it in their reinforcements.78.7 STEP 5&mdash;RETREAT: If a player announced a retreat during step 2, and there is still an eligible system for their units to retreat to, they must retreat.a If a player announced a retreat during the &ldquo;Announce " +
                "Retreats&rdquo; step, but their opponent has no ships remaining in the system, the combat immediately ends and the retreat does not occur.b To retreat, a player takes all of their ships with a move value in the combat and moves them to a single system that is adjacent to the active system. That player&rsquo;s fighters and ground forces in the space area of the active system that are unable to move or be transported are removed.c The system that a player&rsquo;s units retreat to must contain one " +
                "or more of that player&rsquo;s units, a planet they control, or both. Additionally, the system cannot contain ships controlled by another player.d If any of a player&rsquo;s units successfully retreat and are moved into an adjacent system, that player must place a command token from their reinforcements in the system to which their units retreated. If that system already contains one of their command tokens, that player does not place an additional token there. If the player has no command tokens " +
                "in their reinforcements, that player must use one from their command sheet instead.</p>\r\n<p>78.8 After the &ldquo;Retreat&rdquo; step, if both players still have ships in the active system, those players resolve another round of space combat beginning with the &ldquo;Announce Retreats&rdquo; step.78.9 Space combat ends when only one player&mdash;or neither player&mdash;has a ship in the space area of the active system.a During the last round of a combat, &ldquo;end of combat&rdquo; and " +
                "&ldquo;end of combat round&rdquo; effects occur during the same timing window.78.10 After a combat ends, the player with one or more ships remaining in the system is the winner of the combat; the other player is the loser of the combat. If neither player has a ship remaining, the combat ends in a draw and there is no winner.a If the winner of the combat has fighters or ground forces in the space area of the active system and those units exceed the capacity of that player&rsquo;s ships in that system, " +
                "that player must choose and remove any excess units.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.SpaceDock,
                Content = "<p>79 SPACE DOCKA space dock is a structure that allows players to produce units.79.1 Each space dock has a &ldquo;Production&rdquo; ability that indicates the number of units it can produce.79.2 The primary way in which players acquire space docks is by resolving either the primary or secondary abilities of the &ldquo;Construction&rdquo; strategy card.79.3 Space docks are placed on planets. Each planet can have a maximum of one space dock." +
                "79.4 If a player&rsquo;s space dock is ever on a planet that does not contain any of their ground forces and contains a unit that belongs to another player, that space dock is destroyed.a The Clan of Saar&rsquo;s &ldquo;Floating Factory&rdquo; faction-specific space dock is destroyed when it is blockaded; that is to say, when it is in a system with another player&rsquo;s ships and none of the Clan of Saar&rsquo;s ships.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Speaker,
                Content = "<p>80 SPEAKERThe speaker is the player who has the speaker token.80.1 During the strategy phase, the speaker is the first player to choose a strategy card.80.2 During the agenda phase, the speaker reveals the top agenda card from the agenda deck before each vote. The speaker is always the last player to vote and decides which outcome to resolve if the outcomes are tied.</p>\r\n<p>80.3 During setup, the speaker prepares the objectives." +
                "80.4 During the status phase, the speaker reveals a public objective.80.5 A random player gains the speaker token during setup before the game begins.80.6 During the action phase, if a player resolves the primary ability on the &ldquo;Politics&rdquo; strategy card, that player chooses any player other than the current speaker to gain the speaker token.80.7 If the speaker is eliminated from the game, the speaker token is passed to the player to the speaker&rsquo;s left.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.StatusPhase,
                Content = "<p>81 STATUS PHASEDuring the status phase, players score objectives and prepare for the next game round. To resolve the status phase, players perform the following steps:81.1 STEP 1&mdash;SCORE OBJECTIVES: Following initiative order, each player may score up to one public objective and one secret objective that can be fulfilled during the status phase. To score an objective, a player must fulfill the requirements on the " +
                "card; if that player does, they gain a number of victory points indicated on the card.81.2 STEP 2&mdash;REVEAL PUBLIC OBJECTIVE: The speaker reveals an unrevealed public objective card by flipping that card faceup.a The speaker cannot reveal &ldquo;Stage II&rdquo; objectives until all &ldquo;Stage I&rdquo; objectives are revealed.b The game ends if there are no unrevealed public objectives at the start of this step." +
                "81.3 STEP 3&mdash;DRAW ACTION CARDS: Following initiative order, each player draws one action card.81.4 STEP 4&mdash;REMOVE COMMAND TOKENS: Each player removes all of their command tokens from the game board. Each player returns each removed token to their reinforcements.81.5 STEP 5&mdash;GAIN AND REDISTRIBUTE COMMAND TOKENS:Each player gains two command tokens. A player takes each gained token from their reinforcements. Then, each player can " +
                "redistribute each command token on their command sheet, including the two they just gained, among their strategy, tactic, and fleet pools.a Players should remember to check the number of their ships in each system after reducing the size of their fleet pools.b This step can usually be resolved simultaneously, but if there is a timing conflict, it is resolved in initiative order.</p>\r\n<p>81.6 STEP 6&mdash;READY CARDS: Each player readies all of their " +
                "exhausted cards, including strategy cards.81.7 STEP 7&mdash;REPAIR UNITS: Each player repairs all of their damaged units by turning those units upright.81.8 STEP 8&mdash;RETURN STRATEGY CARDS: Each player returns their strategy card to the common play area. Then, if a player has removed the custodians token from Mecatol Rex, the game round continues to the agenda phase. Otherwise, a new game round begins with the strategy phase.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.StrategicAction,
                Content = "<p>82 STRATEGIC ACTIONDuring the action phase, the active player may perform a strategic action to resolve the primary ability on their strategy card.82.1 After the active player resolves the primary ability on their strategy card, each other player, beginning with the player to the left of the active player and proceeding clockwise, may resolve that strategy card&rsquo;s secondary ability.a Players do not have to resolve the secondary abilities of the " +
                "active player&rsquo;s strategy card.82.2 After each player has had an opportunity to resolve a strategy card&rsquo;s secondary ability, the active player exhausts their strategy card so that it is facedown&mdash;this indicates that they cannot use this card again this round and is a reminder that they can now pass during one of their later turns.a During three- and four-player games, a player must resolve the strategic action on each of their chosen strategy cards " +
                "before they can pass.82.3 When a player is resolving either the primary or secondary abilities from a strategy card, that player resolves each of the ability&rsquo;s effects from top to bottom.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.StrategyCard,
                Content = "<p>83 STRATEGY CARDStrategy cards determine initiative order and provide each player with a powerful ability that they can use one time during the action phase.83.1 During the strategy phase, each player chooses a strategy card from the common play area and places it in their play area faceup.83.2 Each strategy card has a readied and an exhausted side.</p>\r\n<p>a The readied side contains the strategy card&rsquo;s name, initiative number, and abilities." +
                "b The exhausted side contains the strategy card&rsquo;s initiative number.83.3 A player can only resolve the primary ability of their ownstrategy cards.83.4 A player can only resolve the secondary ability of strategy cards that were chosen by other players.83.5 There are eight strategy cards, each of which has a name and an initiative number.83.6 The initiative number on a player&rsquo;s strategy card determines the initiative order for the action phase and status phase." +
                "83.7 A strategy card has both a primary ability and a secondary ability. These abilities are resolved during a strategic action.83.8 Each strategy card exists in either the common play area or a player&rsquo;s play area.a Strategy cards in the common play area are available for players to choose during the strategy phase.b A strategy card in a player&rsquo;s play area belongs to that player until it is returned to the common play area during the status phase.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.StrategyPhase,
                Content = "<p>84 STRATEGY PHASEDuring the strategy phase, each player chooses a strategy card to use during the round.To resolve the strategy phase, players perform the following steps:84.1 STEP 1&mdash;Starting with the speaker and proceeding clockwise, each player chooses one strategy card from the common play area and places it faceup in their play area.a If there are one or more trade good tokens on a strategy card when a player chooses it, that player gains those trade goods." +
                "b A player cannot choose a strategy card that another player has already chosen during the current strategy phase.c When playing with three or four players, each player will choose a second strategy card. After the last player has received their first strategy card, each player chooses a second strategy card, starting with the speaker and proceeding clockwise.84.2 STEP 2&mdash;The speaker places one trade good token from the " +
                "supply on each strategy card that was not chosen.</p>\r\n<p>a During a four- or eight-player game, all strategy cards will be chosen, and therefore no trade good tokens will be placed on strategy cards.Then, players proceed to the action phase.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Structures,
                Content = "<p>85 STRUCTURESA structure is a type of unit. PDS units and space docks are structures.85.1 Structures are always placed on planets.a The Clan of Saar&rsquo;s &ldquo;Floating Factory&rdquo; faction-specific space dock is placed in a system&rsquo;s space area.85.2 Structures are primarily placed on planets using the &ldquo;Construction&rdquo; strategy card.85.3 Structures cannot move or be transported.85.4 A player can have a maximum of one space dock on each planet." +
                "85.5 A player can have a maximum of two PDS units on each planet.85.6 A player cannot place a structure on a planet if it would exceed the maximum number of allowed structures of that type on that planet.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Supernova,
                Content = "<p>86 SUPERNOVAA supernova is an anomaly that affects movement.86.1 A ship cannot move through or into a supernova.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.SustainDamage,
                Content = "<p>87 SUSTAIN DAMAGE (UNIT ABILITY)Some units have the &ldquo;Sustain Damage&rdquo; ability. Immediately before a player assigns hits to their units, that player can use the &ldquo;Sustain Damage&rdquo; ability of any of their units in the active system.87.1 For each &ldquo;Sustain Damage&rdquo; ability that a player uses, one hit produced by another player&rsquo;s units is canceled. Then, each unit using this ability is placed on its side to indicate that it is damaged." +
                "87.2 A damaged unit does not have reduced capabilities and is functionally the same as an undamaged unit, except that it cannot use the &ldquo;Sustain Damage&rdquo; ability.87.3 A damaged unit cannot use the &ldquo;Sustain Damage&rdquo; ability until it is repaired during the status phase or by another game effect.</p>\r\n<p>87.4 A unit can use its &ldquo;Sustain Damage&rdquo; ability any time a hit is produced against it. This includes hits produced during combat " +
                "and from unit abilities such as the &ldquo;Space Cannon&rdquo; ability.a A unit can only use the &ldquo;Sustain Damage&rdquo; ability if it is eligible to be hit. For example, a player cannot use a dreadnought&rsquo;s &ldquo;Sustain Damage&rdquo; ability to cancel a hit from &ldquo;Anti-Fighter Barrage.&rdquo;87.5 The &ldquo;Sustain Damage&rdquo; ability cannot be used to cancel an effect that directly destroys a unit.87.6 The Barony of Letnev&rsquo;s &ldquo;Non-Euclidean Shielding&rdquo; faction " +
                "technology allows the Letnev player&rsquo;s units with the &ldquo;Sustain Damage&rdquo; ability to cancel up to two hits instead of one.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.SystemTiles,
                Content = "<p>88 SYSTEM TILESA system tile represents an area of the galaxy. Players place system tiles during setup to create the game board.88.1 The back of each system tile is colored green, blue, or red.88.2 System tiles with a green-colored back are home systems and faction-specific tiles. Each home system is unique to one of the game&rsquo;s factions.88.3 System tiles with a blue-colored back each contain one or more planets.88.4 System tiles with a red-colored back are anomalies or are " +
                "systems that do not contain planets.88.5 Planets are located in systems. Ground forces and structures are usually placed on planets.88.6 Any area on a system tile that is not a planet is space. Ships are usually placed in the space area.88.7 Double-sided tiles that have lines crossing from one edge to another are hyperlane tiles. Hyperlane tiles are not systems.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.TacticalAction,
                Content = "<p>89 TACTICAL ACTIONThe tactical action is the primary method by which players produce units, move ships, and extend their dominion within the galaxy. To perform a tactical action, the active player performs the following steps:89.1 STEP 1&mdash;ACTIVATION: The active player must activate a system that does not contain one of their command tokens.a To activate a system, the active player places a command token from their tactic pool in that system. That system is the active system." +
                "b Other players&rsquo; command tokens do not prevent a player from activating a system.</p>\r\n<p>89.2 STEP 2&mdash;MOVEMENT: The active player may move any number of ships that have a sufficient move value from any number of systems that do not contain one of their command tokens into the active system, following the rules for movement.a Ships that have capacity values can transport ground forces and fighters when moving.b The player may choose to not move any ships." +
                "c After the &ldquo;Move Ships&rdquo; step, all players can use the &ldquo;Space Cannon&rdquo; abilities of their units in the active system.89.3 STEP 3&mdash;SPACE COMBAT: If two players have ships in the active system, those players must resolve a space combat.a If the active player is the only player with ships in the system, they skip this step.89.4 STEP 4&mdash;INVASION: The active player may use their &ldquo;Bombardment&rdquo; abilities, commit units to land on planets, and " +
                "resolve ground combat against other players&rsquo; units.89.5 STEP 5&mdash;PRODUCTION: The active player may resolve each of their unit&rsquo;s &ldquo;Production&rdquo; abilities in the active system.a The active player may do this even if they did not move units or land ground forces during this tactical action.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.TechnologySystem,
                Content = "<p>90 TECHNOLOGYTechnology cards allow players to upgrade units and acquire powerful abilities.90.1 Each player places any technology they have gained faceup near their faction sheet. For the duration of the game, they own any technology cards they gain and can use the abilities on those cards.90.2 A player does not own any technology card that is in their technology deck.90.3 A player can gain a technology card from their technology deck " +
                "by researching technology.a Both the primary and secondary abilities of the &ldquo;Technology&rdquo; strategy card allow a player to research a technology.90.4 Any technology card that a player has not gained remains in their technology deck. A player can look through their technology deck at any time.</p>\r\n<p>90.5 If an ability instructs a player to gain a technology, they do not research it; they take it from their technology deck and place it in their play area, ignoring prerequisites." +
                "90.6 Some technologies are unit upgrades. Unit upgrades share a name with a unit that is printed on a player&rsquo;s faction sheet.a Players place any unit upgrades they gain faceup on their faction sheets, covering the unit that shares a name with that upgrade card.90.7 Each technology that is not a unit upgrade has a colored symbol displayed in the lower-right corner of the card and on its card back that indicates that technology&rsquo;s color.a A technology&rsquo;s color has no inherent game effect; however, " +
                "each technology a player owns can satisfy a prerequisite of a matching color when researching other technology.b Unit upgrades do not have a color and do not satisfy prerequisites.c There are four colors of technologies as follows:90.8 Most technology cards have a column of colored symbols displayed in the lower-left corner of the card. Each symbol in this column is a prerequisite.a A technology card&rsquo;s prerequisites indicate the number and color of technologies a player must own to research that " +
                "technology card.90.9 RESEARCHING TECHNOLOGYA player can research technology by resolving either the primary or secondary ability of the &ldquo;Technology&rdquo; strategy card during the action phase. Other game effects may also instruct a player to research technology.90.10 To research technology, a player gains that technology card from their technology deck and places it in their play area near their faction sheet.a Players place any unit upgrades they gain faceup on their faction sheets, covering the unit " +
                "that shares a name with that upgrade card.90.11 A player cannot research a faction technology that does not match their faction.</p>\r\n<p>90.12 When researching technology, a player must satisfy each of a technology&rsquo;s prerequisites to research it. To satisfy a technology&rsquo;s prerequisites, that player must own one technology of the matching color for each prerequisite symbol on the technology card they wish to research.a Prerequisites symbols are displayed as symbols on the lower-left corner of the card." +
                "b Unit upgrade technologies do not have a color and do not satisfy prerequisites.c Players may use certain abilities or technology specialties to ignore some prerequisites.90.13 TECHNOLOGY SPECIALTIESA technology specialty is a technology symbol found on some planets.90.14 When researching technology, a player can exhaust a planet they control that has a technology specialty to ignore one prerequisite symbol of the matching type on the technology card they are researching.90.15 If the planet card is already exhausted, " +
                "it cannot be used to ignore a prerequisite.90.16 VALEFAR ASSIMILATORThe Nekro Virus faction can use its faction abilities in combination with its two &ldquo;Valefar Assimilator&rdquo; faction technologies to gain faction technologies that have been researched by other players, including unit upgrades.90.17 Basic units printed on faction sheets are not technologies and are not eligible targets for &ldquo;Valefar Assimilator.&rdquo;90.18 If a player becomes eliminated while one of their technologies " +
                "has a Valefar Assimilator token on it, the Nekro Virus player places that technology in their play area; it is not removed from the game.90.19 If a Valefar Assimilator token is removed from the Saar &ldquo;Floating Factory&rdquo; unit upgrade, each of the Nekro Virus&rsquo; space docks must be placed on an eligible planet they control in that space dock&rsquo;s system, or the space dock is returned to their reinforcements.90.20 If a Valefar Assimilator token is removed from a war sun unit " +
                "upgrade and the Nekro Virus player does not have the standard war sun unit upgrade, each of the Nekro Virus player&rsquo;s war suns are returned to their reinforcements.90.21 The Nekro Virus player may upgrade their units with units of the same type (e.g., &ldquo;dreadnought&rdquo; or &ldquo;infantry&rdquo;) even if those unit&rsquo;s names do not match. If the Nekro Virus gains a unit upgrade technology of the same unit type as a unit upgrade technology they already have, the previous upgrade is removed, " +
                "and they must use the same Valefar Assimilator token that was used to copy the previous upgrade.</p>\r\n<p>90.22 When a Valefar Assimilator token is on a technology, that technology&rsquo;s color and type count toward objectives.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Technology,
                Content = "<p>91 TECHNOLOGY (STRATEGY CARD)The &ldquo;Technology&rdquo; strategy card allows players to research new technology. This card&rsquo;s initiative value is &ldquo;7.&rdquo;91.1 During the action phase, if the active player has the &ldquo;Technology&rdquo; strategy card, they can perform a strategic action to resolve that card&rsquo;s primary ability.91.2 To resolve the primary ability on the &ldquo;Technology&rdquo; strategy card, the active player can research one technology of their choice. Then, " +
                "they may research one additional technology of their choice by spending six resources.91.3 After the active player resolves the primary ability of the &ldquo;Technology&rdquo; strategy card, each other player, beginning with the player to the left of the active player and proceeding clockwise, may research one technology of their choice by spending one command token from their strategy pool and four resources</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Trade,
                Content = "<p>92 TRADE (STRATEGY CARD)The &ldquo;Trade&rdquo; strategy card allows players to gain trade goods and replenish commodities. This card&rsquo;s initiative value is &ldquo;5.&rdquo; 92.1 During the action phase, if the active player has the &ldquo;Trade&rdquo; strategy card, they can perform a strategic action to resolve that card&rsquo;s primary ability. To resolve the primary ability on the &ldquo;Trade&rdquo; strategy card, the active player resolves the following effects in order:" +
                "92.2 STEP 1&mdash;The active player gains 3 trade goods.92.3 STEP 2&mdash;The active player replenishes their commodities by taking the number of commodity tokens necessary to have an amount equal to the commodity value on their faction sheet. Then, they place those tokens in the commodity area of their faction sheet.a A player cannot have more commodities than the commodity value printed on their faction sheet.92.4 STEP 3&mdash;The active player chooses any number of other " +
                "players. Those players use the secondary ability of this card without spending a command token.a The chosen players must resolve the secondary ability of this card without spending a command token after the active player finishes resolving the primary ability.</p>\r\n<p>b The chosen players can only use the secondary ability once, and they cannot use it by spending command tokens.92.5 After the active player resolves the primary ability of the &ldquo;Trade&rdquo; strategy card, each other player, " +
                "beginning with the player to the left of the active player and proceeding clockwise, may spend one command token from their strategy pool to replenish their commodities.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.TradeGoods,
                Content = "<p>93 TRADE GOODSA trade good represents a player&rsquo;s buying and trading power beyond their planet&rsquo;s raw resources.93.1 Trade goods and commodities are represented by opposite sides of the same token.93.2 When a player gains a trade good, they take a trade good token from the supply and place it on the trade good area on their command sheet, making sure the trade good side is faceup.93.3 A player can spend trade goods at any time during the game." +
                "93.4 A player can spend a trade good in one of the following ways:a In place of spending one resource.b In place of spending one influence. However, trade goods cannot be spent to cast votes during the agenda phase.c To resolve an effect that specifically requires that a trade good be spent.93.5 A player can exchange their trade goods with other players during a transaction.93.6 When a player receives a commodity token from another player, the player who received that token places it in their " +
                "trade good area with the trade good side of the token faceup.a That token is no longer a commodity token; it is a trade good token.93.7 Trade good tokens come in values of one and three. A player can swap between these tokens as necessary.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Transactions,
                Content = "<p>94 TRANSACTIONSA transaction is a way for a player to exchange commodities, trade goods, promissory notes, and relic fragments.94.1 During the active player&rsquo;s turn, they may resolve up to one transaction with each of their neighbors.a A player can resolve a transaction at any time during their turn, even during a combat.</p>\r\n<p>94.2 To resolve a transaction, a player gives any number of trade goods and commodities and up to one promissory note to " +
                "a neighbor in exchange for any number of trade goods, commodities, and relic fragments, and up to one promissory note.94.3 Players can exchange commodities, trade goods, promissory notes, and relic fragments, but cannot exchange other types of cards or tokens.a The Emirates of Hacan can also exchange action cards with other players as part of their transactions.94.4 A transaction does not have to be even. A player may exchange components of unequal value or give components without " +
                "receiving something in return.a The players agree on the terms of the transaction before exchanging any components. After the components are traded, the transaction cannot be undone.94.5 Players can resolve a transaction as part of a deal.94.6 While resolving each agenda during the agenda phase, a player may perform one transaction with each other player.a Players do not need to be neighbors to perform these transactions.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Transport,
                Content = "<p>95 TRANSPORTWhen a ship moves, it may transport any combination of fighters and ground forces, but the number of units it transports cannot exceed that ship&rsquo;s capacity value.95.1 The ship can pick up and transport fighters and ground forces when it moves. During a tactical action, it can pick up and transport units from the active system, the system it started its movement in, and each system it moves through.a These transported units remain with the transporting ship " +
                "until it is finished moving.b Units being transported by a ship that is removed from the board by a gravity rift are also removed from the board.95.2 Any fighters and ground forces that a ship transports must move with the ship and remain in the space area of a system.95.3 Fighters and ground forces cannot be picked up from a system that contains one of their faction&rsquo;s command tokens other than the active system.95.4 A player can land ground forces on a planet in a system during " +
                "the &ldquo;Invasion&rdquo; step of a tactical action.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Units,
                Content = "<p>96 UNITSA unit is represented by a plastic figure.96.1 There are three types of units: ships, ground forces, and structures.96.2 Each color of plastic comes with the following units:&bull; 3 Space Docks&bull; 6 PDS units&bull; 8 Destroyers&bull; 8 Cruisers&bull; 2 War Suns&bull; 12 Infantry &bull; 10 Fighters&bull; 4 Carriers&bull; 5 Dreadnoughts&bull; 1 Flagship&bull; 4 Mechs96.3 Units exist either on the game board or in a player&rsquo;s " +
                "reinforcements.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.UnitUpgrades,
                Content = "<p>97 UNIT UPGRADESA unit upgrade is a type of technology card.97.1 Unit upgrades share a name with a unit that is printed on a player&rsquo;s faction sheet, but have a higher roman numeral. For example, a player&rsquo;s &ldquo;Carrier I&rdquo; unit is upgraded by the unit upgrade technology &ldquo;Carrier II.&rdquo;a The Nekro Virus player may upgrade their units with units of the same type (e.g., &ldquo;dreadnought&rdquo; or &ldquo;infantry&rdquo;) even if " +
                "those unit&rsquo;s names do not match. If the Nekro Virus gains a unit upgrade technology of the same unit type as a unit upgrade technology they already have, the previous upgrade is removed, and they must use the same Valefar Assimilator token that was used to copy the previous upgrade.97.2 Players place unit upgrades they gain faceup on their faction sheets, covering the unit that shares a name with that upgrade card.97.3 The white arrows next to an attribute on a faction sheet indicate " +
                "that the attribute will improve when the unit is upgraded.97.4 After a player gains a unit upgrade card, each of that player&rsquo;s units that correspond to that upgrade card is treated as having the attributes and abilities printed on that upgrade card. Any previous attributes of that unit, such as the one printed on that player&rsquo;s faction sheet, are ignored.97.5 A mech unit card is not a technology.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.VictoryPoints,
                Content = "<p>98 VICTORY POINTSThe first player to gain 10 victory points wins the game.98.1 Players gain victory points in a variety of ways. A common way that a player can gain victory points is by scoring objectives.98.2 Each player uses the victory point track to indicate the number of victory points they have gained.a If the players are using the 14-space side of the victory point track, the game ends and a player wins when they have 14 victory points instead of 10.98.3 Each player places " +
                "one of their control tokens on space &ldquo;0&rdquo; of the victory point track during setup.98.4 When a player gains a victory point, they advance their control token a number of spaces along the victory point track equal to the number of victory points gained.a A player&rsquo;s control token must always be on the space of the victory point track that shows a number that matches the number of victory points that player has gained during the game. A player cannot have more than 10 victory points." +
                "98.5 If an ability refers to the player with the &ldquo;most&rdquo; or &ldquo;fewest&rdquo; victory points, and more than one player is tied in that respect, the effect applies to all of the tied players.98.6 If a player gains a victory point from a law, and that law is discarded, that player does not lose that victory point.98.7 The game ends immediately when one player has 10 victory points. If multiple players would simultaneously gain their 10th victory point, the player who is earliest in initiative order " +
                "among those players is the winner; if this occurs when players do not have strategy cards, the player who is nearest the speaker (including the speaker) in clockwise order is the winner.98.8 If the game ends because the speaker cannot reveal an objective card, the player with the most victory points is the winner. If one or more players are tied for having the most victory points, the tied player who is first in initiative order is the winner.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Warfare,
                Content = "<p>99 WARFARE (STRATEGY CARD)The &ldquo;Warfare&rdquo; strategy card allows a player to remove a command token from the board and redistribute their command tokens between their tactic, fleet, and strategy pools. This card&rsquo;s initiative value is &ldquo;6.&rdquo; During the action phase, if the active player has the &ldquo;Warfare&rdquo; strategy card, they can perform a strategic action to resolve that card&rsquo;s primary ability.To resolve the primary ability on the &ldquo;Warfare&rdquo; strategy card, the " +
                "active player performs the following steps:99.1 STEP 1&mdash;The active player removes any one of their command tokens from the game board. Then, that player gains that command token by placing it in a pool of their choice on their command sheet.</p>\r\n<p>99.2 STEP 2&mdash;The active player can redistribute their command tokens.99.3 After the active player resolves the primary ability of the &ldquo;Warfare&rdquo; strategy card, each other player, beginning with the player to the left of the active player and proceeding clockwise, " +
                "may spend one command token from their strategy pool to resolve the &ldquo;Production&rdquo; ability of one space dock in their home system.a The command token is not placed in their home system.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.WormholeNexus,
                Content = "<p>100 WORMHOLE NEXUSThe wormhole nexus is a system tile where several wormholes converge.100.1 The wormhole nexus begins the game in play with its inactive side up.a The inactive side of the wormhole nexus contains a gamma wormhole. The active side of the wormhole nexus contains an alpha, beta, and gamma wormhole.b The wormhole nexus is treated as part of the game board.c The wormhole nexus is on the edge of the game board.100.2 After a player moves or places a unit into the wormhole nexus, " +
                "or gains control of the planet Mallice, that player flips the wormhole nexus to its active side.a When a ship moves into the wormhole nexus, the nexus becomes active at the end of the &ldquo;Movement&rdquo; step.</p>",
            },
            new Rule()
            {
                RuleCategory = RuleCategory.Wormholes,
                Content = "<p>101 WORMHOLESSome systems contain wormholes. Systems that contain identical wormholes are adjacent.101.1 There are two basic types of wormholes: alpha and beta.101.2 If a player has a PDS unit upgrade technology, they can use the &ldquo;Space Cannon&rdquo; abilities of their PDS units through wormholes.101.3 Players can be neighbors and perform transactions through wormholes.101.4 There are two advanced types of wormhole: delta and gamma. These wormholes follow all other wormhole rules." +
                "a The delta wormholes are present on the Creuss Gate system tile and the Ghosts of Creuss home system tile.b The gamma wormholes are present on the wormhole nexus and can be discovered during exploration</p>",
            },
        };

        var updatedRulesByLanguage = rules.Select(rule =>
        {
            rule.Language = Language.English;
            return rule;
        }).ToList();

        context.AddRange(updatedRulesByLanguage);
        context.SaveChanges();
    }
}
