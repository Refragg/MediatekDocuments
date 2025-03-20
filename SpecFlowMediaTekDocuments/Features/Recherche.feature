Feature: Recherche

Scenario: Recherche livre
Given je suis dans longlet 'Livres'
When je selectionne le genre 'Bande dessinée'
Then le nombre de livres obtenu est de 5