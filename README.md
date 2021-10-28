<div align="center">

<img src="doc/ReadMeBanner.png" />

# General Commands

</div>

## fm!prefix [prefix]
Set a new prefix for the bot. Required `Administrator` permissions in the server

## fm!setscrimrole [role]
Set the scrim role for the server. Required `Administrator` permissions in the server

## fm!setmutedrole [role]
Set the muted role for the server. Required `Administrator` permissions in the server

## fm!ban [user] (reason)
Ban a user from the server. Required `Ban Members` permission in the server

## fm!kick [user] (reason)
Kick a user from the server. Required `Kick Members` permission in the server

## fm!mute [user]
Mute a user. Required `Mute Members` permission in the server

## fm!unmute [user]
Unmute a user. Required `Mute Members` permission in the server

## fm!sping [user]
Will ping the given user five times, once every second

<div align="center">

# Scrim / Roster Commands

</div>

## fm!roster
Displays the current roster.

## fm!addroster [team name]
Add a new team to the roster. Required `Administrator` permissions in the server

## fm!removeroster [team index]
Remove a team from the roster. Required `Administrator` permissions in the server

## fm!setroster [new roster...]
Set the roster to the given roster Required `Administrator` permissions in the server

### Example
```
fm!setroster
🇳🇱 Team 1
🇩🇪 Team 2
```

## fm!clearroster
Clears the roster. Required `Administrator` permissions in the server

<div align="center">

# Triggers

</div>

## "uwu" and "owo"
If a user sends a message containing "uwu" or "owo", the bot will reply with either "uwu" or "owo" depending on the user's message

## "scrim"
If a user sends a message containing "scrim", the bot will ping the configured role.