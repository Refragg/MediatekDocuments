# Documentation

Ce dossier contient le projet DocFX permettant de générer la documentation technique

La dernière version est accessible en ligne [ici](https://docs.mediatekdocuments.jfrancart.fr/)

## Préparation

Si ce n'est pas déjà fait, il faut installer les outils dotnet CLI, plus d'informations sont disponibles [ici](https://learn.microsoft.com/fr-fr/dotnet/core/tools/)

Il faut d'abord installer l'outil dotnet docfx, pour ce faire, exécutez cette commande :

`dotnet tool update -g docfx`

Ensuite, il faut naviguer vers ce dossier dans votre terminal puis exécuter cette commande :

`docfx docfx.json --serve`

Après quelques minutes, le serveur web devrait être lancé et le site avec la documentation sera accessible à l'adresse donnée par DocFX.