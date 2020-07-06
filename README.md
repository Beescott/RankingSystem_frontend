# RankingSystem_frontend

Time to make the frontend: 3 days

## Technical / Architectural choices
To create the connection with the server, I used the asset Socket.IO which you can find on the asset store. It allows socket.io communication with a specified server as well as handles the JSON format. The only downside is that it is only usable while in play mode. Connection with the server with the editor is not possible with this asset.

The RankingSystemController class allows the devs to customize the ranking system by changing the web URL, the number of requested players, the primary and secondary colors of the ranking system display, the float precision, whether the list is ascending or descending and finally if they want to put a sprite instead of just a numerical rank.
The RankingSystemController has a custom inspector: it does not have a window editor. I will explain the reason in the "Further Ahead" section.

The scene is made with only one scene, as it would be overkill to make a whole scene for each of these pages.

## Difficulties
I had a big issue with JSON format. For some reason, when having decimal numbers in the database, Unity always read that value as null. When working on my other computer (which system is set to the US), I had no such problems. I also tried it on a german system computer which had no problems as well: the problem only arose when working on a french system. I spent a few hours figuring out why, but it turns out the JSON parser used in the asset was not conceived to be working with the french system. So I had to modify it in order to make it work again.

I also had difficulties designing the tool: how to make it look, what should the tool modify, what it shouldn't... Putting too much control on the tool also removes custom control for the users: which would be counter-productive. Thus, the tool should have control over things that are sure to be in every ranking system and let the developers have some freedom about certain things.

## Further ahead
The RankingSystemController class is displayed in a custom inspector. Given so little time, I could not implement what I think would be preferable. I believe a custom editor with basically the same options but with a dummy preview list of rankings would be more interesting. That would also allow the developers to see the end result (the two pages side by side) faster than with the current way.

For now, the players can't have several scores: when given the same name, the score updates if it is higher than before, otherwise, nothing happens. It could be a good idea to give each player a list of scores instead and display only the highest in the ranks.

Giving the possibility of displaying a maximum number of people per page would be interesting as well.

## My comment

Socket.IO asset choice is not really a  good choice in the end:
It is not possible to use it in the editor, so we can only remove a player when in game.

This does not really make sense, as it should only be possible to delete a score in the editor or with a third-party.
However, I implemented it anyway to show that the functionality is indeed working.