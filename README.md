# MediatekDocuments

La présentation de l'application globale est présente sur [le dépôt GitHub d'origine](https://github.com/CNED-SLAM/MediaTekDocuments?tab=readme-ov-file#mediatekdocuments).

## Présentation des nouvelles fonctionnalités

La partie authentification et gestion des documents à été développée complètement. Elle contient les fonctionnalités globales suivantes :

![Diagramme de cas d'utilisation](https://github.com/user-attachments/assets/ca9a8345-f39e-4957-80c5-61e89ae37bde)

## Les nouvelles fonctionnalités

### Onglet livres / DVDs

Il est maintenant possible d'ajouter, modifier et supprimer des livres / DVDs grâce à la barre d'actions en bas de la fenêtre.  
Il n'est pas possible de modifier le numéro d'un document existant.  
Lors de l'ajout ou de la modification d'un livre / DVD, il ne sera pas possible de changer d'onglet ou d'effectuer des recherches.  
Aussi, un bouton parcourir pour sélectionner une image du livre / DVD apparaitra.
Il n'est pas possible de supprimer un livre / DVD si une commande ou des exemplaires y sont rattachés.  
Enfin, pour chaque livre, la liste de ses exemplaires s'affiche. Il est alors possible de modifier leur état en double cliquant sur l'état. Il est aussi possible de supprimer un exemplaire en cliquant sur le bouton supprimer.

![Modification d'un livre et de ses exemplaires](https://github.com/user-attachments/assets/b4e38fc5-d756-4bb5-a30c-2cd0f34ab6db)

Il est maintenant possible de gérer les commandes d'un livre / DVD en cliquant sur le bouton gérer les commandes.  
La gestion des commandes se passe de manière similaire à la gestion des documents.

![Modification d'une commande d'un livre](https://github.com/user-attachments/assets/c9980800-e4e7-417c-85be-6fe273f30bd1)

### Onglet revues

Il est maintenant possible d'ajouter, modifier et supprimer des revues grâce à la barre d'actions en bas de la fenêtre de la même façon que pour les livres / DVDs.  
Il n'est pas possible de supprimer une revue si un abonnement ou des parutions y sont rattachés.  

![Modification d'un revue](https://github.com/user-attachments/assets/0886863c-a927-48f8-a838-7531a0747dcc)

Il est maintenant possible de gérer les abonnements d'une revue en cliquant sur le bouton gérer les abonnements.  
La gestion des abonnements se passe de manière presque identique à la gestion des commandes.

![Modification d'un abonnement d'une revue](https://github.com/user-attachments/assets/ead580ac-d16c-44eb-8b98-8ad8e8891c46)

### Onglet parutions

Il est maintenant possible pour chaque parution d'une revue de modifier leur état en double cliquant sur l'état. Il est aussi possible de supprimer une parution en cliquant sur le bouton supprimer.

![Modification d'une parution d'une revue](https://github.com/user-attachments/assets/392780ba-a7df-460b-8a92-a98bda571dcb)

## L'API REST
L'application exploit une base de données à travers une API REST protégée par une authentification basique.<br>
Le code de l'API se trouve ici : https://github.com/Refragg/rest_mediatekdocuments avec toutes les explications pour l'utiliser (dans le readme).  

## Installation de l'application

Afin d'installer l'application, il faut récupérer la dernière version disponible [ici](https://github.com/Refragg/MediaTekDocuments/releases/latest).  
Ensuite, il faut exécuter le fichier installeur en `.msi`.  
Après, il faudra renseigner l'URL d'accès et les identifiants pour l'accès à l'API dans le fichier `MediaTekDocuments.exe.config` (qui se trouvera par défaut dans le dossier `C:\Program Files (x86)\MediaTek86\MediaTekDocuments`).  
L'application sera accessible depuis un raccourci sur le bureau et dans le menu démarrer.

## Licence de l'icône

Image originale du document [Annuaire, Bleu, Catégorie](https://pixabay.com/fr/vectors/annuaire-bleu-cat%C3%A9gorie-dossier-151106/) par [OpenClipart-Vectors](https://pixabay.com/fr/users/openclipart-vectors-30363/?utm_source=link-attribution&utm_medium=referral&utm_campaign=image&utm_content=151106) de [Pixabay](https://pixabay.com/fr//?utm_source=link-attribution&utm_medium=referral&utm_campaign=image&utm_content=151106)
